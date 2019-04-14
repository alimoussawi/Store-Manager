using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace MyWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :Window

    {

        MySqlConnection connection = new MySqlConnection(Helper.CnnVal("MyWPF"));

        public MainWindow()
        {
            
            InitializeComponent();
           
        }

        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            this.Close();
            loginPage.Show();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ItemsTable.Visibility = Visibility.Visible;
            AddItems.Visibility = Visibility.Collapsed;
            EditItemsGrid.Visibility = Visibility.Collapsed;
            WelcomeGrid.Visibility = Visibility.Collapsed;
            RequestsGrid.Visibility = Visibility.Collapsed;



        }
        private void RequestButtonPage_Click(object sender, RoutedEventArgs e)
        {
            ItemsTable.Visibility = Visibility.Collapsed;
            AddItems.Visibility = Visibility.Collapsed;
            EditItemsGrid.Visibility = Visibility.Collapsed;
            WelcomeGrid.Visibility = Visibility.Collapsed;
            RequestsGrid.Visibility = Visibility.Visible;
        }

        private void AddingButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AddName.Text) && !string.IsNullOrWhiteSpace(AddDescription.Text)&& !string.IsNullOrWhiteSpace(AddPrice.Text)&& !string.IsNullOrWhiteSpace(AddQuantity.Text))
            {
                try
                {
                    string InsertQuery = "insert into items(ITEM_NAME,ITEM_PRICE,ITEM_DESCRIPTION,ITEM_QUANTATY) VALUES('" + AddName.Text + "','" + int.Parse(AddPrice.Text) + "','" + AddDescription.Text + "'," + int.Parse(AddQuantity.Text) + ")";
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(InsertQuery, connection);
                    cmd.ExecuteNonQuery();
                    
                  
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    connection.Close();
                    MessageBox.Show("Successfully Added","Success");
                    AddName.Clear();
                    AddDescription.Clear();
                    AddPrice.Clear();
                    AddQuantity.Clear();
                }

              
            }
            else
                MessageBox.Show("incorrect input","ERROR");
        }

        private void AddButtonPage_Click(object sender, RoutedEventArgs e)
        {
            EditItemsGrid.Visibility = Visibility.Collapsed;
            ItemsTable.Visibility = Visibility.Collapsed;
            AddItems.Visibility = Visibility.Visible;
            WelcomeGrid.Visibility = Visibility.Collapsed;
            RequestsGrid.Visibility = Visibility.Collapsed;


        }

        private void EditButtonPage_Click(object sender, RoutedEventArgs e)
        {
            ItemsTable.Visibility = Visibility.Collapsed;
            AddItems.Visibility = Visibility.Collapsed;
            EditItemsGrid.Visibility = Visibility.Visible;
            WelcomeGrid.Visibility = Visibility.Collapsed;
            RequestsGrid.Visibility = Visibility.Collapsed;

        }



        private void SeachButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ItemNameSeach.Text))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand($"Select * from items where ITEM_NAME like '%{ItemNameSeach.Text}%'", connection);

                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "LoadDataBinding");
                    McDataGrid.DataContext = ds;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    ItemNameSeach.Clear();
                    connection.Close();

                }
            }
            else MessageBox.Show("type a name please ","ERROR");

        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from items ", connection);

                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                McDataGrid.DataContext = ds;
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

        private void AddPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void AddQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

        }

        private void EditDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView Row_selected = gd.SelectedItem as DataRowView;
            if (Row_selected!=null)
            {
                EditId.Text = Row_selected["ITEM_ID"].ToString();
                EditName.Text = Row_selected["ITEM_NAME"].ToString();
                EditDescription.Text = Row_selected["ITEM_DESCRIPTION"].ToString();
                EditPrice.Text = Row_selected["ITEM_PRICE"].ToString();
                EditQuantity.Text = Row_selected["ITEM_QUANTATY"].ToString();
            }
        }

        private void LoadEdits_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from items ", connection);

                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                EditDataGrid.DataContext = ds;
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

        private void SubmitEdits_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EditName.Text)&& !string.IsNullOrWhiteSpace(EditDescription.Text)&& !string.IsNullOrWhiteSpace(EditPrice.Text) && !string.IsNullOrWhiteSpace(EditQuantity.Text))
            {
                try
                {
                    connection.Open();
                   
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText= "update items set ITEM_NAME=@ItemName,ITEM_PRICE=@ItemPrice,ITEM_DESCRIPTION=@ItemDescription,ITEM_QUANTATY=@ItemQuantity where ITEM_ID=@ItemId";
                    cmd.Parameters.AddWithValue("@ItemName", EditName.Text);
                    cmd.Parameters.AddWithValue("@ItemPrice", EditPrice.Text);
                    cmd.Parameters.AddWithValue("@ItemDescription", EditDescription.Text);
                    cmd.Parameters.AddWithValue("@ItemQuantity", EditQuantity.Text);
                    cmd.Parameters.AddWithValue("@ItemId", EditId.Text);
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    MySqlCommand Com = new MySqlCommand("Select * from items ", connection);

                    MySqlDataAdapter adp = new MySqlDataAdapter(Com);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "LoadDataBinding");
                    EditDataGrid.DataContext = ds;


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    connection.Close();
                    MessageBox.Show("Successfully updated","Success");
                    EditName.Clear();
                    EditDescription.Clear();
                    EditPrice.Clear();
                    EditQuantity.Clear();
                    EditId.Clear();
                }
            }
            else
                MessageBox.Show("Cannot Edit Empty Row ","ERROR");
        }

        private void EditPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

        }

        private void EditQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

        }

        private void DeleteEdits_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EditName.Text) && !string.IsNullOrWhiteSpace(EditDescription.Text) && !string.IsNullOrWhiteSpace(EditPrice.Text) && !string.IsNullOrWhiteSpace(EditQuantity.Text))
            {
                MessageBoxResult choice = MessageBox.Show("delete selected item ?", "confirmation", MessageBoxButton.YesNo);
                if (choice == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand("delete from items where ITEM_ID='" + int.Parse(EditId.Text) + "'", connection);
                        cmd.ExecuteNonQuery();
                        MySqlCommand Com = new MySqlCommand("Select * from items ", connection);

                        MySqlDataAdapter adp = new MySqlDataAdapter(Com);
                        DataSet ds = new DataSet();
                        adp.Fill(ds, "LoadDataBinding");
                        EditDataGrid.DataContext = ds;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        connection.Close();
                        EditId.Clear();
                        EditName.Clear();
                        EditQuantity.Clear();
                        EditPrice.Clear();
                        EditDescription.Clear();
                        MessageBox.Show("item deleted", "confirmed");

                    }

                }

            }
            else
                MessageBox.Show("Select a row to delete", "ERROR");
        }

        private void LoadLowQuan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"Select * from items where ITEM_QUANTATY={0} ", connection);

                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                McDataGrid.DataContext = ds;
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

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            PrintDG print = new PrintDG();

            print.printDG(McDataGrid, "Title");
        }

        private void LoadRequests_Click(object sender, RoutedEventArgs e)
        {
            string sts = "'notchecked'";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"Select * from requests where REQUEST_STATUS={sts} ", connection);

                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadRequests");
                RequestsDataGrid.DataContext = ds;
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

        private void RequestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView Row_selected = gd.SelectedItem as DataRowView;
            if (Row_selected != null)
            {
                RId.Text = Row_selected["REQUEST_ID"].ToString();
                RName.Text = Row_selected["REQUEST_NAME"].ToString();
                RQuantity.Text = Row_selected["REQUEST_QUANTATY"].ToString();
               
            }
        }

        private void AcceptRequest_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(RId.Text) && !string.IsNullOrWhiteSpace(RName.Text) && !string.IsNullOrWhiteSpace(RQuantity.Text))
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "update requests set REQUEST_ID=@requestid,REQUEST_NAME=@requestname,REQUEST_QUANTATY=@requestquantity,REQUEST_STATUS=@requeststs where REQUEST_ID=@requestid";
                    cmd.Parameters.AddWithValue("@requestid", RId.Text);
                    cmd.Parameters.AddWithValue("@requestname", RName.Text);
                    cmd.Parameters.AddWithValue("@requestquantity", int.Parse(RQuantity.Text));
                    cmd.Parameters.AddWithValue("@requeststs", "checked");
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    string sts = "'notchecked'";
                    MySqlCommand Com = new MySqlCommand($"Select * from requests where REQUEST_STATUS={sts} ", connection);

                    MySqlDataAdapter adp = new MySqlDataAdapter(Com);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "LoadRequests");
                    RequestsDataGrid.DataContext = ds;
                    connection.Close();
                    RId.Clear();
                    RName.Clear();
                    RQuantity.Clear();
                    MessageBox.Show("Request Reviewed", "Success");

                }
            }
            else
                MessageBox.Show("please select a request !", "ERROR");
        }

    }
 }

