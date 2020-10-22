using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Http.Protocol
{
    public class GetFolderInfo : Result
    {
        public List<string> fileID;
        public List<string> fileName; 
    }
}
