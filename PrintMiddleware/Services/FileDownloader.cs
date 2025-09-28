using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrintMiddleware.Services
{
    public static class FileDownloader
    {
        private static readonly HttpClient httpClient = new HttpClient();
        /// <summary>
        /// 下载文件到临时目录，返回本地路径
        /// </summary>
        /// <param name="fileUrl">文件的 URL</param>
        /// <returns>本地临时文件路径</returns>
        /// 
        /// 使用示例
        /// string localPath = await FileDownloader.DownloadPdfAsync("https://example.com/test.pdf");
        /// 打印完之后记得删除文件
        /// File.Delete(localPath);
        public static async Task<string> DownloadPdfAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                throw new ArgumentException("File url connot be empty.");

            string tempFileName = Path.GetTempFileName();
            string tempPdfPath = Path.ChangeExtension(tempFileName, ".pdf");

            try
            {
                using (var response = await httpClient.GetAsync(fileUrl))
                {
                    response.EnsureSuccessStatusCode();

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = File.Create(tempPdfPath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
                return tempPdfPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to download pdf: {fileUrl}\nError: {ex.Message}", ex);
            }        
    }


    }
}
