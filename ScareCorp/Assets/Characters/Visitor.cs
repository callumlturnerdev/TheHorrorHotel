using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using fearTypes;

public class Visitor : MonoBehaviour
{ 
    private ParticleSystem particleSys;
    public Text visitorName;
    SavingLoading saveSystem;
    AI aiScriptRef;
    Rigidbody rb;
    float maxFear = 100;
    float currentFear = 0;
    private Image fearBar;
    private NavMeshAgent agent;
    private Animator anim;
    GridBuilder gridBuilderRef;
    public GameObject lastFearObject;
    [SerializeField]
    bool man = true;
    void Awake()
    {
        particleSys = GetComponent<ParticleSystem>();
        visitorName.text = GetRandomName(man);
        aiScriptRef = gameObject.GetComponent<AI>();
        saveSystem = GameObject.FindGameObjectWithTag("save").GetComponent<SavingLoading>();
        agent = GetComponent<NavMeshAgent>();
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        fearBar = transform.GetChild(0).transform.Find( "VisitorUI").Find(name: "fearFill").GetComponent<Image>();
        fearBar.fillAmount = currentFear / maxFear;
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
    public void SetCurrentFear(float fear) {  currentFear = fear; }
    public float GetCurrentFear() { return currentFear; }
    public void Update()
    {
       // anim.speed = agent.speed;
        //agent.speed
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
       
        currentFear += amount;
        BuildController.instance.AddPoints(amount * 10);
        fearBar = transform.GetChild(0).Find("VisitorUI").Find(name: "fearFill").GetComponent<Image>();
        fearBar.fillAmount = currentFear / maxFear;
        particleSys.Play();
    }

    public void TurnOffScareParticle()
    {
       // particleSys.Pause();
        particleSys.Stop();
        DebugConsole.Log("hi", "error");
    }

    public string GetName() {return visitorName.text;}
    private string  GetRandomName(bool man)
    {
        string[] menNames = new string[] {"Bob", "Dave" , "Malcolm" , "Justin" , "Bruce" , "Alan" , "Jerry" , "Mark" , "Jeremy"};
         string[] womenNames = new string[] {"Sarah", "Ripley" , "Clair" , "Jill" , "Barbara" , "Lola" , "Ramona" , "Tracy" , "Margaret"};
        if(man)
        {
            int randnum = Random.Range(0, menNames.Length);
            return menNames[randnum];
        }
        else
        {
            int randnum = Random.Range(0, womenNames.Length);
            return womenNames[randnum];
        }
    }

}
