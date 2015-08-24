using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackpackSystem : MonoBehaviour {

	public int maxItemStack = 4;
	public float maxItemWight = 100f;
	
	public List<ItemSlot> presetItems;

	public int maxEqiupStack = 2;
	public float maxEqiupWight = 100f;
	public List<ItemSlot> presetEqiup;
	
	[HideInInspector]
	public Backpack backpack;

	[HideInInspector]
	public Backpack equipment;
	
	// Use this for initialization
	void Start () 
	{
		Init();
	}
	
	// Update is called once per frame
	void Update () {
	}

	virtual public void Init()
	{
		backpack = new Backpack(maxItemStack, maxItemWight);
		for(int i=0;i<maxItemStack && i<presetItems.Count; i++)
		{
			if (ItemDatabase.GetItem(presetItems[i].Get_Item_ID()) != null)
				backpack.Add(presetItems[i].Get_Item_ID(),presetItems[i].Get_Stack());
		}

		equipment = new Backpack(maxEqiupStack, maxEqiupWight);
		for(int i=0;i<maxEqiupStack && i<presetEqiup.Count; i++)
		{
			if (ItemDatabase.GetItem(presetEqiup[i].Get_Item_ID()) != null)
				equipment.Add(i, presetEqiup[i].Get_Item_ID(), 1);
		}
	}

}
