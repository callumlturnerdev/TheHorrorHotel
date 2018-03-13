using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBusterTrigger : TriggerBase {

	protected Animator anim;
    public GameObject zombie;  // Probably temp
    public bool hasZombie = false;
    private Transform spawnPos;
    private void OnEnable()
    {
        anim = transform.GetChild(1).GetComponent<Animator>();
        spawnPos = gameObject.transform.Find("SpawnPoint");
    }

    protected override void Init()
    {
        base.Init();
        anim = transform.GetChild(1).GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "visitor")
        {
            anim.SetTrigger("Scare");
        }
    }

public override void ObjectOffEvent()
	{
		if(linkedTrigger)
		{
            LightUpLineRend(null);
			linkedTrigger.ObjectOffEvent();
		}
	}

    public override void ObjectEvent()
	{
		
        LightUpLineRend(null);
            hasZombie = true;
           anim.SetTrigger("Scare"); 
           if(linkedTrigger){linkedTrigger.ObjectEvent();}
			triggered = true;
			beenUsed = true;
		
	}
		
}
