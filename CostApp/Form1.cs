using System;
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

namespace CostApp
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        public int monthCosts=0;
        public int weekCosts=0;
        public int dayCosts=0;
        public Form1()
        {
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Dmitry\source\repos\CostApp\CostApp\Database.mdf; Integrated Security = True";
            sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();

            SqlDataReader sqlReader = null;

            SqlCommand command = new SqlCommand("SELECT * FROM [Costs]", sqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                
                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(sqlReader["DateTime"] + "  " + Convert.ToString(sqlReader["Category"]) + "  " + Convert.ToString(sqlReader["Cost"]));
                    
                    if (DateTime.Now.Month == Convert.ToDateTime(sqlReader["DateTime"]).Month)
                    {
                        monthCosts += Convert.ToInt32(sqlReader["Cost"]);
                    }
                    if (DateTime.Now.Day == Convert.ToDateTime(sqlReader["DateTime"]).Day)
                    {
                        dayCosts += Convert.ToInt32(sqlReader["Cost"]);
                    }
                }
                label8.Text = dayCosts.ToString();
                label10.Text = monthCosts.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command;
            if (label3.Visible)
                label3.Visible = false;
            if(!string.IsNullOrEmpty(textBox1.Text)&&!string.IsNullOrEmpty(textBox2.Text))
            {
                command = new SqlCommand("INSERT INTO [Costs] (DateTime, Category, Cost)VALUES(@DateTime, @Category, @Cost)", sqlConnection);
                command.Parameters.AddWithValue("DateTime", DateTime.Now);
                command.Parameters.AddWithValue("Category", textBox1.Text);
                command.Parameters.AddWithValue("Cost", textBox2.Text);

                await command.ExecuteNonQueryAsync();
            }
            else
            {
                label3.Visible = true;
                label3.Text = "Поля Категория и Цена не заполнены";
            }
            
        }

        private async void tabControl1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            SqlDataReader sqlReader = null;

            SqlCommand command = new SqlCommand("SELECT * FROM [Costs]", sqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                monthCosts = 0;
                dayCosts = 0;

                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(sqlReader["DateTime"] + "  " + Convert.ToString(sqlReader["Category"]) + "  " + Convert.ToString(sqlReader["Cost"]));
                    if (DateTime.Now.Month == Convert.ToDateTime(sqlReader["DateTime"]).Month)
                    {
                        monthCosts += Convert.ToInt32(sqlReader["Cost"]);
                    }
                    if (DateTime.Now.Day == Convert.ToDateTime(sqlReader["DateTime"]).Day)
                    {
                        dayCosts += Convert.ToInt32(sqlReader["Cost"]);
                    }
                }
                label8.Text = dayCosts.ToString();
                label10.Text = monthCosts.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("TRUNCATE TABLE [Costs] ", sqlConnection);
            await command.ExecuteNonQueryAsync();
            listBox1.Items.Clear();

            monthCosts = 0;
            dayCosts = 0;
            label8.Text = Convert.ToString(0);
            label10.Text = Convert.ToString(0);
        }

        private async void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            listBox1.Items.Clear();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [Costs]", sqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                monthCosts = 0;
                dayCosts = 0;
                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(sqlReader["DateTime"] + "  " + Convert.ToString(sqlReader["Category"]) + "  " + Convert.ToString(sqlReader["Cost"]));
                    if (DateTime.Now.Month == Convert.ToDateTime(sqlReader["DateTime"]).Month)
                    {
                        monthCosts += Convert.ToInt32(sqlReader["Cost"]);
                    }
                    if (DateTime.Now.Day == Convert.ToDateTime(sqlReader["DateTime"]).Day)
                    {
                        dayCosts += Convert.ToInt32(sqlReader["Cost"]);
                    }
                }
                label8.Text = dayCosts.ToString();
                label10.Text = monthCosts.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

            
        }
    }
}
