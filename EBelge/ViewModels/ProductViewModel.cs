using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace EBelge.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private QueryableEntityCoreCollectionView<Product> entityCollectionView;
        private MyContext context;
        
        public ProductViewModel() 
        {
            this.context = new MyContext();
            this.InsertCommand = new DelegateCommand(Insert);
            this.EntityCollectionView = new QueryableEntityCoreCollectionView<Product>(context, context.Products, new Collection<string>());
        }

        public DelegateCommand InsertCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        public void Insert(object obj)
        {
            var product = new Product() { };
            context.Products.Add(product);
            context.SaveChanges();      
            entityCollectionView.Refresh();
        }

        public void Delete(ObservableCollection<object> obj)
        {
            
            context.RemoveRange(obj);
            context.SaveChanges();
            entityCollectionView.Refresh();
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public QueryableEntityCoreCollectionView<Product> EntityCollectionView
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
    }
}
