using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;

namespace CoalFileManagerWinForms
{
    public partial class Form1 : Form
    {
        private DriveInfo[] _drives;
        private DirectoryInfo _left;
        private DirectoryInfo _right;
        private ListView _active;
        private PropsForm _propsForm;
        private object _memory;

        public Form1()
        {
            InitializeComponent();
            _drives = DriveInfo.GetDrives();
            _propsForm = new PropsForm();
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
            else if(Directory.Exists(_left.FullName + "\\" + listView1.FocusedItem.Text))
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
            else if (Directory.Exists(_right.FullName + "\\" + listView2.FocusedItem.Text))
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
                listView.Items.Add("..");
            foreach (var dir in directory.GetDirectories())
            {
                ListViewItem tmp = new ListViewItem(dir.Name);
                tmp.SubItems.Add("");
                tmp.SubItems.Add("<Папка>");
                tmp.SubItems.Add(dir.CreationTime.ToShortDateString());
                tmp.ImageIndex = 0;
                listView.Items.Add(tmp);
            }
            foreach (var file in directory.GetFiles())
            {
                ListViewItem tmp = new ListViewItem(file.Name);
                tmp.SubItems.Add(file.Extension);
                tmp.SubItems.Add(file.Length/1000 + " Кб");
                tmp.SubItems.Add(file.CreationTime.ToShortDateString());
                tmp.ImageIndex = 1;
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
                comboBox1.SelectedIndex = 0;
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
                comboBox1.SelectedIndex = 0;
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
            Update(_active, _active==listView1?_left:_right);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            _active = listView1;
            if(e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == (Keys.Control | Keys.C))
            {
                ContextMenuMethods.Copy(_active, (_active == listView1 ? label1 : label2).Text);
                return true;
            }
            else if(keyData == (Keys.Control | Keys.V))
            {
                ContextMenuMethods.Paste(_active, (_active == listView1 ? label1 : label2).Text + "\\" + _active.FocusedItem.Text);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            _active = listView2;
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
        }

        private void коопироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextMenuMethods.Copy(_active, (_active == listView1 ? label1 : label2).Text);
            Update(_active, _active == listView1 ? _left : _right);
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextMenuMethods.Paste(_active, (_active == listView1 ? label1 : label2).Text + "\\" + _active.FocusedItem.Text);
            Update(_active, _active == listView1 ? _left : _right);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show($"Вы уверены, что хотите удалить {_active.FocusedItem.Text} ?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ContextMenuMethods.Delete(_active, (_active == listView1 ? label1 : label2).Text);
                Update(_active, _active == listView1 ? _left : _right);
            }
        }

        private void папкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem tmp = new ListViewItem("Новая папка", 0);
            _active.Items.Insert(0, tmp);
            Directory.CreateDirectory((_active == listView1 ? label1 : label2).Text + "\\" + tmp.Text);
            _active.Items[0].BeginEdit();
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                if (Directory.Exists(label1.Text + "\\" + _memory as string) && listView1.FocusedItem.Text != _memory as string)
                    Directory.Move(label1.Text + "\\" + (_memory as string), label1.Text + "\\" + listView1.FocusedItem.Text);
                else if (File.Exists(label1.Text + "\\" + _memory as string) && listView1.FocusedItem.Text != _memory as string)
                    File.Move(label1.Text + "\\" + (_memory as string), label1.Text + "\\" + listView1.FocusedItem.Text);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally
            {
                Update(listView1, _left);
            }
        }

        private void listView1_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            _memory = listView1.FocusedItem.Text;
        }

        private void переименоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _active.FocusedItem.BeginEdit();
        }

        private void listView2_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                if (Directory.Exists(label2.Text + "\\" + _memory as string) && listView2.FocusedItem.Text != _memory as string)
                    Directory.Move(label2.Text + "\\" + (_memory as string), label2.Text + "\\" + listView2.FocusedItem.Text);
                else if (File.Exists(label2.Text + "\\" + _memory as string) && listView2.FocusedItem.Text != _memory as string)
                    File.Move(label2.Text + "\\" + (_memory as string), label2.Text + "\\" + listView2.FocusedItem.Text);
            }
            catch (Exception) { }
            finally
            {
                Update(listView2, _right);
            }
        }

        private void listView2_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            _memory = listView2.FocusedItem.Text;
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem tmp = new ListViewItem("Новый файл", 0);
            _active.Items.Insert(0, tmp);
            File.Create((_active == listView1 ? label1 : label2).Text + "\\" + tmp.Text);
            _active.Items[0].BeginEdit();
        }

        private void свойстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists((_active == listView1 ? label1 : label2).Text + "\\" + _active.FocusedItem.Text))
                _propsForm.SetTargetObject(new FileInfo((_active == listView1 ? label1 : label2).Text + "\\" + _active.FocusedItem.Text));
            else if (Directory.Exists((_active == listView1 ? label1 : label2).Text + "\\" + _active.FocusedItem.Text))
                _propsForm.SetTargetObject(new DirectoryInfo((_active == listView1 ? label1 : label2).Text + "\\" + _active.FocusedItem.Text));
            _propsForm.ShowDialog();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listView1.DoDragDrop(label1.Text + "\\" + listView1.FocusedItem.Text, DragDropEffects.Copy);
        }

        private void listView2_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else if(e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void listView2_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                foreach (var file in files)
                {
                    if (File.Exists(file))
                        File.Copy(file, label2.Text);
                    else if (Directory.Exists(file))
                        DirMethods.CopyDirectory(new DirectoryInfo(file), new DirectoryInfo(label2.Text));
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                if (File.Exists(e.Data.GetData(DataFormats.Text) as string))
                    File.Copy(e.Data.GetData(DataFormats.Text) as string, label2.Text);
                else if (Directory.Exists(e.Data.GetData(DataFormats.Text) as string))
                    DirMethods.CopyDirectory(new DirectoryInfo(e.Data.GetData(DataFormats.Text) as string), new DirectoryInfo(label2.Text));
            }
        }
    }
}
