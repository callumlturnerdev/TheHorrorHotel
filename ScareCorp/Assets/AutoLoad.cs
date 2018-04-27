using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoad : MonoBehaviour {
	public string saveToLoad;
	public SavingLoading saveref;
	// Use this for initialization
	void Awake () {
		StartCoroutine(loadGame());
	}
	
	IEnumerator loadGame()
	{
		yield return new WaitForSeconds(1);
		
		if(saveref)
		{
			saveref.Load("/"+saveToLoad+".dat");
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
