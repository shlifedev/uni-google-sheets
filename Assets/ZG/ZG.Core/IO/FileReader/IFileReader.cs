using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.IO
{
    public interface IFileReader
    { 
        string ReadData(string fileName);
    }
}
