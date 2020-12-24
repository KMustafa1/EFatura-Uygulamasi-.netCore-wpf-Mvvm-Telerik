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
using Telerik.Windows.Controls;

namespace EBelge.Views
{
    /// <summary>
    /// ProductView.xaml etkileşim mantığı
    /// </summary>
    public partial class ProductView : UserControl
    {
        ProductViewModel vm = new ProductViewModel();
        public ProductView()
        {
            this.DataContext = vm;
            InitializeComponent();
        }

        private void RadDataForm_AutoGeneratingField(object sender, Telerik.Windows.Controls.Data.DataForm.AutoGeneratingFieldEventArgs e)
        {
  
            if (e.PropertyName == "Product_Id")
            {
                e.DataField.Label = "Ürün No";
                e.DataField.IsReadOnly = true;
                e.DataField.Visibility = Visibility.Collapsed;
            }
            if (e.PropertyName == "Product_Name")     
                e.DataField.Label = "Ürün İsmi";             
            if (e.PropertyName == "Model")
                e.DataField.Label = "Model";
            if (e.PropertyName == "Price")
                e.DataField.Label = "Fiyat";
            if (e.PropertyName == "Quantity")
                e.DataField.Label = "Miktar";

        }

        private void RadDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Commit)
                vm.Save();
        }

        private void RadDataFilter_AutoGeneratingItemPropertyDefinition(object sender, Telerik.Windows.Controls.Data.DataFilter.DataFilterAutoGeneratingItemPropertyDefinitionEventArgs e)
        {
            if (e.ItemPropertyDefinition.PropertyName == "Product_Id")
            {
                e.ItemPropertyDefinition.DisplayName = "Ürün No";
                e.ItemPropertyDefinition.PropertyName = null;
            }
            if (e.ItemPropertyDefinition.PropertyName == "Product_Name")
                e.ItemPropertyDefinition.DisplayName = "Ürün İsmi";
            if (e.ItemPropertyDefinition.PropertyName == "Model")
                e.ItemPropertyDefinition.DisplayName = "Model";
            if (e.ItemPropertyDefinition.PropertyName == "Price")
                e.ItemPropertyDefinition.DisplayName = "Fiyat";
            if (e.ItemPropertyDefinition.PropertyName == "Quantity")
                e.ItemPropertyDefinition.DisplayName = "Miktar";
        }

        private void SatirSilButton_Click(object sender, RoutedEventArgs e)
        {
            vm.Delete(radGridView.SelectedItems);
            radGridView.SelectedItems.Clear();
        }
    }
}
