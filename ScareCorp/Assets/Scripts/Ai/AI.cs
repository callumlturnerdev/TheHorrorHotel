//#TODO2 AI characters need to stop constantly trying to do same action if only that object exists in the world.  IF next need is same as last need change to leave for now
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateAI;
using UnityEngine.UI;
using needTypes;
using fearTypes;

public class AI : MonoBehaviour
{

    AI_StateNeeds stateNeeds;
    public Animator anim;
    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;
    [HideInInspector] public NavMeshAgent navAgent;
    public AINeeds aiNeeds;
    public Transform exitSeek;

    [Header("Navigation")]
    //NAVIGATION
    public List<GameObject> usedScares; // Temp way of stopping the scared loop.
    public GameObject fearTarget;
    public int nextWayPoint;
    public State<AI> currentState;
    public GameObject currentTarget;
    //STATS
    public Transform eyes;
    // UI
    public Text StateText;

    [Header("NeedObjects")]
    public GameObject assignedBed;
    public List<GameObject> hungerObjects;
    public List<GameObject> tirednessObjects;
    public List<GameObject> boredomObjects;
    public List<GameObject> hygieneObjects;
    public List<GameObject> hidingPlaces;

    [Header("FearStats")]
    [Range(0, 1)]
    public float enviroment = 1.0f;
    [Range(0, 1)]
    public float gore = 1.0f;
    [Range(0, 1)]
    public float jumpScare = 1.0f;
    [Range(0, 1)]
    public float seperation = 1.0f;

    [Header("Debug")]
    // NEEDS
    [Range(0, 1)]
    public float hunger = 1.0f;
    [Range(0, 1)]
    public float hygiene = 1.0f;
    [Range(0, 1)]
    public float boredom = 1.0f;
    [Range(0, 1)]
    public float tiredness = 0.3f;

    [Header("Stats")]
    [HideInInspector] public float searchingTurnSpeed = 360;
    private float lookSphereCastRadius = 0.25f;
    private float lookRange = 190;
    [HideInInspector] public float scarelookRange = 4;
    public float stateTimeElapsed;
    [Header("UI")] 
    public Slider hungerSlider,hygieneSlider,boredomSlider,tirednessSlider,fearSlider;
    private bool timerFinished;
    private void Start()
    {
        stateNeeds = GetComponent<AI_StateNeeds>();
        TimeManager.DepartTime += DayChange;
        TimeManager.PlayRateChange += SetSpeed;
        TimeManager.timeStopped += SetSpeed;
        GameManager.ObjectAdd += GetNeedObjects;
        
        
        // FEAR SETUP
        enviroment = Random.Range(0,1.0F);
        gore = Random.Range(0, 1.0F);
        jumpScare = Random.Range(0, 1.0F);
        seperation = Random.Range(0, 1.0F);


        // NEEDS SETUP
        aiNeeds = gameObject.GetComponent<AINeeds>();
        hunger = 1.0f;
        hygiene = 1.0f;
        boredom = 1.0f;
        tiredness = 0.4f;

        anim = transform.GetChild(0).GetComponent<Animator>();
        GetNeedObjects();
        List<Transform> trans = GameManager.instance.GetWayPoints();
        exitSeek = GameManager.instance.wayPointList[0];
        searchingTurnSpeed = 180;
        navAgent = this.gameObject.GetComponent<NavMeshAgent>();
       
        gameTimer = Time.time;
        UpdateNeeds();
        SetSpeed();
        
    }

 
    private void DayChange()
    {
        if (aiNeeds.CheckIfTimeToLeave())
        {
            
            if(assignedBed)
            {
                assignedBed.GetComponent<Bed>().UnassignBed();
                assignedBed = null;
            }
        }
    }

    private void SetSpeed()
    {
        if(navAgent)
        {
            navAgent.speed = (1 *  TimeManager.instance.GetPlayRate()) * 3;
            //if(navAgent.speed == 0){anim.speed = 0;} 
            anim.speed= navAgent.speed;
        }
    }
    public void GetABed()
    {
          assignedBed = GameManager.instance.GetABed();
          assignedBed.GetComponent<Bed>().AssignBed(gameObject.GetComponent<Visitor>().GetName(), this.gameObject);
          GetComponent<AI_StateNeeds>().enabled = true;
    }
    public void GetNeedObjects()
    {
    
        hungerObjects = new List<GameObject>(GameManager.instance.hungerObjects);
        
        boredomObjects = new List<GameObject>(GameManager.instance.boredomObjects);
        hygieneObjects = new List<GameObject>(GameManager.instance.hygieneObjects);
        hidingPlaces = new List<GameObject>(GameManager.instance.hidingPlaces);
    }

    void OnDestroy()
    {
        AI_StateNeeds stateNeeds = GetComponent<AI_StateNeeds>();
        Destroy(stateNeeds);
        GameManager.instance.AddToTakenBeds(-1);
        if(assignedBed)
        {
            assignedBed.GetComponent<Bed>().UnassignBed();
        }
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
        hunger = gameObject.GetComponent<AINeeds>().GetHunger();
        UpdateNeeds();
    }

    public void DestroyOwner()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
       switchState = true;
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
        if(anim)
        {
            anim.SetBool("sleeping",false);
            anim.SetBool("scared",false);
            anim.SetBool("reading", false);
            anim.SetBool("eating", false);

            if(name == "Sleeping"){anim.SetBool("sleeping",true); }
            else if(name == "Scared"){anim.SetBool("scared",true);}
            else if(name == "Reading"){anim.SetBool("reading", true);}
            else if(name == "Eating"){anim.SetBool("eating", true);}
            else {anim.SetBool("sleeping",false); anim.SetBool("scared",false);
            
            }
        }
        else
        {
            anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        }
    
    }

    private void UpdateNeeds()
    {
        hunger = aiNeeds.GetHunger();
        hygiene = aiNeeds.GetHygiene();
        boredom = aiNeeds.GetBoredom();
        tiredness = aiNeeds.GetTiredness();
        if (aiNeeds.lowestNeedValue() == 0)
        {
            
        }
        UpdateSliders();
    }

    private void UpdateSliders()
    {
        if(boredomSlider && hygieneSlider && hungerSlider && tirednessSlider )
        {
            boredomSlider.value = boredom;
            hygieneSlider.value = hygiene;
            hungerSlider.value = hunger;
            tirednessSlider.value = tiredness;
        }
    }
   public GameObject SelectTarget(List<GameObject> e)
    {
        foreach (GameObject _e in e)
        {
            if (_e == null)
            {
                e.Remove(_e);
            }
        }
        if (e.Count > 1)
        {
            e.Sort(SortByDistance);
            //e.Reverse();
        }
                return e[0];
    }

     int SortByDistance( GameObject p1, GameObject p2)
    {
            return Vector2.Distance(transform.position,p1.transform.position).CompareTo(Vector2.Distance(transform.position, p2.transform.position));
    }
    static int SortByPriority(GameObject p1, GameObject p2)
    {
        return p1.GetComponent<Buildable>().needFulfillment.CompareTo(p2.GetComponent<Buildable>().needFulfillment);
    }
}
