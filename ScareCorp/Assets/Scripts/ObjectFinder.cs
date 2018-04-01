using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// custom script to find Object base on past into object ID


public class ObjectFinder : MonoBehaviour {

	public List<GameObject> obj;
    public List<Material> groundMats;
    void Awake()
    {
        for(int i = 0; i < obj.Count; i++)
        {
            obj[i].GetComponent<Buildable>().SetIndex(i);
        }
    }

    public int GetMaterialIndexInArray(Material mat)
    {
        for(int i = 0; i < groundMats.Count; i++)
        {
            Debug.Log(mat.name + " " + groundMats[i].name );
            if(mat == groundMats[i])
            {
                return i;
            }
        }
        return 2;
    }
    public GameObject FindObjectBasedOnID(int ID)
    {
        if (ID == -1) { return null; }
        return obj[ID];
    }
}
