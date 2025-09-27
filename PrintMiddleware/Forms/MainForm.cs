using Microsoft.Win32;
using PrintMiddleware.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintMiddleware.Forms
{
    public partial class MainForm : Form
    {

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        public MainForm()
        {
            InitializeComponent();
            InitTrayIcon();
            LoadPrinters(); 
        }

        private void LoadPrinters()
        {
            listBoxPrinters.Items.Clear();
            var printers = new List<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (printer.StartsWith("#"))
                {
                    printers.Add(printer);
                }
            }
            printers.Sort(StringComparer.CurrentCultureIgnoreCase);
            listBoxPrinters.Items.AddRange(printers.ToArray());
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            textBoxPort.Text = ConfigManager.Get("port") ?? "5888";
            checkBoxAutoStart.Checked = ConfigManager.Get("autostart")  == "1"; 
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
            ConfigManager.Set("autostart", checkBoxAutoStart.Checked ? "1": "0");

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
            MessageBox.Show("设置已保存");                    
        }

        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    e.Cancel = true;
        //    this.Hide(); // 最小化到托盘
        //}
    }
}
