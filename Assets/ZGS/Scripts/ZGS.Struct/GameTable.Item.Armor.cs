using System;
using System.Collections.Generic;
using System.IO;

namespace GameTable.Item
{
    public class Armor
    {
        public static Dictionary<int, Armor> ArmorMap = new Dictionary<int, Armor>();
        public static List<Armor> ArmorList = new List<Armor>();  

	public Int32 index;
	public String itemLocalizationID;
	public List<String> jobList;
	public Int32 armor;
	public Int32 requireLv;
	public String uiDesc;
 
public void Load(){} 
    }
}
        