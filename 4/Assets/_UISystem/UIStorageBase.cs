using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class UIStorageBase : MonoBehaviour,
IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
{

	// slots
	public List<UISlot> slots;
	
	public BackpackSystem backpackSys = null;
	public string moneyID = "Money";
	public UIDescrption descrption;

	// some debug values
	bool setEmptySlotAtBegin = true;

	// Use this for initialization
	void Start () 
	{
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
	
	virtual public void Sync()
	{
		for(int i=0;i<slots.Count;i++)
		{
			slots[i].SetIconEmpty();
		}
	}

	virtual public void Assign_Slot()
	{
		for(int i=0;i<slots.Count;i++)
		{
			slots[i].Init(this, i, Slot_OnClick, Slot_OnEnter, Slot_OnExit);
			if (setEmptySlotAtBegin)
				slots[i].SetIconEmpty();
		}
	}

	// this is called by slot, need to assign to slot first
	virtual public void Slot_OnClick(int id)
	{
		Debug.Log("Slot " + id + " on click!");
	}

	// this is called by slot, need to assign to slot first
	virtual public void Slot_OnEnter(int id)
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
	virtual public void Slot_OnExit(int id)
	{
		if (descrption)
		{
			descrption.Set(null,"");
		}
	}

	// this function will be called by slot of other storage, when the slot be dropped on this storage
	virtual public void Income_Slot(UISlot slot)
	{
		Debug.Log("Slot income: " + slot.slot_id);

	}

	// get the ui slot which will be hit by the ray after the draging action.
	virtual public UISlot Get_UISlot_On_End_Drag()
	{
		// set current camera
		if (Camera.current == null)
			Camera.SetupCurrent(Camera.main);
		
		RaycastHit hitinfo;
		Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitinfo))
		{
			UISlot us = hitinfo.collider.gameObject.GetComponent<UISlot>();
			return us;
		}

		return null;
	}

	// *********************************************************************
	// event system
	// *********************************************************************
	#region IPointerClickHandler implementation
	virtual public void OnPointerClick (PointerEventData eventData)
	{
		
	}
	#endregion
	
	#region IPointerDownHandler implementation
	virtual public void OnPointerDown (PointerEventData eventData)
	{
		
	}
	#endregion
	
	#region IPointerUpHandler implementation
	virtual public void OnPointerUp (PointerEventData eventData)
	{
		
	}
	#endregion
	
	#region IDropHandler implementation
	virtual public void OnDrop (PointerEventData eventData)
	{

	}
	#endregion
	
	#region IDragHandler implementation
	virtual public void OnDrag (PointerEventData eventData)
	{

	}
	#endregion
	
	#region IBeginDragHandler implementation
	virtual public void OnBeginDrag (PointerEventData eventData)
	{

	}
	#endregion
	
	#region IEndDragHandler implementation
	virtual public void OnEndDrag (PointerEventData eventData)
	{
	
	}
	#endregion

}
