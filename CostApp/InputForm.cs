using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CostLib;

namespace CostApp
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (/*каждый элемент в таблице Wallet*/)
                count++;
            if (textBox1.Text != null&&count==0)
            {
                CostsDB.SqlCmdIncert(textBox1);
                //Program.mainform.monthWallet = Convert.ToInt32(textBox1.Text);
            }
            else
                label3.Text = "Поле ввода пусто или же бюджет уже установлен";
        }
    }
}
