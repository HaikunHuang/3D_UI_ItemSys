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
		// 初始化UISlot，和赋值索引号。
		Assign_Slot();
		// 同步数据模型和UISlot
		Sync();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// 同步数据模型和UISlot
	override public void Sync()
	{
		// 清空UIslot
		for(int i=0;i<slots.Count;i++)
		{
			slots[i].SetIconEmpty();
		}


		if (backpackSys)
		{
			// 获取背包数据系统，
			ItemSlot[] item_ids = backpackSys.backpack.Get_All_Item();
			for(int i=0; 
			    i<item_ids.Length && i<slots.Count; 
			    i++)
			{
				// 如果当前道具是空道具，则忽略
				ItemBase item = ItemDatabase.GetItem(item_ids[i].Get_Item_ID());
				if (item == null)
					continue;

				// 同步UISlot的数据
				slots[i].SetItemID(item.item_id);
				slots[i].SetAtt1(item.att1);
				slots[i].SetColor(item.color);
				slots[i].SetIcon(item.icon);
				slots[i].SetInfo(backpackSys.backpack.Get_Stack(i)+"");

				// 打开拖放功能
				slots[i].SetDragbale(true);
			}
		}
	}

	// this is called by slot, need to assign to slot first
	// 鼠标双击，如果道具是消耗类型的，则使用它。数量－1
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
	// 鼠标滑入，在描述面板中显示
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
	// 鼠标滑出，则清空描述
	override public void Slot_OnExit(int id)
	{
		if (descrption)
		{
			descrption.Set(null,"");
		}
	}
	
	// this function will be called by slot of other storage, when the slot be dropped on this storage
	// 负责处理和UISolt的交互
	override public void Income_Slot(UISlot slot)
	{
		//Debug.Log("Slot income: " + slot.slot_id);
		// 如果UISlot不是属于本面板
		if (slot.storage != this)
		{
			// buy from UI Shopping
			// 如果面板属于商店，执行买操作，
			if (slot.storage.GetType() == typeof(UIShopping))
			{
				Buy(slot);
			}
			
			// unequip
			// 如果面板属于装备，执行卸载操作
			if (slot.storage.backpackSys == this .backpackSys && slot.storage.GetType() == typeof(UIEquipment))
			{
				
				Unequip(slot);
			}
		}
		// 如果UISlot是属于本面板
		else
		{
			// do exchange
			// 交换／合并分栏
			Exchange(slot);
		}
		
	}

	// 交换／合并分栏
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

	// 买操作
	void Buy(UISlot slot)
	{
		// 通过数据库查询道具，因为空的UISlot不能被拖放，所以这里就不判断空道具了
		ItemBase item = ItemDatabase.GetItem(slot.item_id);

		// 获取本面板中的被交互的UISlot
		UISlot uis = Get_UISlot_On_End_Drag();
		if (uis != null)
		{
			// enougth money
			// 是否有足够的通货买道具，
			// 每次买卖一组道具，通过for循环来购买，每次购买一个，直到钱不足或满足购买数量。
			for(int i=0;i<item.maxStack; i++)
			{
				// 判断是否有足够的钱购买一个道具
				if (backpackSys.backpack.Get_Stack(slot.storage.moneyID) >= item.buy)
				{
					// 是否有足够的空间购买道具
					if (backpackSys.backpack.Add(uis.slot_id,item.item_id,1) == 1)
					{
						// 勾除响应的通货
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
			// 同步双方的面板
			slot.storage.Sync();
			Sync();
		}

	}

	// 卸载
	void Unequip(UISlot slot)
	{
		// 获取本面板中的被交互的UISlot
		UISlot uis = Get_UISlot_On_End_Drag();
		if (uis != null)
		{
			// 是否有足够的空位放卸载的道具
			if (backpackSys.backpack.Add(uis.slot_id,slot.item_id,1) == 1)
			{
				// 移除装备栏中的道具
				backpackSys.equipment.Remove(slot.slot_id,slot.item_id,1);
			}
			// 同步双方的面板
			Sync();
			slot.storage.Sync();
		}

	}
}
