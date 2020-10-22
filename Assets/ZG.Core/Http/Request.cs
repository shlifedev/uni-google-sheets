using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Http
{
    public static class Request
    {
        public static IWebRequester ReqBase;
        public static void Init(IWebRequester requester)
        {
            Request.ReqBase = requester;
        }
        public static string ScriptLink
        {
            get
            {
#if UNITY_EDITOR
                return  UnityEditor.EditorPrefs.GetString("ZGS_SCRIPT_LINK", null);
#endif
                return "https://script.google.com/macros/s/AKfycbzSQGu-2g2B9r8HOiaISj5bboAIzLPjNF1K2Zm5FQ/exec";
            }
        }
        public static string Execute(Query query)
        {
            query.baseURL = ScriptLink;
            query.requester = ReqBase;
            return query.Execute();
        }  
    }
}
