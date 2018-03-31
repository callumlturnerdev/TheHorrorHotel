using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pPlateTrigger : TriggerBase {

   

    void Start()
    {
        canCreateLink = true;
    }
	void OnTriggerEnter(Collider other)
	{
        reTriggerTimer = 4;
        if (( other.tag == "visitor") && !beenUsed) 
		{
			OnCollision();
        }
	}
    public override void OnCollision()
    {
        triggered = true;
            if (linkedTrigger != null)
            {
                if (triggered == true)
                {
                    LightUpLineRend(triggerLineMat);
                    linkedTrigger.ObjectEvent();
                }
            }
            StartCoroutine(resetTrigger());
    }
	public override void ObjectEvent()
	{
        
       // StartCoroutine(resetTrigger());
    }
    public override void ObjectOffEvent()
    {
        LightUpLineRend(null);
        if(linkedTrigger) linkedTrigger.ObjectOffEvent();
    }
		
}
