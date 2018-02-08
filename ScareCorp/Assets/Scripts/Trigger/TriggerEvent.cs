using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour {


	public GameObject triggerObject; // The object that will trigger the event.
	private TriggerBase trigBase;
	private LineRenderer line;
	private bool lineToMouse;
	private GameObject cursor;
	// Use this for initialization
	void Awake () {
		line = GetComponent<LineRenderer> ();
		cursor = GameObject.FindGameObjectWithTag ("cursor");
	}


	public void ObjectEvent()
	{
		
		if (transform.GetChild (0).gameObject.activeSelf) {
			transform.GetChild (0).gameObject.SetActive (false);
		} else 
		{
			transform.GetChild (0).gameObject.SetActive (true);
		}

		//trigBase.SetTriggered (false);

	}
	// Update is called once per frame
	void Update () {
		LineRendMouse ();
		if (trigBase != null) {
			
		}
	}

	private void LineRendMouse()
	{
		if (lineToMouse)
		{
			line.SetPosition(0, this.transform.position);
			line.SetPosition (1, cursor.transform.position);
		}
	}

	public void SetTriggerObject(GameObject trigger)
	{
		lineToMouse = false;
		triggerObject = trigger;
		trigBase = triggerObject.GetComponent<TriggerBase> ();
		line.SetPosition(0, this.transform.position);
		line.SetPosition (1, triggerObject.transform.position);
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (1)) 
		{
			lineToMouse = true;
			TriggerController.instance.SetCurrentTriggerable (this.gameObject);
		}

	}
}
