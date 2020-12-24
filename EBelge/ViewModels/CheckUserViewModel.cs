using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Telerik.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using EBelge.Models;
using Telerik.Windows.Data;
using Microsoft.Win32;
using AuthenticationWS;

namespace EBelge.ViewModels
{
	public class CheckUserViewModel : ViewModelBase
	{
		private QueryableEntityCoreCollectionView<Customer> entityCollectionView;
        private MyContext context;
        

		public CheckUserViewModel()
		{
            this.context = new MyContext();
            this.InsertCommand = new DelegateCommand(Insert);
            this.EntityCollectionView = new QueryableEntityCoreCollectionView<Customer>(context, context.Customers, new Collection<string>() {"Users"});
        }

        public DelegateCommand InsertCommand { get; }

        public QueryableEntityCoreCollectionView<Customer> EntityCollectionView
        {
            get { return entityCollectionView; }
            set
            {
                if(value != this.entityCollectionView)
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

        public void Insert(object obj)
        {
            var customer = new Customer() {};
            context.Customers.Add(customer);
            context.SaveChanges();
            entityCollectionView.Refresh();
        }

        public void Delete(ObservableCollection<object> obj)
        {

            context.RemoveRange(obj);
            context.SaveChanges();
            entityCollectionView.Refresh();
        }

    }
}
