using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimateText : MonoBehaviour {
	Text UItext;
	string stringToText;
	public Grimm grim;
	public bool lastText = false;
	// Use this for initialization
	void Start () {
		UItext = GetComponent<Text>();
		stringToText = UItext.text;
		UItext.text = "";
		StartCoroutine(animateText(stringToText));
		grim.Talking(true);
	}
	public IEnumerator animateText(string strComplete){
		grim.Talking(true);
       int i = 0;
       string str = "";
       while(i < strComplete.Length){
		   UpdateUI(str);
          str += strComplete[i++];
          yield return new WaitForSeconds(0.03f);
       }
	   
	   grim.Talking(false);
	   if(lastText)
	   {
		   grim.Leave(true);
	   }
    }


	void OnDisable()
	{
		if( grim)
		{
			 grim.Talking(false);
	  		 if(lastText)
	   		{
		   		grim.Leave(true);
	   		}
		}
	}
	// Update is called once per frame
	void UpdateUI(string newS)
	{
		Debug.Log("UPDATE UI");
		UItext.text = newS;
	}
}
