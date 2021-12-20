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
    public partial class Efficiency : Form
    {
        private CitySmart _citySmart = new CitySmart();
        public Efficiency()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BasicInfo basic = new BasicInfo();

            double startTime = Convert.ToDouble(basic.textBox3.Text);
            double endTime = Convert.ToDouble(basic.textBox4.Text);
            double buttonX = Convert.ToDouble(basic.textBox6.Text);
            double topX = Convert.ToDouble((basic.textBox5.Text));
            double buttonY = Convert.ToDouble(basic.textBox8.Text);
            double topY = Convert.ToDouble(basic.textBox7.Text);


            double elength = Convert.ToDouble(basic.textBox10.Text);
            double ewidth = Convert.ToDouble(basic.textBox14.Text);
            double slength = Convert.ToDouble(basic.textBox2.Text);
            double swidth = Convert.ToDouble(basic.textBox15.Text);
            double nlength = Convert.ToDouble(basic.textBox9.Text);
            double nwidth = Convert.ToDouble(basic.textBox1.Text);
            double wlength = Convert.ToDouble(basic.textBox11.Text);
            double wwidth = Convert.ToDouble(basic.textBox12.Text);
            double length = (elength + slength + nlength + wlength) / 4;

            double tstep = Convert.ToDouble(this.textBox8.Text);

            double space = elength * (ewidth-3) + wlength * (wwidth-3) + nlength * (nwidth-3) + slength * (swidth-3) + wwidth * ewidth;
            double _totalvolume = (endTime - startTime) * space;

            CRstSet rst = _citySmart.CSCompute_Efficiency(startTime, endTime, buttonX, topX, buttonY, topY, tstep, _totalvolume);
            string stats_car = PrivateRound(rst.carNum, 4).ToString();
            string stats_v = PrivateRound(rst.avaMS, 4).ToString();
            string stats_delay = PrivateRound(rst.avaTQDelay, 4).ToString();
            string steingstats_queue = PrivateRound(rst.density * 1000, 4).ToString();

            this.textBox10.Text = Convert.ToString(rst.utilization * 100);
            double carnumber = Convert.ToDouble(stats_car);
            this.textBox9.Text = Convert.ToString(rst.utilization * 100/carnumber);

        }


        private double PrivateRound(double v_, int length_)
        {
            if (v_ % 1 < 0.0001)
            {
                v_ = Convert.ToInt32(v_);
            }

            string tmp = v_.ToString();
            string[] array = tmp.Split('.');
            if (array.Length == 1)
            {
                return v_;
            }
            else
            {
                if (array[1].Length > length_)
                {
                    char[] a = array[1].Substring(0, length_).ToArray();
                    foreach (char character in a)
                    {
                        if (character != '0')
                        {
                            return Convert.ToDouble((array[0] + "." + array[1].Substring(0, length_)));
                        }
                    }
                    return Convert.ToDouble((array[0]));
                }
                else
                {
                    return v_;
                }
            }
        }
    }
}
