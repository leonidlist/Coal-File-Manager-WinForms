using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CoalFileManagerWinForms
{
    public partial class PropsForm : Form
    {
        private object _target;
        public PropsForm()
        {
            InitializeComponent();
        }

        public void SetTargetObject(object target)
        {
            _target = target;
            ResetValues();
            Update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rename();
            this.Close();
        }

        private bool Rename()
        {
            try
            {
                if (_target is DirectoryInfo dir)
                    dir.MoveTo(dir.Root.FullName + "\\" + textBox1.Text);
                else if (_target is FileInfo file)
                    file.MoveTo(file.Directory + "\\" + textBox1.Text);

                return true;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private new void Update()
        {
            if(_target is FileInfo file)
            {
                pictureBox1.Image = Resource1.file;
                textBox1.Text = file.Name;
                label2.Text = file.Extension;
                label4.Text = file.FullName;
                label6.Text = file.Length.ToString() + " байт";
                label7.Text = file.CreationTime.ToShortDateString();
                label10.Text = file.LastWriteTime.ToShortDateString();
                label12.Text = file.LastAccessTime.ToShortDateString();
            }
            else if(_target is DirectoryInfo dir)
            {
                pictureBox1.Image = Resource1.folder;
                textBox1.Text = dir.Name;
                label2.Text = "<Папка>";
                label4.Text = dir.FullName;
                Thread thread = new Thread(new ParameterizedThreadStart(SetFolderLength));
                thread.Start(dir);
                label7.Text = dir.CreationTime.ToShortDateString();
                label10.Text = dir.LastWriteTime.ToShortDateString();
                label12.Text = dir.LastAccessTime.ToShortDateString();
            }
        }

        private void ResetValues()
        {
            pictureBox1.Image = null;
            textBox1.Text = "Вычисление...";
            label2.Text = "Вычисление...";
            label4.Text = "Вычисление...";
            label6.Text = "Вычисление...";
            label7.Text = "Вычисление...";
            label10.Text = "Вычисление...";
            label12.Text = "Вычисление...";
        }

        private void SetFolderLength(object dir)
        {
            try
            {
                label6.Text = DirMethods.DirectoryLength(dir as DirectoryInfo).ToString() + " байт";
            } catch(Exception e)
            {
                MessageBox.Show(e.Message);
                label6.Text = "Ошибка вычисления.";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Rename();
            Update();
        }
    }
}
