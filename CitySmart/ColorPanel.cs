using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CitySmart
{
    public partial class ColorPanel : Form
    {
        public int _colorID = 0;
        public EventHandler ReflashColor;
        public ColorPanel()
        {
            InitializeComponent();
        }

        public ColorPanel(int colorID_)
        {
            InitializeComponent();
            InitPanel();
            _colorID = colorID_;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void InitPanel()
        {
            int imgNum = 0;
            string add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 1;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox2.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 2;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox3.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 3;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox4.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 4;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox8.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 5;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox7.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 6;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox6.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 7;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox5.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 8;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox12.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 9;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox11.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 10;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox10.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = 11;
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox9.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 0;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 1;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 2;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 3;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 4;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 5;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 6;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 7;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 8;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 9;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 10;
            ReflashColor(this, new EventArgs());
            this.Close();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            ConfigInfo.colorSet[_colorID] = 11;
            ReflashColor(this, new EventArgs());
            this.Close();
        }
    }
}
