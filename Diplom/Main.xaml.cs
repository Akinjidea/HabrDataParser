using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        private void StopProgram(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        //Viewing tab
        private void ConnectDB(object sender, RoutedEventArgs e)
        {
            Viewing viewing = new Viewing();
            viewing.ConnectDB();
        }
        private void ComboxSelection(object sender, SelectionChangedEventArgs e)
        {
            Viewing viewing = new Viewing();
            viewing.ComboxSelection(sender);
        }
        private void ExecuteCommand(object sender, RoutedEventArgs e)
        {
            Viewing viewing = new Viewing();
            viewing.ExecuteCommand();
        }
        //Save as JSON
        private void SaveJSON(object sender, RoutedEventArgs e)
        {
            int count = comboxTables.Items.Count;
            Converting converting = new Converting();
            converting.Show();
            converting.GetItems();
        }

        //Statistics
        private void FirstStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.FirstStatisticsFun();
        }
        private void SecondStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.SecondStatisticsFun();
        }
        private void ThirdStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.ThirdStatisticsFun();
        }
        private void ForthStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.ForthStatisticsFun();
        }
        private void FifthStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.FifthStatisticsFun();
        }
        private void SixthStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.SixthStatisticsFun();
        }
        private void SeventhStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.SeventhStatisticsFun();
        }
        private void EighthStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.EighthStatisticsFun();
        }
        private void NinthStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.NinthStatisticsFun();
        }
        private void TenthStatisticsFun(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.TenthStatisticsFun();
        }
        private void ExecuteQueryStatistics(object sender, RoutedEventArgs e)
        {

        }
        private void CleanStatisticssWindow(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.CleanStatisticssWindow();
        }

        //Parsing tab
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
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
        private void CheckArticleFunction(object sender, RoutedEventArgs e)
        {
            Parsing parsing = new Parsing();
            parsing.CheckArticleFunction(sender);
        }
        private void CheckUserFunction(object sender, RoutedEventArgs e)
        {
            Parsing parsing = new Parsing();
            parsing.CheckUserFunction(sender);
        }
        private void CheckUserMultiFunction(object sender, RoutedEventArgs e)
        {
            Parsing parsing = new Parsing();
            parsing.CheckUserMultiFunction(sender);
        }
        private void CheckUserSingleFunction(object sender, RoutedEventArgs e)
        {
            Parsing parsing = new Parsing();
            parsing.CheckUserSingleFunction(sender);
        }
        
        private void ParsingData(object sender, RoutedEventArgs e)
        {
            Parsing parsing = new Parsing();

            if (radioButtonArticle.IsChecked == true)
            {
                parsing.ParsingArticles();
            }
            else if (radioButtonUser.IsChecked == true)
            {
                if (radioButtonUserMulti.IsChecked == true)
                {
                    parsing.ParsingUserMulti();
                }
                else if (radioButtonUserSingle.IsChecked == true)
                {
                    parsing.ParsingUserSingle();
                }
                else MessageBox.Show("You haven't select secondary mode!");
            }
            else MessageBox.Show("You haven't select main mode!");
        }        
    }
}
