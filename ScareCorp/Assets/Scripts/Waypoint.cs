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
        normalMat = this.GetComponent<MeshRenderer>().material;
        nextWayPoint = null;
        lineToMouse = true;
        line = GetComponent<LineRenderer>();
        cursor = GameObject.FindGameObjectWithTag("cursor");
        if (monsterRef != null)
        {
            previousWaypoint = monsterRef.GetComponent<MonsterBase>().GetLastPoint();
            monsterRef.GetComponent<MonsterBase>().SetCurrentWaypoint(this.gameObject);
        }
        if (previousWaypoint != null)
        {
            previousWaypoint.GetComponent<Waypoint>().SetNextWayPoint(this.gameObject);
        }
	}

    public GameObject GetNextWayPoint() { return nextWayPoint; }
    public GameObject GetPreviousWayPoint() { return previousWaypoint; }

    public void SetPreviousWayPoint(GameObject prv)
    {
        previousWaypoint = prv;
    }
    public void SetNextWayPoint(GameObject nxt)
    {
        nextWayPoint = nxt;
    }

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
    public void DestroyWaypoint()
    {
        monsterRef.GetComponent<MonsterBase>().RemoveWayPoint(this.gameObject);
        previousWaypoint.GetComponent<Waypoint>().SetNextWayPoint(nextWayPoint);
        nextWayPoint.GetComponent<Waypoint>().SetPreviousWayPoint(previousWaypoint);
       // monsterRef.GetComponent<MonsterBase>().SetWaypoint(nextWayPoint);
        Destroy(this.gameObject);
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
            if (nextWayPoint != null)
            {
                line.SetPosition(1, nextWayPoint.transform.position);
            }  
        }
    }

    public void SetIndex(GameObject i)
    {
        monsterRef = i;
    }

    private void Update()
    {
        LineRendMouse();
    }
}
