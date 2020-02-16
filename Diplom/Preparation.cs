using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Net;

namespace Diplom
{
    class Preparation
    {
        internal MySqlConnection connection;
        internal WebClient client;
        internal bool success = false;
        internal Preparation()
        {
            try
            {
                string connectionStringLocal = "Data Source=localhost;Port=3306;Initial Catalog=habr;Integrated Security=False;user ID=root;password=;Connection Timeout=10;Convert Zero Datetime=True;";
                connection = new MySqlConnection(connectionStringLocal);
                connection.Open();

                client = new WebClient { Encoding = Encoding.UTF8 };
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
                success = true;
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("You haven't connected to DB!");
            }
        }
    }
}
