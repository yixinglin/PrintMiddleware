using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("更新失败: " + ex.Message);
            }
        }

        static async Task Run()
        {
            string server = "http://localhost:5018";
            string appName = "print-middleware";
            string appDir = "./latest"; // 程序安装路径
            string versionFile = Path.Combine(appDir, "version.txt");
            string mainExe = Path.Combine(appDir, "PrintMiddleware.exe");

            Console.OutputEncoding = Encoding.UTF8;

            // === 本地版本号 ===
            string localVersion = File.Exists(versionFile) ? File.ReadAllText(versionFile).Trim() : "0.0.0";
            Console.WriteLine("当前本地版本: " + localVersion);

            using (var client = new HttpClient())
            {
                // === 获取最新版本 ===
                Console.WriteLine("正在检查服务器上的最新版本...");
                var latestUrl = server + "/api/v1/app_version/" + appName + "/latest";
                var latestResponse = await client.GetAsync(latestUrl);
                if (!latestResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("无法获取最新版本信息");
                    return;
                }

                string latestJson = await latestResponse.Content.ReadAsStringAsync();
                var latestObj = JObject.Parse(latestJson);
                string latestVersion = latestObj["version"]?.ToString() ?? "0.0.0";
                string filePath = latestObj["file_path"]?.ToString();

                Console.WriteLine("服务器最新版本: " + latestVersion);

                Version newVer, oldVer;
                try
                {
                    newVer = new Version(latestVersion);
                    oldVer = new Version(localVersion);
                }
                catch
                {
                    Console.WriteLine("版本号格式错误");
                    return;
                }

                if (newVer <= oldVer)
                {
                    Console.WriteLine("已是最新版本，无需更新。");
                    return;
                }

                // === 下载更新包 ===
                string downloadUrl = server + "/static2/uploads" + filePath;
                string tempZip = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));

                Console.WriteLine("⬇️ 正在下载更新包: " + downloadUrl);
                var zipBytes = await client.GetByteArrayAsync(downloadUrl);
                File.WriteAllBytes(tempZip, zipBytes);
                Console.WriteLine("下载完成: " + tempZip);

                // === 解压更新包 ===
                Console.WriteLine("正在解压到 " + appDir);
                if (Directory.Exists(appDir))
                {
                    // 先备份
                    string backupDir = appDir + "_backup_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    DirectoryCopy(appDir, backupDir, true);
                    Console.WriteLine("已备份旧版本到: " + backupDir);
                }

                try
                {
                    UnzipWithOverwrite(tempZip, appDir);
                    Console.WriteLine("解压完成。");
                } 
                catch (Exception ex)
                {
                    Console.WriteLine("解压失败: " + ex.Message);
                    return;
                }
                                
                // === 更新本地版本号 ===
                File.WriteAllText(versionFile, latestVersion);
                Console.WriteLine("已更新本地版本号: " + latestVersion);
            }
        }

        private static void UnzipWithOverwrite(string zipPath, string targetDir)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in archive.Entries)
                {
                    string filePath = Path.Combine(targetDir, entry.FullName);

                    // 确保目录存在
                    string dir = Path.GetDirectoryName(filePath);
                    if (!string.IsNullOrEmpty(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    if (string.IsNullOrEmpty(entry.Name))
                    {
                        // 这是一个目录，不需要解压
                        continue;
                    }

                    // 覆盖已有文件
                    entry.ExtractToFile(filePath, true);
                }
            }
        }

        // 目录拷贝函数
        private static void DirectoryCopy(string sourceDir, string destDir, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) return;

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDir, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
