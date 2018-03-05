using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wardrobeTrigger : TriggerBase {

	protected Animator anim;
    public GameObject zombie;  // Probably temp
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
            hasZombie = true;
            anim = GetComponent<Animator> ();
			anim.SetBool ("Open", true);
            GameObject zomb;
            zomb = Instantiate(zombie) as GameObject;
            zomb.transform.position = new Vector3(spawnPos.position.x, spawnPos.transform.position.y , spawnPos.position.z);
            zomb.transform.rotation = spawnPos.transform.rotation;
			triggered = true;
			beenUsed = true;
            if(otherTrigger) otherTrigger.ObjectEvent();
		}
	}

    public override void ObjectOffEvent()
    {
       
             hasZombie = true;
            anim = GetComponent<Animator> ();
			anim.SetBool ("Open", false);
            GameObject zomb;
            zomb = Instantiate(zombie) as GameObject;
            zomb.transform.position = new Vector3(spawnPos.position.x, spawnPos.transform.position.y , spawnPos.position.z);
            zomb.transform.rotation = spawnPos.transform.rotation;
			triggered = false;
			beenUsed = false;
            if(otherTrigger)otherTrigger.ObjectOffEvent();
    }

    private void ToggleWardrobeOpen()
    {
        
    }
}
