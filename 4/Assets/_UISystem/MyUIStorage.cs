using UnityEngine;
using System.Collections;

public class MyUIStorage : UIStorageBase {

	// Use this for initialization
	void Start () 
	{
		if (GetComponent<Collider>() == null)
		{
			Debug.LogError("Require a collider");
		}
		Assign_Slot();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// this is called by slot, need to assign to slot first
	override public void Slot_OnClick(int id)
	{
		// use this item

		slots[id].SetIconEmpty();
	}

	// this function will be called by slot of other storage, when the slot be dropped on this storage
	override public void Income_Slot(UISlot slot)
	{
		// Debug.Log("Slot income: " + slot.slot_id);
		if (slot.storage != this)
		{
			// *********** this code for test ***********
			// put the income item to this storage, and empty the imcome slot
			for(int i=0;i<slots.Count;i++)
			{
				if (slots[i].isEmpty)
				{
					slots[i].SetItemID(slot.item_id);
					slots[i].SetAtt1(slot.att1.texture);
					slots[i].SetIcon(slot.icon.texture);
					slots[i].SetColor(slot.fg.color);
					slots[i].SetInfo(slot.info.text);
					slots[i].SetDragbale(true);
					
					slot.SetIconEmpty();
					
					break;
				}
			}

		}

	}
}
