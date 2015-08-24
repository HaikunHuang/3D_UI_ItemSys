using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIEquipment : UIStorageBase {

	public List<ItemBase.Type> itemTypes;

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
	void Update () 
	{
	
	}

	override public void Sync()
	{
		for(int i=0;i<slots.Count;i++)
		{
			slots[i].SetIconEmpty(); 
		}
		
		if (backpackSys)
		{
			ItemSlot[] item_ids = backpackSys.equipment.Get_All_Item();
			for(int i=0; 
			    i<item_ids.Length && i<slots.Count; 
			    i++)
			{

				ItemBase item = ItemDatabase.GetItem(item_ids[i].Get_Item_ID());
				if (item == null)
					continue;

				for(int j=0; j< itemTypes.Count && j<slots.Count; j++)
				{
					if (item.type == itemTypes[j])
					{
						slots[j].SetItemID(item.item_id);
						slots[j].SetAtt1(item.att1);
						slots[j].SetColor(item.color);
						slots[j].SetIcon(item.icon);
						//slots[i].SetInfo("$ " + item.buy);
						slots[j].SetDragbale(true);
						break;
					}
				}
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
			// slot from this sys
			if (slot.storage.backpackSys == this.backpackSys)
			{
				Equip_Item(slot);
			}
		}

	}

	// exchange item
	void Equip_Item(UISlot slot)
	{

		ItemBase item = ItemDatabase.GetItem(slot.item_id);

		UISlot us = Get_UISlot_On_End_Drag();
		if (us != null)
		{
			int index = slots.IndexOf(us);
			
			if (item.type == itemTypes[index])
			{
				ItemSlot oldItem = backpackSys.equipment.Get_Item_By_Index(us.slot_id);
				// un equip
				bool canEquip = true;
				if (oldItem.Get_Item_ID()!= "")
				{
					if (backpackSys.backpack.Add(oldItem.Get_Item_ID(),1) == 1)
					{
						backpackSys.equipment.Remove(us.slot_id,oldItem.Get_Item_ID(),1);
					}
					else
					{
						canEquip = false;
					}
				}

				// equip
				if (canEquip)
				{
					if (backpackSys.equipment.Add(us.slot_id,slot.item_id,1) == 1)
					{
						backpackSys.backpack.Remove(slot.slot_id,slot.item_id,1);
					}
				}
				else
				{

				}

				Sync();
				slot.storage.Sync();
			}
		}

	}
}
