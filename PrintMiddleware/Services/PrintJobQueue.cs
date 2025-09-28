using Newtonsoft.Json.Linq;
using PrinterMiddleware.Services;
using PrintMiddleware.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PrintMiddleware.Services
{
    /// <summary>
    /// 打印任务实体类
    /// </summary>
    public class PrintJob
    {
        public int PrinterId { get; set; }         // 打印机编号（如 1, 2, 3）
        public string PrinterName { get; set; }    // 打印机系统名称（#1 HP LaserJet ...）
        public string FileUrl { get; set; }        // PDF 文件 URL
        public string LocalFilePath { get; set; }  // 下载后本地路径（内部使用）

        public PrintJob(int printerId, string printerName, string fileUrl)
        {
            PrinterId = printerId;
            PrinterName = printerName;
            FileUrl = fileUrl;
        }

        public override string ToString()
        {
            return $"[Printer#{PrinterId}] {FileUrl}";
        }
    }

    /// <summary>
    /// 打印任务队列
    /// </summary>
    /* 
     * 示例代码
        var queue = new PrintJobQueue(); // 全局只需要一个
        // 解析 JSON
        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(wsMessage);
        foreach (var file in data.files)
        {
            int printerId = (int)file.printer_id;
            string fileUrl = (string)file.url;

            // 根据编号查找系统打印机名（只列出#开头）
            string printerName = System.Drawing.Printing.PrinterSettings.InstalledPrinters
                                    .Cast<string>()
                                    .FirstOrDefault(p => p.StartsWith($"#{printerId}"));
            if (!string.IsNullOrEmpty(printerName))
            {
                var job = new PrintJob(printerId, printerName, fileUrl);
                queue.Enqueue(job);
            }
        }
     */
    public class PrintJobQueue
    {
        private readonly ConcurrentQueue<PrintJob> _queue = new ConcurrentQueue<PrintJob>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public PrintJobQueue()
        {
            Task.Run(ProcessQueueAsync);
        }

        /// <summary>
        /// 入队打印任务
        /// </summary>
        public void Enqueue(PrintJob job)
        {
            _queue.Enqueue(job);
            _signal.Release();
        }


        /// <summary>
        /// 后台循环处理队列任务
        /// </summary>
        private async Task ProcessQueueAsync()
        {
            while (!_cts.IsCancellationRequested)
            {
                await _signal.WaitAsync(_cts.Token); // 等待队列中有任务
                if (_queue.TryDequeue(out var job))
                {
                    try
                    {
                        Logger.Info($"[Queue] Start processing job: {job}");

                        // 下载PDF
                        string localPath = await FileDownloader.DownloadPdfAsync(job.FileUrl);
                        job.LocalFilePath = localPath;

                        // 打印PDF
                        bool success = await PrintExecutor.PrintPdfAsync(job.LocalFilePath, job.PrinterName);

                        // 删除临时文件
                        if (File.Exists(job.LocalFilePath))
                        {
                            File.Delete(job.LocalFilePath);
                        }
                        Logger.Info($"[Queue] Job processed: {job}, Success: {success}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"[Queue] Job failed: {job}, Error: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 停止队列处理
        /// </summary>
        /// 
        public void Stop()
        {
            _cts.Cancel();
        }
    }


    public static class PrintJobValidator
    {
        public static bool ValidatePrintJobs(JArray files, out List<PrintJob> validJobs, out string errorMessage)
        {
            validJobs = new List<PrintJob>();
            errorMessage = null;

            if (files == null || files.Count == 0)
            {
                errorMessage = "No files provided.";
                return false;
            }

            foreach (var file in files)
            {
                try
                {
                    int printerId = (int)file["printer_id"];
                    string fileUrl = (string)file["url"];

                    if (string.IsNullOrWhiteSpace(fileUrl))
                        throw new Exception($"Missing or empty URL for printer #{printerId}");

                    // 打印机名校验
                    if (!PrinterManager.Exists(printerId))
                        throw new Exception($"Invalid printer id: #{printerId}");

                    // 打印机ID校验
                    string printerName = PrinterManager.GetPrinterNameById(printerId);
                    if (printerName == null)
                        throw new Exception($"No corresponding printer found for #{printerId}");

                    // URL格式校验
                    if (!Uri.TryCreate(fileUrl, UriKind.Absolute, out Uri uri) ||
                        !(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                    {
                        throw new Exception($"Invalid URL format for: {fileUrl}");
                    }

                    // URL可访问性校验（返回状态200）
                    if (!IsUrlAccessible(fileUrl))
                    {
                        throw new Exception($"URL not accessible or not found: {fileUrl}");
                    }

                    validJobs.Add(new PrintJob(printerId, printerName, fileUrl));
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return false;
                }
            }

            return true;
        }


        private static bool IsUrlAccessible(string url)
        {
            try
            {
                var request = WebRequest.CreateHttp(url);
                request.Method = "HEAD";
                request.Timeout = 8000; // 8s 超时

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }
    }

}
