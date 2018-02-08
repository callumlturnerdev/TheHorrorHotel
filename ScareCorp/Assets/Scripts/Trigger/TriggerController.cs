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

	public void SetCurrentTrigger(GameObject trigger)
	{

		if (trigger.gameObject.GetComponent<TriggerBase>()) 
		{
			if (firstTrigger == null) {
				firstTrigger = trigger;

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
	private void CheckConnection()
	{
		if (firstTrigger != null && secondTrigger != null)
		{
			firstTrigger.GetComponent<TriggerBase> ().SetTriggerObject (secondTrigger);
			secondTrigger.GetComponent<TriggerBase> ().SetTriggerParent (firstTrigger);

			line.SetPosition(0, firstTrigger.transform.position);
			line.SetPosition (1, secondTrigger.transform.position);
			firstTrigger = null;
			secondTrigger = null;
		}

	}

}
