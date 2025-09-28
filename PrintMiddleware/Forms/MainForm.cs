using Microsoft.Win32;
using PrinterMiddleware.Services;
using PrintMiddleware.Services;
using PrintMiddleware.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PrintMiddleware.Forms
{
    public partial class MainForm : Form
    {

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        private WebSocketServerManager wsServer;
        private PrintJobQueue jobQueue;

        private Queue<(string, string, string)> logBuffer = new Queue<(string, string, string)>();
        private const int maxLogLines = 50;

        private HashSet<string> clientIpSet = new HashSet<string>();


        public MainForm()
        {
            InitializeComponent();
            InitTrayIcon();
            LoadPrinters();
            Logger.LogUpdated += OnLogUpdated;

            WebSocketServerManager.ClientConnected += OnClientConnected;
            WebSocketServerManager.ClientDisconnected += OnClientDisconnected;
        }

        private void OnLogUpdated(string level, string timestamp, string message)
        {
            if (richTextBoxBackgroundMessages.InvokeRequired)
            {
                richTextBoxBackgroundMessages.Invoke(new Action(() => UpdateLogTextBox(level, timestamp, message)));
            }
            else
            {
                UpdateLogTextBox(level, timestamp, message);
            }
        }

        private void UpdateLogTextBox(string level, string timestamp, string message)
        {
            logBuffer.Enqueue((level, timestamp, message));
            if (logBuffer.Count > maxLogLines)
            {
                logBuffer.Dequeue();
            }

            // 清空当前显示
            richTextBoxBackgroundMessages.Clear();

            // 重新构建
            foreach (var (lvl, time, msg) in logBuffer)
            {
                richTextBoxBackgroundMessages.SelectionStart = richTextBoxBackgroundMessages.TextLength;
                if (lvl == "ERROR")
                    richTextBoxBackgroundMessages.SelectionColor = Color.Red;
                else
                    richTextBoxBackgroundMessages.SelectionColor = Color.Black;
                richTextBoxBackgroundMessages.AppendText($"[{time}] [{lvl}] {msg}{Environment.NewLine}");
             }
            richTextBoxBackgroundMessages.SelectionStart = richTextBoxBackgroundMessages.Text.Length;
            richTextBoxBackgroundMessages.ScrollToCaret();
        }

        private void LoadPrinters()
        {
            listViewPrinters.Items.Clear();

            foreach (string printer in PrinterManager.GetValidNumberedPrinters())
            {
                string paperSize = PrinterManager.GetDefaultPaperSize(printer);

                var item = new ListViewItem(printer);         // 第一列：打印机名
                item.SubItems.Add(paperSize);                 // 第二列：纸张尺寸

                listViewPrinters.Items.Add(item);
            }

            Logger.Info("Loaded printers");

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string portStr = ConfigManager.Get("port") ?? "5888";
            int port = int.TryParse(portStr, out int p) ? p : 5888;

            textBoxPort.Text = portStr;
            checkBoxAutoStart.Checked = ConfigManager.Get("autostart") == "1";

            RefreshNetworkDisplay();
            
            jobQueue = new PrintJobQueue();            
            wsServer = new WebSocketServerManager(port, jobQueue);
            wsServer.Start();            
        }

        private void RefreshNetworkDisplay()
        {
            List<string> ipList = NetworkHelper.GetLocalIPv4();
            string port = textBoxPort.Text.Trim();
            foreach(string ip in ipList)
            {
                textBoxAllWsAddresses.Text += $"ws://{ip}:{port}{Environment.NewLine}";
            }

        }

        private void InitTrayIcon()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Open", null, (s, e) => this.Show());
            trayMenu.Items.Add("Exit", null, (s, e) => Application.Exit());

            string iconPath = Path.Combine("Assets", "icon.ico");
            if (File.Exists(iconPath))
            {
                Icon = new Icon(iconPath);
            }
            else
            {
                MessageBox.Show("找不到图标文件：" + iconPath);
            }


            trayIcon = new NotifyIcon
            {
                Text = "Print Middleware",
                Icon = Icon,
                ContextMenuStrip = trayMenu,
                Visible = true
            };
            trayIcon.DoubleClick += (s, e) => this.Show();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            ConfigManager.Set("port", textBoxPort.Text.Trim());
            ConfigManager.Set("autostart", checkBoxAutoStart.Checked ? "1" : "0");

            if (checkBoxAutoStart.Checked)
            {
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)
                    ?.SetValue("PrinterMiddleware", Application.ExecutablePath);
            }
            else
            {
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)
                    ?.DeleteValue("PrinterMiddleware", false);
            }

            RefreshNetworkDisplay();
            MessageBox.Show("设置已保存");
        }

        private async Task buttonPrintTest_ClickAsync(object sender, EventArgs e)
        {
            //string sumatraPath = @"Assets\SumatraPDF.exe";
            string pdfPath = @"G:\hansagt\Print-Shop\middleware\sumatra\sumatrapdfcache\05050.pdf";
            string printerName = "#2. Virtual PDF Printer_LB (Label) @ PC230-2";

            try
            {
                bool success = await PrintExecutor.PrintPdfAsync(pdfPath, printerName);
                if (success)
                {
                    MessageBox.Show("打印成功！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("打印失败，SumatraPDF 未能正常退出。", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打印出错：{ex.Message}", "异常",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonPrintTest_Click(object sender, EventArgs e)
        {
            _ = buttonPrintTest_ClickAsync(sender, e);
        }



        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    e.Cancel = true;
        //    this.Hide(); // 最小化到托盘
        //}


        private void OnClientConnected(string ip, string hostname)
        {
            string ip_content = $"[{ip}] {hostname}";
            if (listBoxConnectedClients.InvokeRequired)
            {
                listBoxConnectedClients.Invoke(new Action(() => AddClientIp(ip_content)));
            }
            else
            {                
                AddClientIp(ip_content);
            }
        }

        private void OnClientDisconnected(string ip, string hostname)
        {
            string ip_content = $"[{ip}] {hostname}";
            if (listBoxConnectedClients.InvokeRequired)
            {
                listBoxConnectedClients.Invoke(new Action(() => RemoveClientIp(ip_content)));
            }
            else
            {                
                RemoveClientIp(ip_content);
            }
        }

        private void AddClientIp(string ip)
        {
            if (clientIpSet.Add(ip)) // 避免重复添加
            {
                listBoxConnectedClients.Items.Add(ip);
            }
        }

        private void RemoveClientIp(string ip)
        {
            if (clientIpSet.Remove(ip))
            {
                listBoxConnectedClients.Items.Remove(ip);
            }
        }

    }
}
