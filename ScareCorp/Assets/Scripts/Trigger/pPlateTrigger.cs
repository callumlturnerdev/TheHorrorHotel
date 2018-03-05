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
        if (( other.tag == "scary" ||other.tag == "visitor") && !beenUsed) 
		{
			OnCollision();
        }
	}
    public override void OnCollision()
    {
        triggered = true;
            if (otherTrigger != null)
            {
                if (triggered == true)
                {
                    otherTrigger.ObjectEvent();
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
        if(otherTrigger) otherTrigger.ObjectOffEvent();
    }
		
}
