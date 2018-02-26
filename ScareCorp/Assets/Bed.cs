using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bed : MonoBehaviour {


	public Text text;
	// Use this for initialization

	void Awake()
	{
		text.text = "";
	}
	public void SetBedName(string name)
	{
		text.text = name + "'s Bed";
	}
}
