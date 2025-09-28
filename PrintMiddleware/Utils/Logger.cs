using System;
using System.IO;
using System.Text;

namespace PrintMiddleware.Utils
{
    internal class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly int retentionDays = 14; // 日志保留天数

        public static event Action<string, string, string> LogUpdated; // 新增事件，用于通知界面更新日志

        static Logger()
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // 清理旧日志
            CleanOldLogs(retentionDays);
        }

        /// <summary>
        /// 写入普通日志
        /// </summary>
        public static void Info(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        public static void Error(string message, Exception ex = null)
        {
            var fullMessage = ex == null ? message : $"{message}\n{ex.Message}\n{ex.StackTrace}";
            WriteLog("ERROR", fullMessage);
        }

        /// <summary>
        /// 日志写入核心方法（线程安全）
        /// </summary>
        private static void WriteLog(string level, string message)
        {
            lock (_lock)
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logFile = Path.Combine(logDirectory, $"log_{date}.log");                

                string logLine = $"[{time}] [{level}] {message}{Environment.NewLine}";

                try
                {
                    File.AppendAllText(logFile, logLine, Encoding.UTF8);
                }
                catch
                {
                    // 如果日志写入失败，静默失败（避免死循环）
                }

                // 触发事件，将日志信息传给ui
                LogUpdated?.Invoke(level, time, message);
            }
        }

        private static void CleanOldLogs(int _retentionDays)
        {
            try
            {
                var files = Directory.GetFiles(logDirectory, "log_*.log");

                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file); // e.g. log_2025-09-20
                    string datePart = fileName.Replace("log_", "");

                    if (DateTime.TryParse(datePart, out DateTime fileDate))
                    {
                        if ((DateTime.Now - fileDate).TotalDays > _retentionDays)
                        {
                            File.Delete(file);
                        }
                    }
                }
            }
            catch
            {
                // 日志清理失败不影响程序运行
            }
        }


    }
}
