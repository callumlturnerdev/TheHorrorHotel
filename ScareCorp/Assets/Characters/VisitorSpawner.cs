using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
            int randnum = Random.Range(0, visitor.Length);
            GameObject vis = Instantiate (visitor[randnum]) as GameObject;
            
          
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
