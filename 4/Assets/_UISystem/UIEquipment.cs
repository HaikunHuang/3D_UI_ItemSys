using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIEquipment : UIStorageBase {

	// 装备的类型，对应UISlot的索引，
	// 例如第0个Slot是武器栏，第1个Slot是防具。
	public List<ItemBase.Type> itemTypes;

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
	void Update () 
	{
	
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
			ItemSlot[] item_ids = backpackSys.equipment.Get_All_Item();
			for(int i=0; 
			    i<item_ids.Length && i<slots.Count; 
			    i++)
			{
				// 如果当前道具是空道具，则忽略
				ItemBase item = ItemDatabase.GetItem(item_ids[i].Get_Item_ID());
				if (item == null)
					continue;

				for(int j=0; j< itemTypes.Count && j<slots.Count; j++)
				{
					// 对应装备栏的可装备类型
					if (item.type == itemTypes[j])
					{
						slots[j].SetItemID(item.item_id);
						slots[j].SetAtt1(item.att1);
						slots[j].SetColor(item.color);
						slots[j].SetIcon(item.icon);
						//slots[i].SetInfo("$ " + item.buy);
						// 打开拖放功能
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
			// slot from this sys
			// 如果双方同属于一个背包系统，即（玩家背包），执行装备操作
			if (slot.storage.backpackSys == this.backpackSys)
			{
				Equip_Item(slot);
			}
		}

	}

	// exchange item
	// 装备
	void Equip_Item(UISlot slot)
	{
		// 通过数据库查询道具，因为空的UISlot不能被拖放，所以这里就不判断空道具了
		ItemBase item = ItemDatabase.GetItem(slot.item_id);
		// 获取本面板中的被交互的UISlot
		UISlot us = Get_UISlot_On_End_Drag();
		if (us != null)
		{
			// 获取本方Slot索引号
			int index = slots.IndexOf(us);

			// 装备类型是否符合
			if (item.type == itemTypes[index])
			{
				// 获取旧的道具信息
				ItemSlot oldItem = backpackSys.equipment.Get_Item_By_Index(us.slot_id);
				// un equip
				bool canEquip = true;
				// 是否有卸载的必要
				if (oldItem.Get_Item_ID()!= "")
				{
					// 是否有足够的空间卸载
					if (backpackSys.backpack.Add(oldItem.Get_Item_ID(),1) == 1)
					{
						// 卸载旧装备
						backpackSys.equipment.Remove(us.slot_id,oldItem.Get_Item_ID(),1);
					}
					else
					{
						// 卸载失败，不能执行装备操作
						canEquip = false;
					}
				}

				// equip
				// 是否可以装备
				if (canEquip)
				{
					// 是否装备成功
					if (backpackSys.equipment.Add(us.slot_id,slot.item_id,1) == 1)
					{
						// 从背包中移除当前装备
						backpackSys.backpack.Remove(slot.slot_id,slot.item_id,1);
					}
				}
				else
				{

				}
				// 同步双方面板
				Sync();
				slot.storage.Sync();
			}
		}

	}
}
