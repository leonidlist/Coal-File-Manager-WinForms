using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CoalFileManagerWinForms
{
    public partial class Form1 : Form
    {
        private DriveInfo[] _drives;
        private DirectoryInfo _left;
        private DirectoryInfo _right;               

        public Form1()
        {
            InitializeComponent();
            _drives = DriveInfo.GetDrives();
            InitTabs();
            InitComboBoxes();
            UpdateLabels();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems[0].Text == "..")
            {
                _left = _left.Parent ?? _left;
                Update(listView1, _left);
            }
            else if(Directory.Exists(_left.FullName + "\\" + listView1.SelectedItems[0].Text))
            {
                _left = new DirectoryInfo(_left.FullName + "\\" + listView1.SelectedItems[0].Text);
                Update(listView1, _left);
            }
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            if (listView2.SelectedItems[0].Text == "..")
            {
                _right = _right.Parent ?? _right;
                Update(listView2, _right);
            }
            else if (Directory.Exists(_left.FullName + "\\" + listView2.SelectedItems[0].Text))
            {
                _right = new DirectoryInfo(_right.FullName + "\\" + listView2.SelectedItems[0].Text);
                Update(listView2, _right);
            }
        }

        private void InitTabs(int index = 0)
        {
            _left = _drives[index].RootDirectory;
            _right = _drives[index].RootDirectory;
            InitColumns();
            listView1.Items.Add("..");
            listView2.Items.Add("..");
            foreach (var dir in _left.GetDirectories())
            {
                listView1.Items.Add(dir.Name);
                listView2.Items.Add(dir.Name);
            }
            foreach(var file in _left.GetFiles())
            {
                listView1.Items.Add(file.Name);
                listView2.Items.Add(file.Name);
            }
        }

        private void InitColumns()
        {
            listView1.Columns.Add("Имя", 200);
            listView1.Columns.Add("Тип", 100);
            listView1.Columns.Add("Размер", 100);
            listView1.Columns.Add("Дата", 100);
            listView2.Columns.Add("Имя", 200);
            listView2.Columns.Add("Тип", 100);
            listView2.Columns.Add("Размер", 100);
            listView2.Columns.Add("Дата", 100);
        }

        private void InitComboBoxes()
        {
            comboBox1.Items.AddRange(_drives);
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.AddRange(_drives);
            comboBox2.SelectedIndex = 0;
        }

        private void UpdateLabels()
        {
            label1.Text = _left.FullName;
            label2.Text = _right.FullName;
        }

        private void Update(ListView listView, DirectoryInfo directory)
        {
            UpdateLabels();
            listView.Items.Clear();
            if(directory.Parent != null)
            {
                listView.Items.Add("..");
            }
            foreach (var dir in directory.GetDirectories())
            {
                ListViewItem tmp = new ListViewItem(dir.Name);
                tmp.SubItems.Add("");
                tmp.SubItems.Add("<Папка>");
                tmp.SubItems.Add(dir.CreationTime.ToShortDateString());
                listView.Items.Add(tmp);
            }
            foreach (var file in directory.GetFiles())
            {
                ListViewItem tmp = new ListViewItem(file.Name);
                tmp.SubItems.Add(file.Extension);
                tmp.SubItems.Add(file.Length/1000 + " Кб");
                tmp.SubItems.Add(file.CreationTime.ToShortDateString());
                listView.Items.Add(tmp);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(DriveInfo.GetDrives()[comboBox1.SelectedIndex].IsReady)
            {
                label3.Text = DriveInfo.GetDrives()[comboBox1.SelectedIndex].AvailableFreeSpace/1000000 + " Мб из " + DriveInfo.GetDrives()[comboBox1.SelectedIndex].TotalSize/1000000 + " Мб";
                _left = DriveInfo.GetDrives()[comboBox1.SelectedIndex].RootDirectory;
                Update(listView1, _left);
            }
            else
            {
                MessageBox.Show("Drive is not ready.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DriveInfo.GetDrives()[comboBox2.SelectedIndex].IsReady)
            {
                label4.Text = DriveInfo.GetDrives()[comboBox2.SelectedIndex].AvailableFreeSpace / 1000000 + " Мб из " + DriveInfo.GetDrives()[comboBox2.SelectedIndex].TotalSize / 1000000 + " Мб";
                _right = DriveInfo.GetDrives()[comboBox2.SelectedIndex].RootDirectory;
                Update(listView2, _right);
            }
            else
            {
                MessageBox.Show("Drive is not ready.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripView1Button_Click(object sender, EventArgs e)
        {
            UncheckAllButtons();
            toolStripView1Button.Checked = true;
            listView1.View = View.Details;
            listView2.View = View.Details;
        }

        private void toolStripView2Button_Click(object sender, EventArgs e)
        {
            UncheckAllButtons();
            toolStripView2Button.Checked = true;
            listView1.View = View.LargeIcon;
            listView2.View = View.LargeIcon;
        }

        private void toolStripView3Button_Click(object sender, EventArgs e)
        {
            UncheckAllButtons();
            toolStripView3Button.Checked = true;
            listView1.View = View.List;
            listView2.View = View.List;
        }

        private void toolStripView4Button_Click(object sender, EventArgs e)
        {
            UncheckAllButtons();
            toolStripView4Button.Checked = true;
            listView1.View = View.SmallIcon;
            listView2.View = View.SmallIcon;
        }

        private void toolStripView5Button_Click(object sender, EventArgs e)
        {
            UncheckAllButtons();
            toolStripView5Button.Checked = true;
            listView1.View = View.Tile;
            listView2.View = View.Tile;
        }

        private void UncheckAllButtons()
        {
            toolStripView1Button.Checked = false;
            toolStripView2Button.Checked = false;
            toolStripView3Button.Checked = false;
            toolStripView4Button.Checked = false;
            toolStripView5Button.Checked = false;
        }

        private void toolStripRefreshButton_Click(object sender, EventArgs e)
        {
            Update(listView1, _left);
            Update(listView2, _right);
        }
    }
}
