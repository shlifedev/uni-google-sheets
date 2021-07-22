using Hamster.UG;
using Hamster.UG.IO.FileReader;
using Hamster.UG.IO.FileWriter;
using UGS.Protocol.v2.Req;
using System;
using System.IO;
using Hamster.UG.Reflection;
using Hamster.UGS.Validator;

namespace ConsoleApp1
{
    class Program
    { 
        static void Main(string[] args)
        {
       
            // Your Script Link
            GoogleSheet.Initialize("https://script.google.com/macros/s/AKfycbxpqlYM5SfX0pL2RHzgiT_cFykKFLkcr_PgzU1KKnVx2Aa6YNN3/exec", "123123");


            string id = "1SgkBh-HngzRYL3Vl9PrxbMQ_vY89f0hx_BGpgXP4Ffc";
            // Your Google Folder ID
            GoogleSheet.Generate(id,
            () => {
                System.IO.DirectoryInfo di1 = new System.IO.DirectoryInfo("TableScript");
                System.IO.DirectoryInfo di2 = new System.IO.DirectoryInfo("CachedJson");

                DirectoryCopy(di1.FullName, "../../../TableScript", true);
                DirectoryCopy(di2.FullName, "../../../CachedJson", true);
            });

            //Hamster.UGS.Validator.UGSValidator.InitializeValidatorRequester(new GoogleDriveWebRequesterV2());
            //CompareSheetId validator = new CompareSheetId();
            //validator.isValid(x => {
            //    Console.WriteLine(x);
            //}, "1YDztWDjHvyvgyM9tVLlJIxk16gDjLZLm");

            GoogleSheet.LoadAllData();
            foreach (var value in DefaultTable.Data.DataList)
                Console.WriteLine($"{value}");


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
