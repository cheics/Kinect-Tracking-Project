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
//using System.Windows.Forms;
using System.Data;
using System.ComponentModel;
using System.Collections;
using Microsoft.Windows.Controls;
using MongoDB.Bson;
using MongoDB.Driver;
using Visiblox.Charts;
using System.Collections.Generic;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Wrappers;


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
        public DateTime RandomDay()
        {
            DateTime start = new DateTime(2011, 1, 1);
            Random gen = new Random();

            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return start.AddDays(gen.Next(range));
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox2.SelectedValue != null)
            {
                example.Series[0].DataSeries = null;
                example.Series[1].DataSeries = null;
                example.Series[2].DataSeries = null;

                var query = new QueryDocument();

                MongoServer server = MongoServer.Create();
                MongoDatabase exerHist = server.GetDatabase("exerHist");
                MongoCollection<BsonDocument> exercises = exerHist.GetCollection<BsonDocument>("exercises");
                MongoCollection<BsonDocument> hostory = exerHist.GetCollection<BsonDocument>("hostory");


                int b = 0;
                while (b < 200)
                {
                    Random random = new Random();
                    int randomNumber1 = random.Next(0, 3);
                    int randomNumber2 = random.Next(0, 3);
                    int randomNumber3 = random.Next(0, 3);
                    BsonDocument experiments = new BsonDocument { {"name", "Mehrad"}, {"exercise", "Squat"}, {"cc1",randomNumber1 } , {"cc2", randomNumber2},
                    {"cc3", randomNumber3}, {"time",RandomDay() }};
                    hostory.Insert(experiments);
                    b++;
                }


                DataSeries<DateTime, double> cc1 = new DataSeries<DateTime, double>(comboBox1.Items[0].ToString());
                DataSeries<DateTime, double> cc2 = new DataSeries<DateTime, double>(comboBox1.Items[1].ToString());
                DataSeries<DateTime, double> cc3 = new DataSeries<DateTime, double>(comboBox1.Items[2].ToString());

                // var sortBy = SortBy.Ascending("time");

                // hostory.Find().SetSortOrder(sortBy.Descending("time"));
                IMongoSortBy sort = SortBy.Descending("time");

                var querytest2 = Query.And(Query.EQ("exercise", comboBox2.SelectedValue.ToString()), Query.EQ("name", "Mehrad"));

                foreach (BsonDocument instance in hostory.Find(querytest2).SetSortOrder(sort))
                {
                    cc1.Add(new DataPoint<DateTime, double>() { Y = instance["cc1"].ToInt32(), X = instance["time"].AsDateTime });
                    cc2.Add(new DataPoint<DateTime, double>() { Y = instance["cc2"].ToInt32(), X = instance["time"].AsDateTime });
                    cc3.Add(new DataPoint<DateTime, double>() { Y = instance["cc3"].ToInt32(), X = instance["time"].AsDateTime });
                }


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
                cc1 = null;
                cc2 = null;
                cc3 = null;
            }
            else
            {
                MessageBox.Show("Please select an exercise");
            }

       }
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void calendar2_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox2.SelectedValue != null)
            {
                DataSeries<int, int> Dcc1 = new DataSeries<int, int>(comboBox1.Items[0].ToString());
                DataSeries<int, int> Dcc2 = new DataSeries<int, int>(comboBox1.Items[1].ToString());
                DataSeries<int, int> Dcc3 = new DataSeries<int, int>(comboBox1.Items[2].ToString());

                string connectionstring = "mongodb://CHEICS-PC";
                MongoServer server = MongoServer.Create(connectionstring);
                MongoDatabase exerHist = server.GetDatabase("exerHist");
                MongoCollection<BsonDocument> exercises = exerHist.GetCollection<BsonDocument>("exercises");
                MongoCollection<BsonDocument> hostory = exerHist.GetCollection<BsonDocument>("hostory");

                var query = new QueryDocument();
                int cc11 = 0, cc12 = 0, cc10 = 0, cc20 = 0, cc21 = 0, cc22 = 0, cc30 = 0, cc31 = 0, cc32 = 0;

                var querytest1 = Query.And(Query.EQ("exercise", comboBox2.SelectedValue.ToString()), Query.EQ("name", "Mehrad"));

                foreach (BsonDocument instance in hostory.Find(querytest1))
                {
                    string date = null;
                    string selecteddate = null;
                    DateTime daymonth;
                    DateTime selecteddaymonth;


                    daymonth = instance["time"].AsDateTime;
                    date = daymonth.Day.ToString() + "/" + daymonth.Month.ToString() + "/" + daymonth.Year.ToString();
                    selecteddaymonth = calendar2.SelectedDate.Value;
                    selecteddate = selecteddaymonth.Day.ToString() + "/" + selecteddaymonth.Month.ToString() + "/" + selecteddaymonth.Year.ToString();

                    if (date == selecteddate)
                    {
                        if (instance["cc1"].ToInt32() == 0)
                            cc10++;
                        if (instance["cc1"].ToInt32() == 1)
                            cc11++;
                        if (instance["cc1"].ToInt32() == 2)
                            cc12++;
                        if (instance["cc2"].ToInt32() == 0)
                            cc20++;
                        if (instance["cc2"].ToInt32() == 1)
                            cc21++;
                        if (instance["cc2"].ToInt32() == 2)
                            cc22++;
                        if (instance["cc3"].ToInt32() == 0)
                            cc30++;
                        if (instance["cc2"].ToInt32() == 1)
                            cc31++;
                        if (instance["cc2"].ToInt32() == 2)
                            cc32++;
                    }
                }

                Dcc1.Add(new DataPoint<int, int>() { Y = cc10, X = 0 });
                Dcc1.Add(new DataPoint<int, int>() { Y = cc11, X = 1 });
                Dcc1.Add(new DataPoint<int, int>() { Y = cc12, X = 2 });

                Dcc2.Add(new DataPoint<int, int>() { Y = cc20, X = 0 });
                Dcc2.Add(new DataPoint<int, int>() { Y = cc21, X = 1 });
                Dcc2.Add(new DataPoint<int, int>() { Y = cc22, X = 2 });

                Dcc3.Add(new DataPoint<int, int>() { Y = cc30, X = 0 });
                Dcc3.Add(new DataPoint<int, int>() { Y = cc31, X = 1 });
                Dcc3.Add(new DataPoint<int, int>() { Y = cc32, X = 2 });


                DataSeries<String, int> cc1 = new DataSeries<String, int>();
                DataSeries<String, int> cc2 = new DataSeries<string, int>();
                DataSeries<string, int> cc3 = new DataSeries<string, int>();

                cc1.Add(new DataPoint<String, int>("Bad", cc10));
                cc1.Add(new DataPoint<string, int>("Good", cc11));
                cc1.Add(new DataPoint<string, int>("Perfect", cc12));

                cc2.Add(new DataPoint<String, int>("Bad", cc20));
                cc2.Add(new DataPoint<string, int>("Good", cc21));
                cc2.Add(new DataPoint<string, int>("Perfect", cc22));

                cc3.Add(new DataPoint<string, int>("Bad", cc30));
                cc3.Add(new DataPoint<string, int>("Good", cc31));
                cc3.Add(new DataPoint<string, int>("Perfect", cc32));


                cc1chart.DataSeries = cc1;
                cc2chart.DataSeries = cc2;
                cc3chart.DataSeries = cc3;


                cc1chart.Title = comboBox1.Items[0].ToString();
                cc2chart.Title = comboBox1.Items[1].ToString();
                cc3chart.Title = comboBox1.Items[2].ToString();
                cc1chart.ShowLabels = false;
                cc2chart.ShowLabels = false;
                cc3chart.ShowLabels = false;
            }
            else
                MessageBox.Show("Please select an exercise");
            
            //MainChart.DataSeries = test1;
            //MainChart.Title = "random";

        }
    }

}
