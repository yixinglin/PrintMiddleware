using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMiddleware.Utils
{
    // 基础异常
    public class PrintJobException : Exception
    {
        public int Code { get; }

        public PrintJobException(string message, int code) : base(message)
        {
            Code = code;
        }
    }

    // 校验失败
    public class ValidationException : PrintJobException
    {
        public ValidationException(string message)
            : base(message, 1001) { }
    }

    // 打印机不可用
    public class PrinterUnavailableException : PrintJobException
    {
        public PrinterUnavailableException(string message)
            : base(message, 1002) { }
    }

    // 未知错误
    public class UnknownJobException : PrintJobException
    {
        public UnknownJobException(string message)
            : base(message, 1999) { }
    }
}
