using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace CitySmart
{
    class MultiCommbox
    {
        public class ComboBoxEx : ComboBox
        {
            CheckedListBox lst = new CheckedListBox();
            public ComboBoxEx()
            {
            }
            private bool _IsMultiSelect = false;
            public bool IsMultiSelect
            {
                get { return _IsMultiSelect; }
                set
                {
                    _IsMultiSelect = value;
                    if (_IsMultiSelect)
                    {
                        this.DrawMode = DrawMode.OwnerDrawFixed;//只有设置这个属性为OwnerDrawFixed才可能让重画起作用
                        this.IntegralHeight = false;
                        this.DoubleBuffered = true;
                        this.DroppedDown = false;
                        this.DropDownHeight = 1;
                        this.DropDownStyle = ComboBoxStyle.DropDownList;
                        lst.CheckOnClick = true;
                        lst.ItemCheck += new ItemCheckEventHandler(lst_ItemCheck);
                        this.ItemCheck += new ItemCheckEventHandler(lst_ItemCheck);
                        lst.BorderStyle = BorderStyle.Fixed3D;
                        lst.Visible = false;
                    }
                    else
                    {
                        this.DrawMode = DrawMode.Normal;//只有设置这个属性为OwnerDrawFixed才可能让重画起作用
                        this.IntegralHeight = true;
                        this.DoubleBuffered = true;
                        this.DroppedDown = true;
                        this.DropDownHeight = 106;
                    }
                }
            }
            public event ItemCheckEventHandler ItemCheck;
            Dictionary<object, string> Values = new Dictionary<object, string>();
            void lst_ItemCheck(object sender, ItemCheckEventArgs e)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    if (Values.ContainsKey(lst.SelectedValue))
                    {
                        throw new Exception("Value具有重复的值！");
                    }
                    Values.Add(lst.SelectedValue, lst.Text);
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    if (Values.ContainsKey(lst.SelectedValue))
                    {
                        Values.Remove(lst.SelectedValue);
                    }
                }
            }
            public override string Text
            {
                get
                {
                    if (IsMultiSelect)
                    {
                        string s = "";
                        foreach (KeyValuePair<object, string> m in Values)
                        {
                            s = s + m.Value + ";";
                        }
                        return s;
                    }
                    return base.Text;
                }
                set
                {
                    base.Text = value;
                }
            }
            public string SelectedValues
            {
                get
                {
                    if (IsMultiSelect)
                    {
                        string s = "";
                        foreach (KeyValuePair<object, string> m in Values)
                        {
                            s = s + m.Key.ToString() + ";";
                        }
                        return s;
                    }
                    return "";
                }
            }
            #region Property
            
            public ListBox.SelectedObjectCollection SelectedItems
            {
                get
                {
                    return lst.SelectedItems;
                }
            }
            #endregion
            #region override
            protected override void OnMouseDown(MouseEventArgs e)
            {
                if (IsMultiSelect)
                {
                    this.DroppedDown = false;
                }
            }
            protected override void OnMouseUp(MouseEventArgs e)
            {
                if (IsMultiSelect)
                {
                    this.DroppedDown = false;
                    lst.Focus();
                }
            }
            protected override void OnDropDown(EventArgs e)
            {
                if (IsMultiSelect)
                {
                    lst.Visible = !lst.Visible;
                    lst.ItemHeight = this.ItemHeight;
                    lst.BorderStyle = BorderStyle.FixedSingle;
                    lst.Size = new Size(this.DropDownWidth, this.ItemHeight * (this.MaxDropDownItems - 1) - (int)this.ItemHeight / 2);
                    lst.Location = new System.Drawing.Point(this.Left, this.Top + this.ItemHeight + 6);
                    lst.BeginUpdate();
                    if (this.DataSource != null)
                    {
                        lst.DisplayMember = this.DisplayMember;
                        lst.ValueMember = this.ValueMember;
                        lst.DataSource = this.DataSource;
                    }
                    lst.EndUpdate();
                    if (!this.Parent.Controls.Contains(lst))
                    {
                        this.Parent.Controls.Add(lst);
                        lst.BringToFront();
                    }
                }
            }
            #endregion
        }
    }
}
