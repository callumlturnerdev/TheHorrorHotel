using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopTrigger : TriggerBase {


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
			triggered = true;
			beenUsed = true;
            if(linkedTrigger) linkedTrigger.ObjectEvent();
			StartCoroutine(LoopDelay());
		}
	}

	private void CustomOffEvent()
	{
		 LightUpLineRend(null);
			triggered = false;
			beenUsed = false;
			if(linkedTrigger) linkedTrigger.ObjectOffEvent();
			StartCoroutine(LoopDelay());
	}
    public override void ObjectOffEvent()
    {
      		
    }

	
	IEnumerator LoopDelay()
	{
		yield return new WaitForSeconds(1);
		if(beenUsed)
		{
        	CustomOffEvent();
		}
		else
		{
			ObjectEvent();
		}
	}
}
