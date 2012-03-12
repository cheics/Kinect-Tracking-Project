using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            if (comboBox1.SelectedItem == "Squat")
            {
                comboBox2.Items.Add("Squat Depth");
                comboBox2.Items.Add("Spine Stability");
                comboBox2.Items.Add("Crit Comp 3");
            }
            else if (comboBox1.SelectedItem == "Knee Raise")
            {
                comboBox2.Items.Add("Knee Height");
                comboBox2.Items.Add("Spine Stability");
                comboBox2.Items.Add("Crit Comp 3");
            }
            else if (comboBox1.SelectedItem == "Arm Raise")
            {
                comboBox2.Items.Add("Arm height");
                comboBox2.Items.Add("Spine Stability");
                comboBox2.Items.Add("Crit Comp 3");
            }
            else if (comboBox1.SelectedItem == "Lateral Leg Extension")
            {
                comboBox2.Items.Add("Leg Distance");
                comboBox2.Items.Add("Spine Stability");
                comboBox2.Items.Add("Crit Comp 3");
            }
            else
                comboBox2.Items.Add("");
        }
    }
}
