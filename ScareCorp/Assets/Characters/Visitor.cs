using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using fearTypes;

public class Visitor : MonoBehaviour
{ 
    private ParticleSystem particleSys;
    public GameObject floatingTextObj;
    private string name;
    private int daysStaying;
    private eFearTypes fear;
    public Text visitorName;
    SavingLoading saveSystem;
    AI aiScriptRef;
    Rigidbody rb;
    float maxFear = 100;
    float currentFear = 0;
    float newFear;
    private Image fearBar;
    private NavMeshAgent agent;
    private Animator anim;
    GridBuilder gridBuilderRef;
    public GameObject lastFearObject;
    [SerializeField]

    [Header("UI")]
    Slider fearSlider;
      [SerializeField]
    GameObject NeedsUI;

    public void  InitialiseVisitor(string _name, int _daysStaying, eFearTypes _fear ) 
    {
        name = _name;
        daysStaying = _daysStaying;
        fear = _fear;
         visitorName.text = name;
    }

    bool man = true;
    void Awake()
    {
        name = "";
        particleSys = GetComponent<ParticleSystem>();
        aiScriptRef = gameObject.GetComponent<AI>();
        saveSystem = GameObject.FindGameObjectWithTag("save").GetComponent<SavingLoading>();
        agent = GetComponent<NavMeshAgent>();
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
         fearSlider.value = currentFear;
        gridBuilderRef = GameObject.FindGameObjectWithTag("gridbuilder").GetComponent<GridBuilder>();
        gridBuilderRef.AddVisitor(this.gameObject);
    }
    private void OnDestroy()
    {
        saveSystem.DeleteVisitorSave(this.gameObject);
    }

    public void SetNextFearObject(GameObject newFearObject)
    {
        lastFearObject = newFearObject.transform.parent.gameObject;
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "exit") {
            Destroy(this.gameObject);
        }
    }
    public void SetCurrentFear(float fear) {  currentFear = fear;  fearSlider.value = currentFear; }
    public float GetCurrentFear() { return currentFear; }
    public void Update()
    {
       // anim.speed = agent.speed;
        //agent.speed

        if(Input.GetKeyDown(KeyCode.N))
        {
            if(BuildController.instance.UINeedsOn())
            {
                NeedsUI.SetActive(false);
            }
            else
            {
                NeedsUI.SetActive(true);
            }
        }
    }

    public void Scare(float amount ) // Is called when the visitor is scared
    {
        if (lastFearObject != null)
        {
            if (lastFearObject.GetComponent<Buildable>())
            {
                switch (lastFearObject.GetComponent<Buildable>().GetFearType())
                {
                    case eFearTypes.Enviroment:
                        amount = amount * aiScriptRef.enviroment;
                        Debug.Log("amount" + amount + " * " + "scareMultiplyr " + aiScriptRef.enviroment);
                        break;
                    case eFearTypes.Gore:
                        amount = amount * aiScriptRef.gore;
                        Debug.Log("amount" + amount + " * " + "scareMultiplyr " + aiScriptRef.gore);
                        break;
                    case eFearTypes.JumpScare:
                        amount = amount * aiScriptRef.jumpScare;
                        Debug.Log("amount" + amount + " * " + "scareMultiplyr " + aiScriptRef.jumpScare);
                        break;
                    case eFearTypes.Seperation:
                        amount = amount * aiScriptRef.seperation;
                        Debug.Log("amount" + amount + " * " + "scareMultiplyr " + aiScriptRef.seperation);
                        break;
                    case eFearTypes.none:
                        amount = amount * 1;
                        break;
                }
            }
        }
       DebugConsole.Log("WRRRRRRRRRRRRROKING");
        newFear = currentFear + amount;
        AddToFear();
        
        fearSlider.value = currentFear;
        if(particleSys.isStopped)
        {
            particleSys.Play();
        }
    }

    IEnumerator waitforSecondsToAddfear(float f)
    {
        yield return new WaitForSeconds(f);
        DebugConsole.Log("adding fear");
        AddToFear();

    }
    void AddToFear()
    {
        currentFear += 1;
         fearSlider.value = currentFear;
         BuildController.instance.AddPoints(1 * 10);

        GameObject floatText = Instantiate(floatingTextObj) as GameObject;
        floatText.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.green;
        floatText.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = "+" + 1 *10;
        floatText.transform.parent = this.transform;
      
        Vector3 floatTrans = new Vector3(gameObject.transform.position.x-20,gameObject.transform.position.y,gameObject.transform.position.z);
          floatText.transform.position = floatTrans;
        floatText.transform.rotation = Quaternion.Euler(0,0,0);
        if(currentFear < newFear)
        {
            StartCoroutine(waitforSecondsToAddfear(1));
        }
    }
    public void TurnOffScareParticle()
    {
       // particleSys.Pause();
        particleSys.Stop();
        DebugConsole.Log("hi", "error");
    }

   public string GetName(){return name;}

}
