using UnityEngine;
using System.Collections;

public class ItemBase  {

	public enum Type
	{
		Item,
		Item_Weapon,
		Item_Armor,
		Item_Ring,
		Item_Necklace,
		Item_Consumable
	}
	public Type type;

	public ItemBase()
	{
		type = Type.Item;
	}

	// convert the type from string
	virtual public Type ValidType(string typeString)
	{
		
		if (typeString == Type.Item_Weapon.ToString())
		{
			return Type.Item_Weapon;
		}
		if (typeString == Type.Item_Armor.ToString())
		{
			return Type.Item_Armor;
		}
		if (typeString == Type.Item_Ring.ToString())
		{
			return Type.Item_Ring;
		}
		if (typeString == Type.Item_Necklace.ToString())
		{
			return Type.Item_Necklace;
		}
		if (typeString == Type.Item_Consumable.ToString())
		{
			return Type.Item_Consumable;
		}
		
		Debug.LogError("Invalid type: " + typeString);
		return Type.Item;
		
	}

	virtual public void Load_From_Inifile(IniFile inifile, string section)
	{

	}




}
