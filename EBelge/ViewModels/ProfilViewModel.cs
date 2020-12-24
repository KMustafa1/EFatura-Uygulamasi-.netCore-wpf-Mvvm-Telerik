using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using System.Windows;
using BillingWS;
using Microsoft.Win32;
using System.ComponentModel;
using Telerik.Windows.Controls.Navigation;

namespace EBelge.ViewModels
{
    public class ProfilViewModel : ViewModelBase
    {
        private QueryableEntityCoreCollectionView<Profil> entityCollectionView;
        private MyContext context;

        public ProfilViewModel()
        {
            this.context = new MyContext();          
            this.EntityCollectionView = new QueryableEntityCoreCollectionView<Profil>(context, context.Profils, new Collection<string>());
        }

        public QueryableEntityCoreCollectionView<Profil> EntityCollectionView
        {
            get { return entityCollectionView; }
            set
            {
                if (value != this.entityCollectionView)
                {
                    this.entityCollectionView = value;
                    this.OnPropertyChanged(() => this.EntityCollectionView);
                }
            }
        }

        public void Save()
        {
            this.context.SaveChanges();         
        }
    }
}
