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
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Xsl;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.SyntaxEditor.Taggers;
using Telerik.Windows.SyntaxEditor.Core.Text;

namespace EBelge
{
    /// <summary>
    /// XsltEditorWin.xaml etkileşim mantığı
    /// </summary>
    public partial class XsltEditorWin : Window
    {
        private string XmlfileName;
        private string XsltfileName;
        public XsltEditorWin()
        {
            InitializeComponent();
        }

        private void RadButton_Click_1(object sender, RoutedEventArgs e)
        {
            XmlOpenFileDialog();
        }

        private void RadButton_Click_2(object sender, RoutedEventArgs e)
        {
            XsltOpenFileDialog();
        }

        private void RadButton_Click_3(object sender, RoutedEventArgs e)
        {
            OnIzle();
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            ShowSaveFileDialog();
        }
        private void OnIzle()
        {
            var xslTransform = new XslCompiledTransform();
            var strWriter = new StringWriter();
            var xReader = XmlReader.Create(XsltfileName, new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse });
            xslTransform.Load(xReader);
            xslTransform.Transform(XmlfileName, null, strWriter);
            WebBrowser.NavigateToString(strWriter.ToString());
        }

        private void XmlOpenFileDialog()
        {
            RadOpenFileDialog openFileDialog = new RadOpenFileDialog();
            openFileDialog.Owner = this;
            openFileDialog.Filter = "Xml Documents|*.xml";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            if (openFileDialog.DialogResult == true)
            {
                XmlfileName = openFileDialog.FileName;
                XmlTagger xmlTagger = new XmlTagger(this.XmlEditor);
                this.XmlEditor.TaggersRegistry.RegisterTagger(xmlTagger);
                using (StreamReader reader = new StreamReader(XmlfileName))
                {
                    this.XmlEditor.Document = new TextDocument(reader);
                }
            }
            xmlPane.Header = XmlfileName;
        }

        private void XsltOpenFileDialog()
        {
            RadOpenFileDialog openFileDialog = new RadOpenFileDialog();
            openFileDialog.Owner = this;
            openFileDialog.Filter = "Xml Documents|*.xslt";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            if (openFileDialog.DialogResult == true)
            {
                XsltfileName = openFileDialog.FileName;
                XmlTagger xmlTagger = new XmlTagger(this.XsltEditor);
                this.XsltEditor.TaggersRegistry.RegisterTagger(xmlTagger);
                using (StreamReader reader = new StreamReader(XsltfileName))
                {
                    this.XsltEditor.Document = new TextDocument(reader);
                }
            }
            OpenDocument.Header = XsltfileName;
        }

        private void ShowSaveFileDialog()
        {
            RadSaveFileDialog saveFileDialog = new RadSaveFileDialog();
            saveFileDialog.Owner = this;
            saveFileDialog.Filter = "Xml Documents|*.xslt";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.ShowDialog();
            if (saveFileDialog.DialogResult == true)
            {

                XsltEditor.SelectAll();
                string selectedText = XsltEditor.Selection.GetSelectedText();
                XsltfileName = saveFileDialog.FileName;
                if (!File.Exists(XsltfileName))
                {
                    File.Create(XsltfileName).Close();
                    using (StreamWriter sw = File.AppendText(XsltfileName))
                    {
                        sw.WriteLine(selectedText);
                    }
                }
                else
                {
                    File.WriteAllText(XsltfileName, string.Empty);
                    using (StreamWriter sw = File.AppendText(XsltfileName))
                    {
                        sw.WriteLine(selectedText);
                    }
                }
                OpenDocument.Header = XsltfileName;
            }       
        }

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (XsltfileName == "")
                ShowSaveFileDialog();
            else
            {
                XsltEditor.SelectAll();
                string selectedText = XsltEditor.Selection.GetSelectedText();
                File.WriteAllText(XsltfileName, string.Empty);
                using (StreamWriter sw = File.AppendText(XsltfileName))
                {
                    sw.WriteLine(selectedText);
                }
            }

            OpenDocument.Header = XsltfileName;
        }
        private void XsltEditor_DocumentContentChanged(object sender, TextContentChangedEventArgs e)
        {
            OpenDocument.Header = @$"{XsltfileName}*";
        }
    }
}
