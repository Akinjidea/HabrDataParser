using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using AngleSharp.Html.Parser;

namespace Diplom
{
    internal class Parsing
    {
        internal void CheckArticleFunction(object sender)
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();

            RadioButton pressed = (RadioButton)sender;
            BrushConverter bc = new BrushConverter();

            main.firstParsingArticleText.Clear();
            main.lastParsingArticleText.Clear();
            main.firstParsingArticleText.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            main.lastParsingArticleText.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            main.firstParsingArticleText.IsReadOnly = false;
            main.lastParsingArticleText.IsReadOnly = false;
            main.checkComments.IsChecked = false;
            main.checkComments.IsEnabled = true;

            main.firstParsingUserText.Clear();
            main.lastParsingUserText.Clear();
            main.singleParsingUserText.Clear();
            main.firstParsingUserText.Background = (Brush)bc.ConvertFrom("#EEEEEE");
            main.lastParsingUserText.Background = (Brush)bc.ConvertFrom("#EEEEEE");
            main.singleParsingUserText.Background = (Brush)bc.ConvertFrom("#EEEEEE");
            main.firstParsingUserText.IsReadOnly = true;
            main.lastParsingUserText.IsReadOnly = true;
            main.singleParsingUserText.IsReadOnly = true;

            main.radioButtonUserMulti.IsEnabled = false;
            main.radioButtonUserSingle.IsEnabled = false;
        }
        internal void CheckUserFunction(object sender)
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();

            RadioButton pressed = (RadioButton)sender;
            BrushConverter bc = new BrushConverter();

            main.firstParsingArticleText.Clear();
            main.lastParsingArticleText.Clear();
            main.firstParsingArticleText.Background = (Brush)bc.ConvertFrom("#EEEEEE");
            main.lastParsingArticleText.Background = (Brush)bc.ConvertFrom("#EEEEEE");
            main.firstParsingArticleText.IsReadOnly = true;
            main.lastParsingArticleText.IsReadOnly = true;
            main.checkComments.IsEnabled = false;
            main.checkComments.IsChecked = false;

            main.firstParsingUserText.Clear();
            main.lastParsingUserText.Clear();
            main.singleParsingUserText.Clear();
            main.firstParsingUserText.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            main.lastParsingUserText.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            main.singleParsingUserText.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            main.firstParsingUserText.IsReadOnly = false;
            main.lastParsingUserText.IsReadOnly = false;
            main.singleParsingUserText.IsReadOnly = false;

            main.radioButtonUserMulti.IsEnabled = true;
            main.radioButtonUserSingle.IsEnabled = true;
        }
        internal void CheckUserMultiFunction(object sender)
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();

            RadioButton pressed = (RadioButton)sender;
            BrushConverter bc = new BrushConverter();

            main.firstParsingUserText.Clear();
            main.lastParsingUserText.Clear();
            main.singleParsingUserText.Clear();
            main.firstParsingUserText.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            main.lastParsingUserText.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            main.singleParsingUserText.Background = (Brush)bc.ConvertFrom("#EEEEEE");
            main.firstParsingUserText.IsReadOnly = false;
            main.lastParsingUserText.IsReadOnly = false;
            main.singleParsingUserText.IsReadOnly = true;
        }
        internal void CheckUserSingleFunction(object sender)
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();

            RadioButton pressed = (RadioButton)sender;
            BrushConverter bc = new BrushConverter();

            main.firstParsingUserText.Clear();
            main.lastParsingUserText.Clear();
            main.singleParsingUserText.Clear();
            main.firstParsingUserText.Background = (Brush)bc.ConvertFrom("#EEEEEE");
            main.lastParsingUserText.Background = (Brush)bc.ConvertFrom("#EEEEEE");
            main.singleParsingUserText.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            main.firstParsingUserText.IsReadOnly = true;
            main.lastParsingUserText.IsReadOnly = true;
            main.singleParsingUserText.IsReadOnly = false;
        }

        internal void ParsingArticles()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            Preparation preparation = new Preparation();
            if (!preparation.success)
                return;
            main.textParsingData.Text = "";
            if (main.firstParsingArticleText.Text.Equals("") && main.lastParsingArticleText.Text.Equals(""))
            {
                MessageBox.Show("You haven't input values!");
                return;
            }
            int first = Convert.ToInt32(main.firstParsingArticleText.Text);
            int last = Convert.ToInt32(main.lastParsingArticleText.Text);

            for (int i = first; i <= last; i++)
            {
                try
                {
                    main.textParsingData.Text = main.textParsingData.Text + $"\n{ i}////////////////////////////////////////////////////{i}\n";

                    //Check is exist in DB
                    MySqlCommand mySqlCommand = new MySqlCommand($"select * from articles where id = {i}", preparation.connection);
                    object checkId = mySqlCommand.ExecuteScalar();
                    if (Convert.ToInt32(checkId) > 0)
                    {
                        main.textParsingData.Text = main.textParsingData.Text + "     This article has already been parsed!\n";
                        continue;
                    }

                    //Preparation
                    System.Threading.Thread.Sleep(1000);
                    var httpData = preparation.client.DownloadString("https://habr.com/ru/post/" + i);
                    var parser = new HtmlParser();
                    var document = parser.ParseDocument(httpData);
                    bool validUrl = true;

                    //Check URL
                    var urlName = document.QuerySelectorAll("head > meta:nth-child(n+6):nth-child(-n+10)");
                    foreach (var item in urlName)
                    {
                        if (item.GetAttribute("property") == "og:url")
                        {
                            int count = item.GetAttribute("content").Split('/').Count();
                            if (i == Convert.ToInt32(item.GetAttribute("content").Split('/')[count - 2]))
                            {
                                main.textParsingData.Text = main.textParsingData.Text + $"     This is {i} URL. Its correct!\n";
                                break;
                            }
                            else
                            {
                                main.textParsingData.Text = main.textParsingData.Text + $"     This is {i} URL. Its not valid!\n";
                                validUrl = false;
                                SaveToDB(i, -2, preparation.connection);
                                continue;
                            }
                        }
                    }

                    if (!validUrl)
                        continue;

                    //Parsing
                    var authorArticle = document.QuerySelector("div > header > a > span.user-info__nickname.user-info__nickname_small");
                    var dateArticle = document.QuerySelector("div > header > span");
                    var nameArticle = document.QuerySelector("div > h1 > span.post__title-text");
                    var rateArticle = document.QuerySelector("li.post-stats__item.post-stats__item_voting-wjt > div > span");
                    var bookmarkArticle = document.QuerySelector("li.post-stats__item.post-stats__item_bookmark > button > span > span");
                    var viewsArticle = document.QuerySelector("li.post-stats__item.post-stats__item_views > div > span");

                    DateTime datetime = DateTime.Parse(dateArticle.GetAttribute("data-time_published"));
                    //To DB
                    mySqlCommand = new MySqlCommand("insert into articles(id, nameAuthor, dateArticle, nameArticle, ratingArticle, bookmarksArticle, viewsArticle, code)" +
                        "values (@1,@2,@3,@4,@5,@6,@7,@8)", preparation.connection);
                    mySqlCommand.Parameters.AddWithValue("@1", i);
                    mySqlCommand.Parameters.AddWithValue("@2", authorArticle.TextContent);
                    mySqlCommand.Parameters.AddWithValue("@3", datetime);
                    mySqlCommand.Parameters.AddWithValue("@4", nameArticle.TextContent);
                    mySqlCommand.Parameters.AddWithValue("@5", rateArticle.TextContent);
                    mySqlCommand.Parameters.AddWithValue("@6", bookmarkArticle.TextContent);
                    mySqlCommand.Parameters.AddWithValue("@7", viewsArticle.TextContent);
                    mySqlCommand.Parameters.AddWithValue("@8", 0);
                    mySqlCommand.ExecuteNonQuery();


                    //Result                    
                    main.textParsingData.Text = main.textParsingData.Text + $"Article id:  {i}\n";
                    main.textParsingData.Text = main.textParsingData.Text + $"Author's name:  { authorArticle.TextContent}\n";
                    main.textParsingData.Text = main.textParsingData.Text + $"Article date:  {datetime}\n";
                    main.textParsingData.Text = main.textParsingData.Text + $"Name of the article:  {nameArticle.TextContent}\n";
                    main.textParsingData.Text = main.textParsingData.Text + $"Article rating:  {rateArticle.TextContent}\n";
                    main.textParsingData.Text = main.textParsingData.Text + $"Number of bookmarks:  {bookmarkArticle.TextContent}\n";
                    main.textParsingData.Text = main.textParsingData.Text + $"Number of views:  {viewsArticle.TextContent}\n";

                    if (main.checkComments.IsChecked == true)
                    {
                        main.textParsingData.Text = main.textParsingData.Text + "\n       Comments:     \n\n";

                        var totalComments = document.QuerySelector("#comments_count");
                        var nameComments = document.QuerySelectorAll("div.comment__head > a > span");
                        var dateComments = document.QuerySelectorAll("div.comment__head > time");
                        var ratingComments = document.QuerySelectorAll("div.comment__head > div > span");

                        int length = nameComments.Length;
                        main.textParsingData.Text = main.textParsingData.Text + $"Total count of comments:     {totalComments.TextContent}\n";
                        for (int j = 0; j < length; j++)
                        {
                            string result = "";                            
                            if (dateComments[j].TextContent.Contains("сегодня"))
                            {
                                result = dateComments[j].TextContent.Remove(dateComments[j].TextContent.IndexOf(" в"), " в".Length);
                                string day = DateTime.Today.ToString("yyyy-MM-dd");
                                result = result.Replace("сегодня", day);
                            }
                            else if (dateComments[j].TextContent.Contains("вчера"))
                            {
                                result = dateComments[j].TextContent.Remove(dateComments[j].TextContent.IndexOf(" в"), " в".Length);
                                string day = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                                result = result.Replace("вчера", day);
                            }
                            else if (dateComments[j].TextContent.Contains("в"))
                                result = dateComments[j].TextContent.Remove(dateComments[j].TextContent.IndexOf(" в"), " в".Length);

                            DateTime datetimeComment = DateTime.Parse(result);
                            mySqlCommand = new MySqlCommand("insert into comments(idArticle,nameComment,dateComment,rating) values (@1,@2,@3,@4)", preparation.connection);
                            mySqlCommand.Parameters.AddWithValue("@1", i);
                            mySqlCommand.Parameters.AddWithValue("@2", nameComments[j].TextContent);
                            mySqlCommand.Parameters.AddWithValue("@3", datetimeComment);
                            mySqlCommand.Parameters.AddWithValue("@4", ratingComments[j].TextContent);
                            mySqlCommand.ExecuteNonQuery();
                            main.textParsingData.Text = main.textParsingData.Text + $"    {j} comment. Name:   {nameComments[j].TextContent}\n";
                            main.textParsingData.Text = main.textParsingData.Text + $"    {j} comment. Date:   {dateComments[j].TextContent}\n";
                            main.textParsingData.Text = main.textParsingData.Text + $"    {j} comment. Rating:   {ratingComments[j].TextContent}\n";
                        }

                        main.textParsingData.Text = main.textParsingData.Text + "\n       Parsing of comments complete!\n";
                    }

                    main.textParsingData.Text = main.textParsingData.Text + "Success! \n";
                }
                catch (WebException wex)
                {
                    if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.Forbidden)
                    {
                        SaveToDB(i, -1, preparation.connection);
                        main.textParsingData.Text = main.textParsingData.Text + $"     This is {i} URL. The access is denied!\n";
                        continue;
                    }
                    if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        SaveToDB(i, -3, preparation.connection);
                        main.textParsingData.Text = main.textParsingData.Text + $"     This is {i} URL. The page is not found!\n";
                        continue;
                    }
                }
                catch(MySqlException)
                {
                    MessageBox.Show("Connection was denied!");
                    return;
                }
                finally
                {
                    GC.Collect();
                }
            }
            preparation.connection.Close();
            MessageBox.Show("Parsing complete!");
            main.firstParsingArticleText.Text = "";
            main.lastParsingArticleText.Text = "";
        }
        private void SaveToDB(int i, int code, MySqlConnection connection)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand($"insert into articles(id, nameAuthor, dateArticle, nameArticle, ratingArticle, bookmarksArticle, viewsArticle, code)" +
                            $"values ({i},'null','0001-01-01 00:00:00','null','0','0','0', {code})", connection);
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Connection was denied!");
                return;
            }
        }

        internal void ParsingUserMulti()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            Preparation preparation = new Preparation();
            if (!preparation.success)
                return;
            main.textParsingData.Text = "";
            if (main.firstParsingUserText.Text.Equals("") && main.lastParsingUserText.Text.Equals(""))
            {
                MessageBox.Show("You haven't input values!");
                return;
            }
            int first = Convert.ToInt32(main.firstParsingUserText.Text);
            int last = Convert.ToInt32(main.lastParsingUserText.Text);

            for (int i = first; i <= last; i++)
            {
                main.textParsingData.Text = main.textParsingData.Text + "\n";
                string authorLogin = "";
                MySqlCommand mySqlCommand = new MySqlCommand($"select nameAuthor from articles where id={i}", preparation.connection);
                var mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    authorLogin = mySqlDataReader.GetString(0);
                }
                mySqlDataReader.Close();
                if (authorLogin.Equals("null"))
                    continue;

                main.textParsingData.Text = main.textParsingData.Text + $"\n{i}////////////////////////////////////////////////////{i}\n";

                Kernel(authorLogin, preparation);
            }
            MessageBox.Show("Parsing complete!");
        }
        internal void ParsingUserSingle()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textParsingData.Text = "" + "\n";
            Preparation preparation = new Preparation();
            if (!preparation.success)
                return;
            string authorLogin = main.singleParsingUserText.Text;
            try
            {
                Kernel(authorLogin, preparation);
            }
            catch (WebException exception)
            {
                if (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    MessageBox.Show("\nSorry, current user doesn't exist.");
                }
            }
            main.singleParsingUserText.Text = "";
        }
        private void Kernel(string authorLogin, Preparation preparation)
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();

            try
            {
                var mySqlCommand = new MySqlCommand($"select * from users where id = '{authorLogin}'", preparation.connection);
                object checkId = mySqlCommand.ExecuteScalar();
                if (checkId != null)
                {
                    main.textParsingData.Text = main.textParsingData.Text + $"     This author ({authorLogin}) has already been parsed!\n";
                    return;
                }

                System.Threading.Thread.Sleep(1000);
                var httpData = preparation.client.DownloadString("https://habr.com/ru/users/" + authorLogin);
                var parser = new HtmlParser();
                var document = parser.ParseDocument(httpData);

                var status = document.QuerySelector("div.user-info__about > div.user-info__specialization");
                if(status == null)
                {
                    MessageBox.Show("\nSorry, current user doesn't exist.");
                    return;
                }
                var name = document.QuerySelector("div.user-info__about > div.user-info__links > h1 > a.user-info__fullname.user-info__fullname_medium");
                var karma = document.QuerySelector("div.user-info__stats > div.media-obj.media-obj_user-info > div > a:nth-child(1) > div.stacked-counter__value.stacked-counter__value_green");
                var rating = document.QuerySelector("div.user-info__stats > div.media-obj.media-obj_user-info > div > a.user-info__stats-item.stacked-counter.stacked-counter_rating > div.stacked-counter__value.stacked-counter__value_magenta");
                var subscribers = document.QuerySelector("div.user-info__stats > div.media-obj.media-obj_user-info > div > a.user-info__stats-item.stacked-counter.stacked-counter_subscribers > div.stacked-counter__value.stacked-counter__value_blue");
                var dateRegValue = document.QuerySelector("div.default-block__content.default-block__content_profile-summary > ul > li:nth-last-child(1) > span.defination-list__value");
                var dateActValue = document.QuerySelector("div.default-block__content.default-block__content_profile-summary > ul > li:nth-last-child(2) > span.defination-list__value");
                var dateRegBase = document.QuerySelector("li:nth-last-child(1) > span.defination-list__label.defination-list__label_profile-summary");
                var dateActBase = document.QuerySelector("li:nth-last-child(2) > span.defination-list__label.defination-list__label_profile-summary");

                mySqlCommand = new MySqlCommand("insert into users(id, name, status, karma, rating, subscribers, dateReg, dateActivity)" +
                            "values (@1,@2,@3,@4,@5,@6,@7,@8)", preparation.connection);

                mySqlCommand.Parameters.AddWithValue("@1", authorLogin);
                main.textParsingData.Text = main.textParsingData.Text + $"Author id:  {authorLogin}\n";
                if (name == null)
                {
                    mySqlCommand.Parameters.AddWithValue("@2", "null");
                    main.textParsingData.Text = main.textParsingData.Text + "Author's name:  null\n";
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue("@2", name.TextContent);
                    main.textParsingData.Text = main.textParsingData.Text + $"Author's name:  { name.TextContent}\n";
                }
                mySqlCommand.Parameters.AddWithValue("@3", status.TextContent);
                main.textParsingData.Text = main.textParsingData.Text + $"Author's status:  {status.TextContent}\n";
                if (karma == null)
                {
                    mySqlCommand.Parameters.AddWithValue("@4", 0.0);
                    main.textParsingData.Text = main.textParsingData.Text + "Author's karma:  0,0\n";
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue("@4", karma.TextContent);
                    main.textParsingData.Text = main.textParsingData.Text + $"Author's karma:  {karma.TextContent}\n";
                }
                if (rating == null)
                {
                    mySqlCommand.Parameters.AddWithValue("@5", 0.0);
                    main.textParsingData.Text = main.textParsingData.Text + "Author's rating:  0,0\n";
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue("@5", rating.TextContent);
                    main.textParsingData.Text = main.textParsingData.Text + $"Author's rating:  {rating.TextContent}\n";
                }
                if (subscribers == null)
                {
                    mySqlCommand.Parameters.AddWithValue("@6", 0);
                    main.textParsingData.Text = main.textParsingData.Text + "Author's subscribers:  0\n";
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue("@6", subscribers.TextContent);
                    main.textParsingData.Text = main.textParsingData.Text + $"Author's subscribers:  {subscribers.TextContent}\n";
                }
                
                if(dateRegBase.TextContent == "Зарегистрирован" && dateActBase.TextContent == "Активность")
                {
                    DateTime dateReg = DateTime.Parse(dateRegValue.TextContent).Date;
                    DateTime dateAct = DateTime.Parse(dateActValue.TextContent);
                    mySqlCommand.Parameters.AddWithValue("@7", dateReg);
                    mySqlCommand.Parameters.AddWithValue("@8", dateAct);
                    main.textParsingData.Text = main.textParsingData.Text + $"Author's registration:  {dateReg}\n";
                    main.textParsingData.Text = main.textParsingData.Text + $"Author's activity:  {dateAct}\n";
                }
                else
                {
                    DateTime dateReg = DateTime.Parse(dateRegValue.TextContent).Date;
                    mySqlCommand.Parameters.AddWithValue("@7", dateReg);
                    mySqlCommand.Parameters.AddWithValue("@8", "0001-01-01 00:00:00");
                    main.textParsingData.Text = main.textParsingData.Text + $"Author's registration:  {dateReg}\n";
                    main.textParsingData.Text = main.textParsingData.Text + $"Author's activity:  0001-01-01 00:00:00\n";
                }


                mySqlCommand.ExecuteNonQuery();
            }
            catch (WebException exception)
            {
                if (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    MessageBox.Show("\nSorry, current user doesn't exist.");
                }
                if (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.Forbidden)
                {
                    MessageBox.Show("\nSorry, current user doesn't exist.");
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show("Connection was denied!");
                return;
            }
        }
    }
}
