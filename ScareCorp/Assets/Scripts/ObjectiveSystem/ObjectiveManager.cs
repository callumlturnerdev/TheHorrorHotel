using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour {

	[SerializeField]
	Text ObjectiveText;
	[SerializeField]
	GameObject ObjectivesCompleteUI;
	string ObjectivesList;
	public static ObjectiveManager instance = null;
	[SerializeField]
	private List<GameObject> gridsWithObjectives;
	private Dictionary<string,Objective> objectiveDictionary = new Dictionary<string,Objective>();
	// Use this for initialization
	void Awake () 
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
	}
	
	private void UpdateUI()
	{
		string tempObj = "";
		foreach(string key in objectiveDictionary.Keys)
		{
			tempObj = tempObj + key + "\n";
		}
		ObjectiveText.text = tempObj;
	}

	public void AddObjective(string _name, Objective _objective)
	{
		objectiveDictionary.Add(_name,_objective);
		gridsWithObjectives.Add(_objective.gameObject);
		UpdateUI();
		// Create a UI element for objective
	}

	public void RemoveObjective(string _name, Objective _objective)
	{
		objectiveDictionary.Remove(_name);
		Destroy(_objective);
		UpdateUI();
		CheckObjectivesComplete();
		// Disable linked UI element
	}

	private void CheckObjectivesComplete()
	{
		if(objectiveDictionary.Count < 1)
		{
			// Code here to display win screen and then allow player to keep playing
			ObjectivesCompleteUI.SetActive(true);
		}
	}
}
