using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    
	void Awake () {
        Transform myTransform;
        myTransform = this.transform;
        GameManager.instance.AddToWayPointList(myTransform); 
	}
}
