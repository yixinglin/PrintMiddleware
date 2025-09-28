using Fleck;
using Newtonsoft.Json.Linq;
using PrinterMiddleware.Services;
using PrintMiddleware.Utils;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PrintMiddleware.Services
{
    public class WebSocketServerManager
    {
        private WebSocketServer server;
        private List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        private PrintJobQueue jobQueue;

        public static event Action<string> ClientConnected;
        public static event Action<string> ClientDisconnected;

        public WebSocketServerManager(int port, PrintJobQueue queue)
        {
            jobQueue = queue;
            server = new WebSocketServer($"ws://0.0.0.0:{port}");
            FleckLog.Level = LogLevel.Warn; 
        }

        public void Start()
        {
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    var ip = socket.ConnectionInfo.ClientIpAddress;                                        
                    allSockets.Add(socket);
                    Logger.Info($"[WS] Client connected: {socket.ConnectionInfo.ClientIpAddress}");
                    ClientConnected?.Invoke(ip);
                };

                socket.OnClose = () =>
                {
                    var ip = socket.ConnectionInfo.ClientIpAddress;                    
                    allSockets.Remove(socket);
                    Logger.Info("[WS] Client disconnected");
                    ClientDisconnected?.Invoke(ip);
                };

                socket.OnMessage = message =>
                {
                    Logger.Info("[WS] Task message received");
                    try
                    {
                        HandleMessage(message);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"[WS] Failed to process message: {ex.Message}");
                    }
                };
            });
        }

        private void HandleMessage(string json)
        {
            var jObject = JObject.Parse(json);
            var files = jObject["files"] as JArray;

            foreach (var file in files)
            {
                int printerId = (int)file["printer_id"];
                string fileUrl = (string)file["url"];

                // 查找以 #1, #2 开头的打印机
                if (!PrinterManager.Exists(printerId))
                {
                    Logger.Error($"[WebSocket] Invalid printer id: #{printerId}");
                    continue;
                }
                string printerName = PrinterManager.GetPrinterNameById(printerId);

                if (printerName == null)
                {
                    Logger.Error($"[WebSocket] No correspoinding printer found: #{printerId}");
                    continue;
                }

                var job = new PrintJob(printerId, printerName, fileUrl);
                jobQueue.Enqueue(job);
               Logger.Info($"[WebSocket] Print job enqueued: [{printerName}] <- {fileUrl}");
            }
        }

    }
}
