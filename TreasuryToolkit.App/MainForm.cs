using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TreasuryToolkit.Core.Contracts;

namespace TreasuryToolkit.App
{
    public partial class MainForm : Form
    {
        private UcFileRenamer _fileRenamer;
        private readonly Func<ProgressForm> _progressFormFactory;
        private readonly IPdfProcessor pdfProcessor;
        private readonly IFileScanner fileScanner;

        public MainForm()
        {
            InitializeComponent();
            InitViews();
            ShowView(_fileRenamer);
        }

        private void ShowView(UserControl view)
        {
            PnlMainContent.SuspendLayout();
            PnlMainContent.Controls.Clear();
            PnlMainContent.Controls.Add(view);
            PnlMainContent.ResumeLayout();
        }

        private void InitViews()
        {
            _fileRenamer = new UcFileRenamer(_progressFormFactory, pdfProcessor, fileScanner)
            {
                Dock = DockStyle.Fill
            };
        }
    }
}
