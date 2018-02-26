using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using buildModes;
public class MonsterBase : MonoBehaviour {

    GameObject WayPointButton;
    [SerializeField]
    GameObject WayPointObj;
    [SerializeField]
    List<GameObject> waypoints;

    // Navigation
    NavMeshAgent nav;
    [SerializeField]
    int currentWaypoint;
    GameObject currentWayPointobj;
    GameObject firstWaypoint, lastWayPoint;
    private void Awake()
    {
        currentWaypoint = 0;
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
    }
    void SetNextDestination()
    {
        if (currentWayPointobj != null)
        {
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
        if ((nav.remainingDistance <= nav.stoppingDistance  ) )
        {
            if (currentWayPointobj)
            {
                if (currentWayPointobj.GetComponent<Waypoint>().GetNextWayPoint() != null)
                {
                    currentWayPointobj = currentWayPointobj.GetComponent<Waypoint>().GetNextWayPoint();
                }
            }
            SetNextDestination();
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



    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            WayPointObj.GetComponent<Waypoint>().SetIndex(this.gameObject);
            if (BuildController.instance.GetCurrentBuildMode() == eBuildMode.building)
            {
                BuildController.instance.SetBuildObject(WayPointObj);
                BuildController.instance.WayPointMode(true);
            }
            else
            {
                BuildController.instance.WayPointMode(false);
                Debug.Log("Swapping to building mode");
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            foreach (GameObject waypoint in waypoints)
            {
                waypoint.GetComponent<Waypoint>().ToggleVisibiltiy();
            }
        }
    }
}
