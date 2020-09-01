using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CostLib
{
    public static class CostsDB
    {
        public static SqlConnection sqlConnection;
        public static SqlDataReader sqlReader;
        public static SqlCommand sqlCommand;

        public static int monthCosts = 0;
        public static int dayCosts = 0;
        public async static void OpenSqlConnection(string connStr)
        {
            sqlConnection = new SqlConnection(connStr);
            await sqlConnection.OpenAsync();
        }
        public async static void SqlCmdView(string request, ListBox lsCosts, Label day, Label month)
        {
            sqlReader = null;
            sqlCommand = new SqlCommand(request, sqlConnection);
            try
            {
                sqlReader = await sqlCommand.ExecuteReaderAsync();

                monthCosts = 0;
                dayCosts = 0;

                while (await sqlReader.ReadAsync())
                {
                    lsCosts.Items.Add(sqlReader["DateTime"] + "  " + Convert.ToString(sqlReader["Category"]) + "  " + Convert.ToString(sqlReader["Cost"]));
                    DayMonthAddCost();
                }
                day.Text = dayCosts.ToString();
                month.Text = monthCosts.ToString();
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
        public async static void SqlCmdIncert(TextBox category, TextBox cost)
        {
            sqlCommand = new SqlCommand("INSERT INTO [Costs] (DateTime, Category, Cost)VALUES(@DateTime, @Category, @Cost)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("DateTime", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("Category", category.Text);
            sqlCommand.Parameters.AddWithValue("Cost", cost.Text);

            await sqlCommand.ExecuteNonQueryAsync();
        }
        private static void DayMonthAddCost()
        {
            if (DateTime.Now.Month == Convert.ToDateTime(sqlReader["DateTime"]).Month)
            {
                monthCosts += Convert.ToInt32(sqlReader["Cost"]);
            }
            if (DateTime.Now.Day == Convert.ToDateTime(sqlReader["DateTime"]).Day)
            {
                dayCosts += Convert.ToInt32(sqlReader["Cost"]);
            }
        }
        private static void ViewCostInfo(Label day, Label month)
        {
            day.Text = dayCosts.ToString();
            month.Text = monthCosts.ToString();
        }
    }
}
