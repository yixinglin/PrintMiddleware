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
            this.listBoxPrinters = new System.Windows.Forms.ListBox();
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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(245, 157);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(126, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save Settings";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(76, 158);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(110, 21);
            this.textBoxPort.TabIndex = 1;
            // 
            // listBoxPrinters
            // 
            this.listBoxPrinters.FormattingEnabled = true;
            this.listBoxPrinters.ItemHeight = 12;
            this.listBoxPrinters.Location = new System.Drawing.Point(28, 65);
            this.listBoxPrinters.Name = "listBoxPrinters";
            this.listBoxPrinters.Size = new System.Drawing.Size(340, 76);
            this.listBoxPrinters.TabIndex = 2;
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(245, 195);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(126, 16);
            this.checkBoxAutoStart.TabIndex = 3;
            this.checkBoxAutoStart.Text = "Launch at startup";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "#1: Printer - A4 (210 × 297)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "#2: Printer - A6 (105 × 148)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 241);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(191, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "#3: Printer - Fncode (80 × 40)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 264);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(191, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "#4: Printer - Tcode (102 × 35)";
            // 
            // buttonPrintTest
            // 
            this.buttonPrintTest.Location = new System.Drawing.Point(394, 158);
            this.buttonPrintTest.Name = "buttonPrintTest";
            this.buttonPrintTest.Size = new System.Drawing.Size(159, 23);
            this.buttonPrintTest.TabIndex = 9;
            this.buttonPrintTest.Text = "Test Print";
            this.buttonPrintTest.UseVisualStyleBackColor = true;
            this.buttonPrintTest.Click += new System.EventHandler(this.buttonPrintTest_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 292);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "BG Messages";
            // 
            // richTextBoxBackgroundMessages
            // 
            this.richTextBoxBackgroundMessages.Location = new System.Drawing.Point(34, 316);
            this.richTextBoxBackgroundMessages.Name = "richTextBoxBackgroundMessages";
            this.richTextBoxBackgroundMessages.ReadOnly = true;
            this.richTextBoxBackgroundMessages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxBackgroundMessages.Size = new System.Drawing.Size(337, 88);
            this.richTextBoxBackgroundMessages.TabIndex = 12;
            this.richTextBoxBackgroundMessages.Text = "";
            this.richTextBoxBackgroundMessages.WordWrap = false;
            // 
            // textBoxAllWsAddresses
            // 
            this.textBoxAllWsAddresses.Location = new System.Drawing.Point(394, 65);
            this.textBoxAllWsAddresses.Multiline = true;
            this.textBoxAllWsAddresses.Name = "textBoxAllWsAddresses";
            this.textBoxAllWsAddresses.ReadOnly = true;
            this.textBoxAllWsAddresses.Size = new System.Drawing.Size(159, 76);
            this.textBoxAllWsAddresses.TabIndex = 14;
            this.textBoxAllWsAddresses.WordWrap = false;
            // 
            // listBoxConnectedClients
            // 
            this.listBoxConnectedClients.FormattingEnabled = true;
            this.listBoxConnectedClients.ItemHeight = 12;
            this.listBoxConnectedClients.Location = new System.Drawing.Point(394, 316);
            this.listBoxConnectedClients.Name = "listBoxConnectedClients";
            this.listBoxConnectedClients.Size = new System.Drawing.Size(159, 88);
            this.listBoxConnectedClients.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(394, 292);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "Clients";
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
            this.label9.Location = new System.Drawing.Point(394, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 18;
            this.label9.Text = "Websockets";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 461);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
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
            this.Controls.Add(this.listBoxPrinters);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.buttonSave);
            this.Name = "MainForm";
            this.Text = "Print";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.ListBox listBoxPrinters;
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
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
    }
}