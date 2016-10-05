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
        byte[,,] byteOfPicture;
        public Form1()
        {
            InitializeComponent();
            

        }
        public static void BitmapToByteRgb(ref Bitmap bmp, ref byte[,,] Grafikbyte)
        {
            int width = bmp.Width,
                height = bmp.Height;
            if (Grafikbyte != null)
                Array.Clear(Grafikbyte, 0, Grafikbyte.Length);
            Grafikbyte = new byte[3, height, width];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Color color = bmp.GetPixel(x, y);
                    Grafikbyte[0, y, x] = color.R;
                    Grafikbyte[1, y, x] = color.G;
                    Grafikbyte[2, y, x] = color.B;
                }
            }
        }
      
        public static void ByteRgbToBitmap( ref byte [,,] byteOfPicture, ref int width, ref int height, ref Bitmap Picture)
            {
             Picture = new Bitmap(width, height);
            for (int x = 0; x < height; x++)
                for (int y = 0; y < width; y++)
                {
                    Picture.SetPixel(y, x, Color.FromArgb(byteOfPicture[0, x, y], byteOfPicture[1, x, y], byteOfPicture[2, x, y]));
                }
        }
        
        public static void ThreeToOne <T> (ref T [,,] byteOfPicture3,ref int width, ref int height, ref T [] byteOfPicture1)
        {
            byteOfPicture1 = new T[3 * width * height];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < height; j++)
                    for (int k = 0; k < width; k++)
                    {
                        byteOfPicture1[i * height * width + j * width + k] = byteOfPicture3[i, j, k];
                    }

       }

   
        public static void OneToThree <T> ( ref T [] byteOfPicture1,ref int width,ref int height, ref T[,,] byteOfPicture3)
        {
            byteOfPicture3 = new T[3, height, width];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < height; j++)
                    for (int k = 0; k < width; k++)
                    {
                        byteOfPicture3[i, j, k] = byteOfPicture1[i * height * width + j * width + k];
                    }


                    }


        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


                Bitmap image1 = new Bitmap(openFileDialog1.FileName);
                width = image1.Width;
                height = image1.Height;
                BitmapToByteRgb(ref image1, ref byteOfPicture);

                pictureBox1.Image = System.Drawing.Bitmap.FromFile(openFileDialog1.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }

        }
    }
    }

