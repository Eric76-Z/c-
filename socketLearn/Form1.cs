using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace socketLearn
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //用来存储音乐文件的全路径
        List<string> listSongs = new List<string>();

        SoundPlayer sp = new SoundPlayer();

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择音乐文件";
            ofd.InitialDirectory = @"C:\Users\Administrator\Music";
            ofd.Multiselect = true;
            ofd.Filter = "音乐文件|*.mp3|所有文件|*.*";
            ofd.ShowDialog();
            string[] path = ofd.FileNames;
            for (int i = 0; i < path.Length; i++)
            {
                listBox1.Items.Add(Path.GetFileName(path[i]));
                //将音乐文件的全路径存储到泛型集合中
                listSongs.Add(path[i]);
            }

        }


        /// <summary>
        /// 实现双击播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {

            sp.SoundLocation = listSongs[listBox1.SelectedIndex];
            sp.Play();
        }
        /// <summary>
        /// 点击下一曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            index++;
            if (index == listBox1.Items.Count)
            {
                index = 0;
            }

            listBox1.SelectedIndex = index;
            sp.SoundLocation = listSongs[index];
            sp.Play();
        }
    }
}
