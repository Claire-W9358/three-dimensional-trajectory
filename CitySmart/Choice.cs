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
    public partial class Choice : Form
    {
        private SelectOption _option;
        private string _dlgKind;
        private bool[] backColor = new bool[2];
        private List<string> _sourceStr;
        private List<string> _selected;
        private string _lane;

        public EventHandler submitSelectOpt;
        public EventHandler selectVehileRoute;
        public Choice _choice;
        


        public Choice(SelectOption option_, List<string> sourceStr_, List<string> selected_)
        {
            InitializeComponent();
            _option = option_;
            _sourceStr = sourceStr_;
            _selected = selected_;
            switch (_option)
            { 
                case SelectOption.ROUT_DEC:
                    _dlgKind = "路径选择";
                    break;
                case SelectOption.VEHC_TYPE:
                    _dlgKind = "车辆类型";
                    break;
                case SelectOption.LINK:
                    _dlgKind = "路段编号";
                    break;
                case SelectOption.LANE:
                    _dlgKind = "车道编号";
                    break;
            }
            InitList();
            
        }

        public Choice(string lane_, List<string> sourceStr_, List<string> selected_)
        {
            InitializeComponent();
            _lane = lane_;
            _sourceStr = sourceStr_;
            _selected = selected_;
            _option = SelectOption.VEHICLE_ROUTE;
            _dlgKind = "路径选择";

            InitList();

        }

        private void button5_Click(object sender, EventArgs e)
        {            
            List<string> param = new List<string>();
            foreach (ListViewItem item in this.listView2.Items)
            {
                param.Add(item.SubItems[1].Text);               
            }

            if (_option == SelectOption.VEHICLE_ROUTE)
            {
                submitSelectOpt(this, new SelectParams(_lane, param));
                this.Close();
                return;
            }

            submitSelectOpt(this, new SelectParams(_option, param));
            this.Close();
        }

        private void InitList()
        {
            this.listView1.Clear();
            this.listView1.Columns.Clear();

            this.listView2.Clear();
            this.listView2.Columns.Clear();

            this.listView1.GridLines = true; //显示表格线
            this.listView1.View = View.Details;//显示表格细节
            this.listView1.LabelEdit = true; //是否可编辑,ListView只可编辑第一列。
            this.listView1.Scrollable = true;//有滚动条
            this.listView1.HeaderStyle = ColumnHeaderStyle.Clickable;//对表头进行设置
            this.listView1.FullRowSelect = true;//是否可以选择行

            this.listView2.GridLines = true; //显示表格线
            this.listView2.View = View.Details;//显示表格细节
            this.listView2.LabelEdit = true; //是否可编辑,ListView只可编辑第一列。
            this.listView2.Scrollable = true;//有滚动条
            this.listView2.HeaderStyle = ColumnHeaderStyle.Clickable;//对表头进行设置
            this.listView2.FullRowSelect = true;//是否可以选择行

            int disWidth = 240;
            this.listView1.Columns.Add("", 0);
            this.listView1.Columns.Add(_dlgKind, disWidth);

            this.listView2.Columns.Add("", 0);
            this.listView2.Columns.Add(_dlgKind, disWidth);

            backColor[0] = false;
            backColor[1] = false;

            ImportAll(0);
            SetSelected();
        }

        private void SetSelected()
        {
            foreach (string str in _selected)
            {
                DeleteRow(0, str);
                AddRow(1, str);
            }
        }

        private void AddRow(int list_, string data_)
        {            
            

            ListViewItem[] p = new ListViewItem[1];
            switch (list_)
            {
                case 0:
                    foreach (ListViewItem item in this.listView1.Items)
                    {
                        if (data_ == item.SubItems[1].Text)
                        {
                            return;
                        }
                    }
                    p[0] = new ListViewItem(new string[] { "", data_ });
                    this.listView1.Items.AddRange(p);
                    break;

                case 1:
                    foreach (ListViewItem item in this.listView2.Items)
                    {
                        if (data_ == item.SubItems[1].Text)
                        {
                            return;
                        }
                    }
                    p[0] = new ListViewItem(new string[] { "", data_ });
                    this.listView2.Items.AddRange(p);
                    break;
            }            
        }

        private void DeleteRow(int list_, string data_)
        {
            switch(list_)
            {
                case 0:                    
                    foreach (ListViewItem item in this.listView1.Items)
                    {
                        if (data_ == item.SubItems[1].Text)
                        {
                            item.Remove();
                        }
                    }
                    return;
                case 1:
                    foreach (ListViewItem item in this.listView2.Items)
                    {
                        if (data_ == item.SubItems[1].Text)
                        {
                            item.Remove();
                        }
                    }
                    return;
            } 
        }

        private void DeleteAll(int list_)
        { 
            switch(list_)
            {
                case 0:                    
                        foreach (ListViewItem item in this.listView1.Items)
                        {
                            item.Remove();
                        }                    
                    break;

                case 1:
                    foreach (ListViewItem item in this.listView2.Items)
                        {
                            item.Remove();
                        }                    
                    break;
            }
        }

        private void ImportAll(int list_)
        {
            DeleteAll(list_);
            foreach (string data in _sourceStr)
            {
                AddRow(list_, data);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteAll(0);
            ImportAll(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteAll(1);
            ImportAll(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    string value = item.SubItems[1].Text;
                    AddRow(1, value);                   
                }

                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    string value = item.SubItems[1].Text;
                    DeleteRow(0, value);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listView2.SelectedIndices != null && listView2.SelectedIndices.Count > 0)
            {
                foreach (ListViewItem item in listView2.SelectedItems)
                {
                    string value = item.SubItems[1].Text;
                    AddRow(0, value);
                }

                foreach (ListViewItem item in listView2.SelectedItems)
                {
                    string value = item.SubItems[1].Text;
                    DeleteRow(1, value);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            if (_option != SelectOption.ROUT_DEC)
            {
                return;
            }

            string routDec = "";
            if (this.listView2.SelectedIndices != null && listView2.SelectedIndices.Count > 0)
            {
                foreach (ListViewItem item in listView2.SelectedItems)
                {
                    routDec = item.SubItems[1].Text;
                }
            }

            selectVehileRoute(this, new SelectParams(routDec));
            
        }
        
    }
}
