using Hamster.ZG.Http.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Http
{
    public abstract class Query
    {
        public enum METHOD
        {
            GET, POST
        }
        public METHOD method; 
        public string MethodString 
        {
            get
            {
                switch(method)
                {
                    case METHOD.GET:
                        return "GET"; 
                    case METHOD.POST:
                        return "POST";
                    default:
                        return "GET";
                }
            }
        }
        public string baseURL = null;
        public IWebRequester requester;  
        public string callbackID;
        public abstract string Execute();  
    }
}
