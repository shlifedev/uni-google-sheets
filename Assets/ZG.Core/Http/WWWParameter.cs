using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Http
{
    public class WWWParameter
    {
        public WWWParameter(string id, string value)
        {
            this.parameterID = id;
            this.parameterValue = value;
        }
        public string parameterID;
        public string parameterValue;
    }
}
