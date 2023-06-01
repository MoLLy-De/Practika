using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; //ObservableCollection колекция для списков
using System.ComponentModel;//ObservableCollection колекция для списков
using System.Linq; //Язык заапросов sql
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using V10AF.DB; //база

namespace V10AF
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public static yvelirkaEntities connect = new yvelirkaEntities();

        private EditProduct editproduct;

        public class SortCost
        {
            public string DisplayName { get; set; }
            public string PropertyName { get; set; }
            public bool Sort { get; set; }
        }


        public ObservableCollection<SortCost> sorts { get; set; } = new ObservableCollection<SortCost>()
        {
            new SortCost() {DisplayName = "По возрвстанию", PropertyName = "ProductCost",  Sort = false},
            new SortCost() {DisplayName = "По убыванию", PropertyName = "ProductCost",  Sort = true}
        };


        public static ObservableCollection<DB.Product> Products { get; set; }
        
        public static ObservableCollection<DB.Provider> Providers { get; set; }

        public Admin(string user)
        {
            InitializeComponent();
            FIOLab.Content = user;
            Products = new ObservableCollection<DB.Product>(connect.Product.ToList());
            Providers = new ObservableCollection<DB.Provider>(connect.Provider.ToList());
            DataContext = this;

            FilterBoxCategory.Items.Add("Кольцо");
            FilterBoxCategory.Items.Add("Колье");
            FilterBoxCategory.Items.Add("Серьги");
            FilterBoxCategory.Items.Add("Браслет");
            FilterBoxCategory.Items.Add("Подвеска");
            FilterBoxCategory.Items.Add("Ожерелье");
            FilterBoxCategory.Items.Add("Брошь");
            FilterBoxCategory.Items.Add("Все категории");
        }

        private void CloseBut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void SerchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(SearchText.Text);
        }

        private void Search(string text)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);
            if (view == null) { return; }

            int viewCounter = 0;

            view.Filter = new Predicate<object>(obj =>
            {
                bool isView = ((DB.Product)obj).ProductName.ToLower().Contains(text.ToLower());
                if (isView) viewCounter++;
                return isView;
            });
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = SortBox.SelectedItem as SortCost;

            var view = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);

            var direction = item.Sort ? 
                ListSortDirection.Ascending :
                ListSortDirection.Descending;

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(item.PropertyName, direction));
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Provider provider = FilterBox.SelectedItem as Provider;

            var view = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);

            view.Filter = new Predicate<object>(prov =>
            {
                if (provider.NameProvider == "Все производители") return true;
                return (prov as Product).ProductProvider == provider.NameProvider;

            });
        }

        private void FilterBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string prod = FilterBoxCategory.SelectedItem.ToString();

            var view = CollectionViewSource.GetDefaultView(ProductList.ItemsSource);

            view.Filter = new Predicate<object>(prouct =>
            {
                if (prod == "Все категории") return true;
                return (prouct as Product).ProductCategory.ToString() == prod;
            });
        }

        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editproduct != null) return;
            
            var selectedproduct = ProductList.SelectedItem as Product;

            editproduct = new EditProduct(selectedproduct);
            editproduct.Closed += Editproduct_Closed;
            editproduct.Show();
        }

        private void Editproduct_Closed(object sender, EventArgs e)
        {
            editproduct = null;
        }

        private void CreateProductBut_Click(object sender, RoutedEventArgs e)
        {
            CreateMaterial createMaterial = new CreateMaterial();
            createMaterial.Show();

        }

    }
}
