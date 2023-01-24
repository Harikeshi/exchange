using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{

    class Product { public string Name { get; set; } }

    class Category : INotifyPropertyChanged
    {
        private string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                onPropertyChanged(new PropertyChangedEventArgs("CategoryName"));
            }
        }
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                onPropertyChanged(new PropertyChangedEventArgs("Products"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void onPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public Category(string categoryName, ObservableCollection<Product> p)
        {
            CategoryName = categoryName;
            Products = p;
        }
    }
}
