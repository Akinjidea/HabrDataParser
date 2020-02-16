using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Media;
using System.Data;
using System.Net;


namespace Diplom
{
    internal class Viewing
    {
        string selectedTableName;
        DataSet dataset;

        internal void ConnectDB()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            List<String> result = new List<string>();
            main.comboxTables.Items.Clear();
            try
            {
                Preparation preparation = new Preparation();
                if (!preparation.success)
                    return;
                MySqlCommand cmd = new MySqlCommand("select table_name from INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' and TABLE_SCHEMA ='habr'", preparation.connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader.GetString(0));
                }
                reader.Close();
                preparation.connection.Close();
                foreach (string text in result)
                    main.comboxTables.Items.Add(text);
                BrushConverter bc = new BrushConverter();
                main.textBox.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                main.textBox.IsReadOnly = false;
                main.convertingJSON.Visibility = Visibility.Visible;

                main.buttonConnectToDb.Content = " Update DB ";
                MessageBox.Show("Connection successful!");
            }
            catch (Exception e_connectButton)
            {
                if (e_connectButton.Source != null)
                {
                    Console.WriteLine("IOException source: {0}", e_connectButton.Source);
                    main.buttonConnectToDb.Content = " Connect to DB ";
                    MessageBox.Show("Error!");
                }
                return;
            }
        }
        internal void ComboxSelection(object sender)
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();

            var selectedcomboitem = sender as ComboBox;
            selectedTableName = selectedcomboitem.SelectedItem as string; //Name of selected table 
            try
            {
                Preparation preparation = new Preparation();
                if (!preparation.success)
                    return;
                MySqlDataAdapter adp = new MySqlDataAdapter($"select * from {selectedTableName}", preparation.connection);
                dataset = new DataSet();
                adp.Fill(dataset);
                DataTable datatable = dataset.Tables[0];
                main.dataGridTable.DataContext = datatable;
                preparation.connection.Close();
            }
            catch (Exception e_selectTable)
            {
                if (e_selectTable.Source != null)
                    Console.WriteLine("IOException source: {0}", e_selectTable.Source);
                return;
            }
        }
        internal void ExecuteCommand()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();

            if (main.textBox.IsReadOnly)
            {
                MessageBox.Show("You haven't connected to the DB!");
                return;
            }
            else if (main.textBox.Text == "")
            {
                MessageBox.Show("You haven't entered a command!");
                return;
            }

            try
            {
                Preparation preparation = new Preparation();
                if (!preparation.success)
                    return;
                string textQuery = main.textBox.Text;
                var firstWord = main.textBox.Text.Substring(0, textQuery.IndexOf(" "));
                if (firstWord.ToLower() == "select")
                {
                    MySqlDataAdapter adp = new MySqlDataAdapter(textQuery, preparation.connection);
                    dataset = new DataSet();
                    adp.Fill(dataset);
                    DataTable datatable = dataset.Tables[0];
                    main.dataGridTable.DataContext = datatable;
                    preparation.connection.Close();

                    main.textBox.Text = "";
                    MessageBox.Show("Selection was successful!");
                    return;
                }
                else MessageBox.Show("Can't recognize this query!");
            }
            catch (Exception e_executeDB)
            {
                if (e_executeDB.Source != null)
                {
                    Console.WriteLine("IOException source: {0}", e_executeDB.Source);
                    MessageBox.Show("Can't execute this command!");
                }
                return;
            }

        }
    }
}
    
    