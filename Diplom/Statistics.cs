using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Diplom
{
    class Statistics
    {
        internal void FirstStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select id, name from users order by rating desc limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
                main.textStatisticsData.Text = $"The most popular user - {reader["id"]}, {reader["name"]}";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }
        internal void SecondStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            object temp = 0;
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select karma from users order by karma desc limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["karma"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select id, name from users where karma = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
                main.textStatisticsData.Text = main.textStatisticsData.Text + $"User with the best karma - {reader["id"]}, {reader["name"]}\n";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }
        internal void ThirdStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            object temp = 0;
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select karma from users order by karma limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["karma"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select id, name from users where karma = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
                main.textStatisticsData.Text = main.textStatisticsData.Text + $"User with the worst karma - {reader["id"]}, {reader["name"]}\n";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }
        internal void ForthStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            object temp = 0;
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select rating from users order by rating desc limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["rating"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select id, name from users where rating = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
                main.textStatisticsData.Text = main.textStatisticsData.Text + $"User with the best rating - {reader["id"]}, {reader["name"]}\n";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }
        internal void FifthStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            object temp = 0;
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select rating from users order by rating limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["rating"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select id, name from users where rating = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
                main.textStatisticsData.Text = main.textStatisticsData.Text + $"User with the worst rating - {reader["id"]}, {reader["name"]}\n";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }
        internal void SixthStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            object temp = 0;
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select ratingArticle from articles order by ratingArticle desc limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["ratingArticle"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select nameAuthor, nameArticle from articles where ratingArticle = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
                main.textStatisticsData.Text = main.textStatisticsData.Text + $"Article with the best rating - {reader["nameArticle"]}, it author is {reader["nameAuthor"]}\n";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }
        internal void SeventhStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            object temp = 0;
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select ratingArticle from articles order by ratingArticle limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["ratingArticle"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select nameAuthor, nameArticle from articles where ratingArticle = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
                if ((string)reader["nameArticle"] == "null")
                    continue;
                else main.textStatisticsData.Text = main.textStatisticsData.Text + $"Article with the worst rating - {reader["nameArticle"]}, it author is {reader["nameAuthor"]}\n";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }
        internal void EighthStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            object temp = 0;
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select rating from comments order by rating desc limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["rating"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select idArticle from comments where rating = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["idArticle"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select nameAuthor, nameArticle from articles where id = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
                main.textStatisticsData.Text = main.textStatisticsData.Text + $"Article with the best comment - {reader["nameArticle"]}, it author is {reader["nameAuthor"]}\n";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }
        internal void NinthStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            object temp = 0;
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select viewsArticle from articles order by viewsArticle desc limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["viewsArticle"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select nameAuthor, nameArticle from articles where viewsArticle = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
                main.textStatisticsData.Text = main.textStatisticsData.Text + $"Article with the most views - {reader["nameArticle"]}, it author is {reader["nameAuthor"]}\n";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }
        internal void TenthStatisticsFun()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
            Preparation preparation = new Preparation();
            object temp = 0;
            if (!preparation.success)
            {
                MessageBox.Show("Error!");
                return;
            }
            MySqlCommand mySqlCommand = new MySqlCommand("select bookmarksArticle from articles order by bookmarksArticle desc limit 1", preparation.connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            if (reader.Read())
            {
                temp = reader["bookmarksArticle"];
            }
            reader.Close();
            mySqlCommand = new MySqlCommand($"select nameAuthor, nameArticle from articles where bookmarksArticle = '{temp}'", preparation.connection);
            reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
                main.textStatisticsData.Text = main.textStatisticsData.Text + $"The most preserved article - {reader["nameArticle"]}, it author is {reader["nameAuthor"]}\n";
            MessageBox.Show("Success!");
            preparation.connection.Close();
        }

        internal void ExecuteQueryStatistics()
        {            
        }
        internal void CleanStatisticssWindow()
        {
            Main main = Application.Current.Windows.OfType<Main>().FirstOrDefault();
            main.textStatisticsData.Text = "";
        }
    }
}
