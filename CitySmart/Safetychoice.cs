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
    public partial class Safetychoice : Form
    {
        public Safetychoice()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SafeParameter parameter = new SafeParameter();
            parameter.ShowDialog();
            this.Close();
        }
    }
}
