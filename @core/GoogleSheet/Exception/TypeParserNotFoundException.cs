using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Exception
{
    public class TypeParserNotFoundException : System.Exception
    {
        public TypeParserNotFoundException(string message) : base(message)
        {
           
        }
    }
}
