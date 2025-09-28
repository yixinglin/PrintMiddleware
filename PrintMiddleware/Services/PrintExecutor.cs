using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PrintMiddleware.Services
{
    internal class PrintExecutor
    {
        private static readonly string SumatraPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets\\SumatraPDF.exe");
        /// <summary>
        /// 静默打印 PDF 文件
        /// </summary>
        /// <param name="pdfPath">本地 PDF 路径</param>
        /// <param name="printerName">打印机名称</param>
        /// 
        /*
            使用示例
            string localPath = await FileDownloader.DownloadPdfAsync(fileUrl);
            bool success = await PrintExecutor.PrintPdfAsync(localPath, printerName);
            if (success)
            {
                File.Delete(localPath); // 打印完成后删除
            }
         */
        public static async Task<bool> PrintPdfAsync(string pdfPath, string printerName)
        {
            if (!File.Exists(SumatraPath))
                throw new FileNotFoundException("SumatraPDF.exe not found. Please make sure the file exists in the program directory.");

            if (!File.Exists(pdfPath))
                throw new FileNotFoundException($"PDF file does not exist: {pdfPath}");

            if (string.IsNullOrWhiteSpace(printerName))
                throw new ArgumentException("Printer name cannot be empty.");

            string args = $"-print-to \"{printerName}\" \"{pdfPath}\"";

            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = SumatraPath,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                using (var process = Process.Start(processStartInfo))
                {
                    await Task.Run(() => process.WaitForExit());
                    // 返回是否成功退出
                    return process.ExitCode == 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to print file: {pdfPath}\nError: {ex.Message}", ex);
            }
        }    
    }
}
