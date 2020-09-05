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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                CostsDB.SqlCmdIncert(textBox1);
                //Program.mainform.monthWallet = Convert.ToInt32(textBox1.Text);
            }
            else
                label3.Text = "Поле ввода пусто";
        }
    }
}
