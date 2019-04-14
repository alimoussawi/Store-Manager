using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MyWpfApp
{
   public class DataAccess
    {
        public List<Item> GetItems(string ItemName)
        {
            using (MySqlConnection connection = new MySqlConnection(Helper.CnnVal("MyWPF")))
            {
                connection.Open();
               var output= connection.Query<Item>($"select * from items where ITEM_NAME = '{ItemName}'");
               
                return output.ToList();
            }


        }


    }
}
