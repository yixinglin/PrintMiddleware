using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrintMiddleware.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace PrintMiddleware.Services
{

    public class WebSocketServerManager
    {
        private WebSocketServer server;
        private List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        private PrintJobQueue jobQueue;
        private JsonSerializerSettings jsonSerializerSettings;

        public static event Action<string, string> ClientConnected;
        public static event Action<string, string> ClientDisconnected;

        public WebSocketServerManager(int port, PrintJobQueue queue)
        {
            jobQueue = queue;
            server = new WebSocketServer($"ws://0.0.0.0:{port}");
            FleckLog.Level = LogLevel.Warn; 
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
        }

        public void Start()
        {
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    var ip = socket.ConnectionInfo.ClientIpAddress;
                    var hostname = NetworkHelper.GetHostNameFromIp(ip);
                    allSockets.Add(socket);
                    Logger.Info($"[WS] Client connected: {socket.ConnectionInfo.ClientIpAddress} / {hostname}");
                    ClientConnected?.Invoke(ip, hostname);
                };

                socket.OnClose = () =>
                {
                    var ip = socket.ConnectionInfo.ClientIpAddress;
                    var hostname = NetworkHelper.GetHostNameFromIp(ip);
                    allSockets.Remove(socket);
                    Logger.Info("[WS] Client disconnected");
                    ClientDisconnected?.Invoke(ip, hostname);
                };

                socket.OnMessage = message =>
                {
                    Logger.Info("[WS] Task message received");
                    try
                    {
                        HandleMessage(socket, message);
                    }                     
                    catch (Exception ex)
                    {
                        Logger.Error($"[WS] Failed to process message: {ex.Message}");
                        var errorResponse = new WsResponse<object>
                        {
                            Status = "error",
                            Code = (int)ResponseCode.UnknownError,
                            Message = ex.Message,
                            Data = null
                        };
                        socket.Send(JsonConvert.SerializeObject(errorResponse, jsonSerializerSettings));
                    }
                };
            });
        }

        private void HandleMessage(IWebSocketConnection socket, string json)
        {
            var jObject = JObject.Parse(json);
            var files = jObject["files"] as JArray;

            if (!PrintJobValidator.ValidatePrintJobs(files, out var jobsToEnqueue, out var errorMessage))
            {
                Logger.Error($"[WebSocket] Validation failed: {errorMessage}");
                TrayNotifier.Show("Rejected", $"Job rejected: {errorMessage}", ToolTipIcon.Error);
                // 失败响应
                var errorResponse = new WsResponse<object>
                {
                    Status = "error",
                    Code = (int)ResponseCode.ValidationFailed,
                    Message = errorMessage,
                    Data = null
                };
                socket.Send(JsonConvert.SerializeObject(errorResponse, jsonSerializerSettings));
                return;
            }

            foreach (var job in jobsToEnqueue)
            {
                jobQueue.Enqueue(job);
                Logger.Info($"[WebSocket] Print job enqueued: [{job.PrinterName}] <- {job.FileUrl}");
            }
            TrayNotifier.Show("Task Accepted", $"{jobsToEnqueue.Count} job(s) accepted and enqueued.");

            // 成功响应
            var successResponse = new WsResponse<object>
            {
                Status = "ok",
                Code = (int)ResponseCode.Success,
                Message = "Job(s) enqueued successfully",
                Data = new { Count = jobsToEnqueue.Count }
            };
            socket.Send(JsonConvert.SerializeObject(successResponse, jsonSerializerSettings));

        }

    }


    public enum ResponseCode
    {
        Success = 0,
        ValidationFailed = 1001,
        UnknownError = 1999
    }

    public class WsResponse<T>
    {
        public string Status { get; set; }  // "ok" / "error"
        public int Code { get; set; }       // 0 成功, 非0 错误码
        public string Message { get; set; } // 提示信息
        public T Data { get; set; }         // 泛型数据（灵活扩展）
    }

}
