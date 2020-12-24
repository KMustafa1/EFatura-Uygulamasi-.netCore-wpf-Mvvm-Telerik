using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using Telerik.Windows.Controls;

namespace EBelge.Models
{
    public class NavigationItemModel
    {
        public string Title { get; set; }
        public string IconGlyph { get; set; }
        public FrameworkElement FrameworkElement {get; set;}
        public DelegateCommand Command { get; set; }
        public ObservableCollection<NavigationItemModel> Children { get; set; }
    }
}
