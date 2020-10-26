
using System;
using Hamster.ZG.Http.Protocol;
using UnityEngine;

namespace Hamster.ZG
{
    public class UnityPlayerWebRequest : MonoBehaviour, IZGRequester
    {
        public void POST_CreateDefaultTable(string folderID, string fileName, Action<string> callback)
        {
          
        }

        public void GET_ReqFolderFiles(string folderID, Action<GetFolderInfo> callback)
        {
             
        }

        public void GET_TableData(string sheetID, Action<GetTableResult, string> callback)
        {
             
        }

        public void POST_WriteData(string spreadSheetID, string sheetID, string key, string[] value)
        {
      
        }
    }
}