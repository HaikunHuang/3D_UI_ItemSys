using UnityEngine;
using System.Collections;

// this only used for test


public class UIDebug : MonoBehaviour {


	public Texture[] icons;
	public Texture[] atts;
	// Use this for initialization
	void Start () 
	{
		SetUISlot();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void SetUISlot()
	{
		UISlot[] slots = FindObjectsOfType<UISlot>();
		foreach(UISlot s in slots)
		{
			if (s.icon.texture != null)
			{
				s.SetItemID(""+Random.Range(0,100));
				s.SetInfo("$ " + Random.Range(10,200));
				s.SetIcon(icons[Random.Range(0,icons.Length)]);
				s.SetAtt1(atts[Random.Range(0,icons.Length)]);
			}
		}
	}
}
