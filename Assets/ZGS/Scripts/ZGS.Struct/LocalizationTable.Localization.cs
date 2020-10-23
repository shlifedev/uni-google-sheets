using System;
using System.Collections.Generic;
using System.IO;

namespace LocalizationTable
{
    public class Localization
    {
        public static Dictionary<string, Localization> LocalizationMap = new Dictionary<string, Localization>();
        public static List<Localization> LocalizationList = new List<Localization>();  

	public String stringID;
	public String English;
	public String Korean;
	public String Chinese;
 
public void Load(){} 
    }
}
        