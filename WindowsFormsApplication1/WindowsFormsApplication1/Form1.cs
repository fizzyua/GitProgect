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
        byte[,,] byteOfPicture;//картинка
        double[] coefficientsOfWalsh;//весовые кофиценты


        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowCount = 8; dataGridView2.RowCount = 8;

            label3.Text = "Det= 0";
            label4.Text = "Det= 0";

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = "0";
                }
            }

            for (int i = 0; i < 8; i++) dataGridView1.Rows[0].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 1; i < 8; i++) dataGridView1.Rows[1].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 2; i < 8; i++) dataGridView1.Rows[2].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 3; i < 8; i++) dataGridView1.Rows[3].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 4; i < 8; i++) dataGridView1.Rows[4].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 5; i < 8; i++) dataGridView1.Rows[5].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 6; i < 8; i++) dataGridView1.Rows[6].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 7; i < 8; i++) dataGridView1.Rows[7].Cells[7 - i].Style.BackColor = Color.DarkGray;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    dataGridView2.Rows[i].Cells[j].Value = "0";
                }
            }

            for (int i = 0; i < 8; i++) dataGridView2.Rows[0].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 1; i < 8; i++) dataGridView2.Rows[1].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 2; i < 8; i++) dataGridView2.Rows[2].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 3; i < 8; i++) dataGridView2.Rows[3].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 4; i < 8; i++) dataGridView2.Rows[4].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 5; i < 8; i++) dataGridView2.Rows[5].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 6; i < 8; i++) dataGridView2.Rows[6].Cells[7 - i].Style.BackColor = Color.DarkGray;
            for (int i = 7; i < 8; i++) dataGridView2.Rows[7].Cells[7 - i].Style.BackColor = Color.DarkGray;
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

        public static void swapRowsColumns<T>(ref T[] rgbImage, int width)
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

        public static void GenereteBasisOfWals(ref int[,] IndexMatrix, ref int[,] BasisOfWalsh)
        {
            BasisOfWalsh = new int[256, 256];
            for (int a = 0; a < 256; a++)
                for (int b = 0; b < 256; b++)
                {
                    BasisOfWalsh[a, b] = -1;
                }

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    BasisOfWalsh[(int)Math.Pow(2, i), (int)Math.Pow(2, j)] = IndexMatrix[7 - i, j];
                }

            for (int j = 1; j < 256; j=j*2)
                for (int i = 1; i < 256; i++)
                {
                    
                    if (BasisOfWalsh[j, i] == -1)
                    {
                        BasisOfWalsh[j, i] = 0;
                        int n = i;                      
                        for (int k = 128; k >= 1; k = k / 2)
                        {
                            if (k <= n)
                            {
                                BasisOfWalsh[j, i] += BasisOfWalsh[j, k];
                                n = n - k;
                            }
                        }
                    }
                }

            for (int j = 1; j < 256; j++)
                for (int i = 1; i < 256; i++)
                {
                    if (BasisOfWalsh[i, j] == -1)
                    {
                        BasisOfWalsh[i, j] = 0;
                        int n = i;
                        for (int k = 128; k >= 1; k = k / 2)
                        {
                        if (k <= n)
                        {
                            BasisOfWalsh[i, j] += (int)BasisOfWalsh[k, j];
                                n = n - k;
                            }
                        }
                    }
                }

            for (int i = 0; i < 256; i++)
            {
                BasisOfWalsh[0, i] = 1;
                BasisOfWalsh[i, 0] = 1;
            }
            for (int a = 1; a < 256; a++)
                for (int b = 1; b < 256; b++)
                {
                    BasisOfWalsh[a, b] = BasisOfWalsh[a, b] % 2;
                    
                }
            for (int a = 0; a < 256; a++)
                for (int b = 0; b < 256; b++)
                {
                    if (BasisOfWalsh[a, b] == 0) BasisOfWalsh[a, b] = -1;
                }


        }//Создание базиса на основе индексной матрици

        public static double[,] GetMinor(double[,] matrix, int row, int column)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1)) throw new Exception(" Число строк в матрице не совпадает с числом столбцов");
            double[,] buf = new double[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if ((i != row) || (j != column))
                    {
                        if (i > row && j < column) buf[i - 1, j] = matrix[i, j];
                        if (i < row && j > column) buf[i, j - 1] = matrix[i, j];
                        if (i > row && j > column) buf[i - 1, j - 1] = matrix[i, j];
                        if (i < row && j < column) buf[i, j] = matrix[i, j];
                    }
                }
            return buf;
        }//минор Матрици
  
        public static double Determ(double[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1)) throw new Exception(" Число строк в матрице не совпадает с числом столбцов");
            double det = 0;
            int Rank = matrix.GetLength(0);
            if (Rank == 1) det = matrix[0, 0];
            if (Rank == 2) det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            if (Rank > 2)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    det += Math.Pow(-1, 0 + j) * matrix[0, j] * Determ(GetMinor(matrix, 0, j));
                }
            }
            return det;
        }//Поиск опредилителя матрици


        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


                Bitmap image1 = new Bitmap(openFileDialog1.FileName);
                width = image1.Width;
                height = image1.Height;
                BitmapToByteRgb(ref image1, ref byteOfPicture);

                pictureBox1.Image = System.Drawing.Bitmap.FromFile(openFileDialog1.FileName);
                //pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    
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
                System.IO.File.WriteAllLines(saveFileDialog1.FileName + ".txt", S);
            }
            Array.Clear(S, 0, S.Length);
            Array.Clear(Bufer, 0, Bufer.Length);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] buferImage = new byte[3 * width * height];
            ThreeToOne(ref byteOfPicture, ref width, ref height, ref buferImage);
           // double[] array = new double[3 * width * height];


            int[,] firstMatrix = new int[8, 8];
            int[,] firstBasis = new int[256, 256];

           

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    firstMatrix[i, j] = Convert.ToInt16(dataGridView1.Rows[i].Cells[j].Value);
                }
            GenereteBasisOfWals(ref firstMatrix, ref firstBasis);

            coefficientsOfWalsh = FastWalsTransform(ref buferImage, ref firstBasis);

            Array.Clear(buferImage, 0, buferImage.Length);

           

            //swapRowsColumns(ref array, width);
            //for (int i = 0; i < 8; i++)
            //    for (int j = 0; j < 8; j++)
            //    {
            //        secondMatrix[i, j] = Convert.ToInt16(dataGridView1.Rows[i].Cells[j].Value);

            //    }
            //GenereteBasisOfWals(ref secondMatrix, ref secondBasis);

            //coefficients = FastWalsTransform(ref array, ref secondBasis);



        }


        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            double[] Image = new double[3 * width * height];

            int[,] firstMatrix = new int[8, 8];
            int[,] firstBasis = new int[256, 256];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    firstMatrix[i, j] = Convert.ToInt16(dataGridView1.Rows[i].Cells[j].Value);

                }
            GenereteBasisOfWals(ref firstMatrix, ref firstBasis);
            Image = WHT_double_in_byte(ref coefficientsOfWalsh,ref firstBasis);

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[,] FirsrIndexMatrix = new int[8, 8];
            int[,] FirstBasisOfWalsh = new int[256, 256];
            string[] bufer = new string[256 * 256];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    FirsrIndexMatrix[i, j] = Convert.ToInt16(dataGridView1.Rows[i].Cells[j].Value);
                }
            GenereteBasisOfWals(ref FirsrIndexMatrix, ref FirstBasisOfWalsh);

            for (int i = 0; i < 256; i++)
                for (int j = 0; j < 256; j++)
                {
                    bufer[i + 256 * j] = Convert.ToString(FirstBasisOfWalsh[i, j]);
                }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllLines(saveFileDialog1.FileName + ".txt", bufer);
            }

        }

        

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            double[,] Matrix = new double[8, 8];

            if ((e.RowIndex == 0 && e.ColumnIndex < 8) || (e.RowIndex == 1 && e.ColumnIndex < 7) || (e.RowIndex == 2 && e.ColumnIndex < 6) || (e.RowIndex == 3 && e.ColumnIndex < 5) || (e.RowIndex == 4 && e.ColumnIndex < 4) || (e.RowIndex == 5 && e.ColumnIndex < 3) || (e.RowIndex == 6 && e.ColumnIndex < 2) || (e.RowIndex == 7 && e.ColumnIndex < 1))
            {
                if (Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == "0")
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                    dataGridView1.Rows[7 - e.ColumnIndex].Cells[7 - e.RowIndex].Value = 1;
                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                        {
                            Matrix[i, j] = Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value);
                        }
                    double det;
                    det = Determ(Matrix);
                    label3.Text = "Det= " +Convert.ToString(det);
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                    dataGridView1.Rows[7 - e.ColumnIndex].Cells[7 - e.RowIndex].Value = 0;
                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                        {
                            Matrix[i, j] = Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value);
                        }
                    double det;
                    det = Determ(Matrix);
                    label3.Text = "Det= " + Convert.ToString(det);
                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            double[,] Matrix = new double[8, 8];

            if ((e.RowIndex == 0 && e.ColumnIndex < 8) || (e.RowIndex == 1 && e.ColumnIndex < 7) || (e.RowIndex == 2 && e.ColumnIndex < 6) || (e.RowIndex == 3 && e.ColumnIndex < 5) || (e.RowIndex == 4 && e.ColumnIndex < 4) || (e.RowIndex == 5 && e.ColumnIndex < 3) || (e.RowIndex == 6 && e.ColumnIndex < 2) || (e.RowIndex == 7 && e.ColumnIndex < 1))
            {
                if (Convert.ToString(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == "0")
                {
                    dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                    dataGridView2.Rows[7 - e.ColumnIndex].Cells[7 - e.RowIndex].Value = 1;
                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                        {
                            Matrix[i, j] = Convert.ToDouble(dataGridView2.Rows[j].Cells[i].Value);
                        }
                    double det;
                    det = Determ(Matrix);
                    label4.Text = "Det= " + Convert.ToString(det);
                }

                else
                {
                    dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                    dataGridView2.Rows[7 - e.ColumnIndex].Cells[7 - e.RowIndex].Value = 0;
                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                        {
                            Matrix[i, j] = Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value);
                        }
                    double det;
                    det = Determ(Matrix);
                    label4.Text = "Det= " + Convert.ToString(det);


                }

            }
        }
    }
}


