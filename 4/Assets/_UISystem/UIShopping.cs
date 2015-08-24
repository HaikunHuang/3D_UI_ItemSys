using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIShopping : UIStorageBase {

	public List<ItemBase.Type> rejectType;

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
				if (item.maxStack > 1)
				{
					slots[i].SetInfo("$ " + item.buy * item.maxStack + " ["+item.maxStack+"]");
				}
				else
				{
					slots[i].SetInfo("$ " + item.buy);
				}

				slots[i].SetDragbale(true);
			}
		}
	}
	
	// this is called by slot, need to assign to slot first
	override public void Slot_OnClick(int id)
	{
		
	}
	
	// this is called by slot, need to assign to slot first
	override public void Slot_OnEnter(int id)
	{
		if (descrption)
		{
			ItemBase item = ItemDatabase.GetItem(slots[id].item_id);
			if (item != null)
			{
				string info = "["+item.displayName +"]\n\n"+ item.comment;
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
		if (slot.storage != this)
		{
			// sell from UIBackpack
			if (slot.storage.GetType() == typeof(UIBackpack))
			{
				Sell(slot);
			}
		}

		
	}

	void Sell(UISlot slot)
	{
		ItemBase item = ItemDatabase.GetItem(slot.item_id);
		
		// reject
		foreach(ItemBase.Type t in rejectType)
		{
			if (t == item.type)
				return;
		}
		
		ItemBase money = ItemDatabase.GetItem(moneyID);
		int sellStatk = slot.storage.backpackSys.backpack.Get_Stack(slot.slot_id);
		// add money and remove the item to/from slot.storage
		slot.storage.backpackSys.backpack.Add(money.item_id, item.sell*sellStatk);
		slot.storage.backpackSys.backpack.Remove(slot.slot_id,item.item_id,sellStatk);
		// sync
		slot.storage.Sync();
		Sync();
	}
}
