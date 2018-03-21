using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialDisplay : MonoBehaviour {

	[SerializeField]
	GameObject tutTxt;
	[SerializeField]
	GameObject tutObj;
	public string tutTextDisplay;
	bool showTut;
	// Use this for initialization
	void Awake () {
		tutObj = GameObject.FindGameObjectWithTag("tut");
		showTut = false;
	}
	
	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(1))
		{
			showTut = !showTut;
			if(showTut)
			{
				tutTxt = tutObj.transform.GetChild(0).gameObject;
				tutTxt.SetActive(true);
				tutTxt.transform.GetChild(1).GetComponent<Text>().text = tutTextDisplay;

			}
			else if(!showTut)
			{
				tutTxt = tutObj.transform.GetChild(0).gameObject;
				tutTxt.SetActive(false);
			}
		}
	}

}
