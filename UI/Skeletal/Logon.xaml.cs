using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Text.RegularExpressions;

namespace SkeletalViewer
{
    /// <summary>
    /// Interaction logic for Logon.xaml
    /// </summary>
    public partial class Logon : Window
    {
        ArrayList searchList = new ArrayList();
        char[] charsToTrim = { ' ' };
        string newName;
        MongoCollection<BsonDocument> names;
        public Logon()
        {
            InitializeComponent();
            ArrayList list = new ArrayList();
            MongoServer server = MongoServer.Create();
            MongoDatabase exerHist = server.GetDatabase("exerHist");
            names = exerHist.GetCollection<BsonDocument>("names");
            var query = new QueryDocument();
            foreach (BsonDocument name in names.Find(query))
            {
                string nameToAdd = name["name"].ToString();
                list.Add(nameToAdd);
                searchList.Add(nameToAdd.ToLower());
            }
            list.Sort();
            nameCB.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addNewUser();

            exerciseInterface();
        }

        private void nameCB_KeyUp(object sender, KeyEventArgs e)
        {
            // trim leading/trailing white spaces
            ComboBox cb = (ComboBox)sender;
            string comparison = cb.Text;
            comparison = comparison.Trim(charsToTrim);
            // replace intermediate, multiple white spaces with one white space
            string pattern = "\\s+";
            string replacement = " ";
            Regex rgx = new Regex(pattern);
            comparison = rgx.Replace(comparison, replacement);
            newName = comparison;
            // make it to lower case
            comparison = comparison.ToLower();
            if (searchList.IndexOf(comparison) == -1 & comparison != "")
            {
                enter.IsEnabled = true;
                arrow.Opacity = 1;
            }
            else
            {
                enter.IsEnabled = false;
                arrow.Opacity = 0.5;
            }
        }

        private void nameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox cb = (ComboBox)sender;
            newName = cb.SelectedValue.ToString();

            exerciseInterface();
        }

        private void exerciseInterface()
        {
            

            MainWindow main = new MainWindow();
            main.passName(newName);
            App.Current.MainWindow = main;
            main.Show();
            this.Close();
        }

        private void addNewUser()
        {

            BsonDocument name = new BsonDocument {
                { "name", newName }
                };
            names.Insert(name);
        }

        private void nameCB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & enter.IsEnabled)
            {
                addNewUser();

                exerciseInterface();
            }
        }
    }
}
