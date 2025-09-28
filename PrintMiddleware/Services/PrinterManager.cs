using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;

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
    }
}
