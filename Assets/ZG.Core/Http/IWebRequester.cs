using System;
using System.Collections.Generic;
using System.IO;
using System.Net; 

namespace Hamster.ZG.Http
{
    public interface IWebRequester
    {
        string Get(string url, string method, Query query);
        string Post(string url, string method, List<WWWParameter> postFormParameter, Query query); 
    }
}
