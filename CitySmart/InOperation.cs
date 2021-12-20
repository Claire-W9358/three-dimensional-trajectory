using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CitySmart
{
    public partial class InOperation : Form
    {

        public InOperation()
        {
            InitializeComponent();

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void InOperation_Load(object sender, EventArgs e)
        {
            progressBar1.Maximum = 150;//设置最大长度值
            progressBar1.Value = 0;//设置当前值
            progressBar1.Step = 5;//设置没次增长多少
            for (int i = 0; i < 10; i++)//循环
            {
                System.Threading.Thread.Sleep(1000);//暂停1秒
                progressBar1.Value += progressBar1.Step; //让进度条增加一次

            }
        }
    }
}
