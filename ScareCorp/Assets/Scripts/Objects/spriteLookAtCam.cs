using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteLookAtCam : MonoBehaviour {

	private bool isActive = true;
	// Update is called once per frame
	void Update () {
		
		Vector3 targetPos = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
		transform.LookAt(targetPos, Vector3.up);
	//	transform.rotation = Quaternion.Euler(0,transform.rotation.y,0);
		
	}

	public void SetIsActive(bool b)
	{
	//	isActive = b;
	}
}
