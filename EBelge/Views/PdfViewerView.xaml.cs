using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model;

namespace EBelge.Views
{
    /// <summary>
    /// PdfViewerView.xaml etkileşim mantığı
    /// </summary>
    public partial class PdfViewerView : UserControl
    {

        public string Path;

        public PdfViewerView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string pdfFilePath = Path;
            MemoryStream stream = new MemoryStream();

            using (Stream input = File.OpenRead(pdfFilePath))
            {
                input.CopyTo(stream);
            }

            PdfFormatProvider provider = new PdfFormatProvider();
            provider.ImportSettings = PdfImportSettings.ReadOnDemand;
            RadFixedDocument doc = provider.Import(stream);
            pdfViewer.Document = doc;
            pdfTool.RadPdfViewer = pdfViewer;
        }
    }
}
