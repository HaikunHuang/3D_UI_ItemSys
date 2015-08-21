using UnityEngine;
using System.Collections;
using System;

[Serializable]
[SerializePrivateVariables]
public class ItemSlot {

	string item_id;
	int stack;

	public ItemSlot(string _item_id, int _stack)
	{
		item_id = _item_id;
		stack = _stack;
	}

	public float GetWeight()
	{
		ItemBase item = ItemDatabase.GetItem(item_id);
		return stack * item.weight;
	}

	public string Get_Item_ID()
	{
		return item_id;
	}

	public int Get_Stack()
	{
		return stack;
	}

	public int Add(int i)
	{
		stack += i;
		return stack;
	}

	public int Remove(int i)
	{
		stack -= i;
		return stack;
	}

}
