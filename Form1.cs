using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Diskretochka_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int[][] A = new int[8][];//матрица смежностей для графа
        int[] B = new int[8];//вектор для номеров вершин
        int[] Sum = new int[8];//вектор суммы для результата
        
        private void matr_do_button_Click(object sender, EventArgs e)
        {
            string s;
            int i=1;
            string MyFile = "MyFile.txt";
            if (form_rBtn.Checked == false && file_rBtn.Checked == false)
                MessageBox.Show("Выберите источник данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                for (int k = 0; k < A.Length; k++)
                {
                    A[k] = new int[8];
                }
                for (int l = 0; l < B.Length; l++)
                {
                    B[l] = l;
                }
                for (int z = 0; z < Sum.Length; z++)
                {
                    Sum[z] = 0;
                }
                //ввод с экрана
                if (form_rBtn.Checked)
                {
                    A[1][2] = 1;
                    A[1][3] = 1;
                    A[1][4] = 1;
                    A[3][5] = 1;
                    A[4][2] = 1;
                    A[4][6] = 1;
                    A[4][7] = 1;
                    A[5][6] = 1;
                    A[6][7] = 1;
                }
                else
                {
                    //ввод из файла
                    if (file_rBtn.Checked)
                    {
                        StreamReader Read = new StreamReader(MyFile);
                        while ((s = Read.ReadLine()) != null)
                        {
                            string[] numbers = s.Split(' ');
                                for (int j = 0; j < A.Length - 1; j++)
                                {
                                    if (int.Parse(numbers[j]) == 1)
                                        A[i][j + 1] = 1;
                                }
                                i++;                          
                        }
                        Read.Close();
                    }
                }
                label2.Visible = true;
                Matr_dataGridView.Visible = true;
                //вывод матрицы на экран в датагрид
                Output_Matrix(A, B, Matr_dataGridView);
                matr_do_button.Visible = false;
                form_rBtn.Visible = false;
                file_rBtn.Visible = false;
                label3.Visible = true;
                ur_listBox.Visible = true;
                do_button.Visible = true;
            }
        }
        static void Output_Matrix(int[][] Matr, int[] Batr, DataGridView Matr_dataGridView)
        {
            //заголовок столбцов
            Matr_dataGridView.ColumnCount = 8;
            Matr_dataGridView.RowCount = 8;
            //установки для столбцов
            for (int i = 0; i < 8; i++)
            {
                //запрещаем менять значения ячеек
                Matr_dataGridView.Columns[i].ReadOnly = true;
                //отключение режима сортировки элементов столбца
                Matr_dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                Matr_dataGridView.Columns[i].Width = 25;//ширина столбцов
            }

            for (int k = 1; k < Batr.Length; k++)
            {
                Matr_dataGridView.Rows[0].Cells[k].Value = "X" + Batr[k].ToString();

            }
            for (int k = 1; k < Batr.Length; k++)
            {

                Matr_dataGridView.Rows[k].Cells[0].Value = "X" + Batr[k].ToString();
            }

            //отображение значений элементов матрицы в DataGridView
            for (int i = 1; i < Matr.Length; i++)
                for (int j = 1; j < Matr[i].Length; j++)
                //заполнение ячейки 
                {
                    Matr_dataGridView.Rows[i].Cells[j].Value = "".ToString();
                    Matr_dataGridView.Rows[i].Cells[j].Value = Matr[i][j].ToString();
                }

        }
        
        //функция вывода результата на экран
        void Output()
        {
            string result = "N=";
            for (int m=1;m<B.Length; m++)
                if (Sum[m] == 0)
                {
                    result += "X" + B[m] + ", ";
                }
            ur_listBox.Items.Add(result);
           
        }


        //кнопка завершения работы программы
        private void exit_button_Click(object sender, EventArgs e)
        {
            Close();
        }
        //функция выполнения программы
        private void do_button_Click(object sender, EventArgs e)
        {
            int k=0;
            do
            {
                for (int i = 1; i < A.Length; i++)
                {
                    for (int j = 1; j < A.Length; j++)
                    {
                        if (A[j][i] == 1)
                           k++;
                            
                    }
                    Sum[i] = k;
                    k = 0;
                }
                Output();
                for (int m = 1; m < Sum.Length; m++)
                {
                    if (Sum[m] == 0)
                    {
                        cut(m);
                        m--;
                    }
                }
                
            }
            while (Sum.Length>1);
            ur_listBox.Items.Add("Больше уровней нет!");
            pictureBox2.Visible = true;
            label4.Visible = true;
        }
        void cut(int k)
        {
            del_row(k);
            del_col(k);
            del_elem(ref B, k);
            del_elem(ref Sum, k);
        }
        void del_col(int c)
        {
            for (int i = 0; i < A.Length; i++)
            {
                for (int j = c; j < A[i].Length - 1; j++)
                {
                    A[i][j] = A[i][j + 1];
                }
                Array.Resize(ref A[i], A[i].Length - 1);
            }
        }

        void del_row(int r)
        {
            for (int j = 0; j < A.Length; j++)
            {
                for (int i = r; i < A.Length - 1; i++)
                {
                    A[i][j] = A[i + 1][j];
                }
            }
            Array.Resize(ref A, A.Length - 1);
        }
        void del_elem(ref int[] Mas, int n)
        {
            for (int i = n; i < Mas.Length - 1; i++)
                Mas[i] = Mas[i + 1];
            Array.Resize(ref Mas, Mas.Length - 1);
        }
    }
}
