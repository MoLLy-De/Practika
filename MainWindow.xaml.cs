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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using V10AF.DB;

namespace V10AF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static yvelirkaEntities connect = new yvelirkaEntities();
        bool statususer = false;
        string capcode = "";
        Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            LoginText.Text = "dlh4o1tzrbse@yahoo.com";
            PassText.Text = "2L6KZG";
#endif
        }

        private void ButSing_Click(object sender, RoutedEventArgs e)
        {
            string log = LoginText.Text;
            string pas = PassText.Text;

            DB.User a = (DB.User)connect.User.Where(u => u.UserLogin == log && u.UserPassword == pas).FirstOrDefault();
            if (a == null) 
            {
                if (statususer == false) 
                {
                    MessageBox.Show("Данные введены неверно!");
                    OpenCapcha();
                    return;
                } else
                {
                    MessageBox.Show("Данные введены неверно!");                    
                    BlockUser();
                    return;
                }
                
            } else
            {
                if (statususer == false)
                {
                    if (a.UserRole == 1)
                    {
                        Admin admin = new Admin(a.UserSurname + " " + a.UserName + " " + a.UserPatronymic);
                        admin.Show();
                        this.Close();
                    }
                    else if (a.UserRole == 2)
                    {
                        Menedger meneger = new Menedger(a.UserSurname + " " + a.UserName + " " + a.UserPatronymic);
                        meneger.Show();
                        this.Close();
                    }
                    else if (a.UserRole == 3)
                    {
                        Klient klient = new Klient(a.UserSurname + " " + a.UserName + " " + a.UserPatronymic);
                        klient.Show();
                        this.Close();
                    }
                } else
                {
                    if (CapchaText.Text == capcode)
                    {
                        if (a.UserRole == 1)
                        {
                            Admin admin = new Admin(a.UserSurname + " " + a.UserName + " " + a.UserPatronymic);
                            admin.Show();
                            this.Close();
                        }
                        else if (a.UserRole == 2)
                        {
                            Menedger meneger = new Menedger(a.UserSurname + " " + a.UserName + " " + a.UserPatronymic);
                            meneger.Show();
                            this.Close();
                        }
                        else if (a.UserRole == 3)
                        {
                            Klient klient = new Klient(a.UserSurname + " " + a.UserName + " " + a.UserPatronymic);
                            klient.Show();
                            this.Close();
                        }
                    } else
                    {
                        MessageBox.Show("Неверная капча!!");
                        CanvaCapcha.Children.Clear();
                        GenerationCapcha();
                        return;
                    }
                }
               
            }
        }

        private void OpenCapcha()
        {
            statususer = true;
            GenerationCapcha();
            Capchalab.Content = capcode;
            CapchaText.Visibility = Visibility.Visible;
            CanvaCapcha.Visibility = Visibility.Visible;
        }

        private void BlockUser() 
        {
            LogPasPanel.IsEnabled = false;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += new EventHandler((s, e) =>
            {
                timer.Stop();
                LogPasPanel.IsEnabled = true;
            });
            timer.Start();
        }

        private void GenerationCapcha() 
        {
            capcode = "";
            for (int i = 0; i < 2; i++)
            {
                capcode += (char)random.Next(65, 91);
                capcode += (char)random.Next(48, 58);
            }
            GenerateCapchaImege();
        }

        private void GenerateCapchaImege()
        {
            CanvaCapcha.Children.Clear();

            for (int i = 0; i < capcode.Length; i++)
            {
                CreateLable(i);
            }

            CreateLine();
            CreateLine();
        }

        private void CreateLable(int index)
        {
            Label label = new Label();
            label.Content= capcode[index];
            label.Height = 64;
            label.Width = random.Next(28, 30);
            label.FontWeight = FontWeights.Black;
            label.RenderTransformOrigin = new Point(0.5, 0.5);
            label.RenderTransform = new RotateTransform(random.Next(-30, 20));
            CanvaCapcha.Children.Add(label);
            Canvas.SetLeft(label, 120 + (index * 30));
            Canvas.SetTop(label, random.Next(0, 10));
        }

        private void CreateLine()
        {
            Line line = new Line();
            line.X1 = random.Next(100, 120);
            line.Y1 = random.Next(10, 54);
            line.X2 = random.Next(260, 280);
            line.Y2 = random.Next(10, 54);
            line.Stroke = new SolidColorBrush(Colors.Black);
            line.StrokeThickness = random.Next(2, 4);
            CanvaCapcha.Children.Add(line);

        }

        private void ButG_Click(object sender, RoutedEventArgs e)
        {
            Klient klient = new Klient("Гость");
            klient.Show();
            this.Close();
        }
    }
}
