using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIDescrption : MonoBehaviour {

	public UISlot slot;
	public Text info;
	// Use this for initialization
	void Start () {
		Set(null,"");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Set(UISlot _slot, string _info)
	{
		if (_slot == null)
		{
			slot.SetIconEmpty();
			slot.gameObject.SetActive(false);
		}
		else
		{
			slot.gameObject.SetActive(true);
			slot.SetIcon(_slot.icon.texture);
			slot.SetAtt1(_slot.att1.texture);
			slot.SetColor(_slot.fg.color);
			info.color = _slot.fg.color;
		}

		info.text = _info;
	}
}
