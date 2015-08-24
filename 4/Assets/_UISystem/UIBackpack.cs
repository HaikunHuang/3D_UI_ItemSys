using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBackpack : UIStorageBase {


	// Use this for initialization
	void Start () {
		if (GetComponent<Collider>() == null)
		{
			Debug.LogError("Require a collider");
		}
		Assign_Slot();
		Sync();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public void Sync()
	{
		for(int i=0;i<slots.Count;i++)
		{
			slots[i].SetIconEmpty();
		}
		
		if (backpackSys)
		{
			ItemSlot[] item_ids = backpackSys.backpack.Get_All_Item();
			for(int i=0; 
			    i<item_ids.Length && i<slots.Count; 
			    i++)
			{
				ItemBase item = ItemDatabase.GetItem(item_ids[i].Get_Item_ID());
				if (item == null)
					continue;

				slots[i].SetItemID(item.item_id);
				slots[i].SetAtt1(item.att1);
				slots[i].SetColor(item.color);
				slots[i].SetIcon(item.icon);
				slots[i].SetInfo(backpackSys.backpack.Get_Stack(i)+"");
				slots[i].SetDragbale(true);
			}
		}
	}

	// this is called by slot, need to assign to slot first
	override public void Slot_OnClick(int id)
	{
		//Debug.Log("Slot " + id + " on click!");
		// use item
		ItemBase item = ItemDatabase.GetItem(slots[id].item_id);
		if (item.type == ItemBase.Type.Item_Consumable)
		{
			// use it
			backpackSys.backpack.Remove(id,item.item_id,1);
			Sync();
		}
	}
	
	// this is called by slot, need to assign to slot first
	override public void Slot_OnEnter(int id)
	{
		if (descrption)
		{
			ItemBase item = ItemDatabase.GetItem(slots[id].item_id);
			if (item != null)
			{
				string info = "["+item.displayName +"]\n\n"+ item.comment + "\n$ " + item.sell;
				descrption.Set(slots[id],info);
			}

		}
	}
	
	// this is called by slot, need to assign to slot first
	override public void Slot_OnExit(int id)
	{
		if (descrption)
		{
			descrption.Set(null,"");
		}
	}
	
	// this function will be called by slot of other storage, when the slot be dropped on this storage
	override public void Income_Slot(UISlot slot)
	{
		//Debug.Log("Slot income: " + slot.slot_id);
		if (slot.storage != this)
		{
			// buy from UI Shopping
			if (slot.storage.GetType() == typeof(UIShopping))
			{
				Buy(slot);
			}
			
			// unequip
			if (slot.storage.backpackSys == this .backpackSys && slot.storage.GetType() == typeof(UIEquipment))
			{
				Unequip(slot);
			}
		}
		else
		{
			// do exchange
			Exchange(slot);
		}
		
	}

	void Exchange(UISlot slot)
	{
		ItemBase item = ItemDatabase.GetItem(slot.item_id);
		UISlot us = Get_UISlot_On_End_Drag();
		if (us != null)
		{
			if (us.slot_id != slot.slot_id)
			{
				backpackSys.backpack.ExChange(us.slot_id, slot.slot_id);

				Sync();
			}
		
		}
	}

	void Buy(UISlot slot)
	{
		ItemBase item = ItemDatabase.GetItem(slot.item_id);

		UISlot uis = Get_UISlot_On_End_Drag();
		if (uis != null)
		{
			// enougth money
			for(int i=0;i<item.maxStack; i++)
			{
				if (backpackSys.backpack.Get_Stack(slot.storage.moneyID) >= item.buy)
				{
					if (backpackSys.backpack.Add(uis.slot_id,item.item_id,1) == 1)
					{
						backpackSys.backpack.Remove(slot.storage.moneyID,item.buy);
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
			slot.storage.Sync();
			Sync();
		}

	}

	void Unequip(UISlot slot)
	{
		UISlot uis = Get_UISlot_On_End_Drag();
		if (uis != null)
		{
			if (backpackSys.backpack.Add(uis.slot_id,slot.item_id,1) == 1)
			{
				backpackSys.equipment.Remove(slot.slot_id,slot.item_id,1);
			}
			Sync();
			slot.storage.Sync();
		}

	}
}
