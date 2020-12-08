using System;
using System.IO;
using System.Net;
using System.Text;
using Hamster.ZG;
using Hamster.ZG.IO.FileReader;
using Hamster.ZG.IO.FileWriter; 
internal class Program
{
    public static void Main(string[] args)
    {
        UnityGoogleSheet.Initalize("https://script.google.com/macros/s/AKfycbyOBVdYiUz6W1WJCHhV5SS4r0Bq3NIyCKW8ugVunsBD-4Bbn30U/exec", "dpqlcb123");
        UnityGoogleSheet.GenerateSheetInFolder("1Cm5ETfvReGvx2hRHJpxOoGoRSQhLkyxO", true, true); 
        UnityGoogleSheet.Load<Example1.Localization.Item.Name>();
        foreach (var value in Example1.Localization.Item.Name.NameList)
        {
            Console.WriteLine(value.KR);
        }
    }
}