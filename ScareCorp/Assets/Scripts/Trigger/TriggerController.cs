using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour {

	public static TriggerController instance = null;

	// RIGHT CLICK A TRIGGERABLE TO SET IT AS CURRENT AND THEN RIGHT CLICK A TRIGGER TO LINK THEM.
	// Use this for initialization
	private LineRenderer line;
	public GameObject firstTrigger;
	public GameObject secondTrigger;
	public bool canLink = false; 
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) 
		{
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this.gameObject);
		Init ();
	}

	void Init()
	{
		line = GetComponent<LineRenderer> ();
	}
	public bool GetCanLink(){return canLink;}
	public void SetCurrentTrigger(GameObject trigger)
	{
		if (trigger.gameObject.GetComponent<TriggerBase>()) 
		{
				if (firstTrigger == null) {
					firstTrigger = trigger;
					canLink = true;

				} else 
				{
					secondTrigger = trigger;
				}
				CheckConnection ();
		}
	}
		
	public void SetCurrentTriggerable(GameObject triggerable)
	{
		if (triggerable.tag == "triggerable") 
		{		
			secondTrigger = triggerable;
			CheckConnection ();
			
		}
	}

	private void SetUpLineRenderer()
	{
		
	}

	public GameObject GetFirstTrigger() {return firstTrigger;}
	public GameObject GetSecondTrigger() {return secondTrigger;}
	private void CheckConnection()
	{
		if (firstTrigger != null && secondTrigger != null)
		{
			if(secondTrigger.GetComponent<TriggerBase>().otherTrigger == null)
					firstTrigger.GetComponent<TriggerBase> ().SetTriggerObject (secondTrigger);
					secondTrigger.GetComponent<TriggerBase> ().SetTriggerParent (firstTrigger);
					secondTrigger.GetComponent<TriggerBase> ().SetCanLink(true);
					firstTrigger.GetComponent<TriggerBase> ().LineRendToOtherObject();
					firstTrigger.GetComponent<TriggerBase> ().SetCanLink(false);
					firstTrigger = null;
					secondTrigger = null;
					canLink = false;
				}
			}
		
		}
	

