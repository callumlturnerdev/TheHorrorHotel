using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wardrobeTrigger : TriggerBase {

	protected Animator anim;
   
    public bool hasZombie = false;
    private Transform spawnPos;
    private void OnEnable()
    {
        spawnPos = gameObject.transform.Find("SpawnPoint");
    }

    public override void ObjectEvent()
	{
		if (!beenUsed)
		{
            LightUpLineRend(null);
            hasZombie = true;
            anim = GetComponent<Animator> ();
			anim.SetBool ("Open", true);
			triggered = true;
			beenUsed = true;
            if(linkedTrigger) linkedTrigger.ObjectEvent();
		}
	}

    public override void ObjectOffEvent()
    {
       LightUpLineRend(null);
             hasZombie = true;
            anim = GetComponent<Animator> ();
			anim.SetBool ("Open", false);
			triggered = false;
			beenUsed = false;
            if(linkedTrigger)linkedTrigger.ObjectOffEvent();
    }

    private void ToggleWardrobeOpen()
    {
        
    }
}
