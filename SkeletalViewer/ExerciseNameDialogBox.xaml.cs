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
using System.IO;

namespace SkeletalViewer
{
    /// <summary>
    /// Interaction logic for ExerciseNameDialogBox.xaml
    /// </summary>
    public partial class ExerciseNameDialogBox : Window
    {
        bool angleSelected = false;
        bool performanceSelected = false;
        bool deficiencySelected = false;
        public string angle;
        public string performance;
        public string deficiency;
        public string index;
        string trainingPath = @"C:\Users\Abdi\Documents\Visual Studio 2010\Projects\Kinect-Tracking-Project\DataFiles\Experiment3\";
        public ExerciseNameDialogBox()
        {
            InitializeComponent();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rba1_Checked(object sender, RoutedEventArgs e)
        {
            angleSelected = true;
            angle = "Front";
            if (performanceSelected & deficiencySelected | performanceSelected & defPar.IsEnabled)
            {
                Ok.IsEnabled = true;
                trainingIndex();
            }
        }
        private void rba2_Checked(object sender, RoutedEventArgs e)
        {
            angleSelected = true;
            angle = "45";
            if (performanceSelected & deficiencySelected | performanceSelected & defPar.IsEnabled)
            {
                Ok.IsEnabled = true;
                trainingIndex();
            }
        }
        private void rba3_Checked(object sender, RoutedEventArgs e)
        {
            angleSelected = true;
            angle = "Profile";
            if (performanceSelected & deficiencySelected | performanceSelected & defPar.IsEnabled)
            {
                Ok.IsEnabled = true;
                trainingIndex();
            }
        }

        private void rbp1_Checked(object sender, RoutedEventArgs e)
        {
            performanceSelected = true;
            performance = "Good";
            rbd1.IsChecked = false;
            rbd2.IsChecked = false;
            deficiency = "";
            defPar.IsEnabled = false;
            deficiencySelected = false;
            if (angleSelected)
            {
                Ok.IsEnabled = true;
                trainingIndex();
            }
        }

        private void rbp2_Checked(object sender, RoutedEventArgs e)
        {
            Ok.IsEnabled = false;
            FileIndex.Visibility = System.Windows.Visibility.Hidden;
            performanceSelected = true;
            performance = "Medium";
            defPar.IsEnabled = true;
            if (angleSelected & deficiencySelected)
            {
                Ok.IsEnabled = true;
                trainingIndex();
            }
        }

        private void rbp3_Checked(object sender, RoutedEventArgs e)
        {
            Ok.IsEnabled = false;
            FileIndex.Visibility = System.Windows.Visibility.Hidden;
            performanceSelected = true;
            performance = "Bad";
            defPar.IsEnabled = true;
            if (angleSelected & deficiencySelected)
            {
                Ok.IsEnabled = true;
                trainingIndex();
            }
        }

        private void rbd1_Checked(object sender, RoutedEventArgs e)
        {
            deficiencySelected = true;
            deficiency = "Hip";
            if (angleSelected & deficiencySelected)
            {
                Ok.IsEnabled = true;
                trainingIndex();
            }
        }

        private void rbd2_Checked(object sender, RoutedEventArgs e)
        {
            deficiencySelected = true;
            deficiency = "Back";
            if (angleSelected & deficiencySelected)
            {
                Ok.IsEnabled = true;
                trainingIndex();
            }
        }

        private void trainingIndex()
        {
            FileIndex.Content = (Directory.GetFiles(trainingPath, angle + performance + deficiency + "*").Length + 1).ToString();
            index = FileIndex.Content.ToString();
            FileIndex.Visibility = System.Windows.Visibility.Visible;
        }

    }
}
