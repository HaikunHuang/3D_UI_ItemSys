using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIShopping : UIStorageBase {

	// 拒绝列表，处于拒绝列表中的道具类型，不能被进行出售操作。
	// PS: 可以自定义NPC可交易的道具项目和类型
	public List<ItemBase.Type> rejectType;

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

				// 修正文本的显示
				if (item.maxStack > 1)
				{
					slots[i].SetInfo("$ " + item.buy * item.maxStack + " ["+item.maxStack+"]");
				}
				else
				{
					slots[i].SetInfo("$ " + item.buy);
				}

				// 打开拖放功能
				slots[i].SetDragbale(true);
			}
		}
	}
	
	// this is called by slot, need to assign to slot first
	override public void Slot_OnClick(int id)
	{
		
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
				string info = "["+item.displayName +"]\n\n"+ item.comment;
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
		// 如果UISlot不是属于本面板
		if (slot.storage != this)
		{
			// sell from UIBackpack
			// 如果UISlot属于背包面板的，这里的背包面板UIBackpack是玩家的背包面板
			if (slot.storage.GetType() == typeof(UIBackpack))
			{
				// 出售道具
				Sell(slot);
			}
		}

		
	}

	// 出售道具
	void Sell(UISlot slot)
	{
		// 通过数据库查询道具，因为空的UISlot不能被拖放，所以这里就不判断空道具了
		ItemBase item = ItemDatabase.GetItem(slot.item_id);
		
		// reject
		// 该道具是否属于被拒绝的类型
		foreach(ItemBase.Type t in rejectType)
		{
			if (t == item.type)
				return;
		}

		// 获取作为通货的道具
		ItemBase money = ItemDatabase.GetItem(moneyID);
		// 获取当前UISlot中的道具数量
		int sellStatk = slot.storage.backpackSys.backpack.Get_Stack(slot.slot_id);
		// add money and remove the item to/from slot.storage
		// 为那个传进来的UISlot所属的面板的背包系统中的背包添加适当的通货，同时移除道具。
		// 说人话就是，为玩家背包添加金钱，移除出售了的道具。
		slot.storage.backpackSys.backpack.Add(money.item_id, item.sell*sellStatk);
		slot.storage.backpackSys.backpack.Remove(slot.slot_id,item.item_id,sellStatk);
		// sync
		// 同步双方的面板和数据
		slot.storage.Sync();
		Sync();
	}
}
