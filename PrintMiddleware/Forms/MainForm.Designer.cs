namespace PrintMiddleware.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonPrintTest = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.richTextBoxBackgroundMessages = new System.Windows.Forms.RichTextBox();
            this.textBoxAllWsAddresses = new System.Windows.Forms.TextBox();
            this.listBoxConnectedClients = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.listViewPrinters = new System.Windows.Forms.ListView();
            this.columnHeaderPrinterName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPapersize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(456, 267);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(184, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save Settings";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(505, 235);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(135, 21);
            this.textBoxPort.TabIndex = 1;
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(456, 309);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(126, 16);
            this.checkBoxAutoStart.TabIndex = 3;
            this.checkBoxAutoStart.Text = "Launch at startup";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(458, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 244);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "#1: Printer - A4 (210 × 297 mm)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "#2: Printer - A6 (105 × 148 mm)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 290);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(209, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "#3: Printer - Fncode (80 × 40 mm)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 313);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(209, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "#4: Printer - Tcode (102 × 35 mm)";
            // 
            // buttonPrintTest
            // 
            this.buttonPrintTest.Location = new System.Drawing.Point(456, 438);
            this.buttonPrintTest.Name = "buttonPrintTest";
            this.buttonPrintTest.Size = new System.Drawing.Size(184, 23);
            this.buttonPrintTest.TabIndex = 9;
            this.buttonPrintTest.Text = "Test Print";
            this.buttonPrintTest.UseVisualStyleBackColor = true;
            this.buttonPrintTest.Click += new System.EventHandler(this.buttonPrintTest_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 349);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "BG Messages";
            // 
            // richTextBoxBackgroundMessages
            // 
            this.richTextBoxBackgroundMessages.Location = new System.Drawing.Point(28, 373);
            this.richTextBoxBackgroundMessages.Name = "richTextBoxBackgroundMessages";
            this.richTextBoxBackgroundMessages.ReadOnly = true;
            this.richTextBoxBackgroundMessages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxBackgroundMessages.Size = new System.Drawing.Size(404, 88);
            this.richTextBoxBackgroundMessages.TabIndex = 12;
            this.richTextBoxBackgroundMessages.Text = "";
            this.richTextBoxBackgroundMessages.WordWrap = false;
            // 
            // textBoxAllWsAddresses
            // 
            this.textBoxAllWsAddresses.Location = new System.Drawing.Point(456, 65);
            this.textBoxAllWsAddresses.Multiline = true;
            this.textBoxAllWsAddresses.Name = "textBoxAllWsAddresses";
            this.textBoxAllWsAddresses.ReadOnly = true;
            this.textBoxAllWsAddresses.Size = new System.Drawing.Size(184, 54);
            this.textBoxAllWsAddresses.TabIndex = 14;
            this.textBoxAllWsAddresses.WordWrap = false;
            // 
            // listBoxConnectedClients
            // 
            this.listBoxConnectedClients.FormattingEnabled = true;
            this.listBoxConnectedClients.ItemHeight = 12;
            this.listBoxConnectedClients.Location = new System.Drawing.Point(456, 157);
            this.listBoxConnectedClients.Name = "listBoxConnectedClients";
            this.listBoxConnectedClients.Size = new System.Drawing.Size(184, 64);
            this.listBoxConnectedClients.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "Printers";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(454, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 18;
            this.label9.Text = "Websockets";
            // 
            // listViewPrinters
            // 
            this.listViewPrinters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPrinterName,
            this.columnHeaderPapersize});
            this.listViewPrinters.FullRowSelect = true;
            this.listViewPrinters.GridLines = true;
            this.listViewPrinters.HideSelection = false;
            this.listViewPrinters.Location = new System.Drawing.Point(24, 65);
            this.listViewPrinters.Name = "listViewPrinters";
            this.listViewPrinters.Size = new System.Drawing.Size(408, 157);
            this.listViewPrinters.TabIndex = 19;
            this.listViewPrinters.UseCompatibleStateImageBehavior = false;
            this.listViewPrinters.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderPrinterName
            // 
            this.columnHeaderPrinterName.Text = "Printer Name";
            this.columnHeaderPrinterName.Width = 250;
            // 
            // columnHeaderPapersize
            // 
            this.columnHeaderPapersize.Text = "Papersize";
            this.columnHeaderPapersize.Width = 150;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(454, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "Clients";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 517);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.listViewPrinters);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.listBoxConnectedClients);
            this.Controls.Add(this.textBoxAllWsAddresses);
            this.Controls.Add(this.richTextBoxBackgroundMessages);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonPrintTest);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxAutoStart);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.buttonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Print";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.CheckBox checkBoxAutoStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonPrintTest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox richTextBoxBackgroundMessages;
        private System.Windows.Forms.TextBox textBoxAllWsAddresses;
        private System.Windows.Forms.ListBox listBoxConnectedClients;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListView listViewPrinters;
        private System.Windows.Forms.ColumnHeader columnHeaderPrinterName;
        private System.Windows.Forms.ColumnHeader columnHeaderPapersize;
        private System.Windows.Forms.Label label8;
    }
}