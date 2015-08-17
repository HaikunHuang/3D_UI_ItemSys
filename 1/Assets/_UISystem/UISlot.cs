using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, 
IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
{
	public bool dragable = false;
	[HideInInspector]
	public float dragOffSet = 1f;

	public UISlotObject iconGroup;
	public Image bg; // background frame
	public RawImage icon; // icon frame
	public Image fg; // frontground frame
	public RawImage att1; // att1 frame
	[HideInInspector]
	Vector3 oldIconGroup_position;
	[HideInInspector]
	Quaternion oldIconGroup_rotation;

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


	// Use this for initialization
	void Start () 
	{
		button = GetComponent<Button>();
	}

	
	
	// Update is called once per frame
	void Update () {
	
	}

	void PreDrag()
	{
		// mark the old values

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

	void PostDrag()
	{
		// reset values

		iconGroup.transform.position = oldIconGroup_position;
		iconGroup.transform.rotation = oldIconGroup_rotation;

		textGroup.transform.position = oldTextGroup_position;
		textGroup.transform.rotation = oldTextGroup_rotation;

		transform.parent = oldParent;

	}

	public void OnClick()
	{

	}

	// *********************************************************************
	// event system
	// *********************************************************************
	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{

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
		PostDrag();
	}
	#endregion

}
