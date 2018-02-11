using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// custom script to find Object base on past into object ID


public class ObjectFinder : MonoBehaviour {

	public List<GameObject> obj;
    public GameObject FindObjectBasedOnID(int ID)
    {
        if (ID == -1) { return null; }
        return obj[ID];
    }
}
