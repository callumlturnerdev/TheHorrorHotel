using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIColor : MonoBehaviour {

	[SerializeField]

	private Image ColorChangeImage;
	[SerializeField]
	private Image ColorChangeFont;
	// Use this for initialization
	void Awake () {
		UIColorController.instance.AddUIObject(this);
	}
	
	// Update is called once per frame
	public void ChangeUIColor(Color _newColor)
	{
		if(ColorChangeImage)
		{
			ColorChangeImage.color = _newColor;
		}
		if(ColorChangeFont)
		{
			ColorChangeFont.color = _newColor;
		}
	}
}
