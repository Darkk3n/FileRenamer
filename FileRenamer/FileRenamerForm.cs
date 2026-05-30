using System.Text;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace FileRenamer
{
    public partial class FileRenamerForm : Form
    {
        readonly string formattedDate = DateTime.Now.Date.ToString("yyyyMMdd");
        [System.Runtime.InteropServices.DllImport("shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
        private static extern int StrCmpLogicalW(string psz1, string psz2);
#pragma warning restore SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

        public FileRenamerForm()
        {
            InitializeComponent();
        }

        private void FileRenamerForm_Load(object sender, EventArgs e)
        {
            CmbCompany.SelectedIndex = 0;
            SetupGrid();
        }

        private void SetupGrid()
        {
            DgvPayments.Columns[0].FillWeight = 75;
            DgvPayments.Columns[1].FillWeight = 300;
            DgvPayments.Columns[1].FillWeight = 300;
            DgvPayments.Rows.Clear();
            DgvPayments.Rows.Add(formattedDate);
            DgvPayments.Columns[2].DefaultCellStyle.Format = "N2";
        }

        private void BtnClean_Click(object sender, EventArgs e)
        {
            DgvPayments.Rows.Clear();
            DgvPayments.Rows.Add(formattedDate);
            LblFolder.Text = "...";
        }

        private void DgvPayments_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e) => e.Row.Cells[0].Value = formattedDate;

        private void DgvPayments_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvPayments.Columns[e.ColumnIndex].Name == "dgvPaymentsColAmount")
            {
                var cell = DgvPayments.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // 3. If they typed text, try to convert it to a real number (double)
                if (cell.Value != null && double.TryParse(cell.Value.ToString(), out double numericValue))
                {
                    // By saving it back as a double instead of text, 
                    // the "N2" format rule is instantly triggered!
                    cell.Value = numericValue;
                    cell.Style.Format = "N2";
                }
            }
        }

        private void DgvPayments_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var row = DgvPayments.Rows[e.RowIndex];
            if (DgvPayments.Columns[e.ColumnIndex].Name == "dgvPaymentsColAmount")
            {
                if (DgvPayments.Rows[e.RowIndex].IsNewRow || string.IsNullOrEmpty(e.FormattedValue.ToString()))
                    return;

                var input = e.FormattedValue.ToString();
                if (!double.TryParse(input, out double result))
                {
                    row.ErrorText = "Monto: Solo se permiten numeros.";
                    e.Cancel = true;
                }
                else
                {
                    row.ErrorText = string.Empty;
                }
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (CmbCompany.SelectedIndex == 0)
            {
                MessageBox.Show("Seleccione una empresa para continuar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(LblFolder.Text))
            {
                MessageBox.Show("Seleccione una carpeta para continuar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dialogResult = MessageBox.Show("¿Desea renombrar los archivos? Esta accion es irreversible", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                var files = Directory.GetFiles(LblFolder.Text, "*.pdf").ToList();
                files.Sort((x, y) => StrCmpLogicalW(x, y)); // Enforces 1, 2, 3, 10 order

                int fileIndex = 0;
                foreach (DataGridViewRow row in DgvPayments.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (fileIndex >= files.Count)
                    {
                        MessageBox.Show("Advertencia: La cantidad de filas es mayor a la cantidad de archivos en el folder seleccionado", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }

                    string currentFilePath = files[fileIndex];

                    var datePart = row.Cells[0].Value?.ToString();
                    var vendorPart = row.Cells[2].Value?.ToString();
                    var conceptPart = row.Cells[3].Value?.ToString();
                    var amountPart = row.Cells[4].FormattedValue?.ToString();
                    var currencyPart = row.Cells[5].Value?.ToString();

                    string directory = Path.GetDirectoryName(currentFilePath);
                    string newFileName = $"{datePart}-{CmbCompany.SelectedItem}-{vendorPart} {conceptPart}-{amountPart} {currencyPart}.pdf";
                    string destinationPath = Path.Combine(directory, newFileName);

                    try
                    {
                        File.Move(currentFilePath, destinationPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to rename file {Path.GetFileName(currentFilePath)}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    fileIndex++;
                }

                MessageBox.Show("Proceso Completado", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnFileDialog_Click(object sender, EventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Seleccione una carpeta";
            folderDialog.UseDescriptionForTitle = true;
            folderDialog.ShowNewFolderButton = false; // Prevents them making a mess

            //TODO: Revert this
            //if (folderDialog.ShowDialog() == DialogResult.OK)
            //{
            DgvPayments.Rows.Clear();
            var files = Directory.GetFiles(@"C:\Users\455198\Downloads\20260529-WOREGG-", "*.pdf").ToList();
            files.Sort((x, y) => StrCmpLogicalW(x, y));

            ProgressForm loadingScreen = new();
            // Force the start position to manual so it obeys our custom coordinates
            loadingScreen.StartPosition = FormStartPosition.Manual;

            // 2. MANUAL MATH: Calculate the exact dead-center coordinates of the main form
            int centerX = this.Location.X + (this.Width - loadingScreen.Width) / 2;
            int centerY = this.Location.Y + (this.Height - loadingScreen.Height) / 2;

            // Assign the calculated point to the loading screen
            loadingScreen.Location = new Point(centerX, centerY);

            // 3. Display the form smoothly
            loadingScreen.Show(this);

            int currentFileIndex = 1;

            foreach (string filePath in files)
            {
                loadingScreen.Controls["lblStatus"].Text = $"Procesando PDF {currentFileIndex} de {files.Count}...";
                string fileNameOnly = Path.GetFileName(filePath);

                // Extract the raw text layout using the method we just built
                string rawPdfText = ExtractTextFromPdf(filePath);

                // Skip processing if the file was empty or unreadable
                if (string.IsNullOrWhiteSpace(rawPdfText)) continue;

                ExtractPdfDataPoints(rawPdfText, out string extractedAmount, out string extractedVendorName, out string extractedReason, out string extractedCurrency);
                DgvPayments.Rows.Add(DateTime.Now.ToString("yyyyMMdd"), fileNameOnly, extractedVendorName, extractedReason, extractedAmount, extractedCurrency);
                currentFileIndex++;
                Application.DoEvents();
            }
            DgvPayments.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            loadingScreen.Close();
            loadingScreen.Dispose();
            MessageBox.Show($"Se escanearon y cargaron {files.Count} archivos en la tabla!", "Escaneo Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //TODO: Revert this line
            //LblFolder.Text = folderDialog.SelectedPath;
            LblFolder.Text = @"C:\Users\455198\Downloads\20260529-WOREGG-";
            //}
        }

        private static string ExtractTextFromPdf(string filePath)
        {
            // Double check that the file actually exists before trying to read it
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The system could not find the file at: {filePath}");
            }

            var pdfTextContent = new StringBuilder();

            try
            {
                // 1. Initialize the iText readers
                using var reader = new PdfReader(filePath);
                using var pdfDoc = new PdfDocument(reader);
                // 2. Loop through every page (iText pages are 1-indexed, not 0-indexed!)
                for (int pageNum = 1; pageNum <= pdfDoc.GetNumberOfPages(); pageNum++)
                {
                    // Extract the text layout from the current page
                    string pageText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(pageNum));

                    // Append it to our main data block with a newline breakdown
                    pdfTextContent.AppendLine(pageText);
                }
            }
            catch (Exception ex)
            {
                // Catch password-protected PDFs or corrupted files gracefully
                MessageBox.Show($"Error al intentar leer archivo en ruta: {Path.GetFileName(filePath)}: {ex.Message}",
                                "Error de lectura de PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            return pdfTextContent.ToString();
        }

        private static string CleanseAndHealText(string rawText)
        {
            if (string.IsNullOrEmpty(rawText)) return string.Empty;

            // 1. Replace any literal tabs, carriage returns, or newlines with a single space
            string cleaned = Regex.Replace(rawText, @"[\r\n\t]+", " ");

            // 2. Fix inner-word splitting issues (e.g., " SERVICI OS" or "QUERETA RO")
            // This looks for an isolated trailing part of a word and welds it back to its core.
            cleaned = Regex.Replace(cleaned, @"([A-Za-z]+)\s([A-Za-z]{1,2}\b)", "$1$2");

            // 3. Fix words split by massive gaps (e.g., "LOGISTI   CO")
            cleaned = Regex.Replace(cleaned, @"([A-Za-z]+)\s{2,}([A-Za-z]+)", "$1$2");

            // 4. Collapse any remaining multi-spaces down to a single standard space and trim edges
            cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();

            return cleaned;
        }

        private static void ExtractPdfDataPoints(string rawPdfText, out string amount, out string vendorName, out string reason, out string currency)
        {
            amount = string.Empty;
            vendorName = string.Empty;
            reason = string.Empty;
            currency = string.Empty;

            // --- 1. REAL AMOUNT EXTRACTION (Importe) ---
            // This pattern captures "Importe:", any spaces, and numbers formatted like 12,345.00
            // The \. ensures it looks for a literal period right before the cents
            string amountPattern = @"Importe:\s*([0-9.,]+\.[0-9]{2})";

            Match amountMatch = Regex.Match(rawPdfText, amountPattern, RegexOptions.IgnoreCase);
            if (amountMatch.Success)
            {
                amount = amountMatch.Groups[1].Value; // Captures "12,345.00"
            }

            // 2. REASON EXTRACTION (The Confident Greedy Fix)
            // Changing +? to + forces it to grab the whole phrase on that line.
            // The lookahead ensures that if "Referencia" is present, it stops right before it.
            #region Vendor
            Match reasonMatch = Regex.Match(rawPdfText, @"(?:Motivo|Concepto)\s+de\s+pago:\s*([^\r\n]+)", RegexOptions.IgnoreCase);

            if (reasonMatch.Success)
            {
                string rawReason = reasonMatch.Groups[1].Value;

                // Step 2: Drop the blade if the word "Referencia" exists on that line, 
                // throwing away the reference numbers and keeping only the reason text.
                string cleanReason = Regex.Split(rawReason, @"Referencia", RegexOptions.IgnoreCase)[0];

                // Step 3: Run it through the healer to fix spacing or layout bugs
                reason = CleanseAndHealText(cleanReason);
            }

            // 3. VENDOR NAME EXTRACTION (Tuned with STOP boundary and fallback)
            // Captures everything after the label but STOPS looking if it hits numbers, colons, or "Dato no verificado"
            string structuralPattern = @"Titular\s+de\s+la\s+cuenta:.*?Titular\s+de\s+la\s+cuenta:\s*([^\r\n:]+)";

            Match structuralMatch = Regex.Match(rawPdfText, structuralPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (structuralMatch.Success)
            {
                string rawVendorName = structuralMatch.Groups[1].Value;

                // THE CLEANER: We use a generalized boundary pattern at the end.
                // This catches variations of SA, CV, S, DE, RL, and SC dynamically.
                string cleanVendorName = Regex.Split(rawVendorName,
                    @"(?:RFC|Banco|Monto|Importe|Motivo|Concepto|Dato\s*no\s*verificado" +
                    @"|\bMEXICOS\s*DERL\b" +                 // Explicitly targets your exact problem string
                    @"|\bS\s*DE?\s*R?L?\s*DE?\s*C?V?\b" +    // Dynamic catcher for S DE RL DE CV fragments
                    @"|\bSA\s*DE\s*C?V?\b" +                 // Dynamic catcher for SA DE CV fragments
                    @"|\bS\s*DE?R?L?\b" +                    // Catches "S DERL" or "S DE RL"
                    @"|\bSC\b)",                             // Isolated SC boundary
                    RegexOptions.IgnoreCase)[0];

                vendorName = CleanseAndHealText(cleanVendorName);
            }
            else
            {
                // Fallback: Single match structural processing
                string fallbackPattern = @"Titular\s+de\s+la\s+cuenta:\s*([^\r\n:]+)";
                Match fallbackMatch = Regex.Match(rawPdfText, fallbackPattern, RegexOptions.IgnoreCase);

                if (fallbackMatch.Success)
                {
                    string singleMatch = fallbackMatch.Groups[1].Value;
                    singleMatch = Regex.Split(singleMatch,
                        @"(?:RFC|Banco|Monto|Importe|Motivo|Concepto|Dato\s*no\s*verificado" +
                        @"|\bMEXICOS\s*DERL\b" +
                        @"|\bS\s*DE?\s*R?L?\s*DE?\s*C?V?\b" +
                        @"|\bSA\s*DE\s*C?V?\b" +
                        @"|\bS\s*DE?R?L?\b" +
                        @"|\bSC\b)",
                        RegexOptions.IgnoreCase)[0];

                    vendorName = CleanseAndHealText(singleMatch);
                }
            }
            #endregion

            // --- 4. CURRENCY EXTRACTION (The Grand Finale) ---
            // Structural Sweep: Skip the 1st "Divisa", capture everything on the line of the 2nd "Divisa"
            #region Currency
            string currencyPattern = @"Divisa\s+de\s+la\s+cuenta:.*?Divisa\s+de\s+la\s+cuenta:\s*([^\r\n:]+)";
            Match currencyMatch = Regex.Match(rawPdfText, currencyPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (currencyMatch.Success)
            {
                string rawCurrency = currencyMatch.Groups[1].Value;

                // Chop off any adjacent headers if the layout merges text lines
                string cleanCurrency = Regex.Split(rawCurrency, @"(?:Importe|Titular|RFC|Banco|Motivo|$)", RegexOptions.IgnoreCase)[0];

                currency = CleanseAndHealText(cleanCurrency); // Will return "MXN", "USD", etc.
                if (currency == "MXP") { currency = "MXN"; }
            }
            else
            {
                // Fallback: If a PDF layout only has one single Currency label on the whole page
                Match singleCurrencyMatch = Regex.Match(rawPdfText, @"Divisa\s+de\s+la\s+cuenta:\s*([^\r\n:]+)", RegexOptions.IgnoreCase);
                if (singleCurrencyMatch.Success)
                {
                    string singleCurrency = Regex.Split(singleCurrencyMatch.Groups[1].Value, @"(?:Importe|Titular|RFC|Banco|Motivo|$)", RegexOptions.IgnoreCase)[0];
                    currency = CleanseAndHealText(singleCurrency);
                    if (currency == "MXP") { currency = "MXN"; }
                }
            }
            #endregion
        }
    }
}