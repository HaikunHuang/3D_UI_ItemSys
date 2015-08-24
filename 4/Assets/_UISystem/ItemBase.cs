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
		Item_Consumable,
		Item_Money,
		Item_Formula
	}

	public Type type = Type.Item;
	public string item_id = "";
	public string displayName = "";
	public Color color = Color.white;
	public Texture icon = null;
	public Texture att1 = null;
	public int sell = 0;
	public int buy = 0;
	public float weight = 0f;
	public string comment = "";
	public int maxStack = int.MaxValue;

	public ItemBase()
	{

	}

	// convert the type from string
	virtual public Type ValidType(string typeString)
	{
		if (typeString == Type.Item.ToString())
		{
			type = Type.Item;
			return Type.Item;
		}
		if (typeString == Type.Item_Weapon.ToString())
		{
			type = Type.Item_Weapon;
			return Type.Item_Weapon;
		}
		if (typeString == Type.Item_Armor.ToString())
		{
			type = Type.Item_Armor;
			return Type.Item_Armor;
		}
		if (typeString == Type.Item_Ring.ToString())
		{
			type = Type.Item_Ring;
			return Type.Item_Ring;
		}
		if (typeString == Type.Item_Necklace.ToString())
		{
			type = Type.Item_Necklace;
			return Type.Item_Necklace;
		}
		if (typeString == Type.Item_Consumable.ToString())
		{
			type = Type.Item_Consumable;
			return Type.Item_Consumable;
		}
		if (typeString == Type.Item_Money.ToString())
		{
			type = Type.Item_Money;
			return Type.Item_Money;
		}
		if (typeString == Type.Item_Formula.ToString())
		{
			type = Type.Item_Formula;
			return Type.Item_Formula;
		}
		
		Debug.LogError("Invalid type: " + typeString);
		type = Type.Item;
		return Type.Item;
		
	}

}
