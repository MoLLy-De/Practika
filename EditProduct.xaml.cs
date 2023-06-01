using Microsoft.Win32;
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

namespace V10AF
{
    /// <summary>
    /// Логика взаимодействия для EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {

        public DB.Product Product { get; set; }
        public EditProduct(DB.Product product)
        {
            InitializeComponent();
            Product = product;
            ProviderBox.SetBinding(ComboBox.ItemsSourceProperty, new Binding()
            {
                Source = Admin.Providers
            });
            DataContext = this;

            CategoryBox.Items.Add("Кольцо");
            CategoryBox.Items.Add("Колье");
            CategoryBox.Items.Add("Серьги");
            CategoryBox.Items.Add("Браслет");
            CategoryBox.Items.Add("Подвеска");
            CategoryBox.Items.Add("Ожерелье");
            CategoryBox.Items.Add("Брошь");
        }

        //Update
        private void UpBut_Click(object sender, RoutedEventArgs e)
        {
            Admin.connect.SaveChanges();
            MessageBox.Show("Продукт обновлен!");
        }

        //Remove
        private void RemBut_Click(object sender, RoutedEventArgs e)
        {
            var op = Admin.connect.OrderProduct.Where(p => p.ProductArticleNumber == Product.ProductArticleNumber).FirstOrDefault();
            if (op == null)
            {
                Admin.Products.Remove(Product);
                Admin.connect.Product.Remove(Product);
                Admin.connect.SaveChanges();
                MessageBox.Show("Продукт удален!");
                Close();
                return;
            } else
            {
                MessageBox.Show("Вы не можете удалить продукт, который используется в заказе");
                return;
            }
            
        }

        private void SlectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog().Value == true)
            {
                Product.ProductPhoto = "image/" + dialog.SafeFileName;
                Pict.GetBindingExpression(Image.SourceProperty).UpdateTarget();
            }
        }

    }
}
