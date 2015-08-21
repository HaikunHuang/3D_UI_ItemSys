using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Backpack {

	Dictionary<string,ItemSlot> backpack;
	int maxStack = 0;
	float maxWeight = 0;

	public Backpack(int _max, float _maxWeight)
	{
		backpack = new Dictionary<string, ItemSlot>();
		maxStack = _max;
		maxWeight = _maxWeight;
	}

	public bool Add(string item_id, int stack)
	{
		if (backpack.ContainsKey(item_id))
		{
			// make sure not going to over wieght
			if (!Will_Over_Weight(item_id,stack)){
				backpack[item_id].Add(stack);
				return true;
			}
		}
		else
		{
			// make sure not going to over stack
			if (backpack.Count < maxStack){
				// make sure not going to over wieght
				if (!Will_Over_Weight(item_id,stack)){
					backpack.Add(item_id,new ItemSlot(item_id,0));
					backpack[item_id].Add(stack);
					return true;
				}
			}
		}

		return false;
	}

	bool Will_Over_Weight(string item_id, int stack)
	{
		return (maxWeight < (Get_Total_Weight() + Will_Add_Weight(item_id,stack)));
	}

	float Will_Add_Weight(string item_id, int stack)
	{
		ItemBase item = ItemDatabase.GetItem(item_id);
		return stack * item.weight;
	}
		
	public int Get_Max_Stack()
	{
		return maxStack;
	}

	public int Get_Total_Stack()
	{
		return backpack.Count;
	}

	public void Remove(string item_id, int stack)
	{
		if (backpack.ContainsKey(item_id))
		{
			backpack[item_id].Remove(stack);
			if (backpack[item_id].Get_Stack() <= 0)
			{
				backpack.Remove(item_id);
			}
		}
	}

	public int Get_Stack(string item_id)
	{
		if (backpack.ContainsKey(item_id))
		{
			return backpack[item_id].Get_Stack();
		}
		else
		{
			return 0;
		}
	}

	public float Get_Weight(string item_id)
	{
		if (backpack.ContainsKey(item_id))
		{
			return backpack[item_id].GetWeight();
		}
		else
		{
			return 0f;
		}
	}

	public float Get_Total_Weight()
	{
		float w = 0f;
		foreach(string s in backpack.Keys)
		{
			w += backpack[s].GetWeight();
		}
		return w;
	}

	public string[] Get_All_Item_ID()
	{
		List<string> all = new List<string>();
		foreach(string s in backpack.Keys)
		{
			all.Add(s);
		}
		return all.ToArray();
	}
}
