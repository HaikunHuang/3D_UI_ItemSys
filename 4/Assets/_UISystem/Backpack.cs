using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Backpack {

	List<ItemSlot> backpack;
	int maxStack = 9999;
	float maxWeight = float.MaxValue;

	public Backpack(int _max, float _maxWeight)
	{
		
		maxStack = _max;
		maxWeight = _maxWeight;

		backpack = new List<ItemSlot>();
		for(int i=0; i<maxStack; i++)
		{
			backpack.Add(new ItemSlot("",0));
		}
	}

	public int Get_Item_Index_At_Begin(string item_id)
	{
		int index = -1;

		for(int i=0; i<backpack.Count; i++)
		{
			if (backpack[i].Get_Item_ID() == item_id && !backpack[i].Full())
			{
				index = i;
				break;
			}
		}
		return index;
	}

	public int Get_Item_Index_At_Last(string item_id)
	{
		int index = -1;
		
		for(int i=0; i<backpack.Count; i++)
		{
			if (backpack[i].Get_Item_ID() == item_id && !backpack[i].Full())
			{
				index = i;
			}
		}
		return index;
	}

	public int Get_Next_Available_Index()
	{
		int index = -1;
		for(int i=0; i<backpack.Count; i++)
		{
			if (backpack[i].Get_Item_ID() == "")
			{
				index = i;
				break;
			}
		}

		return index;
	}

	bool Add_One_Item(string item_id)
	{
		if (item_id == "")
		{
			Debug.LogError("Adding a non-name item.");
			return false;
		}

		int index = Get_Item_Index_At_Begin(item_id);
		if (index == -1)
		{
			index = Get_Next_Available_Index();
		}

		if (index == -1)
		{
			return false;
		}

		if (backpack[index].Get_Item_ID() == item_id)
		{
			// make sure not going to over wieght and over stack
			if (!Will_Over_Weight(item_id,1) && !backpack[index].WillFull(1)){
				backpack[index].Add(1);
				return true;
			}
		}
		else if (backpack[index].Get_Item_ID() == "")
		{
			// make sure not going to over wieght
			if (!Will_Over_Weight(item_id,1)){
				backpack[index] = new ItemSlot(item_id,1);
				return true;
			}
		}

		return false;
	}
	// return # of success added
	public int Add(string item_id, int stack)
	{
		int success = 0;

		for(int i=0 ; i<stack; i++)
		{
			if (Add_One_Item(item_id))
			{
				success++;
			}
			else
			{
				break;
			}

		}

		return success;
	}

	bool Add_One_Item(int index, string item_id)
	{
		if (item_id == "")
		{
			Debug.LogError("Adding a non-name item.");
			return false;
		}
		
		
		if (backpack[index].Get_Item_ID() == item_id)
		{
			// make sure not going to over wieght or over stack
			if (!Will_Over_Weight(item_id,1) && !backpack[index].WillFull(1)){
				backpack[index].Add(1);
				return true;
			}
		}
		else if (backpack[index].Get_Item_ID() == "")
		{
			// make sure not going to over wieght
			if (!Will_Over_Weight(item_id,1)){
				backpack[index] = new ItemSlot(item_id,1);
				return true;
			}
		}
		
		
		return false;
	}

	// return # of success added
	public int Add(int index, string item_id, int stack)
	{
		int success = 0;
		
		for(int i=0 ; i<stack; i++)
		{
			if (Add_One_Item(index,item_id))
			{
				success++;
			}
			else
			{
				break;
			}
			
		}
		
		return success;
	}

	public string Get_Item_ID_By_Index(int index)
	{
		return backpack[index].Get_Item_ID();
	}

	public ItemSlot Get_Item_By_Index(int index)
	{
		return backpack[index];
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

	public bool Remove(string item_id, int stack)
	{
		if (stack > Get_Stack(item_id))
			return false;

		for(int i=0; i<stack; i++)
		{
			Remove_One_Item(item_id);
		}

		return true;
	}

	void Remove_One_Item(string item_id)
	{
		int index = Get_Item_Index_At_Last(item_id);
		if (index != -1)
		{
			if (backpack[index].Get_Item_ID() == item_id)
			{
				backpack[index].Remove(1);
				if (backpack[index].Get_Stack() <= 0)
				{
					backpack[index] = new ItemSlot("",0);
				}
			}
		}
	}

	public bool Remove(int index, string item_id, int stack)
	{

		if (backpack[index].Get_Item_ID() == item_id
		    && backpack[index].Get_Stack() >= stack)
		{

			backpack[index].Remove(stack);
			if (backpack[index].Get_Stack() <= 0)
			{
				backpack[index] = new ItemSlot("",0);
			}
			return true;
		}

		return false;
	}

	// return the item_id which is the type we given
	public string[] Contain_Item_Type(ItemBase.Type type)
	{
		HashSet<string> ret = new HashSet<string>();
		foreach(ItemSlot it in backpack)
		{
			ItemBase item = ItemDatabase.GetItem(it.Get_Item_ID());
			if (item == null)
				continue;

			if (type == item.type)
			{
				ret.Add(item.item_id);
			}
		}

		return (new List<string>(ret)).ToArray();
	}

	public int Get_Stack(string item_id)
	{
		int s = 0;
		foreach(ItemSlot it in backpack)
		{
			if (item_id == it.Get_Item_ID())
			{
				s += it.Get_Stack();
			}
		}
		
		return s;
		
	}

	public int Get_Stack(int index)
	{
		return backpack[index].Get_Stack();;
	}

	public float Get_Weight(string item_id)
	{
		float s = 0;
		foreach(ItemSlot it in backpack)
		{
			if (item_id == it.Get_Item_ID())
			{
				s += it.GetWeight();
			}
		}
		
		return s;
	}

	public float Get_Weight(int index)
	{
		return backpack[index].GetWeight();
	}

	public float Get_Total_Weight()
	{
		float w = 0f;
		foreach(ItemSlot s in backpack)
		{
			w += s.GetWeight();
		}
		return w;
	}

	public ItemSlot[] Get_All_Item()
	{
		return backpack.ToArray();
	}

	public void ExChange(int index1, int index2)
	{
		if (backpack[index1].Get_Item_ID() != backpack[index2].Get_Item_ID())
		{
			ItemSlot tmp = backpack[index1];
			backpack[index1] = backpack[index2];
			backpack[index2] = tmp;
		}
		else
		{
			int exchangeCount = Get_Stack(index2);
			exchangeCount = Add(index1, backpack[index1].Get_Item_ID(), exchangeCount);
			Remove(index2, backpack[index1].Get_Item_ID(), exchangeCount);
		}

	}
}
