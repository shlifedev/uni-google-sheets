using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UGSWebError : System.Exception
{
    public UGSWebError(string message) : base(message)
    {
    }
}
