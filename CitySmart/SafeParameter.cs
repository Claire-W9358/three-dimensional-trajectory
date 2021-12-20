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
    public partial class SafeParameter : Form
    {
        public SafeParameter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //0119
            //打算选择安全分析前，做数据预处理
           /* InOperation inOperation = new InOperation();
            inOperation.ShowDialog();
            */
            dataprocess.process dp = new dataprocess.process();
            dp.dataprocess();
            //调用matlab中的定义的function 

            double lc_l = Convert.ToDouble(this.textBox1.Text);
            double lc_w = Convert.ToDouble(this.textBox2.Text);
            double lc_t = Convert.ToDouble(this.textBox3.Text);
            double sc_l = Convert.ToDouble(this.textBox9.Text);
            double sc_w = Convert.ToDouble(this.textBox10.Text);
            double sc_t = Convert.ToDouble(this.textBox11.Text);
            double nm_l = Convert.ToDouble(this.textBox12.Text);
            double nm_w = Convert.ToDouble(this.textBox13.Text);
            double nm_t = Convert.ToDouble(this.textBox14.Text); ;

            double timestep = Convert.ToDouble(this.textBox4.Text);

            double m = Convert.ToDouble(this.textBox5.Text);
            double t = Convert.ToDouble(this.textBox6.Text);
            double w = Convert.ToDouble(this.textBox7.Text);

            double delta = Convert.ToDouble(this.textBox8.Text);

            pet.petvariable pv = new pet.petvariable();
            pv.pet(lc_l, lc_w, lc_t, sc_l, sc_w, sc_t, nm_l, nm_w, nm_t);//调用matlab中的定义的function

            pemethod.pe_method pm = new pemethod.pe_method();
            pm.pemethod(timestep, m, t, w);

            tradition_method.tradition tradition = new tradition_method.tradition();
            tradition.tradition_method(timestep, delta);

            evaluation.final eval = new evaluation.final();
            eval.evaluation();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string path = Application.StartupPath;
            openFileDialog.InitialDirectory = path;
            openFileDialog.Filter = "png文件|*.png*|txt文件|*.txt|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //0223,未操作读取的文件
            }
            
            this.Close();
        }
        
        }
    }
