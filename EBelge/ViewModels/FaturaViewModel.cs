using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Controls;
using System.Windows;
using Telerik.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Xsl;
using System.Windows.Controls;
using Telerik.Windows.Controls.Navigation;
using System.ComponentModel.DataAnnotations;
using EInvoiceWS;
using Microsoft.Win32;

namespace EBelge.ViewModels
{
    public class VergiTypeConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] 
            { 
                "KDV",
                "İskonto(İndirim)",
                "-----<< ÖTV >>-----",
                "ÖTV 1", "ÖTV 2", "ÖTV 3", "ÖTV 4", "ÖTV 3A", "ÖTV 3B", "ÖTV 3C",
                "-----<< Stopaj >>-----",
                "GV. Stopajı", "KV. Stopajı",
                "-----<< İletişim >>-----",
                "Damga Vergisi", "5035SK Damga", "5035 ÖİV", "Ö. İletişim",
                "-----<< Diğer >>-----",
                "Banka SM 4961", "Borsa Tes. Üc", "Enerji Fonu", "TRT Payı", "TK kullanım", "TK Ruhsat", "Çev. Tem. V", "Mera Fonu", "Belediye Tüketim V."
            });
        }
    }
    public class Vergi : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string vergiKod;

        [Display(Order = 0, Name = "Vergi Kodu")]
        public string VergiKod
        {
            get
            {
                return this.vergiKod;
            }
            set
            {
                if (this.vergiKod != value)
                {
                    this.vergiKod = value;
                    this.OnPropertyChanged("VergiKod");
                }
            }
        }
        
        public string vergiKAd;
        [TypeConverter(typeof(VergiTypeConverter))]
        [Display(Order = 2, Name = "Vergi Adı")]
        public string VergiKAd
        {
            get
            {
                return this.vergiKAd;               
            }
            set
            {
                if (this.vergiKAd != value)
                {
                    switch(value)
                    {
                        case "KDV":
                            VergiKod = "0015";
                            break;
                        case "İskonto(İndirim)":
                            VergiKod = null;
                            break;
                        case "ÖTV 1":
                            VergiKod = "0071";
                            break;
                        case "ÖTV 2":
                            VergiKod = "9077";
                            break;
                        case "ÖTV 3":
                            VergiKod = "0073";
                            break;
                        case "ÖTV 4":
                            VergiKod = "0074";
                            break;
                        case "ÖTV 3A":
                            VergiKod = "0075";
                            break;
                        case "ÖTV 3B":
                            VergiKod = "0076";
                            break;
                        case "ÖTV 3C":
                            VergiKod = "0077";
                            break;
                        case "GV. Stopajı":
                            VergiKod = "003";
                            break;
                        case "KV. Stopajı":
                            VergiKod = "0011";
                            break;
                        case "Damga Vergisi":
                            VergiKod = "1047";
                            break;
                        case "5035SK Damga":
                            VergiKod = "1048";
                            break;
                        case "5035 ÖİV":
                            VergiKod = "4081";
                            break;
                        case "Ö. İletişim":
                            VergiKod = "4080";
                            break;
                        case "Banka SM 4961":
                            VergiKod = "9021";
                            break;
                        case "Borsa Tes. Üc":
                            VergiKod = "8001";
                            break;
                        case "Enerji Fonu":
                            VergiKod = "8002";
                            break;
                        case "TRT Payı":
                            VergiKod = "8004";
                            break;
                        case "TK kullanım":
                            VergiKod = "8006";
                            break;
                        case "TK Ruhsat":
                            VergiKod = "8007";
                            break;
                        case "Çev. Tem. V":
                            VergiKod = "8008";
                            break;
                        case "Mera Fonu":
                            VergiKod = "9040";
                            break;
                        case "Belediye Tüketim V.":
                            VergiKod = "9944";
                            break;
                    }
                    this.vergiKAd = value;         
                    this.OnPropertyChanged("VergiKAd");            
                }
            }
        }

        private decimal vergiOran;

        [Display(Order = 3, Name = "Vergi Oranı")]
        public decimal VergiOran
        {
            get
            {
                return this.vergiOran;
            }
            set
            {
                if (this.vergiOran != value)
                {
                    this.vergiOran = value;
                    this.OnPropertyChanged("VergiOran");
                }
            }
        }


        private string baslik;
        
        public override string ToString()
        {
            return this.baslik;
        }

      
        private void PrmtOnClosed(object sender, WindowClosedEventArgs e)
        {
            var result = e.PromptResult;
            this.baslik = result;
        }
        public Vergi() {
            if(baslik == null)
                RadWindow.Prompt("Başlık:", this.PrmtOnClosed);
        }
       
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {            
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class ProductLine : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private string mal_Hizmet_Ad;
        private decimal miktar;
        private decimal birim_Fiyat;
        public virtual List<Vergi> Orders { get; set; }

        public ProductLine()
        {
            
        }

        public string Mal_Hizmet_Ad
        {
            get { return this.mal_Hizmet_Ad; }
            set
            {
                if (value != this.mal_Hizmet_Ad)
                {
                    this.mal_Hizmet_Ad = value;
                    this.OnPropertyChanged("Mal_Hizmet_Ad");
                }
            }
        }

        public decimal Birim_Fiyat
        {
            get { return this.birim_Fiyat; }
            set
            {
                if (value != this.birim_Fiyat)
                {
                    this.birim_Fiyat = value;
                    this.OnPropertyChanged("Birim_Fiyat");
                }
            }
        }

        public decimal Miktar
        {
            get
            {                
                return this.miktar;
            }
            set
            {
                if (value != this.miktar)
                {                    
                    this.miktar = value;
                    this.OnPropertyChanged("Miktar");
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
    public class FaturaViewModel : ViewModelBase
    {

        // Visibility Ayarları ***********************************************************************
        private bool donemToggleChecked = false;
        private Visibility donemToggleVisibility = Visibility.Collapsed;
        private bool siparisToggleChecked = false;
        private Visibility siparisToggleVisibility = Visibility.Collapsed;
        private bool irsaliyeToggleChecked = false;
        private Visibility irsaliyeToggleVisibility = Visibility.Collapsed;
        private bool referansToggleChecked = false;
        private Visibility referansToggleVisibility = Visibility.Collapsed;
        private bool vergiToggleChecked = false;
        private Visibility vergiToggleVisibility = Visibility.Collapsed;
        private bool bankaToggleChecked = false;
        private Visibility bankaToggleVisibility = Visibility.Collapsed;
        private bool notToggleChecked = false;
        private Visibility notToggleVisibility = Visibility.Collapsed;

        public bool DonemToggleChecked
        {
            get { return donemToggleChecked; }
            set
            {
                if (value != this.donemToggleChecked)
                {
                    if (value)
                        DonemToggleVisibility = Visibility.Visible;
                    else
                        DonemToggleVisibility = Visibility.Collapsed;

                    this.donemToggleChecked = value;
                    this.OnPropertyChanged(() => this.DonemToggleChecked);
                }
            }
        }
        public Visibility DonemToggleVisibility
        {
            get { return donemToggleVisibility; }
            set
            {
                if (value != this.donemToggleVisibility)
                {
                    this.donemToggleVisibility = value;
                    this.OnPropertyChanged(() => this.DonemToggleVisibility);
                }
            }
        }
        public bool SiparisToggleChecked
        {
            get { return siparisToggleChecked; }
            set
            {
                if (value != this.siparisToggleChecked)
                {
                    if (value)
                        SiparisToggleVisibility = Visibility.Visible;
                    else
                        SiparisToggleVisibility = Visibility.Collapsed;

                    this.siparisToggleChecked = value;
                    this.OnPropertyChanged(() => this.SiparisToggleChecked);
                }
            }
        }
        public Visibility SiparisToggleVisibility
        {
            get { return siparisToggleVisibility; }
            set
            {
                if (value != this.siparisToggleVisibility)
                {
                    this.siparisToggleVisibility = value;
                    this.OnPropertyChanged(() => this.SiparisToggleVisibility);
                }
            }
        }
        public bool IrsaliyeToggleChecked
        {
            get { return irsaliyeToggleChecked; }
            set
            {
                if (value != this.irsaliyeToggleChecked)
                {
                    if (value)
                        IrsaliyeToggleVisibility = Visibility.Visible;
                    else
                        IrsaliyeToggleVisibility = Visibility.Collapsed;

                    this.irsaliyeToggleChecked = value;
                    this.OnPropertyChanged(() => this.IrsaliyeToggleChecked);
                }
            }
        }
        public Visibility IrsaliyeToggleVisibility
        {
            get { return irsaliyeToggleVisibility; }
            set
            {
                if (value != this.irsaliyeToggleVisibility)
                {
                    this.irsaliyeToggleVisibility = value;
                    this.OnPropertyChanged(() => this.IrsaliyeToggleVisibility);
                }
            }
        }
        public bool ReferansToggleChecked
        {
            get { return referansToggleChecked; }
            set
            {
                if (value != this.referansToggleChecked)
                {
                    if (value)
                        ReferansToggleVisibility = Visibility.Visible;
                    else
                        ReferansToggleVisibility = Visibility.Collapsed;

                    this.referansToggleChecked = value;
                    this.OnPropertyChanged(() => this.ReferansToggleChecked);
                }
            }
        }
        public Visibility ReferansToggleVisibility
        {
            get { return referansToggleVisibility; }
            set
            {
                if (value != this.referansToggleVisibility)
                {
                    this.referansToggleVisibility = value;
                    this.OnPropertyChanged(() => this.ReferansToggleVisibility);
                }
            }
        }
        public bool VergiToggleChecked
        {
            get { return vergiToggleChecked; }
            set
            {
                if (value != this.vergiToggleChecked)
                {
                    if (value)
                        VergiToggleVisibility = Visibility.Visible;
                    else
                        VergiToggleVisibility = Visibility.Collapsed;

                    this.vergiToggleChecked = value;
                    this.OnPropertyChanged(() => this.VergiToggleChecked);
                }
            }
        }
        public Visibility VergiToggleVisibility
        {
            get { return vergiToggleVisibility; }
            set
            {
                if (value != this.vergiToggleVisibility)
                {
                    this.vergiToggleVisibility = value;
                    this.OnPropertyChanged(() => this.VergiToggleVisibility);
                }
            }
        }
        public bool BankaToggleChecked
        {
            get { return bankaToggleChecked; }
            set
            {
                if (value != this.bankaToggleChecked)
                {
                    if (value)
                        BankaToggleVisibility = Visibility.Visible;
                    else
                        BankaToggleVisibility = Visibility.Collapsed;

                    this.bankaToggleChecked = value;
                    this.OnPropertyChanged(() => this.BankaToggleChecked);
                }
            }
        }
        public Visibility BankaToggleVisibility
        {
            get { return bankaToggleVisibility; }
            set
            {
                if (value != this.bankaToggleVisibility)
                {
                    this.bankaToggleVisibility = value;
                    this.OnPropertyChanged(() => this.BankaToggleVisibility);
                }
            }
        }
        public bool NotToggleChecked
        {
            get { return notToggleChecked; }
            set
            {
                if (value != this.notToggleChecked)
                {
                    if (value)
                        NotToggleVisibility = Visibility.Visible;
                    else
                        NotToggleVisibility = Visibility.Collapsed;

                    this.notToggleChecked = value;
                    this.OnPropertyChanged(() => this.NotToggleChecked);
                }
            }
        }
        public Visibility NotToggleVisibility
        {
            get { return notToggleVisibility; }
            set
            {
                if (value != this.notToggleVisibility)
                {
                    this.notToggleVisibility = value;
                    this.OnPropertyChanged(() => this.NotToggleVisibility);
                }
            }
        }
        //*************************************************************************

        //ComboBox Ayarları****************************************************
        private string strSenaryo = null;
        private byte senaryoSelectedIndex;
        private string strFaturaTipi = null;
        private byte faturaTipiSelectedIndex;
        private string strParaBirimi = null;
        private byte paraBirimiSelectedIndex;

        public byte SenaryoSelectedIndex
        {
            get {
                switch (senaryoSelectedIndex)
                {
                    case 0:
                        strSenaryo = "TEMELFATURA";
                        break;
                    case 1:
                        strSenaryo = "TICARIFATURA";
                        break;
                    case 2:
                        strSenaryo = "HKS";
                        break;
                }
                return senaryoSelectedIndex; }
            set
            {
                if (value != this.senaryoSelectedIndex)
                {
                    switch (value)
                    {
                        case 0:
                            strSenaryo = "TEMELFATURA";
                            break;
                        case 1:
                            strSenaryo = "TICARIFATURA";
                            break;
                        case 2:
                            strSenaryo = "HKS";
                            break;
                    }

                    this.senaryoSelectedIndex = value;
                    this.OnPropertyChanged(() => this.SenaryoSelectedIndex);
                }
            }
        }
        public byte FaturaTipiSelectedIndex
        {
            get {
                switch (faturaTipiSelectedIndex)
                {
                    case 0:
                        strFaturaTipi = "SATIS";
                        break;
                    case 1:
                        strFaturaTipi = "IADE";
                        break;
                    case 2:
                        strFaturaTipi = "TEVKIFAT";
                        break;
                    case 3:
                        strFaturaTipi = "ISTISNA";
                        break;
                    case 4:
                        strFaturaTipi = "OZELMATRAH";
                        break;
                    case 5:
                        strFaturaTipi = "IHRACKAYITLI";
                        break;
                }
                return faturaTipiSelectedIndex; }
            set
            {
                if (value != this.faturaTipiSelectedIndex)
                {
                    switch (value)
                    {
                        case 0:
                            strFaturaTipi = "SATIS";
                            break;
                        case 1:
                            strFaturaTipi = "IADE";
                            break;
                        case 2:
                            strFaturaTipi = "TEVKIFAT";
                            break;
                        case 3:
                            strFaturaTipi = "ISTISNA";
                            break;
                        case 4:
                            strFaturaTipi = "OZELMATRAH";
                            break;
                        case 5:
                            strFaturaTipi = "IHRACKAYITLI";
                            break;
                    }

                    this.faturaTipiSelectedIndex = value;
                    this.OnPropertyChanged(() => this.FaturaTipiSelectedIndex);
                }
            }
        }
        public byte ParaBirimiSelectedIndex
        {
            get {
                switch (paraBirimiSelectedIndex)
                {
                    case 0:
                        strParaBirimi = "TRY";
                        break;
                    case 1:
                        strParaBirimi = "USD";
                        break;
                    case 2:
                        strParaBirimi = "EUR";
                        break;
                }
                return paraBirimiSelectedIndex; }
            set
            {
                if (value != this.paraBirimiSelectedIndex)
                {
                    switch (value)
                    {
                        case 0:
                            strParaBirimi = "TRY";
                            break;
                        case 1:
                            strParaBirimi = "USD";
                            break;
                        case 2:
                            strParaBirimi = "EUR";
                            break;
                    }

                    this.paraBirimiSelectedIndex = value;
                    this.OnPropertyChanged(() => this.ParaBirimiSelectedIndex);
                }
            }
        }
        //************************************************************************

        //Veri tabanı ayarları*************************************************
        private MyContext context;
        private QueryableEntityCoreCollectionView<Customer> customerCollectionView;
        private QueryableEntityCoreCollectionView<Product> productCollectionView;
        private ObservableCollection<ProductLine> productLines = new ObservableCollection<ProductLine>();

        public QueryableEntityCoreCollectionView<Customer> CustomerCollectionView
        {
            get { return customerCollectionView; }
            set
            {
                if (value != this.customerCollectionView)
                {
                    this.customerCollectionView = value;
                    this.OnPropertyChanged(() => this.CustomerCollectionView);
                }
            }
        }
        public QueryableEntityCoreCollectionView<Product> ProductCollectionView
        {
            get { return productCollectionView; }
            set
            {
                if (value != this.productCollectionView)
                {
                    this.productCollectionView = value;
                    this.OnPropertyChanged(() => this.ProductCollectionView);
                }
            }
        }
        public ObservableCollection<ProductLine> ProductLines
        {
            get { return this.productLines; }
            set
            {
                if (value != this.productLines)
                {
                    this.productLines = value;
                    this.OnPropertyChanged(() => this.ProductLines);
                }
            }
        }
        //****************************************************************************

        //selected işlmeleri *****************************************
        private Customer selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return this.selectedCustomer; }
            set
            {
                this.selectedCustomer = value;
                this.OnPropertyChanged("SelectedCustomer");
            }
        }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return this.selectedProduct; }
            set
            {
                this.selectedProduct = value;
                this.OnPropertyChanged("SelectedProduct");
            }
        }
        //*************************************************************


        //Listeye Ürün Ekleme****************************************
        public DelegateCommand urunEkleCommand { get; }

        public void UrunEkle(object obj)
        {
            bool eklenebilir = true;

            if (selectedProduct != null) 
            {
                for (int i = 0; i < productLines.Count; i++)
                    if (productLines[i].Mal_Hizmet_Ad == selectedProduct.Model) 
                    { 
                        RadWindow.Alert("Bu Ürün Zaten Ekli!"); 
                        eklenebilir = false; 
                    }

                if (eklenebilir)
                {
                    ProductLine data;
                    data = new ProductLine() { Birim_Fiyat = Convert.ToDecimal(selectedProduct.Price), Mal_Hizmet_Ad = selectedProduct.Model, Orders = new List<Vergi> { } };
                    productLines.Add(data);
                }
            }
            else
                RadWindow.Alert("Bir Ürün Seçin!");
        }

        //**********************************************************

        //busy***********************************************************************
        private bool isBusy;
        private string busyContent;
        private string faturaNo;

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
                        var backgroundWorker = new BackgroundWorker();
                        backgroundWorker.DoWork += this.OnBackgroundWorkerDoWork;
                        backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
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

        private void OnBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompleted;

            InvokeOnUIThread(() =>
            {
                this.IsBusy = false;
                var strWriter = new StringWriter();
                var xslTransform = new XslCompiledTransform();
                var xReader = XmlReader.Create($@"{AppDomain.CurrentDomain.BaseDirectory}\general.xslt", new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse });
                xslTransform.Load(xReader);
                xslTransform.Transform($@"{AppDomain.CurrentDomain.BaseDirectory}\Faturalar\{faturaNo}.xml", null, strWriter);
                WebBrowser webBrowser = new WebBrowser();
                webBrowser.NavigateToString(strWriter.ToString());

                var window = new RadWindow
                {
                    Content = webBrowser,
                    Width = 900,
                    Height = 500,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                RadWindowInteropHelper.SetAllowTransparency(window, false);
                window.Show();
            });
        }

        private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            //Fatura ayarları
            Random rnd = new Random();
            
            string FaturaID = $@"GIB{DateTime.Now.Year}{rnd.Next(000000001, 999999999)}";// Fatura No        
            string UUID = Guid.NewGuid().ToString();//Ettn No
            string schemeID = null;

            decimal topVergi = new decimal();
            decimal topMalHizTutar = new decimal();
            decimal malHizTutar = new decimal();
            decimal topIskonto = new decimal();
            

            Profil profil = context.Profils.Find(1);//Profil bilgisi

            List<TaxSubtotalType> totalType = new List<TaxSubtotalType>();
            List<AllowanceChargeType> allowanceChargeType = new List<AllowanceChargeType>();

            InvoiceLineType[] lineType = new InvoiceLineType[productLines.Count];

            if (SelectedCustomer.VKN_TCKN.Length == 10)
                schemeID = "VKN";
            else if (SelectedCustomer.VKN_TCKN.Length == 11)
                schemeID = "TCKN";       

            //ürün satırları
            for (int i = 0; i < productLines.Count; i++)
            {
                TaxSubtotalType[] taxSubtotalType = new TaxSubtotalType[productLines[i].Orders.Count];
                malHizTutar = (productLines[i].Birim_Fiyat * productLines[i].Miktar);
                
                decimal topSatirVergiTutar = new decimal();
                decimal vergiTutar = new decimal();

                //iskonto artırım işlemleri
                for (int j = 0; j < productLines[i].Orders.Count; j++)
                {
                    if ("İskonto(İndirim)" == productLines[i].Orders[j].VergiKAd)
                    {
                        AllowanceChargeType data = new AllowanceChargeType()
                        {
                            ChargeIndicator = new ChargeIndicatorType() { Value = false },//iskonto
                            AllowanceChargeReason = new AllowanceChargeReasonType() { Value = "İndirim" },
                            MultiplierFactorNumeric = new MultiplierFactorNumericType() { Value = productLines[i].Orders[j].VergiOran / 100 },
                            Amount = new AmountType2() { currencyID = strParaBirimi, Value = malHizTutar * productLines[i].Orders[j].VergiOran / 100 },
                            BaseAmount = new BaseAmountType() { currencyID = strParaBirimi, Value = malHizTutar - (productLines[i].Orders[j].VergiOran / 100) }
                        };

                        topIskonto += malHizTutar = malHizTutar - (malHizTutar * productLines[i].Orders[j].VergiOran / 100);
                        allowanceChargeType.Add(data);

                    }
                }

                for (int j =0; j < productLines[i].Orders.Count; j++)
                {
                    if (productLines[i].Orders[j].VergiKAd != "İskonto(İndirim)")
                    {
                        vergiTutar = malHizTutar * (productLines[i].Orders[j].VergiOran / 100);
                        topSatirVergiTutar += vergiTutar;

                        taxSubtotalType[j] = new TaxSubtotalType()
                        {
                            TaxableAmount = new TaxableAmountType() { currencyID = strParaBirimi, Value = malHizTutar },
                            TaxAmount = new TaxAmountType() { currencyID = strParaBirimi, Value = vergiTutar },
                            CalculationSequenceNumeric = new CalculationSequenceNumericType() { Value = j },
                            Percent = new PercentType1() { Value = productLines[i].Orders[j].VergiOran },
                            TaxCategory = new TaxCategoryType()
                            {
                                TaxScheme = new TaxSchemeType()
                                {
                                    Name = new NameType1() { Value = productLines[i].Orders[j].VergiKAd },
                                    TaxTypeCode = new TaxTypeCodeType() { Value = productLines[i].Orders[j].VergiKod }
                                }
                            }
                        };
                    }
                }

                lineType[i] = new InvoiceLineType()
                {
                    ID = new IDType() { Value = (i + 1).ToString() },
                    InvoicedQuantity = new InvoicedQuantityType() { unitCode = "C62", Value = productLines[i].Miktar },
                    LineExtensionAmount = new LineExtensionAmountType() { currencyID = strParaBirimi, Value = malHizTutar },

                    AllowanceCharge = allowanceChargeType.ToArray(),//iskonto ayarları

                    TaxTotal = new TaxTotalType()
                    {
                        TaxAmount = new TaxAmountType() { currencyID = strParaBirimi, Value =  topSatirVergiTutar},
                        TaxSubtotal = taxSubtotalType
                    },
                    Item = new ItemType() { Name = new NameType1() { Value = productLines[i].Mal_Hizmet_Ad} },
                    Price = new PriceType()
                    {
                        PriceAmount = new PriceAmountType() { currencyID = strParaBirimi, Value = productLines[i].Birim_Fiyat }
                    }     
                };

                topVergi += topSatirVergiTutar;
                topMalHizTutar += malHizTutar;
                
            }

            //totalsubtotal ayarları
            for (int i = 0; i < productLines.Count; i++)
            {
                for (int j = 0; j < productLines[i].Orders.Count; j++)
                {
                    int k=0;
                    bool add = true;
                   
                    do {
                        if (totalType.Count > 0)
                        {
                            if (productLines[i].Orders[j].VergiKod == totalType[k].TaxCategory.TaxScheme.TaxTypeCode.Value)
                            {
                                if (productLines[i].Orders[j].VergiOran == totalType[k].Percent.Value)
                                {
                                    totalType[k].TaxAmount.Value += malHizTutar * (productLines[i].Orders[j].VergiOran / 100);
                                    add = false;
                                }                                  
                            }           
                        }                           
                        k++;
                    }
                    while (k < totalType.Count);

                    if (add && productLines[i].Orders[j].VergiKAd != "İskonto(İndirim)")
                    {
                        TaxSubtotalType data = new TaxSubtotalType()
                        {
                            TaxableAmount = new TaxableAmountType() { currencyID = strParaBirimi, Value = malHizTutar },
                            TaxAmount = new TaxAmountType() { currencyID = strParaBirimi, Value = malHizTutar * (productLines[i].Orders[j].VergiOran / 100) },
                            CalculationSequenceNumeric = new CalculationSequenceNumericType() { Value = j },
                            Percent = new PercentType1() { Value = productLines[i].Orders[j].VergiOran },
                            TaxCategory = new TaxCategoryType()
                            {
                                TaxScheme = new TaxSchemeType()
                                {
                                    Name = new NameType1() { Value = productLines[i].Orders[j].VergiKAd },
                                    TaxTypeCode = new TaxTypeCodeType() { Value = productLines[i].Orders[j].VergiKod }
                                }
                            }
                        };
                        totalType.Add(data);                      
                    }
                    add = true;
                }
            }

                TemelFatura(FaturaID, UUID, schemeID, profil, SelectedCustomer, topVergi, topMalHizTutar, totalType, lineType, topIskonto);
        }
        //*********************************************************************

        //Fatura İşlemleri***************************************************
        private void TemelFatura(string faturaID, string UUID, string schemeID, Profil profil, Customer customer, decimal topVergi, decimal topMalHizTutar, List<TaxSubtotalType> totalType, InvoiceLineType[] lineType, decimal topIskonto) 
        {
            var invoice = new InvoiceType()
            {
                //statik bilgiler****************************************************
                UBLVersionID = new UBLVersionIDType() { Value = "2.1" },
                CustomizationID = new CustomizationIDType() { Value = "TR1.2" },
                ProfileID = new ProfileIDType() { Value = strSenaryo },
                ID = new IDType() { Value = faturaID },
                CopyIndicator = new CopyIndicatorType() { Value = false },
                UUID = new UUIDType() { Value = UUID },
                IssueDate = new IssueDateType() { Value = DateTime.Now },
                IssueTime = new IssueTimeType() { Value = DateTime.Now },
                InvoiceTypeCode = new InvoiceTypeCodeType() { Value = strFaturaTipi },
                DocumentCurrencyCode = new DocumentCurrencyCodeType() { Value = strParaBirimi },
                LineCountNumeric = new LineCountNumericType() { Value = CustomerCollectionView.Count },
                AdditionalDocumentReference = new DocumentReferenceType[]
                {
                    new DocumentReferenceType()
                    {
                        ID = new IDType() { Value = UUID },
                        IssueDate = new IssueDateType() { Value = DateTime.Now },
                        DocumentType = new DocumentTypeType() { Value = "XSLT" },
                        Attachment = new AttachmentType()
                        {
                            EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObjectType()
                            {
                                characterSetCode = "UTF-8",
                                encodingCode = "Base64",
                                filename = @$"{faturaID}.xslt",
                                mimeCode = "application/xml",
                                Value = Encoding.UTF8.GetBytes(new StreamReader(new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + "general.xslt", FileMode.Open, FileAccess.Read), Encoding.UTF8).ReadToEnd())// isteğe bağlı xslt değiştirilebir olması gerekiyor
                            }
                        }
                    }
                },
                Signature = new SignatureType[]
                {
                    new SignatureType()
                    {
                        ID = new IDType() { schemeID = "VKN_TCKN", Value = profil.VKN },
                        SignatoryParty = new PartyType()
                        {
                            PartyIdentification = new[] { new PartyIdentificationType() { ID = new IDType() { schemeID = "VKN", Value = profil.VKN } } },
                            PostalAddress = new AddressType()
                            {
                                Country = new CountryType() { Name = new NameType1() { Value = profil.Country } },
                                CityName = new CityNameType() { Value = profil.CityName },
                                CitySubdivisionName = new CitySubdivisionNameType() { Value = profil.CitySubdivisionName },
                                StreetName = new StreetNameType() { Value = profil.StreetName }
                            }
                        },
                        DigitalSignatureAttachment = new AttachmentType() { ExternalReference = new ExternalReferenceType() { URI = new URIType() { Value = @$"#Singnature_{faturaID}" } } }
                    }
                },
                AccountingSupplierParty = new SupplierPartyType()
                {
                    Party = new PartyType()
                    {
                        WebsiteURI = new WebsiteURIType() { Value = profil.WebsiteURI },
                        PartyIdentification = new PartyIdentificationType[]
                        {
                            new PartyIdentificationType() { ID = new IDType() { schemeID = "VKN", Value = profil.VKN } },
                            new PartyIdentificationType() { ID = new IDType() { schemeID = "MERSISNO", Value = profil.MERSISNO } },
                            new PartyIdentificationType() { ID = new IDType() { schemeID = "TICARETSICILNO", Value = profil.TICARETSICILNO } }
                        },
                        PartyName = new PartyNameType() { Name = new NameType1() { Value = profil.PartyName } },
                        PostalAddress = new AddressType()
                        {
                            Country = new CountryType() { Name = new NameType1() { Value = profil.Country } },
                            CityName = new CityNameType() { Value = profil.CityName },
                            CitySubdivisionName = new CitySubdivisionNameType() { Value = profil.CitySubdivisionName },
                            StreetName = new StreetNameType() { Value = profil.StreetName }
                        },
                        PartyTaxScheme = new PartyTaxSchemeType()
                        {
                            TaxScheme = new TaxSchemeType() { Name = new NameType1() { Value = profil.VD } }
                        },
                        Contact = new ContactType()
                        {

                            Telephone = new TelephoneType() { Value = profil.Telephone },
                            Telefax = new TelefaxType() { Value = profil.Telefax },
                            ElectronicMail = new ElectronicMailType() { Value = profil.ElectronicMail }
                        }
                    }
                },
                AccountingCustomerParty = new CustomerPartyType()
                {
                    Party = new PartyType()
                    {
                        PartyIdentification = new PartyIdentificationType[]
                        {
                            new PartyIdentificationType() { ID = new IDType() { schemeID = schemeID, Value = customer.VKN_TCKN } }
                        },
                        PartyName = new PartyNameType() { Name = new NameType1() { Value = customer.PartyName } },
                        PostalAddress = new AddressType()
                        {
                            StreetName = new StreetNameType() { Value = customer.StreetName },
                            CitySubdivisionName = new CitySubdivisionNameType() { Value = customer.CitySubdivisionName },
                            CityName = new CityNameType() { Value = customer.CityName },
                            Country = new CountryType() { Name = new NameType1() { Value = customer.Country } }
                        },
                        Contact = new ContactType()
                        {
                            ElectronicMail = new ElectronicMailType() { Value = customer.ElectronicMail }
                        }
                    }
                },
                LegalMonetaryTotal = new MonetaryTotalType()
                {
                    LineExtensionAmount = new LineExtensionAmountType() { currencyID = strParaBirimi, Value = topMalHizTutar },
                    TaxExclusiveAmount = new TaxExclusiveAmountType() { currencyID = strParaBirimi, Value = topMalHizTutar },
                    TaxInclusiveAmount = new TaxInclusiveAmountType() { currencyID = strParaBirimi, Value = topMalHizTutar + topVergi },
                    PayableAmount = new PayableAmountType() { currencyID = strParaBirimi, Value = topMalHizTutar + topVergi },
                    AllowanceTotalAmount = new AllowanceTotalAmountType() {currencyID = strParaBirimi, Value = topIskonto }
                },
                //********************************************************************
                //Dinamik Ayarlar*****************************************************
                TaxTotal = new TaxTotalType[]
                {
                    new TaxTotalType()
                    {
                        TaxAmount = new TaxAmountType() { currencyID = strParaBirimi, Value = topVergi },
                        TaxSubtotal = totalType.ToArray()
                    }
                },
                InvoiceLine = lineType

                //*********************************************************************
            };

            //yazma Ve okuma İşlemrleri****************************************************
            XmlSerializerNamespaces xmlNamespaces()
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                ns.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                ns.Add("ns8", "urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2");
                ns.Add("n4", "http://www.altova.com/samplexml/other-namespace");
                ns.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
                ns.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
                return ns;
            }

            var settings = new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true };
            var ms = new MemoryStream();
            var writer = XmlWriter.Create(ms, settings);
            var srl = new XmlSerializer(invoice.GetType());
            srl.Serialize(writer, invoice, xmlNamespaces());
            ms.Flush();
            ms.Seek(offset: 0, SeekOrigin.Begin);
            var srRead = new StreamReader(ms);
            var readXml = srRead.ReadToEnd();
            var path = Path.Combine($@"{AppDomain.CurrentDomain.BaseDirectory}\Faturalar\{invoice.ID.Value}.xml");

            if (!Directory.Exists($@"{AppDomain.CurrentDomain.BaseDirectory}\Faturalar\"))
                Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\Faturalar\");

            if (!File.Exists($@"{AppDomain.CurrentDomain.BaseDirectory}\Faturalar\{invoice.ID.Value}.xml"))
            {
                using (var swriter = new StreamWriter(path, false, Encoding.UTF8))
                {
                    swriter.Write(readXml);
                    swriter.Close();
                }
            }
            else
            {
                if (MessageBox.Show($@"{invoice.ID.Value}.xml dosyası mevcut. Üzerine yazılsın mı?", "Onay", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var swriter = new StreamWriter(path, false, Encoding.UTF8))
                    {
                        swriter.Write(readXml);
                        swriter.Close();
                    }
                }
            }

            faturaNo = invoice.ID.Value;

            //faturayı entegratöre gönder*****************************************************************
            EInvoiceWSPortClient eInvoiceWSPortClient = new EInvoiceWSPortClient();
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EBelge");
            
            Profil profilRow = context.Profils.Find(1);
            base64Binary contentByte = new base64Binary();
            contentByte.Value = Encoding.UTF8.GetBytes(new StreamReader(new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\Faturalar\{faturaNo}.xml", FileMode.Open, FileAccess.Read), Encoding.UTF8).ReadToEnd());

            SendInvoiceRequest req = new SendInvoiceRequest()
            {
                REQUEST_HEADER = new REQUEST_HEADERType()
                {
                    SESSION_ID = key.GetValue("SessionId").ToString(),
                    COMPRESSED = "N"
                },
                SENDER = new SendInvoiceRequestSENDER() { vkn = profilRow.VKN, alias = profilRow.ElectronicMail },
                RECEIVER = new SendInvoiceRequestRECEIVER() { vkn = customer.VKN_TCKN, alias = customer.ElectronicMail },
                INVOICE = new INVOICE[]
                {
                    new INVOICE(){ CONTENT = contentByte }
                }
            };

            SendInvoiceResponse res = new SendInvoiceResponse();
            res = eInvoiceWSPortClient.SendInvoice(req);

            if (res.ERROR_TYPE != null)
                MessageBox.Show(res.ERROR_TYPE.ERROR_SHORT_DES);

            key.Close();
        }
        //******************************************************************

        //Kurucu Fonksiyon**********************************************
        public FaturaViewModel()
        {
            this.context = new MyContext();
            this.CustomerCollectionView = new QueryableEntityCoreCollectionView<Customer>(context, context.Customers, new Collection<string>());
            this.ProductCollectionView = new QueryableEntityCoreCollectionView<Product>(context, context.Products, new Collection<string>());
            this.urunEkleCommand = new DelegateCommand(UrunEkle);
        }
    }
}
