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
using MySql.Data.MySqlClient;
using System.Data;
namespace MyWpfApp
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        MySqlConnection connection = new MySqlConnection(Helper.CnnVal("MyWPF"));

        public LoginPage()
        {
            InitializeComponent();
        }

       

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            String Username = UsernameBox.Text.ToLower();
            String pass = PasswordBox.Password;
            if (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(pass))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "Select * from users where USER_NAME=@user and USER_PASS=@pass";
                    cmd.Parameters.AddWithValue("@user", Username);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.Connection = connection;
                    MySqlDataReader login = cmd.ExecuteReader();
                    if (login.Read())
                    {
                        String role = login.GetString("USER_TYPE");
                        if (role.Equals("admin"))
                        {
                            MainWindow mainWindow = new MainWindow();
                            this.Close();
                            mainWindow.Show();
                        }
                        if (role.Equals("user"))
                        {
                            UserWindow userWindow = new UserWindow();
                            this.Close();
                            userWindow.Show();
                        }
                    }
                    else
                        MessageBox.Show("incorect Username Or Password !", "ERROR");

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    connection.Close();

                }
            }
            else
                MessageBox.Show("empty fields", "ERROR");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

       
    }
}
