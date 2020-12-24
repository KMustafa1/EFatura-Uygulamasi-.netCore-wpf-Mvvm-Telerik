using EBelge.ViewModels;
using System;
using System.Collections.Generic;
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

namespace EBelge.Views
{
    /// <summary>
    /// CheckUserView.xaml etkileşim mantığı
    /// </summary>
    public partial class CheckUserView : UserControl
    {
        CheckUserViewModel vm = new CheckUserViewModel();
        public CheckUserView()
        {
            this.DataContext = vm;
            InitializeComponent();
        }

        private void RadDataForm_AutoGeneratingField(object sender, Telerik.Windows.Controls.Data.DataForm.AutoGeneratingFieldEventArgs e)
        {
            if (e.PropertyName == "CustomerId")
            {
                e.DataField.Label = "Müşteri No";
                e.DataField.IsReadOnly = true;
                e.DataField.Visibility = Visibility.Collapsed;
            }
            if (e.PropertyName == "VKN_TCKN")
                e.DataField.Label = "VKN/TCKN";
            if (e.PropertyName == "PartyName")
                e.DataField.Label = "Ünvan";
            if (e.PropertyName == "Country")
                e.DataField.Label = "Ülke";
            if (e.PropertyName == "CityName")
                e.DataField.Label = "Şehir";
            if (e.PropertyName == "CitySubdivisionName")
                e.DataField.Label = "İlçe";
            if (e.PropertyName == "StreetName")
                e.DataField.Label = "Adres";
            if (e.PropertyName == "ElectronicMail")
                e.DataField.Label = "E Posta";
            if (e.PropertyName == "Users")
                e.DataField.Visibility = Visibility.Collapsed;
        }

        private void RadGridView_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            vm.Save();
        }

        private void RadDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Commit)
                vm.Save();
        }

        private void radDataFilter_AutoGeneratingItemPropertyDefinition(object sender, Telerik.Windows.Controls.Data.DataFilter.DataFilterAutoGeneratingItemPropertyDefinitionEventArgs e)
        {
            if (e.ItemPropertyDefinition.PropertyName == "CustomerId")
            {
                e.ItemPropertyDefinition.DisplayName = "Müşteri No";
                e.ItemPropertyDefinition.PropertyName = null;
            }    
            if (e.ItemPropertyDefinition.PropertyName == "VKN_TCKN")
                e.ItemPropertyDefinition.DisplayName = "VKN/TCKN";
            if (e.ItemPropertyDefinition.PropertyName == "PartyName")
                e.ItemPropertyDefinition.DisplayName = "Ünvan";
            if (e.ItemPropertyDefinition.PropertyName == "Country")
                e.ItemPropertyDefinition.DisplayName = "Ülke";
            if (e.ItemPropertyDefinition.PropertyName == "CityName")
                e.ItemPropertyDefinition.DisplayName = "Şehir";
            if (e.ItemPropertyDefinition.PropertyName == "CitySubdivisionName")
                e.ItemPropertyDefinition.DisplayName = "İlçe";
            if (e.ItemPropertyDefinition.PropertyName == "StreetName")
                e.ItemPropertyDefinition.DisplayName = "Adres";
            if (e.ItemPropertyDefinition.PropertyName == "ElectronicMail")
                e.ItemPropertyDefinition.DisplayName = "E Posta";
        }

        private void SatirSil_Click(object sender, RoutedEventArgs e)
        {
            vm.Delete(radGridView.SelectedItems);
            radGridView.SelectedItems.Clear();
        }
    }
}
