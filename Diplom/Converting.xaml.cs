using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;

namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для Converting.xaml
    /// </summary>
    public partial class Converting : Window
    {
        private const string INDENT_STRING = "    ";
        private string selectedTableName;

        public Converting()
        {
            InitializeComponent();
        }
        internal void GetItems()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            int count = main.comboxTables.Items.Count;
            for (int i = 0; i < count; i++)
            {
                comboxDB.Items.Add(main.comboxTables.Items[i]);
            }
        }

        private List<int> GetNumbers()
        {
            string[] dateSplit = boxConvertingData.Text.Split(',');
            List<int> numbers = new List<int>();
            foreach (string value in dateSplit)
            {
                if (value.Contains('-'))
                {
                    int s = value.IndexOf('-');
                    if (s == 0)
                    {
                        MessageBox.Show("Incorrect data!");
                        break;
                    }
                    else if (value.IndexOf('-') != value.LastIndexOf('-'))
                    {
                        MessageBox.Show("Too many characters!");
                        break;
                    }
                    else
                    {
                        int first = Convert.ToInt32(value.Split('-').First());
                        int last = Convert.ToInt32(value.Split('-').Last());
                        if (first > last)
                        {
                            MessageBox.Show("Incorrect value!");
                            break;
                        }
                        for (int i = first; i <= last; i++)
                        {
                            numbers.Add(i);
                        }
                    }
                }
                else if (value.Equals(""))
                    continue;
                else numbers.Add(Convert.ToInt32(value));
                Console.WriteLine($"Value - {value}");
            }
            numbers = numbers.Distinct().ToList();
            numbers.Sort();

            return numbers;
        }
        static string FormatJson(string json)
        {

            int indentation = 0;
            int quoteCount = 0;
            var result =
                from ch in json
                let quotes = ch == '"' ? quoteCount++ : quoteCount
                let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, indentation)) : null
                let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, ++indentation)) : ch.ToString()
                let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, --indentation)) + ch : ch.ToString()
                select lineBreak == null
                            ? openChar.Length > 1
                                ? openChar
                                : closeChar
                            : lineBreak;

            return String.Concat(result);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextBoxPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
        private void ComboxSelectionJS(object sender, SelectionChangedEventArgs e)
        {
            var selectedcomboitem = sender as ComboBox;
            selectedTableName = selectedcomboitem.SelectedItem as string;

            boxConvertingData.IsReadOnly = false;
            BrushConverter bc = new BrushConverter();
            boxConvertingData.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        }

        private void ConvertingChoosen(object sender, RoutedEventArgs e)
        {
            if (selectedTableName == null)
            {
                MessageBox.Show("You haven't choosed DB's table!");
                return;
            }

            if (boxConvertingData.Text.Equals("0"))
            {
                MessageBox.Show("You haven't input data for converting!");
                return;
            }
            if (boxConvertingData.Text.Equals(""))
            {
                MessageBox.Show("You haven't input data for converting!");
                return;
            }


            Preparation preparation = new Preparation();
            if (!preparation.success)
                return;

            List<int> numbers = GetNumbers();

            if (selectedTableName == "articles")
            {
                ConvertingArticles(numbers, preparation);
            }
            else if (selectedTableName == "users")
            {
                ConvertingUsers(numbers, preparation);
            }
            else if (selectedTableName == "comments")
            {
                ConvertingComments(numbers, preparation);
            }

            MessageBox.Show("Success!");
            return;
        }

        private void ConvertingArticles(List<int> numbers, Preparation preparation)
        {
            try
            {
                MySqlCommand mySqlCommand;
                MySqlDataReader reader;
                string json = null;
                long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + milliseconds + ".json";

                foreach (int i in numbers)
                {
                    mySqlCommand = new MySqlCommand($"select * from articles where id='{i}'", preparation.connection);
                    reader = mySqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        Article article;
                        if (reader["code"].Equals(-2) || reader["code"].Equals(-1) || reader["code"].Equals(-3))
                        {
                            article = new Article
                            {
                                id = null,
                                nameAuthor = null,
                                dateArticle = null,
                                nameArticle = null,
                                rating = null,
                                bookmarks = null,
                                views = null,
                                code = reader["code"]
                            };
                        }
                        else
                        {
                            article = new Article
                            {
                                id = reader["id"],
                                nameAuthor = reader["nameAuthor"],
                                dateArticle = reader["dateArticle"].ToString(),
                                nameArticle = reader["nameArticle"],
                                rating = reader["ratingArticle"],
                                bookmarks = reader["bookmarksArticle"],
                                views = reader["viewsArticle"],
                                code = reader["code"],
                                comments = new List<Comments>()                            
                            };
                            reader.Close();

                            mySqlCommand = new MySqlCommand($"select * from comments where idArticle='{i}'", preparation.connection);
                            reader = mySqlCommand.ExecuteReader();
                            while(reader.Read())
                            {
                                article.comments.Add(new Comments()
                                {
                                    id = reader["id"],
                                    idArticle = reader["idArticle"],
                                    nameComment = reader["nameComment"],
                                    dateComment = reader["dateComment"].ToString(),
                                    rating = reader["rating"]
                                });
                            }
                        }

                        json = json + new JavaScriptSerializer().Serialize(article) + "\n";
                    }
                    else
                    {
                        MessageBox.Show("Can't find data!");
                        return;
                    }
                    reader.Close();
                }
                json = FormatJson(json);
                System.IO.File.WriteAllText(path, json);
            }
            catch(MySqlException)
            {
                MessageBox.Show("Error while connected to DB");
            }
            preparation.connection.Close();
        }
        private void ConvertingUsers(List<int> numbers, Preparation preparation)
        {
            try
            {
                MySqlCommand mySqlCommand;
                MySqlDataReader reader;
                string json = null;
                long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + milliseconds + ".json";
                foreach (int i in numbers)
                {
                    mySqlCommand = new MySqlCommand($"select * from users where number = '{i}'", preparation.connection);
                    reader = mySqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        User user = new User
                        {
                            id = reader["id"],
                            name = reader["name"],
                            status = reader["status"],
                            karma = reader["karma"],
                            rating = reader["rating"],
                            subscribers = reader["subscribers"],
                            dateReg = reader["dateReg"].ToString(),
                            dateActivity = reader["dateActivity"].ToString()
                        };
                        json = json + new JavaScriptSerializer().Serialize(user) + "\n";
                    }
                    else
                    {
                        MessageBox.Show("Can't find data!");
                        return;
                    }
                    reader.Close();
                }
                json = FormatJson(json);
                System.IO.File.WriteAllText(path, json);
            }
            catch (MySqlException)
            {
                MessageBox.Show("Error while connected to DB");
            }
            preparation.connection.Close();
        }
        private void ConvertingComments(List<int> numbers, Preparation preparation)
        {
            try
            {
                MySqlCommand mySqlCommand;
                MySqlDataReader reader;
                string json = null;
                long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + milliseconds + ".json";
                foreach (int i in numbers)
                {
                    mySqlCommand = new MySqlCommand($"select * from comments where id = '{i}'", preparation.connection);
                    reader = mySqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        Comments comments = new Comments
                        {
                            id = reader["id"],
                            idArticle = reader["idArticle"],
                            nameComment = reader["nameComment"],
                            dateComment = reader["dateComment"].ToString(),
                            rating = reader["rating"],
                        };
                        json = json + new JavaScriptSerializer().Serialize(comments) + "\n";
                    }
                    else
                    {
                        MessageBox.Show("Can't find data!");
                        return;
                    }
                    reader.Close();                    
                }
                json = FormatJson(json);
                System.IO.File.WriteAllText(path, json);
            }
            catch (MySqlException)
            {
                MessageBox.Show("Error while connected to DB");
            }
            preparation.connection.Close();
        }        
    }
}
