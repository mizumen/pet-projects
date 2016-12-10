/*
 MP3 files rename for YATOUR device
 */


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
            Console.WriteLine("Input the directory with CD folders:");
            string dirName = Console.ReadLine();
            //var dirName = "E:\\MUSIC FOR LEXUS\\usb1\\CD";
            int dirSubName = 101;

            Console.WriteLine("Input the amount of such folders");
            int foldersAmount = Convert.ToInt32(Console.ReadLine());

            Directory.CreateDirectory(dirName + "_renamed");
            var newDirName = dirName + "_renamed";

            for (int i = 0; i < foldersAmount; i++)
            {
                DirectoryInfo d = new DirectoryInfo(dirName + "\\CD" + dirSubName.ToString().Substring(dirSubName.ToString().Length - 2));
                FileInfo[] infos = d.GetFiles();
                int k = 101;
                foreach (FileInfo f in infos)
                {
                    Directory.CreateDirectory(newDirName + "\\CD" + dirSubName.ToString().Substring(dirSubName.ToString().Length - 2));

                    File.Copy(f.FullName, Path.Combine(newDirName + "\\CD" + dirSubName.ToString().Substring(dirSubName.ToString().Length - 2),
                        "track" + k.ToString().Substring(k.ToString().Length - 2) + ".mp3"), true);
                    
                    k++;
                }
                dirSubName++;
            }
        }
    }
}
