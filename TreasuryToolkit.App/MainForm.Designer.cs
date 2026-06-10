namespace TreasuryToolkit.App
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
            PnlSideBar = new Panel();
            BtnPdfTool = new Button();
            BtnExcelTool = new Button();
            BtnAbout = new Button();
            PnlMainContent = new Panel();
            PnlSideBar.SuspendLayout();
            SuspendLayout();
            // 
            // PnlSideBar
            // 
            PnlSideBar.BackColor = SystemColors.ControlDarkDark;
            PnlSideBar.Controls.Add(BtnAbout);
            PnlSideBar.Controls.Add(BtnExcelTool);
            PnlSideBar.Controls.Add(BtnPdfTool);
            PnlSideBar.Dock = DockStyle.Left;
            PnlSideBar.Location = new Point(0, 0);
            PnlSideBar.Name = "PnlSideBar";
            PnlSideBar.Size = new Size(200, 603);
            PnlSideBar.TabIndex = 0;
            // 
            // BtnPdfTool
            // 
            BtnPdfTool.Dock = DockStyle.Top;
            BtnPdfTool.FlatAppearance.BorderSize = 0;
            BtnPdfTool.FlatStyle = FlatStyle.Flat;
            BtnPdfTool.Location = new Point(0, 0);
            BtnPdfTool.Name = "BtnPdfTool";
            BtnPdfTool.Size = new Size(200, 29);
            BtnPdfTool.TabIndex = 0;
            BtnPdfTool.Text = "Renombrador de PDFs";
            BtnPdfTool.UseVisualStyleBackColor = true;
            // 
            // BtnExcelTool
            // 
            BtnExcelTool.BackgroundImageLayout = ImageLayout.None;
            BtnExcelTool.Dock = DockStyle.Top;
            BtnExcelTool.FlatAppearance.BorderSize = 0;
            BtnExcelTool.FlatStyle = FlatStyle.Flat;
            BtnExcelTool.Location = new Point(0, 29);
            BtnExcelTool.Name = "BtnExcelTool";
            BtnExcelTool.Size = new Size(200, 23);
            BtnExcelTool.TabIndex = 1;
            BtnExcelTool.Text = "Excel";
            BtnExcelTool.UseVisualStyleBackColor = true;
            // 
            // BtnAbout
            // 
            BtnAbout.Dock = DockStyle.Top;
            BtnAbout.FlatAppearance.BorderSize = 0;
            BtnAbout.FlatStyle = FlatStyle.Flat;
            BtnAbout.Location = new Point(0, 52);
            BtnAbout.Name = "BtnAbout";
            BtnAbout.Size = new Size(200, 23);
            BtnAbout.TabIndex = 2;
            BtnAbout.Text = "Acerca De";
            BtnAbout.UseVisualStyleBackColor = true;
            // 
            // PnlMainContent
            // 
            PnlMainContent.Dock = DockStyle.Fill;
            PnlMainContent.Location = new Point(200, 0);
            PnlMainContent.Name = "PnlMainContent";
            PnlMainContent.Size = new Size(999, 603);
            PnlMainContent.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1199, 603);
            Controls.Add(PnlMainContent);
            Controls.Add(PnlSideBar);
            ForeColor = Color.White;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tesoreria";
            PnlSideBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel PnlSideBar;
        private Button BtnExcelTool;
        private Button BtnPdfTool;
        private Button BtnAbout;
        private Panel PnlMainContent;
    }
}