using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum AIStates { Idle, Chase, Patrol};
public class ScareZone : MonoBehaviour {

    public AIStates state;
    public GameObject target;
    public GameObject possibleTarget;
    private NavMeshAgent nav;
    public GameObject[] possibleTargets;
    public GameObject[] visitorTargets;
    public float distanceToTarget;
    private Transform startLoc;
    public float dist;
    // Use this for initialization
    void Awake () {
        dist = 9999;
        state = AIStates.Idle;
        nav = GetComponent<NavMeshAgent>();
     
        target = null;
        startLoc = transform;
        StartCoroutine(Delay());
	}
	
    private void TimedUpdate()
    {
        float distanceToTarg = 9999999;
        DebugConsole.Log("TimedUpdate");
        switch (state)
        {
            case AIStates.Idle:
                DebugConsole.Log("InIdle");
                if (CheckForVisitors() != null)
                {
                    state = AIStates.Chase;
                }
                else if (CheckForAttracters() != null)
                {
                    state = AIStates.Patrol;
                }
                else
                {
                    nav.destination = startLoc.position;
                }
     
                break;

            case AIStates.Chase:
                nav.destination = target.transform.position;
                distanceToTarg = Vector3.Distance(target.transform.position, transform.position);
               
               if (distanceToTarg < 2)
                {
                    state = AIStates.Idle;
                }
                    break;

            case AIStates.Patrol:
                nav.destination = target.transform.position;
                distanceToTarg = Vector3.Distance(target.transform.position, transform.position);

                if (distanceToTarg < 2) { 
                    state = AIStates.Idle;
                }
                break;

            default:
                break;

        }
       StartCoroutine(Delay());
    }

     GameObject CheckForVisitors() // Should check for visitors and if found set as target if not check for patrol route.
    {
        DebugConsole.Log("running");
        target = null;
        float currentClosestDistance = 999;
        visitorTargets = GameObject.FindGameObjectsWithTag("visitor");
        if (visitorTargets.Length > 0)
        {
            foreach (GameObject targ in visitorTargets)
            {
                float distanceToTarg = Vector3.Distance(targ.transform.position, transform.position);
                if (distanceToTarg < currentClosestDistance)
                {
                    possibleTarget = targ;
                    currentClosestDistance = distanceToTarg;
                }
            }
        }
        if (visitorTargets == null)
        {
            visitorTargets = null;
        }
        if (possibleTarget)
        {
            dist = Vector3.Distance(possibleTarget.transform.position, transform.position);
            if (dist < 7)
            {
                target = possibleTarget;
                return target;
            }
            return null;
        }
        return null; 
    }

     GameObject CheckForAttracters() // Attracters are things in the world that attract this monster, if found set as target if not idle.
    {
        target = null;
        float currentClosestDistance = 999;
        possibleTargets = GameObject.FindGameObjectsWithTag("Attracter");
        foreach (GameObject targ in possibleTargets)
        {
            float distanceToTarg = Vector3.Distance(targ.transform.position, transform.position);
            if (distanceToTarg < currentClosestDistance)
            {
                possibleTarget = targ;
                currentClosestDistance = distanceToTarg;
            }
        }
        if (possibleTargets == null)
        {
            possibleTarget = null;
        }
        if (possibleTarget)
        {
            dist = Vector3.Distance(possibleTarget.transform.position, transform.position);
            if (dist < 15)
            {
                target = possibleTarget;
                return target;
            }
            return null;
        }
        return null;
   
    }


    IEnumerator ScanEnviroment()
    {
        yield return new WaitForSeconds(4);
      

    }

    

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        if (nav.enabled == false)
        {
            nav.enabled = true;
        }
        Debug.Log("CheckingUpdates");
        yield return new WaitForSeconds(1);
     
        TimedUpdate();
        

    }


 
}
