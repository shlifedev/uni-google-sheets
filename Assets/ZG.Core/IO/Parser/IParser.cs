using Hamster.ZG.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG
{
    public interface IParser
    {
        void ParseSheet(string sheetJsonData, bool generateCs, bool generateJson, IFIleWriter writer);
    }
}
