﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using CostLib;

namespace CostApp
{
    public partial class Form1 : Form
    {
        public InputForm inputForm;
        //public int monthWallet;
        public int balance;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Dmitry\source\repos\CostApp\CostApp\Database.mdf; Integrated Security = True";
            CostsDB.OpenSqlConnection(connectionString);

            CostsDB.SqlCmdView("SELECT * FROM [Costs]", listBox1, label8, label10);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CostsDB.sqlConnection != null && CostsDB.sqlConnection.State != ConnectionState.Closed)
                CostsDB.sqlConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label3.Visible)
                label3.Visible = false;
            if(!string.IsNullOrEmpty(textBox1.Text)&&!string.IsNullOrEmpty(textBox2.Text))
            {
                CostsDB.SqlCmdIncert(textBox1, textBox2);
            }
            else
            {
                label3.Visible = true;
                label3.Text = "Поля Категория и Цена не заполнены";
            }
            
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            CostsDB.SqlCmdView("SELECT * FROM [Costs]", listBox1, label8, label10);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("TRUNCATE TABLE [Costs] ", CostsDB.sqlConnection);
            await command.ExecuteNonQueryAsync();
            listBox1.Items.Clear();

            CostsDB.monthCosts = 0;
            CostsDB.dayCosts = 0;
            label8.Text = Convert.ToString(0);
            label10.Text = Convert.ToString(0);
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            CostsDB.SqlCmdView("SELECT * FROM [Costs]", listBox1, label8, label10);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            inputForm = new InputForm();
            inputForm.Show();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            CostsDB.sqlReader = null;
            CostsDB.sqlCommand = new SqlCommand("SELECT * FROM [Wallet]", CostsDB.sqlConnection);
            try
            {
                CostsDB.sqlReader = await CostsDB.sqlCommand.ExecuteReaderAsync();

                int monthWallet = 0;
                //monthCosts = Convert.ToInt32(CostsDB.sqlReader["DateTime"]);
                while (await CostsDB.sqlReader.ReadAsync())
                {
                    monthWallet += Convert.ToInt32(CostsDB.sqlReader["MonthWallet"]);
                }
                MessageBox.Show($"Остаток на счете:{monthWallet-CostsDB.monthCosts}");
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString() != "Для BeginExecuteReader нужно открытое и доступное подключение Connection. Подключение устанавливается.")
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (CostsDB.sqlReader != null)
                    CostsDB.sqlReader.Close();
            }
            /*
            if (>CostsDB.monthCosts)
            {
                
                MessageBox.Show($"Остаток на счете:{monthWallet - CostsDB.monthCosts}");
            }
            else
                MessageBox.Show($"Вы исчерпали свой месячный бюджет"); */
        }
    }
}
