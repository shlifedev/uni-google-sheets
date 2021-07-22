using Hamster.UG;
using Hamster.UG.IO.FileReader;
using Hamster.UG.IO.FileWriter;
using UGS.Protocol.v2.Req;
using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    { 
        static void Main(string[] args)
        { 
            // Login
            GoogleSheet.Initialize("https://script.google.com/macros/s/AKfycbxpqlYM5SfX0pL2RHUGiT_cFykKFLkcr_PgzU1KKnVx2Aa6YNN3/exec", "123123");



            // Code Generate And Copy To Folder 
            GoogleSheet.Generate("1BXya0YQq980kbNBN_-hQAvmBrNkHFIoqXJkQTXIsXHQ",
            () => {
                System.IO.DirectoryInfo di1 = new System.IO.DirectoryInfo("TableScript");
                System.IO.DirectoryInfo di2 = new System.IO.DirectoryInfo("CachedJson");

                DirectoryCopy(di1.FullName, "../../../TableScript", true);
                DirectoryCopy(di2.FullName, "../../../CachedJson", true);
            });


            GoogleSheet.LoadAllData();
            foreach (var value in UnitData.Balance.BalanceList) 
                Console.WriteLine($"{value.id} {value.jump} {value.name} {value.speed}"); 
            
        }



        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

    }
}
