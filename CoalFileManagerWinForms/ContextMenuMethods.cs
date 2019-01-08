using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;

namespace CoalFileManagerWinForms
{
    static class ContextMenuMethods
    {
        public static void Delete(ListView _active, string path)
        {
            foreach (ListViewItem item in _active.SelectedItems)
            {
                if (File.Exists(path + "\\" + item.Text))
                    File.Delete(path + "\\" + item.Text);
                else if (Directory.Exists(path + "\\" + item.Text))
                    Directory.Delete(path + "\\" + item.Text, true);
            }
        }

        public static void Copy(ListView listView, string path)
        {
            StringCollection paths = new StringCollection();
            foreach (ListViewItem item in listView.SelectedItems)
            {
                paths.Add(path + "\\" + item.Text);
            }
            Clipboard.SetFileDropList(paths);
        }

        public static void Paste(ListView listView, string path)
        {
            DirectoryInfo tmp = new DirectoryInfo(path);
            if (tmp.Exists)
            {
                foreach (var item in Clipboard.GetFileDropList())
                {
                    FileInfo file = new FileInfo(item);
                    if (file.Exists)
                        File.Copy(item, tmp.FullName + "\\" + file.Name);

                    DirectoryInfo dir = new DirectoryInfo(item);
                    if (dir.Exists)
                        DirMethods.CopyDirectory(dir, tmp);
                }
            }
            else
                MessageBox.Show("Direcotry not exists.");
        }

        public static void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
