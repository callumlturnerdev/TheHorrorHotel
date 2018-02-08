using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateAI;
using UnityEngine.UI;
using needTypes;
public class AI : MonoBehaviour
{

    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;
    [HideInInspector] public NavMeshAgent navAgent;
    public StateMachine<AI> stateMachine { get; set; }
    public AINeeds aiNeeds;
    public Transform treasureSeek;

    [Header("Navigation")]
    //NAVIGATION
    public List<GameObject> pointsOfInterest;
    public List<GameObject> pointsExplored;
    public GameObject fearTarget;
    public int nextWayPoint;
    public State<AI> currentState;
    //STATS
    public Transform eyes;
    // UI
    public Text StateText;

    [Header("NeedObjects")]
    public List<GameObject> hungerObjects;
    public List<GameObject> tirednessObjects;
    public List<GameObject> boredomObjects;
    public List<GameObject> hygieneObjects;




    [Header("Debug")]
    // NEEDS
     [Range(0, 1)]
    public float hunger = 1.0f;
    [Range(0, 1)]
    public float hygiene = 1.0f;
    [Range(0, 1)]
    public float boredom = 1.0f;
    [Range(0, 1)]
    public float tiredness = 1.0f;

    [Header("Stats")]
    [HideInInspector] public float searchingTurnSpeed = 360;
    private float lookSphereCastRadius = 0.25f;
    private float lookRange = 90;
    public float stateTimeElapsed;


    


    private bool timerFinished;
    private void Start()
    {
        

        aiNeeds = gameObject.GetComponent<AINeeds>();
        hunger = 1.0f;
        hygiene = 1.0f;
        boredom = 1.0f;
        tiredness = 1.0f;

        List<Transform>trans = GameManager.instance.GetWayPoints();
        treasureSeek = trans[0];
        searchingTurnSpeed = 180;
        navAgent = this.gameObject.GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(NeedScanState.Instance);
        gameTimer = Time.time;
    }

    public float  GetLookRange(){return lookRange;}
    public float GetSphereCastRadius() { return lookSphereCastRadius; }


    public bool CheckifCountDownElapsed(float duration)
    {
        if (Time.time > gameTimer + 1)
        {
            gameTimer = Time.time;
            seconds++;
        }

        if (seconds == duration)
        {
            seconds = 0;
            return true;
        }
        return false;
    }

   
    private void Update()
    {
        stateMachine.Update();
        currentState = stateMachine.currentState;
        hunger = gameObject.GetComponent<AINeeds>().GetHunger();
        UpdateNeeds();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if(other.tag == "Door")
        {

            
            if (other.name == "Side1")
            {
                DebugConsole.Log("Side1");
            }
            //navAgent.destination = other.transform.parent.Find("Side2").transform.position;
            // transform.position = Vector3.MoveTowards(other.transform.position, other.gameObject.transform.parent.Find("Side2").transform.position, 10);
            this.gameObject.transform.rotation = other.transform.rotation;
             this.gameObject.transform.Rotate(0, 90, 0);
        }

        if (other.name == "Side2")
        {
            DebugConsole.Log("Side2");
           // navAgent.destination = other.transform.parent.Find("Side1").transform.position;
            //transform.position = Vector3.MoveTowards(other.transform.position, other.gameObject.transform.parent.Find("Side1").transform.position, 10);
           this.gameObject.transform.rotation = other.transform.rotation;
            this.gameObject.transform.Rotate(0, -90, 0);
        }
        */
        switchState = true;
            pointsExplored.Add(other.gameObject);
     }
    
    private void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(eyes.position, lookSphereCastRadius);
            Gizmos.DrawIcon(gameObject.transform.position, "test");
            
        }
    }

    public void UpdateStateUI(string name)
    {
        StateText.text = name;
    }

    private void UpdateNeeds()
    {
        hunger = aiNeeds.GetHunger();
        hygiene = aiNeeds.GetHygiene();
        boredom = aiNeeds.GetBoredom();
        tiredness = aiNeeds.GetTiredness();
    }

   public GameObject SelectTarget(List<GameObject> e)
    {
        if (e.Count > 1)
        {

            e.Sort(SortByPriority);
        }
                return e[0];
              

    }

    public GameObject GetRandomDoor()
    {
        List<GameObject> doors =  new List<GameObject>();
        if (nextWayPoint < pointsOfInterest.Count)
        {

            foreach (GameObject point in pointsOfInterest)
            {
                if (point.GetComponent<doorTrigger>())
                {
                    doors.Add(point);

                }
            }
            int randnum = Random.Range(0, doors.Count - 1);
            return doors[randnum];
        }
        else if (pointsExplored != null)
        {
            foreach (GameObject point in pointsExplored)
            {
                if (point.GetComponent<doorTrigger>())
                {
                    doors.Add(point);
                }
            }
            int randnum = Random.Range(0, doors.Count - 1);
            return doors[randnum];
        }
        return null;

    }



    static int SortByPriority(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<Buildable>().needFulfillment.CompareTo(p2.GetComponent<Buildable>().needFulfillment);
    }
}
