using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RenameMp3
{
    class Program
    {
        static void Main(string[] args)
        {
            var dirName = "E:\\MUSIC FOR LEXUS\\usb1\\CD";
            int dirSubName = 101;

            for(int i = 0; i < 10; i++)
            {
                DirectoryInfo d = new DirectoryInfo(dirName + dirSubName.ToString().Substring(dirSubName.ToString().Length - 2));
                FileInfo[] infos = d.GetFiles();
                int k = 101;
                foreach (FileInfo f in infos)
                {
                    File.Move(f.FullName, Path.Combine(f.DirectoryName, "track" + k.ToString().Substring(k.ToString().Length - 2) + ".mp3"));
                    k++;
                }
                dirSubName++;
            }



            
         
        }

    }
}
