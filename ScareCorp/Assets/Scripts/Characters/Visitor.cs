using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class Visitor : MonoBehaviour 
{
    SavingLoading saveSystem;
    Rigidbody rb;
    float maxFear = 100;
    float currentFear = 0;
    private Image fearBar;
    private NavMeshAgent agent;
    private Animator anim;
    GridBuilder gridBuilderRef;
    void Awake()
    {
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
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "exit") {
            Debug.Log("hi");
            Destroy(this.gameObject);
        }
    }
    public void SetCurrentFear(float fear) {  currentFear = fear; }
    public float GetCurrentFear() { return currentFear; }
    public void Update()
    {
        anim.speed = agent.speed;
        //agent.speed
    }

    public void Scare(float amount) // Is called when the visitor is scared
    {
        currentFear += amount;
        BuildController.instance.AddPoints(amount * 10);
        fearBar = transform.GetChild(0).Find("VisitorUI").Find(name: "fearFill").GetComponent<Image>();
        fearBar.fillAmount = currentFear / maxFear;
    }

}
