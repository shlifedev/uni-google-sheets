using System;
using System.Collections.Generic;
using System.IO;

namespace GameTable.Item
{
    public class Weapons
    {
        public static Dictionary<int, Weapons> WeaponsMap = new Dictionary<int, Weapons>();
        public static List<Weapons> WeaponsList = new List<Weapons>();  

	public Int32 index;
	public String itemLocalizationID;
	public List<String> jobList;
	public Int32 power;
	public Int32 requireLv;
	public String uiDesc;
 
public void Load(){} 
    }
}
        