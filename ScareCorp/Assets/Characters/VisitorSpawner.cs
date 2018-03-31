using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using fearTypes;
public class VisitorSpawner : MonoBehaviour {

    private SavingLoading saveRef;
	public GameObject[] visitor;
	public int visitorNum;
	public Transform spawnPos;
    private bool canSpawn;
    private bool spawnFF;
	// Use this for initialization
	void Start () {

        canSpawn = true;
        spawnFF = false;
        EventManager.PauseClicked += Pause;
        EventManager.FastFClicked += FastForward;
        //SpawnMoreVisitors (0);
        saveRef = GameObject.FindGameObjectWithTag("save").GetComponent<SavingLoading>(); 
    }
	


	public void SpawnMoreVisitors( int num)
	{
		for (int i = 0; i < num; i++) {
            int genderInd;
            if(VisitorController.instance.GetIsGenderMale())
            {
               genderInd = 0;
            }
            else
            {
                genderInd = 1;
            }
            
            GameObject vis = Instantiate (visitor[genderInd]) as GameObject;
            if(vis.GetComponent<Visitor>())
            {
                
            vis.GetComponent<Visitor>().InitialiseVisitor(VisitorController.instance.GetAVisitorName(),
                                                         VisitorController.instance.GetAVisitorStayDays(), 
                                                         VisitorController.instance.GetVisitorFear());
            }
            VisitorController.instance.RandomiseVisitorInfo();
            vis.GetComponent<NavMeshAgent>().Warp(transform.position);
			vis.SetActive (true);
            if (vis != null)
            {
                saveRef.SaveVisitor(vis);
            }
            // vis.GetComponent<StateController>().SetupAI(true, GameManager.instance.GetWayPoints()); // Starts StateCOntroller
            vis.GetComponent<NavMeshAgent>().Warp(transform.position);
     
            if (spawnFF)
            {
                vis.GetComponent<MoveTowards>().FastForward();
            }
            vis.GetComponent<AI>().GetABed();
		}
	}

    void OnDisable()
    {
        EventManager.PauseClicked -= Pause;
        EventManager.FastFClicked -= FastForward;

    }

    private void Pause()
    {
        canSpawn = !canSpawn;
    }

    private void FastForward()
    {
        spawnFF = !spawnFF;
    }
   

    IEnumerator  WaitBeforeSpawning()
	{

		yield return new WaitForSeconds(10);
		SpawnMoreVisitors (2);


	}
}
