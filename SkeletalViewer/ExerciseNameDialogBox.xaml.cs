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
        public string file;
        public string criticalC1;
        public string criticalC2;
        public string criticalC3;
        public string index;
        string folder;
        bool passing = true;
        public ExerciseNameDialogBox()
        {
            InitializeComponent();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            file = textBox.Text;
            criticalC1 = CC1.Text;
            criticalC2 = CC2.Text;
            criticalC3 = CC3.Text;
            index = FileIndex.Content.ToString();
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void passFolder(String datafolder, String exercise, String criticalComp1, String criticalComp2, String criticalComp3)
        {
            CC1.Text = criticalComp1;
            CC2.Text = criticalComp2;
            CC3.Text = criticalComp3;
            folder = datafolder;
            //fileName.Content = exercise;
            textBox.Text = exercise;
            passing = false;
            update();
        }

        private void update()
        {
            if (passing)
            {
                return;
            }
            FileIndex.Content = (Directory.GetFiles(folder, textBox.Text + CC1.Text + CC2.Text + CC3.Text + "-" + "*.mat").Length + 1).ToString("D4");
            index = FileIndex.Content.ToString();
            fileName.Content = textBox.Text + CC1.Text + CC2.Text + CC3.Text + "-" + index + ".mat";
            path.Content = folder + @"\" + fileName.Content;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            update();
        }

        private void CC_TextChanged(object sender, TextChangedEventArgs e)
        {
            update();
        }

    }
}
