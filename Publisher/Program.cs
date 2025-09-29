using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class AppPublisher
    {
        static void Main(string[] args)
        {
            try
            {
                Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误: " + ex.Message);
            }
        }

        static async Task Run()
        {
            string server = "http://localhost:5018";
            string appName = "print-middleware";

            Console.OutputEncoding = Encoding.UTF8;

            Console.Write("请输入 Basic Auth 用户名: ");
            string username = Console.ReadLine()?.Trim() ?? "";

            Console.Write("请输入 Basic Auth 密码: ");
            string password = ReadPassword();

            var handler = new HttpClientHandler
            {
                Credentials = new System.Net.NetworkCredential(username, password)
            };

            using (var client = new HttpClient(handler))
            {
                string latestVersion = await GetLatestVersion(client, server, appName);
                Console.WriteLine("服务器最新版本: " + latestVersion);
            }

            Console.Write("请输入版本号 (例如 1.0.1): ");
            string version = Console.ReadLine()?.Trim() ?? "";

            Console.Write("请输入更新说明 (changelog): ");
            string changelog = Console.ReadLine()?.Trim() ?? "";

            // === 打包路径 ===
            string solutionDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            string releaseBaseDir = Path.Combine(solutionDir, "PrintMiddleware", "bin", "Release");

            if (!Directory.Exists(releaseBaseDir))
            {
                Console.WriteLine("未找到 bin/Release 目录，请先执行构建");
                return;
            }

            string targetDir = releaseBaseDir;
            Console.WriteLine("打包目录: " + targetDir);

            // === 压缩文件 ===
            string zipFile = $"publish_{version}_{DateTime.Now:yyyyMMddHHmmss}.zip";
            if (File.Exists(zipFile)) File.Delete(zipFile);

            Console.WriteLine("正在打包 " + targetDir + " 到 " + zipFile);
            ZipFile.CreateFromDirectory(targetDir, zipFile, CompressionLevel.Optimal, false);

            var handler2 = new HttpClientHandler
            {
                Credentials = new System.Net.NetworkCredential(username, password)
            };

            using (var client = new HttpClient(handler2))
            {
                // === 上传文件 ===
                Console.WriteLine("正在上传文件到服务器...");
                var uploadUrl = server + "/api/v1/common/file-upload";

                using (var form = new MultipartFormDataContent())
                {
                    using (var fileStream = File.OpenRead(zipFile))
                    {
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-zip-compressed");
                        form.Add(fileContent, "file", Path.GetFileName(zipFile));

                        var uploadResponse = await client.PostAsync(uploadUrl, form);
                        string uploadJson = await uploadResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("服务器返回: " + uploadJson);

                        var uploadObj = JObject.Parse(uploadJson);
                        var filePath = uploadObj["filename"]?.ToString();
                        if (string.IsNullOrEmpty(filePath))
                        {
                            Console.WriteLine("上传失败，未获取到 file_path");
                            return;
                        }

                        // === 提交版本信息 ===
                        Console.WriteLine("提交版本信息...");
                        var versionUrl = server + "/api/v1/app_version/" + appName;

                        var payload = new JObject
                        {
                            ["app_name"] = appName,
                            ["version"] = version,
                            ["file_path"] = filePath,
                            ["changelog"] = changelog
                        };

                        var jsonPayload = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
                        var versionResponse = await client.PostAsync(versionUrl, jsonPayload);
                        string versionJson = await versionResponse.Content.ReadAsStringAsync();
                        try
                        {
                            var parsed = JToken.Parse(versionJson);
                            string pretty = parsed.ToString(Newtonsoft.Json.Formatting.Indented);
                            Console.WriteLine("版本发布成功:\n" + pretty);
                        }
                        catch
                        {                            
                            Console.WriteLine("版本发布成功: " + versionJson);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取服务器上的最新版本号
        /// </summary>
        static async Task<string> GetLatestVersion(HttpClient client, string server, string appName)
        {
            var latestUrl = server + "/api/v1/app_version/" + appName + "/latest";
            var latestResponse = await client.GetAsync(latestUrl);
            if (!latestResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("无法获取最新版本信息，默认最新版本=0.0.0");
                return "0.0.0";
            }

            string latestJson = await latestResponse.Content.ReadAsStringAsync();
            try
            {
                var latestObj = JObject.Parse(latestJson);
                return latestObj["version"]?.ToString() ?? "0.0.0";
            }
            catch
            {
                Console.WriteLine("最新版本返回格式异常，默认最新版本=0.0.0");
                return "0.0.0";
            }
        }

        // 隐藏密码输入
        static string ReadPassword()
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return sb.ToString();
        }
    }
}
