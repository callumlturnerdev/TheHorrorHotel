using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bed : MonoBehaviour {


	public Text text;
	GameObject owner;
	// Use this for initialization

	void Awake()
	{
		text.text = "";
	}
	public void AssignBed(string name, GameObject _owner)
	{
		text.text = name + "'s Bed";
		owner = _owner;
	}


	public void UnassignBed()
	{
		GameManager.instance.AddBed(this.gameObject);
		text.text =  "";

	}

}
