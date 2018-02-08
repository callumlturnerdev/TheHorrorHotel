using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torchTrigger : TriggerBase {

	float collisionCounter = 0;


    


    public override void ObjectEvent()
	{
        if (!beenUsed)
        {
            DebugConsole.Log(gameObject.name + " was triggered");
            ToggleLights();
            if (otherTrigger) otherTrigger.ObjectEvent();
        }
    }

    public override void ObjectOffEvent()
    {

        ToggleLights();
        if (otherTrigger)
        {
            otherTrigger.ObjectOffEvent();
        }
    }


    private void ToggleLights()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

}
