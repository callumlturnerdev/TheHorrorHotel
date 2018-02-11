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
    private GameObject previousWaypoint;


	void Awake () {
        lineToMouse = true;
        line = GetComponent<LineRenderer>();
        
        cursor = GameObject.FindGameObjectWithTag("cursor");
        if (monsterRef != null)
        {
            previousWaypoint = monsterRef.GetComponent<MonsterBase>().GetLastWayPoint();
            monsterRef.GetComponent<MonsterBase>().AddWayPoint(this.gameObject);
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
            if (previousWaypoint != null)
            {
                line.SetPosition(1, previousWaypoint.transform.position);
            }
            else
            {
                line.SetPosition(1, new Vector3( monsterRef.transform.position.x,this.transform.position.y,monsterRef.transform.position.z));
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
