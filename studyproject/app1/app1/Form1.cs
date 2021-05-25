using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app1
{
    public partial class Form1 : Form
    {
        int count;
        int time;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i;
            for (i = 1; i < 100; i++)
            {
                comboBox1.Items.Add(i.ToString() + "秒");
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = comboBox1.Text;
            timer1.Start();
            time = Convert.ToInt16(str.Substring(0, 2));
            progressBar1.Maximum = time;

        }
    }
}
