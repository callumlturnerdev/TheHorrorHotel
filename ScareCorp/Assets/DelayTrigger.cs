using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayTrigger : TriggerBase {

	
   
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
			DebugConsole.Log("TRIGGGEREDDDD12");
            StartCoroutine(DelayTheLinkedTrigger(4));
		}
	}

    public override void ObjectOffEvent()
    {
		
    }

	private void CustomOffEvent()
	{
		 LightUpLineRend(null);
			triggered = false;
			beenUsed = false;
			if(linkedTrigger) linkedTrigger.ObjectOffEvent();
	}

	IEnumerator TurnOffEventTimer(float t)
	{
		yield return new WaitForSeconds(t);
		 CustomOffEvent();
	}
	IEnumerator DelayTheLinkedTrigger(float t)
	{
		yield return new WaitForSeconds(t);
		DebugConsole.Log("TRIGGGEREDDDD");
		 if(linkedTrigger) linkedTrigger.ObjectEvent();
		 StartCoroutine(TurnOffEventTimer(2));
	}
}
