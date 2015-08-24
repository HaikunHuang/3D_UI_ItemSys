using UnityEngine;
using System.Collections;
using System;

[Serializable]
[SerializePrivateVariables]
public class ItemSlot {

	string item_id;
	int stack;
	int maxStatck;

	public ItemSlot(string _item_id, int _stack)
	{
		item_id = _item_id;
		stack = _stack;
		ItemBase item = ItemDatabase.GetItem(_item_id);
		if (item != null)
		{
			maxStatck = item.maxStack;
		}
		else
		{
			maxStatck = int.MaxValue;
		}

	}

	public float GetWeight()
	{
		ItemBase item = ItemDatabase.GetItem(item_id);
		if (item == null)
			return 0f;

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

	public bool Full()
	{
		return stack >= maxStatck;
 	}

	public bool WillFull(int willAdd)
	{
		return stack + willAdd > maxStatck;
	}

	public bool Empty()
	{
		return stack <= 0;
	}

	public int Add()
	{
		stack ++;
		return stack;
	}
	public int Add(int i)
	{
		stack +=i;
		return stack;
	}

	public int Remove()
	{
		stack --;
		return stack;
	}

	public int Remove(int i)
	{
		stack -=i;
		return stack;
	}


}
