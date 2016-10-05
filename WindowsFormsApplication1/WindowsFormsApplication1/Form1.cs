using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int width;
        int height; 
        public Form1()
        {
            InitializeComponent();
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


                Bitmap image1 = new Bitmap(openFileDialog1.FileName);
                width = image1.Width;
                height = image1.Height;

                pictureBox1.Image = System.Drawing.Bitmap.FromFile(openFileDialog1.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }

        }
    }
    }

