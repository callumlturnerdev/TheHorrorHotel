using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    private void Awake()
    {
        currentWaypoint = 0;
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
    }
    void SetNextDestination()
    {
        if (waypoints.Count > 0)
        {
            nav.destination = waypoints[currentWaypoint].transform.position;
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Count)
            {
                currentWaypoint = 0;
            }
        }
    }
    private void Update()
    {
        CheckDestinationIsReached();
    }

    void CheckDestinationIsReached()
    {
        if ((nav.remainingDistance <= nav.stoppingDistance  ) )
        {
            SetNextDestination();
        }
    }

    public GameObject GetLastWayPoint()
    {
        if (waypoints.Count > 0)
        {
            return waypoints[waypoints.Count -1];
        }
        else return null;
    }

    public void AddWayPoint(GameObject waypoint)
    {
        waypoints.Add(waypoint);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            WayPointObj.GetComponent<Waypoint>().SetIndex(this.gameObject);
            BuildController.instance.SetBuildObject(WayPointObj);
            BuildController.instance.WayPointMode();
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
