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
    Animator anim;

    // Navigation
    NavMeshAgent nav;
    [SerializeField]
    int currentWaypoint;
    private GameObject currentWayPointobj;
    GameObject firstWaypoint, lastWayPoint;
    private void Awake()
    {
        currentWaypoint = 0;
        StartCoroutine(activateAIAfterWait(0.5f));
        anim = transform.GetChild(2).GetComponent<Animator>();
    }
   
    private void Update()
    {
        CheckDestinationIsReached();
    }


    void CheckDestinationIsReached()
    {
        if(currentWayPointobj)
        {
        if (Vector3.Distance(gameObject.transform.position, currentWayPointobj.transform.position) < 4 )
            {
                
                anim.SetBool("Walking", false);
                //SetNextDestination();
            }
        }
    }

    public void SetWaypoint(GameObject wpoint)
    {
            currentWayPointobj = wpoint;
 
    }
    public void SetCurrentWaypoint(GameObject wpoint)
    {
        if(wpoint)
        {
              anim.SetBool("Walking", true);
            Destroy(currentWayPointobj);
             currentWayPointobj = wpoint;
             //WayPointObj = currentWayPointobj;
             currentWayPointobj.GetComponent<Waypoint>().SetMaterialBack(currentWaypointMat);
             nav.destination = currentWayPointobj.transform.position;
        }
              //nav.isStopped = false;
             //SetNextDestination();
           
        
    }

    IEnumerator activateAIAfterWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if(this.GetComponent<Buildable>())
        {
          nav = GetComponent<NavMeshAgent>();
          GetComponent<BoxCollider>().enabled = true;
          nav.enabled = true;
          
        }
    }
                         
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
             
           
            if (BuildController.instance.GetCurrentBuildMode() == eBuildMode.building)
            {
                BuildController.instance.SetBuildObject(WayPointObj);
                BuildController.instance.WayPointMode(true,this.gameObject);
            }
            else
            {
                BuildController.instance.WayPointMode(false,this.gameObject);

            }
        }
        if(Input.GetMouseButtonDown(0) && BuildController.instance.GetCurrentBuildMode() == eBuildMode.deleting)
        {
            Destroy(this.gameObject);
        }
        
    }
}
