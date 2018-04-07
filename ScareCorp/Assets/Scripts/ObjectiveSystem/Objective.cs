using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour {

	[SerializeField]
	private string ObjectiveDescription;
	[SerializeField]
	ObjectiveManager objManager;
	protected virtual void Awake()
	{
		if(ObjectiveDescription != "")
		{
			objManager = GameObject.FindGameObjectWithTag("objManager").GetComponent<ObjectiveManager>();
			if(objManager)
			{
				objManager.AddObjective(ObjectiveDescription,this);
			}
			else
			{
				Debug.LogError("No ObjectiveManager");
			}
		}
		else
		{
			Debug.LogError("Invalid Objective Description");
		}
	} 
	
	protected virtual void ObjectiveComplete()
	{
		if(ObjectiveDescription != "")
		{
			objManager.RemoveObjective(ObjectiveDescription,this);
		}
		else
		{
			Debug.LogError("Invalid Objective Description");
		}
	}
	
	
}
