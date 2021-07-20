using Hamster.ZG;
using Hamster.ZG.IO.FileReader;
using Hamster.ZG.IO.FileWriter;
using HamsterGoogleSpreadSheet.ZG.ZG.Core.Http.ProtocolV2.Req;
using System;

namespace ConsoleApp1
{
    class Program
    {

 
        static void Main(string[] args)
        {
            GoogleDriveWebRequesterV2.Instance.Credential("https://script.google.com/macros/s/AKfycbxpqlYM5SfX0pL2RHzgiT_cFykKFLkcr_PgzU1KKnVx2Aa6YNN3/exec", "123123");
            GoogleDriveWebRequesterV2.Instance.ReadSpreadSheet(new ReadSpreadSheetReqModel("1Wl9pX4RSLPrNoubRO9qPpgPNdOIZKkWPtX8AOHuH1QI"), (err) =>
            {
                Console.WriteLine(err);

            }, (data) =>
            {
                Console.WriteLine(data.sheetIDList.Count);
                Console.WriteLine(data.spreadSheetID);
                Console.WriteLine(data.jsonObject);

            });


            //GoogleDriveWebRequesterV2.Instance.WriteObject(new WriteObjectReqModel("7g6_gVei-Zxh7UCbiIuXw7GAE_u1wG9Me5JJVhn8MZ8", "0", "1234", new string[] { "1234", "2", "3" }), (err) => {
            //    Console.WriteLine(err);

            //}, (data) => {
            //    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            //});


            //GoogleDriveWebRequesterV2.Instance.GetDriveDirectory(new GetDriveDirectoryReqModel("1KRue4wZdH1G0iLfLSUkHjP1i_nsguLNr"), (err) => {
            //    Console.WriteLine(err);

            //}, (data) => {
            //    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            //});


            //GoogleDriveWebRequesterV2.Instance.CreateDefaultSheet(new CreateDefaultReqModel("1KRue4wZdH1G0iLfLSUkHjP1i_nsguLNr", "1234232"), (err) =>
            //{
            //    Console.WriteLine(err.Message);

            //}, (data) =>
            //{
            //    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            //});

            //GoogleDriveWebRequesterV2.Instance.CopyExample(new CopyExampleReqModel(), (err) =>
            //{
            //    Console.WriteLine(err.Message); 
            //}, (data) =>
            //{
            //    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            //});

        }
    }
}
