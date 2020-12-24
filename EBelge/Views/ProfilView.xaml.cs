using EBelge.ViewModels;
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
using Telerik.Windows.Data;

namespace EBelge.Views
{
    /// <summary>
    /// ProfilView.xaml etkileşim mantığı
    /// </summary>
    public partial class ProfilView : UserControl
    {
        ProfilViewModel vm;
        public ProfilView()
        {
            vm = new ProfilViewModel();
            this.DataContext = vm;
            InitializeComponent();
        }


        private void RadDataForm_EditEnding(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndingEventArgs e)
        {
            if(e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Commit)
                vm.Save();

        }

        private void ProfilDataForm_AutoGeneratingField(object sender, Telerik.Windows.Controls.Data.DataForm.AutoGeneratingFieldEventArgs e)
        {
            if (e.PropertyName == "ProfilId")
            {
                e.DataField.Label = "Profil No";
                e.DataField.IsReadOnly = true;
                e.DataField.Visibility = Visibility.Collapsed;
            }
            if (e.PropertyName == "VKN")
                e.DataField.Label = "VKN";
            if (e.PropertyName == "MERSISNO")
                e.DataField.Label = "MERSIS NO";
            if (e.PropertyName == "TICARETSICILNO")
                e.DataField.Label = "TICARET SICILNO";
            if (e.PropertyName == "VD")
                e.DataField.Label = "Vergi Dairesi";
            if (e.PropertyName == "PartyName")
                e.DataField.Label = "Unvan/Ad Soyad";
            if (e.PropertyName == "Country")
                e.DataField.Label = "Ulke";
            if (e.PropertyName == "CityName")
                e.DataField.Label = "Sehir";
            if (e.PropertyName == "CitySubdivisionName")
                e.DataField.Label = "Ilce/Semt";
            if (e.PropertyName == "StreetName")
                e.DataField.Label = "Acık Adres";
            if (e.PropertyName == "Telephone")
                e.DataField.Label = "Telefon";
            if (e.PropertyName == "Telefax")
                e.DataField.Label = "Fax";
            if (e.PropertyName == "ElectronicMail")
                e.DataField.Label = "E-Posta";
            if (e.PropertyName == "WebsiteURI")
                e.DataField.Label = "Website";

        }
    }
}
