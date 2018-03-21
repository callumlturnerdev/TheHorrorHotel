using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using buildModes;
public class MonsterBase : MonoBehaviour {

    [SerializeField]
    Material currentWaypointMat;
    GameObject WayPointButton;
    [SerializeField]
    GameObject WayPointObj;
    [SerializeField]
    List<GameObject> waypoints;

    // Navigation
    NavMeshAgent nav;
    [SerializeField]
    int currentWaypoint;
    public GameObject currentWayPointobj;
    GameObject firstWaypoint, lastWayPoint;
    private void Awake()
    {
        currentWaypoint = 0;
        StartCoroutine(activateAIAfterWait(0.1f));
    }
    void SetNextDestination()
    {
        if (currentWayPointobj != null)
        {
            currentWayPointobj.GetComponent<Waypoint>().SetMaterialBack(currentWaypointMat);
            nav.destination = currentWayPointobj.transform.position;
         
        }
    }
    private void Update()
    {
        CheckDestinationIsReached();
    }

    public void SetCurrentWayPoint(GameObject wpoint) { currentWayPointobj = wpoint; }
    void CheckDestinationIsReached()
    {
        if(nav)
        {
        if ((nav.remainingDistance <= nav.stoppingDistance  ) )
            {
                if (currentWayPointobj)
                {
                    if (currentWayPointobj.GetComponent<Waypoint>().GetNextWayPoint() != null)
                    {
                       // currentWayPointobj.GetComponent<Waypoint>().SetMaterialBack(null);
                        currentWayPointobj.GetComponent<Waypoint>().SetMaterialBack(null);
                        currentWayPointobj = currentWayPointobj.GetComponent<Waypoint>().GetNextWayPoint();
                         currentWayPointobj.GetComponent<Waypoint>().SetMaterialBack(currentWaypointMat);
                        nav.destination = currentWayPointobj.transform.position;
                    }
                }
                //SetNextDestination();
            }
        }
    }
    public GameObject GetLastPoint()
    {
        if (lastWayPoint)
        {
            return lastWayPoint;
        }
        return firstWaypoint;

    }
    public GameObject GetLastWayPoint()
    {
        if (waypoints.Count > 0)
        {
            return waypoints[waypoints.Count -1];
        }
        else return null;
    }
    public void RemoveWayPoint(GameObject waypoint)
    {
        if (waypoint == firstWaypoint)
        {
           firstWaypoint = waypoint.GetComponent<Waypoint>().GetNextWayPoint();
        }
        if(currentWayPointobj == waypoint)
        {
            currentWayPointobj = waypoint.GetComponent<Waypoint>().nextWayPoint;
        }
        waypoints.Remove(waypoint);
        
     
    }
    public void SetWaypoint(GameObject wpoint)
    {
            currentWayPointobj = wpoint;
 
    }
    public void SetCurrentWaypoint(GameObject wpoint)
    {
        if (firstWaypoint == null)
        {
            firstWaypoint = wpoint;
        }
        else
        {
            lastWayPoint = wpoint;
            lastWayPoint.GetComponent<Waypoint>().SetNextWayPoint(firstWaypoint);
            firstWaypoint.GetComponent<Waypoint>().SetPreviousWayPoint(lastWayPoint);
        }

        if (currentWayPointobj == null)
        {
            currentWayPointobj = wpoint;
        }
        waypoints.Add(wpoint);
    }

    IEnumerator activateAIAfterWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
          nav = GetComponent<NavMeshAgent>();
            nav.enabled = true;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(!nav)
            {
                nav.enabled = true;
            }
            WayPointObj.GetComponent<Waypoint>().SetIndex(this.gameObject);
            if (BuildController.instance.GetCurrentBuildMode() == eBuildMode.building)
            {
                BuildController.instance.SetBuildObject(WayPointObj);
                BuildController.instance.WayPointMode(true);
            }
            else
            {
                BuildController.instance.WayPointMode(false);

            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            if(waypoints.Count > 0)
            {
                foreach (GameObject waypoint in waypoints)
                {
                    if(waypoint)
                    {
                        if(waypoint.GetComponent<Waypoint>())
                        {
                        waypoint.GetComponent<Waypoint>().ToggleVisibiltiy();
                        }
                    }
                }
            }
        }
    }
}
