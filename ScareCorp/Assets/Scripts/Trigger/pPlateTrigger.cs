using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pPlateTrigger : TriggerBase {

   
    void FixedUpdate() // TEMP WAY TO ENABLE PPLATE TRIGGER FOR TESTING
    {
        if(triggered == true)
        {
            otherTrigger.ObjectEvent();
            triggered = false;
        }
    }
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
	public override void ObjectEvent()
	{
        StartCoroutine(resetTrigger());
    }
		
}
