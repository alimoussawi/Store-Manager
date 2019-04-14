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
using System.Text.RegularExpressions;

namespace MyWpfApp
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        MySqlConnection connection = new MySqlConnection(Helper.CnnVal("MyWPF"));
        public UserWindow()
        {
            InitializeComponent();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"Select * from items where ITEM_QUANTATY>{0}", connection);

                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                GridItemSell.DataContext = ds;
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

        private void GridItemSell_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataGrid gd = (DataGrid)sender;
            DataRowView Row_selected = gd.SelectedItem as DataRowView;
            if (Row_selected != null)
            {
                ItemQuantity.Visibility = Visibility.Visible;
                ItemName.Text= Row_selected["ITEM_NAME"].ToString();
                ItemId.Text = Row_selected["ITEM_ID"].ToString();
                oldQuan.Text= Row_selected["ITEM_QUANTATY"].ToString();

            }
        }

        private void Sell_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(ItemQuantity.Text) && int.Parse(ItemQuantity.Text) <= int.Parse(oldQuan.Text) && int.Parse(ItemQuantity.Text)!=0)
            {
                int newQuan = int.Parse(oldQuan.Text) - int.Parse(ItemQuantity.Text);
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "update items set ITEM_NAME=@ItemName,ITEM_QUANTATY=@ItemQuantity where ITEM_ID=@ItemId";
                    cmd.Parameters.AddWithValue("@ItemName", ItemName.Text);
                    cmd.Parameters.AddWithValue("@ItemQuantity", newQuan);
                    cmd.Parameters.AddWithValue("@ItemId", ItemId.Text);
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    MySqlCommand Com = new MySqlCommand($"Select * from items where ITEM_QUANTATY>{0}", connection);

                    MySqlDataAdapter adp = new MySqlDataAdapter(Com);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "LoadDataBinding");
                    GridItemSell.DataContext = ds;


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    connection.Close();
                    string sts = ItemQuantity.Text + " " + ItemName.Text + " has been sold";
                    MessageBox.Show(sts, "Success");
               //     ItemName.Clear();
                    ItemQuantity.Clear();
                    ItemId.Clear();
                }
            }
            else
                MessageBox.Show("WRONG QUANTITY", "ERROR");
        }

        private void ItemQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        private void SubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RequestName.Text) && !string.IsNullOrWhiteSpace(RequestQuan.Text) && int.Parse(RequestQuan.Text) != 0)
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "insert into requests(REQUEST_NAME,REQUEST_QUANTATY,REQUEST_STATUS) values(@requestname,@requestquan,@requeststs)";
                    cmd.Parameters.AddWithValue("@requestname", RequestName.Text);
                    cmd.Parameters.AddWithValue("@requestquan", int.Parse(RequestQuan.Text));
                    cmd.Parameters.AddWithValue("@requeststs", "notchecked");

                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();


                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    connection.Close();
                    MessageBox.Show("Request Submited", "Success");
                    RequestName.Clear();
                    RequestQuan.Clear();
                }
            }
            else
                MessageBox.Show("Please Provide Valid Inputs", "ERROR");
        }

        private void RequestQuan_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            this.Close();
            loginPage.Show();
        }
    }
}
