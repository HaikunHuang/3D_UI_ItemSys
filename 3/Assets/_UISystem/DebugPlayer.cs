using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugPlayer : MonoBehaviour {

	public int maxStack = 4;
	public float maxWight = 100;

	public List<ItemSlot> presetItems;
	
	public Backpack backpack;

	// Use this for initialization
	void Start () 
	{
		backpack = new Backpack(maxStack, maxWight);

		for(int i=0;i<maxStack && i<presetItems.Count; i++)
		{
			backpack.Add(presetItems[i].Get_Item_ID(),presetItems[i].Get_Stack());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
