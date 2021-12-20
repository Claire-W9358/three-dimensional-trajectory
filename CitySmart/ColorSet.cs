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
    public partial class ColorSet : Form
    {
        public ColorSet()
        {
            InitializeComponent();
            ReflashColor();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigInfo.ReloadColorSet();
            ReflashColor();
        }

        private void ReflashColor()
        {
            int imgNum = ConfigInfo.colorSet[0];
            string add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox4.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[1];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox5.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[2];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox6.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[3];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox9.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[4];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox8.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[5];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox7.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[6];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox3.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[7];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox2.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[8];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[9];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox12.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[10];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox11.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);

            imgNum = ConfigInfo.colorSet[11];
            add = @"\pic\" + imgNum.ToString() + ".jpg";
            this.pictureBox10.Image = Image.FromFile(Directory.GetCurrentDirectory() + add);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HandleReflashColor(object sender_, EventArgs e)
        {
            ReflashColor();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(8);            
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(7);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(6);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(5);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(4);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(3);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(2);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(1);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(0);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(11);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(10);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            ColorPanel colorPanel = new ColorPanel(9);
            colorPanel.ReflashColor += new EventHandler(HandleReflashColor);
            colorPanel.ShowDialog();
        }
    }
}
