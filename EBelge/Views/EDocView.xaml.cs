using EBelge.ViewModels;
using EInvoiceWS;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls.Navigation;

namespace EBelge.Views
{
    /// <summary>
    /// EDocView.xaml etkileşim mantığı
    /// </summary>
    public partial class EDocView : UserControl
    {
        
        public EDocView()
        {
            InitializeComponent();
        }
        
        private void Kabul_Click(object sender, RoutedEventArgs e)
        {
            void KabulOnClosed(object sender, WindowClosedEventArgs e)
                {
                    var result = e.DialogResult;
                    if (result == true)
                    {
                        List<INVOICE> listInvoice = new List<INVOICE>();

                        for (int i = 0; i < GridView.SelectedItems.Count; i++)
                        {
                            Veriler data = GridView.SelectedItems[i] as Veriler;
                            INVOICE inv = new INVOICE();
                            inv.ID = data.Id;
                            listInvoice.Add(inv);
                        }

                        EInvoiceWSPortClient eInvoiceWSPortClient = new EInvoiceWSPortClient();
                        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EBelge");

                        SendInvoiceResponseWithServerSignRequest req = new SendInvoiceResponseWithServerSignRequest()
                        {
                            REQUEST_HEADER = new REQUEST_HEADERType() { SESSION_ID = key.GetValue("SessionId").ToString() },
                            STATUS = "KABUL",
                            INVOICE = listInvoice.ToArray()
                        };

                        SendInvoiceResponseWithServerSignResponse res = new SendInvoiceResponseWithServerSignResponse();
                        res = eInvoiceWSPortClient.SendInvoiceResponseWithServerSign(req);

                        if (res.ERROR_TYPE != null)
                            RadWindow.Alert(res.ERROR_TYPE.ERROR_SHORT_DES);
                    }
                }

            RadWindow.Confirm("Onaylıyor musunuz?", KabulOnClosed);
        }

        private string message;
        private void PrmtOnClosed(object sender, WindowClosedEventArgs e)
        {
            var result = e.PromptResult;
            message = result;
        }
        
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            void RedOnClosed(object sender, WindowClosedEventArgs e)
            {
                var result = e.DialogResult;
                if (result == true)
                {
                    List<INVOICE> listInvoice = new List<INVOICE>();

                    for (int i = 0; i < GridView.SelectedItems.Count; i++)
                    {
                        Veriler data = GridView.SelectedItems[i] as Veriler;
                        INVOICE inv = new INVOICE();
                        inv.ID = data.Id;
                        listInvoice.Add(inv);
                    }

                    RadWindow.Prompt("Red Nedeniniz:", this.PrmtOnClosed);
                    string[] str = new string[] { message };

                    EInvoiceWSPortClient eInvoiceWSPortClient = new EInvoiceWSPortClient();
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EBelge");


                    SendInvoiceResponseWithServerSignRequest req = new SendInvoiceResponseWithServerSignRequest()
                    {
                        REQUEST_HEADER = new REQUEST_HEADERType() { SESSION_ID = key.GetValue("SessionId").ToString() },
                        STATUS = "RED",
                        INVOICE = listInvoice.ToArray(),
                        DESCRIPTION = str
                    };

                    SendInvoiceResponseWithServerSignResponse res = new SendInvoiceResponseWithServerSignResponse();
                    res = eInvoiceWSPortClient.SendInvoiceResponseWithServerSign(req);

                    if (res.ERROR_TYPE != null)
                        RadWindow.Alert(res.ERROR_TYPE.ERROR_SHORT_DES);
                }
            }

            RadWindow.Confirm("Onaylıyor musunuz?", RedOnClosed);

        }

        private void Okundu_Click(object sender, RoutedEventArgs e)
        {
            void OkunduOnClosed(object sender, WindowClosedEventArgs e)
            {
                var result = e.DialogResult;
                if (result == true)
                {
                    List<INVOICE> listInvoice = new List<INVOICE>();

                    for (int i = 0; i < GridView.SelectedItems.Count; i++)
                    {
                        Veriler data = GridView.SelectedItems[i] as Veriler;
                        INVOICE inv = new INVOICE();
                        inv.ID = data.Id;
                        listInvoice.Add(inv);
                    }

                    EInvoiceWSPortClient eInvoiceWSPortClient = new EInvoiceWSPortClient();
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EBelge");


                    MarkInvoiceRequest req = new MarkInvoiceRequest()
                    {
                        REQUEST_HEADER = new REQUEST_HEADERType() { SESSION_ID = key.GetValue("SessionId").ToString() },
                        MARK = new MarkInvoiceRequestMARK()
                        {
                            value = MarkInvoiceRequestMARKValue.READ,
                            INVOICE = listInvoice.ToArray()
                        }
                    };

                    MarkInvoiceResponse res = new MarkInvoiceResponse();
                    res = eInvoiceWSPortClient.MarkInvoice(req);

                    if (res.REQUEST_RETURN.RETURN_CODE != 0)
                        RadWindow.Alert(res.ERROR_TYPE.ERROR_SHORT_DES);
                }
            }

            RadWindow.Confirm("Onaylıyor musunuz?", OkunduOnClosed);
        }
    }
}
