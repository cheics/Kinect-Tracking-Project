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
        public string criticalC11;
        public string criticalC21;
        public string criticalC31;
        public string criticalC12;
        public string criticalC22;
        public string criticalC32;
        public string criticalC13;
        public string criticalC23;
        public string criticalC33;
        public string criticalC14;
        public string criticalC24;
        public string criticalC34;
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
            criticalC11 = CC11.Text;
            criticalC21 = CC21.Text;
            criticalC31 = CC31.Text;
            criticalC12 = CC12.Text;
            criticalC22 = CC22.Text;
            criticalC32 = CC32.Text;
            criticalC13 = CC13.Text;
            criticalC23 = CC23.Text;
            criticalC33 = CC33.Text;
            criticalC14 = CC14.Text;
            criticalC24 = CC24.Text;
            criticalC34 = CC34.Text;
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
            CC11.Text = criticalComp1;
            CC21.Text = criticalComp2;
            CC31.Text = criticalComp3;
            CC12.Text = criticalComp1;
            CC22.Text = criticalComp2;
            CC32.Text = criticalComp3;
            CC13.Text = criticalComp1;
            CC23.Text = criticalComp2;
            CC33.Text = criticalComp3;
            CC14.Text = criticalComp1;
            CC24.Text = criticalComp2;
            CC34.Text = criticalComp3;
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
            if (grid1.IsEnabled)
            {
                CC11.Text = CC1.Text;
                CC21.Text = CC2.Text;
                CC31.Text = CC3.Text;
                CC12.Text = CC1.Text;
                CC22.Text = CC2.Text;
                CC32.Text = CC3.Text;
                CC13.Text = CC1.Text;
                CC23.Text = CC2.Text;
                CC33.Text = CC3.Text;
                CC14.Text = CC1.Text;
                CC24.Text = CC2.Text;
                CC34.Text = CC3.Text;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            update();
        }

        private void CC_TextChanged(object sender, TextChangedEventArgs e)
        {
            update();
        }

        private void CB_Checked(object sender, RoutedEventArgs e)
        {
            if (passing)
            {
                return;
            }
            CC11.Text = CC1.Text;
            CC21.Text = CC2.Text;
            CC31.Text = CC3.Text;
            CC12.Text = CC1.Text;
            CC22.Text = CC2.Text;
            CC32.Text = CC3.Text;
            CC13.Text = CC1.Text;
            CC23.Text = CC2.Text;
            CC33.Text = CC3.Text;
            CC14.Text = CC1.Text;
            CC24.Text = CC2.Text;
            CC34.Text = CC3.Text;
            grid1.IsEnabled = false;
            grid2.IsEnabled = false;
            grid3.IsEnabled = false;
            grid4.IsEnabled = false;
        }

        private void CB_Unchecked(object sender, RoutedEventArgs e)
        {
            grid1.IsEnabled = true;
            grid2.IsEnabled = true;
            grid3.IsEnabled = true;
            grid4.IsEnabled = true;
        }

    }
}
