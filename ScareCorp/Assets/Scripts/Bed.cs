using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bed : MonoBehaviour {


	private Text text;
	GameObject owner;
	// Use this for initialization


	void OnEnable()
	{
		text = this.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
		text.text = "";
	}
	
	public void AssignBed(string name, GameObject _owner)
	{
		text.text = name + "'s Bed";
		owner = _owner;
	}


	void OnDestroy()
	{
		if(gameObject.GetComponent<Buildable>())
		{
			if(gameObject.GetComponent<Buildable>().GetHasBeenActivated())
			{
				GameManager.instance.AddToBedCount(-1);
			}
		}
	}
	public void UnassignBed()
	{
		GameManager.instance.AddBed(this.gameObject);
		text.text =  "";

	}

}
