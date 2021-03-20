using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZG.Core.Type
{ 
    public class UGSEnum : Attribute
    {
        public System.Type type;
        public string[] sepractor;

        public UGSEnum(System.Type type, params string[] sepractor)
        {
            this.type = type;
            this.sepractor = sepractor;
        }
    }
}
