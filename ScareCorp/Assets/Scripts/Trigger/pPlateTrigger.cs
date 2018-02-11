using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pPlateTrigger : TriggerBase {


	void OnTriggerEnter(Collider other)
	{
        reTriggerTimer = 4;
        if (( other.tag == "scary" ||other.tag == "visitor") && !beenUsed) 
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
	}
    public override void ObjectOffEvent()
    {
        if (otherTrigger)
        {
            otherTrigger.ObjectOffEvent();
        }
    }
	public override void ObjectEvent()
	{
        StartCoroutine(resetTrigger());
    }
		
}
