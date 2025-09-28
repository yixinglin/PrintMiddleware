using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;


namespace PrintMiddleware.Utils
{
    public class NetworkHelper
    {
        public static List<string> GetLocalIPv4()
        {
            var ipList = new List<string>();

            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                ipList = host.AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                .Select(ip => ip.ToString())
                .ToList();
            }
            catch
            {
                ipList.Add("Failed to read local IP address.");
            }

            return ipList;
        }
    }
}
