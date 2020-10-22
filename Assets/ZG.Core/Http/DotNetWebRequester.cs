using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Hamster.ZG.Http
{
    /// <summary>
    /// .Net Only Web Requester
    /// </summary>
    public class DotNetWebRequester : IWebRequester
    {
        public string Get(string url, string method, Http.Query query)
        { 
            WebRequest request = WebRequest.Create($"{url}");
            request.Method = method;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var t = response.ResponseUri;
                Stream responseStream = response.GetResponseStream();
                string data;
                using (var reader = new StreamReader(responseStream))
                    data = reader.ReadToEnd(); 
                responseStream.Close(); 
                return data; 
            }
            catch (System.Exception e)
            {
                return e.Message;
            } 
        }

        public string Post(string url, string method, List<WWWParameter> postFormParameter, Http.Query query)
        {
            throw new NotImplementedException();
        } 
    }
}
