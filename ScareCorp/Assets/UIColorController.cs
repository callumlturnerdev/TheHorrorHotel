using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIColorController : MonoBehaviour {

	// Use this for initialization
	public static UIColorController instance = null;
	[SerializeField]
	private Color UIColor;
	[SerializeField]
	private List<UIColor> UIColorObjects;

	[SerializeField]
	private bool updateColor = false;
	void Awake () {
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
		
	}
	

	
	
	public void AddUIObject(UIColor uicol)
	{
		UIColorObjects.Add(uicol);
	
			uicol.ChangeUIColor(UIColor);
		
	}

	public void RemoveUIObject(UIColor uicol)
	{
		if(UIColorObjects.Contains(uicol))
		{
			UIColorObjects.Remove(uicol);
		}
	}


}
