using PrintMiddleware.Utils;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Management;

namespace PrinterMiddleware.Services
{
    /// <summary>
    /// 管理本地打印机，支持编号过滤 (#1、#2...)
    /// </summary>
    public static class PrinterManager
    {
        /// <summary>
        /// 获取所有本地打印机名称（包含所有）
        /// </summary>
        public static List<string> GetAllPrinters()
        {
            return PrinterSettings.InstalledPrinters
                .Cast<string>()
                .ToList();
        }

        /// <summary>
        /// 获取符合命名规范的打印机（以 #1、#2 开头）
        /// </summary>
        public static List<string> GetValidNumberedPrinters()
        {
            return GetAllPrinters()
                .Where(p => p.Trim().StartsWith("#"))
                .OrderBy(p => GetPrinterId(p))
                .ToList();
        }

        /// <summary>
        /// 根据打印机编号获取打印机名称（如编号 1 返回 "#1 HP LaserJet"）
        /// </summary>
        public static string GetPrinterNameById(int printerId)
        {
            return GetValidNumberedPrinters()
                .FirstOrDefault(p => GetPrinterId(p) == printerId);
        }

        /// <summary>
        /// 从打印机名称中提取编号（如 "#1 HP LaserJet" -> 1）
        /// </summary>
        public static int GetPrinterId(string printerName)
        {
            if (string.IsNullOrWhiteSpace(printerName)) return -1;

            try
            {
                if (printerName.StartsWith("#"))
                {
                    var rest = printerName.Substring(1); // 去掉 #
                    string number = new string(rest.TakeWhile(char.IsDigit).ToArray());

                    if (int.TryParse(number, out int id))
                        return id;
                }
            }
            catch { }

            return -1;
        }

        /// <summary>
        /// 检查指定编号的打印机是否存在
        /// </summary>
        public static bool Exists(int printerId)
        {
            return GetPrinterNameById(printerId) != null;
        }

        /// <summary>
        /// 获取指定打印机的默认纸张尺寸（如 "#1 HP LaserJet" -> "A4 (210 x 297 mm)"）
        /// </summary>
        public static string GetDefaultPaperSize(string printerName)
        {
            if (string.IsNullOrWhiteSpace(printerName))
                return "Unknown";

            try
            {
                var settings = new PrinterSettings
                {
                    PrinterName = printerName
                };

                if (!settings.IsValid)
                    return "Invalid Printer";

                var paper = settings.DefaultPageSettings.PaperSize;

                // 宽高是以百分之一英寸为单位
                // 换算成毫米
                float widthMm = paper.Width * 0.254f;
                float heightMm = paper.Height * 0.254f;

                // 保留 0 位小数，更易读
                return $"{paper.PaperName} ({Math.Round(widthMm)} x {Math.Round(heightMm)} mm)";
            }
            catch (Exception ex)
            {
                // Logger.Error($"读取纸张尺寸失败: {printerName}", ex);
                return "Unknown";
            }
        }

        public static int ClearAllPrintQueues()
        {
            int deletedCount = 0;

            // Win32_PrintJob 控制系统中的所有打印任务
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PrintJob");

            foreach (ManagementObject printJob in searcher.Get())
            {
                try
                {
                    printJob.Delete();
                    deletedCount++;
                }
                catch
                {                                        
                }
            }

            return deletedCount;
        }

    }
}
