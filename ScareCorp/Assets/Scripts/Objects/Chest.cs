using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Transform myTransform;
        myTransform = this.transform;
        GameManager.instance.AddToWayPointList(myTransform); 

	}
	
	// Update is called once per frame

}
