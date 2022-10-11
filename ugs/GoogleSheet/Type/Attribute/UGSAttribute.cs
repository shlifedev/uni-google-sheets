using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Core.Type
{  
    public class UGSAttribute : System.Attribute
    {
        public System.Type type;
        public string[] sepractor;

        public UGSAttribute(System.Type type, params string[] sepractor)
        {
            this.type = type;
            this.sepractor = sepractor;
        }
    }
}
