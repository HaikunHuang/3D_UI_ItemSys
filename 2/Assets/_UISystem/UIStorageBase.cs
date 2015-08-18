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


	// some debug values
	bool setEmptySlotAtBegin = false;

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
	void Update () 
	{
		
	}

	// this is called by slot, need to assign to slot first
	virtual public void Slot_OnClick(int id)
	{
		Debug.Log("Slot " + id + " on click!");
	}

	virtual public void Assign_Slot()
	{
		for(int i=0;i<slots.Count;i++)
		{
			slots[i].Init(this, i, Slot_OnClick);
			if (setEmptySlotAtBegin)
				slots[i].SetIconEmpty();
		}
	}

	// this function will be called by slot of other storage, when the slot be dropped on this storage
	virtual public void Income_Slot(UISlot slot)
	{
		Debug.Log("Slot income: " + slot.slot_id);

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
