﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour {
	public GameObject cursor;
	private GameObject currentHeldObject;
	private GameObject childObj;
	// Use this for initialization
	void Start () {
		childObj = this.transform.GetChild(0).gameObject;
		currentHeldObject = BuildController.instance.GetCurrentObject();
	}
	
	// Update is called once per frame

	public void SetHeldObject(GameObject obj)
	{
		currentHeldObject = obj;
		Destroy(childObj);
		if(currentHeldObject)
		{
			
			
			GameObject heldObj = Instantiate(currentHeldObject) as GameObject;
			heldObj.transform.rotation = Quaternion.Euler(0, BuildController.instance.GetRotation(),0); 
			Destroy(heldObj.GetComponent<Buildable>());
			heldObj.layer = 5;
			if(heldObj.GetComponent<TriggerReceiver>())
			{
				Destroy(heldObj.GetComponent<TriggerReceiver>());
			}
			if(heldObj.GetComponent<Waypoint>())
			{
				Destroy(heldObj.GetComponent<Waypoint>());
			}
			if(heldObj.GetComponent<TriggerSender>())
			{
				Destroy(heldObj.GetComponent<TriggerSender>());
			}
			heldObj.transform.parent = this.transform;
			heldObj.transform.position = this.transform.position;
			childObj = heldObj;
		}
	}

	public void SetRotation(float y)
	{
		childObj.transform.rotation = Quaternion.Euler(0, y, 0);
	}
	void FixedUpdate()
	{
		Vector3 cursorPos = new Vector3(cursor.transform.position.x,cursor.transform.position.y+ 4, cursor.transform.position.z);
		this.transform.position = Vector3.Lerp(this.transform.position,cursorPos, 0.25f);
	} 
		
	
}
