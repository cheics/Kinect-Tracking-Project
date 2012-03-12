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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Data;
using System.ComponentModel;
using System.Collections;
using Microsoft.Windows.Controls;
using MongoDB.Bson;
using MongoDB.Driver;
using Visiblox.Charts;


namespace UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            comboBox2.Items.Add("Squat");
            comboBox2.Items.Add("Knee Raise");
            comboBox2.Items.Add("Arm Raise");
            comboBox2.Items.Add("Lateral Leg Extension");
                                    
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox1.Items.Clear();

            if (comboBox2.SelectedItem == "Squat")
            {
                comboBox1.Items.Add("Squat Depth");
                comboBox1.Items.Add("Straight Back");
                comboBox1.Items.Add("Balance");

            }
            else if (comboBox2.SelectedItem == "Knee Raise")
            {
                comboBox1.Items.Add("Knee angle");
                comboBox1.Items.Add("Spine Stability");
                comboBox1.Items.Add("Knee out");
            }
            else if (comboBox2.SelectedItem == "Arm Raise")
            {
                comboBox1.Items.Add("Straight arm");
                comboBox1.Items.Add("Hands in front");
                comboBox1.Items.Add("Front shoulder angle");
            }
            else if (comboBox2.SelectedItem == "Lateral Leg Extension")
            {
                comboBox1.Items.Add("Spine stability");
                comboBox1.Items.Add("Hip level");
                comboBox1.Items.Add("Hip angle");

            }
            else
                comboBox1.Items.Add("");
        }
        public class Party
        {
            public string Name { set; get; }
            public int Votes { set; get; }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var query = new QueryDocument();
           // query = 
           // var query = Query.EQ("Name", "Mehrad");
            MongoServer server = MongoServer.Create();
            MongoDatabase exerHist = server.GetDatabase("exerHist");
            MongoCollection<BsonDocument> exercises = exerHist.GetCollection<BsonDocument>("exercises");
            MongoCollection<BsonDocument> hostory = exerHist.GetCollection<BsonDocument>("hostory");

            DataSeries<double, double> cc1 = new DataSeries<double, double>();
            DataSeries<double, double> cc2 = new DataSeries<double, double>();
            DataSeries<double, double> cc3 = new DataSeries<double, double>();

            foreach (BsonDocument instance in hostory.Find(query))
            {
                cc1.Add(new DataPoint<double,double>(){Y = instance["cc1"].ToInt32(),X= instance["time"].ToInt32() });
                cc2.Add(new DataPoint<double, double>() { Y = instance["cc2"].ToInt32(), X = instance["time"].ToInt32() });
                cc3.Add(new DataPoint<double, double>() { Y = instance["cc3"].ToInt32(), X = instance["time"].ToInt32() });
            }

            example.Series[0].DataSeries = null;
            example.Series[1].DataSeries = null;
            example.Series[2].DataSeries = null;

            if (comboBox1.SelectedIndex == 0)
                example.Series[0].DataSeries = cc1;
            else if (comboBox1.SelectedIndex == 1)      
                example.Series[1].DataSeries = cc2;
            else if (comboBox1.SelectedIndex == 2)
                example.Series[2].DataSeries = cc3;
            else
            {
                example.Series[0].DataSeries = cc1;
                example.Series[1].DataSeries = cc2;
                example.Series[2].DataSeries = cc3;
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}
