using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    [SerializeField]
    private GameObject monsterRef;
    private bool lineToMouse;
    private LineRenderer line;
    private GameObject cursor;
    [SerializeField]
    public GameObject previousWaypoint;
    public GameObject nextWayPoint;

    Material normalMat;
	void Awake () {
        StartCoroutine(WaitToActivate());
	}

    IEnumerator WaitToActivate()
    {
        yield return new WaitForSeconds(0.5f);
        normalMat = this.GetComponent<MeshRenderer>().material;
    
        lineToMouse = true;
        line = GetComponent<LineRenderer>();
        cursor = GameObject.FindGameObjectWithTag("cursor");
        monsterRef = BuildController.instance.GetMonsterRef();
        if (monsterRef != null)
        {
            monsterRef.GetComponent<MonsterBase>().SetCurrentWaypoint(this.gameObject);
        }
        else
        {
            DebugConsole.Log("NO MONSTEr");
        }
    }
    public void DestroyWaypoint(){Destroy(this.gameObject);}
  

    public void SetMaterialBack(Material mat)
    {
        if(mat != null)
        {
            this.GetComponent<MeshRenderer>().material  = mat;
        }
        else
        {
           this.GetComponent<MeshRenderer>().material  = normalMat; 
        }
    }
  
    public void ToggleVisibiltiy()
    {
        this.gameObject.GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        line.enabled = !line.enabled; 
    }
    void LineRendMouse()
    {
        if (lineToMouse)
        {
                line.SetPosition(0, this.transform.position);
                Vector3 monstPos = new Vector3(monsterRef.transform.position.x, this.transform.position.y, monsterRef.transform.position.z);
                line.SetPosition(1, monstPos);
             
        }
    }
    
   
 

    private void Update()
    {
        LineRendMouse();
    }
}
