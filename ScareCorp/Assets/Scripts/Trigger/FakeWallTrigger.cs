﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWallTrigger : TriggerBase {

	protected Animator anim;
    public GameObject zombie;  // Probably temp
    public bool hasZombie = false;
    private Transform spawnPos;
    private void OnEnable()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        spawnPos = gameObject.transform.Find("SpawnPoint");
        //zombie = this.transform.Find("Zombie").gameObject;
    }

    protected override void Init()
    {
        base.Init();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        
    }
    public override void ObjectOffEvent()
	{
        anim.SetBool("triggerWall", false);
		if(linkedTrigger)
		{
            
            LightUpLineRend(null);
			linkedTrigger.ObjectOffEvent();
		}
	}
    public override void ObjectEvent()
	{
        
		     LightUpLineRend(null);
             anim.SetBool("triggerWall", true);
		     triggered = true;
		     beenUsed = true;
            if(linkedTrigger) linkedTrigger.ObjectEvent();
        
	}
		
}
