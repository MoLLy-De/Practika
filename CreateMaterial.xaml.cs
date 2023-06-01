using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;

namespace V10AF
{
    /// <summary>
    /// Логика взаимодействия для CreateMaterial.xaml
    /// </summary>
    public partial class CreateMaterial : Window
    {
        public DB.Product NewProduct { get; set; }
        public CreateMaterial()
        {
            InitializeComponent();
            NewProduct= new DB.Product();
            ProviderBox.SetBinding(ComboBox.ItemsSourceProperty, new Binding()
            {
                Source = Admin.Providers
            });
            
            CategoryBox.Items.Add("Кольцо");
            CategoryBox.Items.Add("Колье");
            CategoryBox.Items.Add("Серьги");
            CategoryBox.Items.Add("Браслет");
            CategoryBox.Items.Add("Подвеска");
            CategoryBox.Items.Add("Ожерелье");
            CategoryBox.Items.Add("Брошь");
            DataContext = this;
        }

        private void UpBut_Click(object sender, RoutedEventArgs e)
        {
            Admin.connect.Product.Add(NewProduct);
            int result = Admin.connect.SaveChanges();

            if (result != 0) 
            {
                Admin.Products.Add(NewProduct);
                NewProduct = new DB.Product();

                ProductPanel.GetBindingExpression(DataContextProperty).UpdateTarget();

                MessageBox.Show("Материал добавлен!");
            }
        }

        private void SlectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog().Value == true)
            {
                NewProduct.ProductPhoto = "image/" + dialog.SafeFileName;
                Pict.GetBindingExpression(Image.SourceProperty).UpdateTarget();
            }
        }
    }
}
