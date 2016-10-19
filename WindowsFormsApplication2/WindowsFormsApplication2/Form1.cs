using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowCount = 8;
            dataGridView1.ColumnCount = 8;

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

        }
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

            for (int j = 1; j < 256; j = j * 2)
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
                                BasisOfWalsh[i, j] += BasisOfWalsh[k, j];
                                n = n - k;
                            }
                        }
                    }
                }

            //for (int i = 0; i < 256; i++)
            //{
            //    BasisOfWalsh[0, i] = -1;
            //    BasisOfWalsh[i, 0] = -1;
            //}
            for (int a = 1; a < 256; a++)
                for (int b = 1; b < 256; b++)
                {
                    BasisOfWalsh[a, b] = BasisOfWalsh[a, b] % 2;
                    if (BasisOfWalsh[a, b] == 0) BasisOfWalsh[a, b] = -1;

                }

        }//Создание базиса на основе индексной матрици

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
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
                    label1.Text = "Det= " + Convert.ToString(det);
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
                    label1.Text = "Det= " + Convert.ToString(det);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[,] Matrix = new int[8, 8];
            int[,] Basis = new int[256, 256];
            dataGridView2.RowCount = 256;
            dataGridView2.ColumnCount = 256;

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Matrix[i, j] = Convert.ToInt16(dataGridView1.Rows[i].Cells[j].Value);
                }
            GenereteBasisOfWals(ref Matrix, ref Basis);

            for (int i = 0; i < 256; i++)
                for (int j = 0; j < 256; j++)
                {
                   dataGridView2.Rows[i].Cells[j].Value = Basis[i, j];
                    //
                    dataGridView2.Columns[i].Width = 20;
                  
                    //dataGridView2.Rows[i].Cells[j].Size.Height = 25;
                }
            int count1 = 0, count2 = 0;
            for (int i = 0; i < 256; i++)
            {

                if (Basis[1, i] == 1) count1++;
                else count2++;
            }
            label2.Text = Convert.ToString(count1);
            label3.Text = Convert.ToString(count2);
        }
    }
}
