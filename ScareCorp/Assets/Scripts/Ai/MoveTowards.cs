using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class MoveTowards : MonoBehaviour, IPausable
{

	private NavMeshAgent nav;
	public Transform goal;
    private Rigidbody rb;
    private Animator anim;

    private bool currentlyPaused = false;
    private bool currentlyFastF = false;


    private int defaultSpeed = 1;
   
	// Use this for initialization
	void Start () {
        anim = transform.GetChild(0).GetComponent<Animator>();
        StartCoroutine(CheckGoal());
        rb = GetComponent<Rigidbody>();

        EventManager.PauseClicked += Pause;
        EventManager.FastFClicked += FastForward;
        
    }

    void OnDisable()
    {
        EventManager.PauseClicked -= Pause;
        EventManager.FastFClicked -= FastForward;
    }

    public void Pause()
    {
        if (currentlyPaused)
        {
            DebugConsole.Log("Pause was uncalled");
            nav.enabled = true;
            rb.isKinematic = false;
            anim.speed = 2;
            currentlyPaused = false;
            nav.destination = goal.position;
        }
        else
        {
            DebugConsole.Log("Pause was called");
            nav.enabled = false;
            rb.isKinematic = true;
            anim.speed = 0;
            currentlyPaused = true;
        }
    }

    public void FastForward()
    {
        DebugConsole.Log("FastF");
        if (!currentlyPaused)
        {
            if (currentlyFastF)
            {
                SetSpeed(defaultSpeed);
                currentlyFastF = false;
            }
            else
            {
                SetSpeed(defaultSpeed * 4);
                currentlyFastF = true;
            }
        }
    }

    public void SetSpeed(int speed)
    {
            nav = GetComponent<NavMeshAgent>();
            nav.speed = speed;
            anim = transform.GetChild(0).GetComponent<Animator>();
            anim.speed = speed;
    }

    void TimedUpdate()
    {
        goal = GameObject.FindGameObjectWithTag("exit").transform;
        nav = GetComponent<NavMeshAgent>();
        nav.destination = goal.position;
        //StartCoroutine(CheckGoal());
    }

    IEnumerator CheckGoal()
    {
        yield return new WaitForSeconds(2);
        TimedUpdate();

    }


}
