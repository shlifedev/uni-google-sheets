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
            GoogleDriveWebRequesterV2.Instance.Credential(
                "https://script.google.com/macros/s/AKfycbxpqlYM5SfX0pL2RHzgiT_cFykKFLkcr_PgzU1KKnVx2Aa6YNN3/exec",
                "123123"); 

        }

        static void CodeGen(string sheetID)
        { 
            var request = new ReadSpreadSheetReqModel(sheetID);
            GoogleDriveWebRequesterV2.Instance.ReadSpreadSheet(request, err => { }, result => {
                HamsterGoogleSheet.DataParser.ParseSheet(result, true, true, new FileWriter());
            });
        }
    }
}
