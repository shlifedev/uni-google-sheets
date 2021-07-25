using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Attribute
{
    public class DataAttribute : System.Attribute
    { 
        public DataAttribute(System.Type type, object data)
        { 
            Type = type;
            Data = data;
        }

        public System.Type Type { get; }
        public object Data { get; }
    }
}
