using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
using System.Drawing;
using System.Data;

namespace ProjektBaza
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Product> products = new List<Product>();
        const int ROW_HEIGHT = 100;
        public MainWindow(string nick)
        {
            InitializeComponent();

            Header.Text = "Witaj " + nick;

            DownloadData();
            RenderRows();
        }

        private void RenderRows()
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < typeof(Product).GetProperties().Count(); i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < products.Count; i++)
            {
                var p = products[i];
                var row = new DataRow(ref p);
                row.Render(MainGrid, ROW_HEIGHT);
            }

            RenderAddButtton();
        }

        private void RenderAddButtton()
        {
            var button = new Button();
            button.Content = "Dodaj produkt";
            button.Margin = new Thickness(30);
            Grid.SetRow(button, MainGrid.RowDefinitions.Count);
            MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ROW_HEIGHT) });
            MainGrid.Children.Add(button);
            button.Click += Add_Product;
        }

        private void Add_Product(object sender, RoutedEventArgs e)
        {
            products.Add(new Product());
            RenderRows();
        }

        private void DownloadData()
        {
            var connection = new MySqlConnection("datasource=127.0.0.1;port=3306;username=root;password=;database=projekt_baza");
            connection.Open();
            var command = new MySqlCommand("Select * from products;", connection);
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    products.Add(new Product()
                    {
                        Id = Convert.ToInt32(reader.GetString(0)),
                        Name = reader.GetString(1),
                        Cost = float.Parse(reader.GetString(2), CultureInfo.InvariantCulture.NumberFormat),
                        Tax = Convert.ToInt32(reader.GetString(3)),
                        Description = reader.GetString(4),
                        ImageName = reader.GetString(5)
                    });
                }
            }
        }
    }
}
