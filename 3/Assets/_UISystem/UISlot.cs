﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, 
IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
IPointerEnterHandler, IPointerExitHandler,
IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
{

	public bool dragable = true;
	public bool clickable = true;
	public bool infoable = true;
	[HideInInspector]
	public float dragOffSet = 10f;

	public UISlotObject iconGroup;
	public Image bg; // background frame
	public RawImage icon; // icon frame
	public Image fg; // frontground frame
	public RawImage att1; // att1 frame
	[HideInInspector]
	Vector3 oldIconGroup_position;
	[HideInInspector]
	Quaternion oldIconGroup_rotation;
	[HideInInspector]
	Transform oldIconGroup_parent;

	public UISlotObject textGroup;
	public Image textBg;
	public Text info;
	public Image textFg;
	[HideInInspector]
	Vector3 oldTextGroup_position;
	[HideInInspector]
	Quaternion oldTextGroup_rotation;

	[HideInInspector]
	public int slot_id; // id of slot
	[HideInInspector]
	public string item_id; // id of item

	[HideInInspector]
	public Button button;
	[HideInInspector]
	public Transform oldParent;
	[HideInInspector]
	public UIStorageBase storage;
	[HideInInspector]
	public bool isEmpty = false;


	public delegate void OnClick(int id);
	public OnClick onClick;

	// Use this for initialization
	void Start () 
	{
		button = GetComponent<Button>();
		if (icon.texture == null)
			SetIconEmpty();
		else
			isEmpty = false;

		if (!infoable)
			textGroup.gameObject.SetActive(false);
	}

	// this is called by storage
	public void Init(UIStorageBase sb, int id, OnClick clickFunc)
	{
		slot_id = id;
		storage = sb;
		AddOnClick(clickFunc);
		// more here
	}

	// add onClick 
	public void AddOnClick(OnClick clickFunc)
	{
		if (onClick == null)
			onClick = clickFunc;
		else
			onClick += clickFunc;
	}

	public void SetInfo(string s)
	{
		info.text = s;
	}

	public void SetIcon(Texture t)
	{
		icon.enabled = true;
		icon.texture = t;
	}

	public void SetAtt1(Texture t)
	{
		att1.enabled = true;
		att1.texture = t;
	}

	public void SetItemID(string id)
	{
		isEmpty = false;
		item_id = id;
	}

	// empty this slot
	public void SetIconEmpty()
	{
		isEmpty = true;
		SetIcon(null);
		SetInfo("");
		SetDragbale(false);
		icon.enabled = false;
		att1.enabled = false;
		item_id = "";
	}

	public void SetDragbale(bool b)
	{
		dragable = b;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void PreDrag()
	{
		// mark the old values
		oldIconGroup_parent = iconGroup.transform.parent;
		oldIconGroup_position = iconGroup.transform.position;
		oldIconGroup_rotation = iconGroup.transform.rotation;

		oldTextGroup_position = textGroup.transform.position;
		oldTextGroup_rotation = textGroup.transform.rotation;

		// render order patch, in order to force UIslot be render on top at the current canvas
		oldParent = transform.parent;
		transform.parent = null;
		transform.parent = oldParent;

		// no idea why must do this, but like a patch, if not call post drag here, text group's position will be wrong.
		// after reset parent.
		PostDrag();
	}

	public void PostDrag()
	{
		// reset values
		transform.parent = oldParent;
		iconGroup.transform.parent = oldIconGroup_parent;

		iconGroup.transform.position = oldIconGroup_position;
		iconGroup.transform.rotation = oldIconGroup_rotation;

		textGroup.transform.position = oldTextGroup_position;
		textGroup.transform.rotation = oldTextGroup_rotation;


	}

	// this function will be called at OnEndDrag
	// interact with other storage, NOT the same storage this slot has
	void InteractWith_Other_Storage()
	{
		if (!dragable)
			return;
		
		// set current camera
		if (Camera.current == null)
			Camera.SetupCurrent(Camera.main);
		
		RaycastHit hitinfo;
		Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitinfo))
		{
			UIStorageBase sb = hitinfo.collider.gameObject.GetComponent<UIStorageBase>();
			if (sb != null && sb != storage)
			{
				// interact 
				sb.Income_Slot(this);
			}
			
		}
	}


	// *********************************************************************
	// event system
	// *********************************************************************
	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		if (clickable && eventData.clickCount == 2)
		{
			if (onClick!=null)
			{
				onClick(slot_id);
			}
		}
			
	}
	#endregion

	#region IPointerDownHandler implementation
	public void OnPointerDown (PointerEventData eventData)
	{

	}
	#endregion

	#region IPointerUpHandler implementation
	public void OnPointerUp (PointerEventData eventData)
	{

	}
	#endregion

	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData)
	{

	}
	#endregion
	
	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		if (!dragable)
			return;

		// set current camera
		if (Camera.current == null)
			Camera.SetupCurrent(Camera.main);

		RaycastHit hitinfo;
		Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitinfo))
		{
			// move icon group
			iconGroup.transform.position = hitinfo.point + hitinfo.normal * (dragOffSet);
			iconGroup.transform.forward = -hitinfo.normal;

			// re-parent
			Canvas can = hitinfo.collider.gameObject.GetComponent<Canvas>();
			if (can != null)
			{
				iconGroup.transform.parent = can.transform;
			}

			// move text group
			// un-supported

		}
	}
	#endregion

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		PreDrag();
	}
	#endregion
	
	#region IEndDragHandler implementation
	public void OnEndDrag (PointerEventData eventData)
	{
		InteractWith_Other_Storage();
		PostDrag();
	}
	#endregion

	#region IPointerEnterHandler implementation
	
	public void OnPointerEnter (PointerEventData eventData)
	{
		Debug.Log("OnPointerEnter");
	}
	
	#endregion
	
	#region IPointerExitHandler implementation
	
	public void OnPointerExit (PointerEventData eventData)
	{

	}
	
	#endregion

}
