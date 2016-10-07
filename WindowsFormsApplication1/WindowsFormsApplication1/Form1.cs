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
            dataGridView1.RowCount = 8; dataGridView2.RowCount = 8;
            //dataGridView1.AutoSize = true;



        }

        //пытался писать понятный код//
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
        }//картинку в масив байтов

        public static void ByteRgbToBitmap(ref byte[,,] byteOfPicture, ref int width, ref int height, ref Bitmap Picture)
        {
            Picture = new Bitmap(width, height);
            for (int x = 0; x < height; x++)
                for (int y = 0; y < width; y++)
                {
                    Picture.SetPixel(y, x, Color.FromArgb(byteOfPicture[0, x, y], byteOfPicture[1, x, y], byteOfPicture[2, x, y]));
                }
        }//масив байтов в картинку

        public static void ThreeToOne<T>(ref T[,,] byteOfPicture3, ref int width, ref int height, ref T[] byteOfPicture1)
        {
            byteOfPicture1 = new T[3 * width * height];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < height; j++)
                    for (int k = 0; k < width; k++)
                    {
                        byteOfPicture1[i * height * width + j * width + k] = byteOfPicture3[i, j, k];
                    }

        }//трехмерный масив в одномерный


        public static void OneToThree<T>(ref T[] byteOfPicture1, ref int width, ref int height, ref T[,,] byteOfPicture3)
        {
            byteOfPicture3 = new T[3, height, width];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < height; j++)
                    for (int k = 0; k < width; k++)
                    {
                        byteOfPicture3[i, j, k] = byteOfPicture1[i * height * width + j * width + k];
                    }


        }//одномерный масив в трехмерный


        public static double[] FastWalsTransform<T>(ref T[] Image, ref int[,] Matrix_of_Uolsh)
        {
            double[] Resultat = new double[Image.Length];

            for (int j = 0; j < Image.Length; j += 256)
                for (int i = 0; i < 256; i++)
                    for (int k = 0; k < 256; k++)
                    {
                        Resultat[i + j] += Convert.ToDouble(Convert.ToInt16(Image[k + j]) * Matrix_of_Uolsh[i, k]) / 256;
                    }

            return Resultat;
        }//прямое преобразование Уолша

        public static double[] WHT_double_in_byte(ref double[] Masiv, ref int[,] Matrix_of_Uolsh)
        {
            double[] WHT_Masiv = new double[Masiv.Length];

            for (int j = 0; j < Masiv.Length; j += 256)
                for (int i = 0; i < 256; i++)
                    for (int k = 0; k < 256; k++)
                    {
                        WHT_Masiv[i + j] += Convert.ToDouble(Masiv[k + j] * Matrix_of_Uolsh[i, k]);
                    }

            return WHT_Masiv;
        }//обратоное преобразование уолша

        public static void swapRowsColumns <T>(ref T[] rgbImage, int width)
        {
            for (int chanel = 0; chanel < 3; chanel++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = x + 1; y < width; y++)
                    {
                        T temp = rgbImage[chanel * width * width + y * width + x];
                        T b = rgbImage[chanel * width * width + x * width + y];
                        rgbImage[chanel * width * width + y * width + x] = b;
                        rgbImage[chanel * width * width + x * width + y] = temp;
                    }
                }
            }
        }//Разворот масива

       // public static void GenereteBasisOfWals(ref int[,] IndexMatrix, ref int[,] BasisOfWalsh)
       // {
       //     BasisOfWalsh = new int[256, 256];
       //     for (int i = 0; i < 8; i++)
       //         for (int j = 0; j < 8; j++)
       //         {
       //             BasisOfWalsh[2 ^ i, 2 ^ j] = IndexMatrix[7 - i, j];//запонение 2^n-го числа 
       //         }
       //     for(int j = 1; j < 256; j++)
       //     for (int i = 1; i < 256; i++)
       //     {
       //         if (BasisOfWalsh[j, i] == null)
       //         {
       //             for (int k = 128; k > 1; k = k / 2)
       //             {
       //                 int m = i;
       //                 if (k < m)
       //                 {
       //                     BasisOfWalsh[j, i] += BasisOfWalsh[j, k];
       //                 }
       //             }
       //         }
       //     }
       //}//Создание базиса на основе индексной матрици
                




       

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

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] Bufer = new byte[3 * width * height];
            ThreeToOne(ref byteOfPicture, ref width, ref height, ref Bufer);
            string[] S = new string[3 * width * height];
            
            for (int i = 0; i < Bufer.Length; i++)
            {
                S[i] = Convert.ToString(Bufer[i]);
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllLines(saveFileDialog1.FileName+".txt", S);             
            }
            Array.Clear(S, 0, S.Length);
            Array.Clear(Bufer, 0, Bufer.Length);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] bufer = new byte[3 * width * height];
            ThreeToOne(ref byteOfPicture, ref width, ref height, ref bufer);
           double[] MasivRates = new double [3 * width * height];


            int[,] firstMatrix = new int[8, 8];
            int[,] firstBasis = new int[256, 256];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    firstMatrix[i, j] = Convert.ToInt16(dataGridView1.Rows[i].Cells[j].Value);

                }

         

        }
    }
}

