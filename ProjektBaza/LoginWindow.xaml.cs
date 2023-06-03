using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace ProjektBaza
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = loginInput.Text;
            string pass = passInput.Password;
            pass = MD5Generator.FromString(pass);

            var connection = new MySqlConnection("datasource=127.0.0.1;port=3306;username=root;password=;database=projekt_baza");
            connection.Open();
            var command = new MySqlCommand("Select * from users where login = '" + login + "' and pass = '" + pass + "';",connection);
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                IncorrectLoginMessage.Visibility = Visibility.Hidden;
            }
            else
            {
                IncorrectLoginMessage.Visibility = Visibility.Visible;
            }

        }
    }
}
