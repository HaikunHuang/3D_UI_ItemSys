using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase  
{
	static string filePath = "/Data/ItemDatabase.ini";

	static ItemDatabase Instance = null;

	Dictionary<string,ItemBase> database;
	

	static ItemDatabase Get_Instance()
	{
		if (Instance == null)
		{
			Instance = new ItemDatabase();

		}

		return Instance;
	}

	public ItemDatabase()
	{
		string fullPath = Application.dataPath + filePath;
		database = new Dictionary<string, ItemBase>();
		LoadItem(fullPath);
	}

	static public ItemBase GetItem(string item_id)
	{
		if (Get_Instance().database.ContainsKey(item_id))
		{
			return Get_Instance().database[item_id];
		}

		return null;
	}

	void LoadItem(string path)
	{
		IniFile ini = new IniFile();
		if (!ini.Load_File(path))
		{
			Debug.LogError("File " + path + " NOT exists!");
		}

		foreach(string s in ini.Get_All_Section())
		{
			ini.Goto_Section(s);
			ItemBase item = new ItemBase();

			item.item_id = s;

			item.displayName = ini.Get_String("displayName","unnamed");
			item.ValidType(ini.Get_String("type"));
			item.color = ini.Get_Color("color",Color.white);

			string tPath = ini.Get_String("icon","");
			if (tPath != "")
			{
				Texture t = Resources.Load<Texture>(tPath);
				item.icon = t;
			}

			tPath = ini.Get_String("att1","");
			if (tPath != "")
			{
				Texture t = Resources.Load<Texture>(tPath);
				item.att1 = t;
			}

			item.sell = ini.Get_Int("sell",0);
			item.buy = ini.Get_Int("buy",0);
			item.weight = ini.Get_Float("weight",0f);
			item.comment = ini.Get_String("comment","");
			item.maxStack = ini.Get_Int("maxStack",int.MaxValue);

			database.Add(s,item);
		}

	}
}
