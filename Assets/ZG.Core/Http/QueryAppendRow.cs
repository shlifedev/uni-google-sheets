using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Http
{
    public class AppendRowData
    {
        public string sheetID;
        public string spreadSheetID;
        public string[] rowData;
    }
    public class QueryAppendRow : Query
    {  
        public string sheetID;
        public string spreadSheetID;
        public string appendData;
        public QueryAppendRow(string spreadSheetID, string sheetID, string[] rowData)
        {
            this.spreadSheetID = spreadSheetID;
            this.sheetID = sheetID; 
            for(int i = 0; i < rowData.Length; i++)
            {
                this.appendData += rowData[i];
                if(i != rowData.Length-1) 
                    this.appendData+= "\t";
            } 
            method = METHOD.POST;
        }
        public override string Execute()
        {  
            return requester.Post(baseURL + $"?instruction=appendRow&spreadSheetID={spreadSheetID}&sheetID={sheetID}&appendData={appendData}", MethodString, new List<WWWParameter>()
            { 
                new WWWParameter("sheetID", sheetID),
                new WWWParameter("spreadSheetID", spreadSheetID),
                new WWWParameter("appendData", appendData) 
            }, this);
        }
    }
}
