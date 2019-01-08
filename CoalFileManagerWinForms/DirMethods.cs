using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace CoalFileManagerWinForms
{
    internal static class DirMethods
    {
        public static void CopyDirectory(DirectoryInfo from, DirectoryInfo to)
        {
            try
            {
                ArrayList contains = new ArrayList();
                contains.AddRange(from.GetDirectories());
                contains.AddRange(from.GetFiles());
                foreach (var item in contains)
                {
                    if (item is DirectoryInfo)
                    {
                        to.CreateSubdirectory((item as DirectoryInfo).Name);
                        CopyDirectory(item as DirectoryInfo, to.GetDirectories((item as DirectoryInfo).Name)[0]);
                    }
                    else if (item is FileInfo)
                    {
                        (item as FileInfo).CopyTo(to.FullName + "\\" + (item as FileInfo).Name);
                    }
                }
            }
            catch (Exception) { }
        }

        public static long DirectoryLength(DirectoryInfo dir)
        {
            ArrayList contains = new ArrayList();
            contains.AddRange(dir.GetDirectories());
            contains.AddRange(dir.GetFiles());
            long sum = 0;
            foreach(var d in contains)
            {
                if(d is DirectoryInfo)
                {
                    sum += DirectoryLength(d as DirectoryInfo);
                }
                else if(d is FileInfo)
                {
                    sum += (d as FileInfo).Length;
                }
            }
            return sum;
        }
    }
}
