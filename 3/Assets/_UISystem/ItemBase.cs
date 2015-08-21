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

	public Type type = Type.Item;
	public string item_id = "";
	public string displayName = "";
	public Texture icon = null;
	public Texture att1 = null;
	public int sell = 0;
	public int buy = 0;
	public float weight = 0f;
	public string comment = "";

	public ItemBase()
	{

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

}
