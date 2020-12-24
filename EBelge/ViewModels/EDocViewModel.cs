using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using System.Windows;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Xsl;
using System.Windows.Controls;
using Telerik.Windows.Controls.Navigation;
using EInvoiceWS;
using Microsoft.Win32;
using System.IO.Compression;
using System.Drawing;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model;
using EBelge.Views;

namespace EBelge.ViewModels
{
	public class InvoiceResponseStyle : StyleSelector
	{
		public override Style SelectStyle(object item, DependencyObject container)
		{
			if (item is Veriler)
			{
				Veriler data = item as Veriler;
				if (data.Res_desc == "Fatura kabul edildi." && data.Profileid == "TICARIFATURA")
				{
					return Kabul;
				}
				else if (data.Res_desc != null && data.Profileid == "TICARIFATURA")
				{
					return Red;
				}
				else if (data.Res_desc == null && data.Profileid == "TICARIFATURA")
				{
					return Bekle;
				}
			}
			return null;
		}
		public Style Bekle { get; set; }
		public Style Kabul { get; set; }
		public Style Red { get; set; }
	}
	public class  Veriler: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string sender;
		private string supplier;
		
		private decimal payable_amount;
		private string from;
		private string profileid;
		private string invoice_type_code;
		private string res_desc;
		private DateTime cdate;
		private string id;

		public Veriler()
        {

		}

        public string Sender
		{
            get { return this.sender; }
            set
            {
                if (value != this.sender)
                {
                    this.sender = value;
                    this.OnPropertyChanged("Sender");
                }
            }
        }
		public string Supplier
		{
			get { return this.supplier; }
			set
			{
				if (value != this.supplier)
				{
					this.supplier = value;
					this.OnPropertyChanged("Supplier");
				}
			}
		}
		
		public decimal Payable_amount
		{
			get { return this.payable_amount; }
			set
			{
				if (value != this.payable_amount)
				{
					this.payable_amount = value;
					this.OnPropertyChanged("Payable_amount");
				}
			}
		}
		public string From
		{
			get { return this.from; }
			set
			{
				if (value != this.from)
				{
					this.from = value;
					this.OnPropertyChanged("From");
				}
			}
		}
		public string Profileid
		{
			get { return this.profileid; }
			set
			{
				if (value != this.profileid)
				{
					this.profileid = value;
					this.OnPropertyChanged("Profileid");
				}
			}
		}
		public string Invoice_type_code
		{
			get { return this.invoice_type_code; }
			set
			{
				if (value != this.invoice_type_code)
				{
					this.invoice_type_code = value;
					this.OnPropertyChanged("Invoice_type_code");
				}
			}
		}
		public string Res_desc
		{
			get { return this.res_desc; }
			set
			{
				if (value != this.res_desc)
				{
					this.res_desc = value;
					this.OnPropertyChanged("Res_desc");
				}
			}
		}
		public string Id
		{
			get { return this.id; }
			set
			{
				if (value != this.id)
				{
					this.id = value;
					this.OnPropertyChanged("Id");
				}
			}
		}
		public DateTime Cdate
		{
			get { return this.cdate; }
			set
			{
				if (value != this.cdate)
				{
					this.cdate = value;
					this.OnPropertyChanged("Cdate");
				}
			}
		}
		

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
	public class EDocViewModel : ViewModelBase
	{
		private bool isBusy;
		private string busyContent;
		private BackgroundWorker backgroundWorker = new BackgroundWorker();
		
		private object selectedItem;
		public object SelectedItem
		{
			get { return this.selectedItem; }
			set
			{
				if (this.selectedItem != value)
				{
					this.selectedItem = value;
					this.OnPropertyChanged(() => this.SelectedItem);
				}
			}
		}

		private DateTime startD;
		private DateTime endD;
		private string faturaNo;
		private string kimden;
		private int rank = 1000;

		public int Rank
		{
			get { return this.rank; }
			set
			{
				if (this.rank != value)
				{
					this.rank = value;
					this.OnPropertyChanged(() => this.Rank);
				}
			}
		}
		public string Kimden
		{
			get { return this.kimden; }
			set
			{
				if (this.kimden != value)
				{
					this.kimden = value;
					this.OnPropertyChanged(() => this.Kimden);
				}
			}
		}
		public string FaturaNo
		{
			get { return this.faturaNo; }
			set
			{
				if (this.faturaNo != value)
				{
					this.faturaNo = value;
					this.OnPropertyChanged(() => this.FaturaNo);
				}
			}
		}
		public DateTime StartD
		{
			get { return this.startD; }
			set
			{
				if (this.startD != value)
				{
					this.startD = value;
					this.OnPropertyChanged(() => this.StartD);
				}
			}
		}
		public DateTime EndD
		{
			get { return this.endD; }
			set
			{
				if (this.endD != value)
				{
					this.endD = value;
					this.OnPropertyChanged(() => this.EndD);
				}
			}
		}

		public DelegateCommand EFaturaGelenCommand { get; }
		public void EFaturaGelenKutusu(object obj)
		{
			void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
			{
				EInvoiceWSPortClient eInvoiceWSPortClient = new EInvoiceWSPortClient();
				RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EBelge");
				EFaturaGKs.Clear();

				GetInvoiceRequest req = new GetInvoiceRequest()
				{
					REQUEST_HEADER = new REQUEST_HEADERType()
					{
						SESSION_ID = key.GetValue("SessionId").ToString(),

						COMPRESSED = "N"
					},
					INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY()
					{
						LIMIT = rank,
						LIMITSpecified = true,
						READ_INCLUDED = true,
						READ_INCLUDEDSpecified = true,
						DIRECTION = "IN",
						START_DATE = startD,
						START_DATESpecified = true,
						
						ID = faturaNo,
						FROM = kimden					
					},
					HEADER_ONLY = "Y"
				};
				GetInvoiceResponse res = new GetInvoiceResponse();
				res = eInvoiceWSPortClient.GetInvoice(req);

				if (res.ERROR_TYPE != null)
					MessageBox.Show(res.ERROR_TYPE.ERROR_SHORT_DES);
				else
				{
					for (int i = 0; i < res.INVOICE.Length; i++)
					{
						Veriler row = new Veriler() { Cdate = res.INVOICE[i].HEADER.CDATE, From = res.INVOICE[i].HEADER.FROM,
							Invoice_type_code = res.INVOICE[i].HEADER.INVOICE_TYPE_CODE, Payable_amount = res.INVOICE[i].HEADER.PAYABLE_AMOUNT.Value, Profileid = res.INVOICE[i].HEADER.PROFILEID,
							Res_desc = res.INVOICE[i].HEADER.RESPONSE_DESCRIPTION, Sender = res.INVOICE[i].HEADER.SENDER,
							Supplier = res.INVOICE[i].HEADER.SUPPLIER, Id = res.INVOICE[i].ID };

						eFaturaGKs.Add(row);

					}
				}

				Data = new ObservableCollection<Veriler>(EFaturaGKs);
				
				key.Close();
			}

			backgroundWorker.DoWork += OnBackgroundWorkerDoWork;
			backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;

			void OnBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
			{
				var backgroundWorker = sender as BackgroundWorker;
				backgroundWorker.DoWork -= OnBackgroundWorkerDoWork;
				backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompleted;

				InvokeOnUIThread(() =>
				{
					this.IsBusy = false;

				});
			}

			IsBusy = true;
		}

		public DelegateCommand EFaturaGidenCommand { get; }
		public void EFaturaGidenKutusu(object obj)
		{
			void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
			{
				EInvoiceWSPortClient eInvoiceWSPortClient = new EInvoiceWSPortClient();
				RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EBelge");
				EFaturaGKs.Clear();

				GetInvoiceRequest req = new GetInvoiceRequest()
				{
					REQUEST_HEADER = new REQUEST_HEADERType()
					{
						SESSION_ID = key.GetValue("SessionId").ToString(),

						COMPRESSED = "N"
					},
					INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY()
					{
						LIMIT = rank,
						LIMITSpecified = true,
						READ_INCLUDED = true,
						READ_INCLUDEDSpecified = true,
						DIRECTION = "OUT",
						START_DATE = startD,
						START_DATESpecified = true,
						
						ID = faturaNo,
						FROM = kimden
					},
					HEADER_ONLY = "Y"
				};
				GetInvoiceResponse res = new GetInvoiceResponse();
				res = eInvoiceWSPortClient.GetInvoice(req);

				if (res.ERROR_TYPE != null)
					MessageBox.Show(res.ERROR_TYPE.ERROR_SHORT_DES);
				else
				{
					for (int i = 0; i < res.INVOICE.Length; i++)
					{
						Veriler row = new Veriler()
						{
							Cdate = res.INVOICE[i].HEADER.CDATE,
							From = res.INVOICE[i].HEADER.FROM,
							Invoice_type_code = res.INVOICE[i].HEADER.INVOICE_TYPE_CODE,
							Payable_amount = res.INVOICE[i].HEADER.PAYABLE_AMOUNT.Value,
							Profileid = res.INVOICE[i].HEADER.PROFILEID,
							Res_desc = res.INVOICE[i].HEADER.RESPONSE_DESCRIPTION,
							Sender = res.INVOICE[i].HEADER.SENDER,
							Supplier = res.INVOICE[i].HEADER.SUPPLIER,
							Id = res.INVOICE[i].ID
						};

						eFaturaGKs.Add(row);

					}
				}

				Data = new ObservableCollection<Veriler>(EFaturaGKs);

				key.Close();
			}

			backgroundWorker.DoWork += OnBackgroundWorkerDoWork;
			backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;

			void OnBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
			{
				var backgroundWorker = sender as BackgroundWorker;
				backgroundWorker.DoWork -= OnBackgroundWorkerDoWork;
				backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompleted;

				InvokeOnUIThread(() =>
				{
					this.IsBusy = false;

				});
			}

			IsBusy = true;
		}

		
		

		public DelegateCommand GorselCommand { get; }
		public void GorselGetir(object obj)
		{
			string Id = null;
			void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
			{
				EInvoiceWSPortClient eInvoiceWSPortClient = new EInvoiceWSPortClient();
				RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EBelge");
				Veriler data = selectedItem as Veriler;

				GetInvoiceWithTypeRequest req = new GetInvoiceWithTypeRequest()
				{
					REQUEST_HEADER = new REQUEST_HEADERType() { SESSION_ID = key.GetValue("SessionId").ToString(), },
					INVOICE_SEARCH_KEY = new GetInvoiceWithTypeRequestINVOICE_SEARCH_KEY()
					{
						ID = data.Id,
						TYPE = "PDF",
						DIRECTION = "IN",
						READ_INCLUDED = true,
						READ_INCLUDEDSpecified = true
					},
					HEADER_ONLY = "N"
				};
				GetInvoiceWithTypeResponse res = new GetInvoiceWithTypeResponse();
				res = eInvoiceWSPortClient.GetInvoiceWithType(req);

				if (res.ERROR_TYPE != null)
					MessageBox.Show(res.ERROR_TYPE.ERROR_SHORT_DES);

				byte[] zipsizData = { };
				MemoryStream ms = new MemoryStream();
				MemoryStream zippedStream = new MemoryStream(res.INVOICE[0].CONTENT.Value);
				using (ZipArchive archive = new ZipArchive(zippedStream))
				{
					foreach (ZipArchiveEntry entry in archive.Entries)
					{
						ms = new MemoryStream();
						Stream zipStream = entry.Open();
						zipStream.CopyTo(ms);
						zipsizData = ms.ToArray();
					}

				}

				System.IO.File.WriteAllBytes($@"{AppDomain.CurrentDomain.BaseDirectory}\Faturalar\{res.INVOICE[0].ID}.pdf", zipsizData);
				Id = res.INVOICE[0].ID;
				key.Close();
			}

			backgroundWorker.DoWork += OnBackgroundWorkerDoWork;
			backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;

			void OnBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
			{
				var backgroundWorker = sender as BackgroundWorker;
				backgroundWorker.DoWork -= OnBackgroundWorkerDoWork;
				backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompleted;

				InvokeOnUIThread(() =>
				{
					this.IsBusy = false;
					
                    var window = new RadWindow()
					{
						Content = new PdfViewerView() { Path = $@"{AppDomain.CurrentDomain.BaseDirectory}\Faturalar\{Id}.pdf" },
						Width = 900,
						Height = 500,
						WindowStartupLocation = WindowStartupLocation.CenterScreen,
						Header = $@"{AppDomain.CurrentDomain.BaseDirectory}\Faturalar\{Id}.pdf"
					};
					RadWindowInteropHelper.SetAllowTransparency(window, false);
					window.Show();
				});
			}

			IsBusy = true;
		}

		public DelegateCommand FaturaOlusturCommand { get; }
		public void FaturaOlustur(object obj)
		{
			var window = new RadWindow
			{
				Content = new FaturaView(),
				Width = 800,
				Height = 500,
				WindowStartupLocation = WindowStartupLocation.CenterScreen,
				Background = Brushes.White,
				
			};
			RadWindowInteropHelper.SetAllowTransparency(window, false);
			window.Show();
		}

		
		

		public EDocViewModel()
        {
			this.EFaturaGelenCommand = new DelegateCommand(EFaturaGelenKutusu);
			this.EFaturaGidenCommand = new DelegateCommand(EFaturaGidenKutusu);
			this.GorselCommand = new DelegateCommand(GorselGetir);
			this.FaturaOlusturCommand = new DelegateCommand(FaturaOlustur);
			
			

			StartD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,00,00,00);
			EndD = DateTime.Now;		
		}

		public bool IsBusy
		{
			get { return this.isBusy; }
			set
			{
				if (this.isBusy != value)
				{
					this.isBusy = value;
					this.OnPropertyChanged(() => this.IsBusy);

					if (this.isBusy)
					{
						backgroundWorker.RunWorkerAsync();
					}
				}
			}
		}
		public string BusyContent
		{
			get { return this.busyContent; }
			set
			{
				if (this.busyContent != value)
				{
					this.busyContent = value;
					this.OnPropertyChanged(() => this.BusyContent);
				}
			}
		}

		private ObservableCollection<Veriler> eFaturaGKs = new ObservableCollection<Veriler>();	
		public ObservableCollection<Veriler> EFaturaGKs
		{
			get { return this.eFaturaGKs; }
			set
			{
				if (value != this.eFaturaGKs)
				{
					this.eFaturaGKs = value;
					this.OnPropertyChanged(() => this.EFaturaGKs);
				}
			}
		}

		private ObservableCollection<Veriler> data = new ObservableCollection<Veriler>();
		public ObservableCollection<Veriler> Data
		{
			get { return this.data; }
			set
			{
				if (value != this.data)
				{
					this.data = value;
					this.OnPropertyChanged(() => this.Data);
				}
			}
		}
	}
}
