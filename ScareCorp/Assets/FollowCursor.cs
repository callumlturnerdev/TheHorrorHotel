using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour {
	public GameObject cursor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame


	void FixedUpdate()
	{
		Vector3 cursorPos = new Vector3(cursor.transform.position.x,cursor.transform.position.y+ 4, cursor.transform.position.z);
		this.transform.position = Vector3.Lerp(this.transform.position,cursorPos, 0.25f);
	} 
		
	
}
