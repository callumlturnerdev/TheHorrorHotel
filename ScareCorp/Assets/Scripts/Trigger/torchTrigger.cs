using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torchTrigger : TriggerBase {

	float collisionCounter = 0;
    public GameObject particleObj;
    public override void ObjectEvent()
	{
        if (!beenUsed)
        {
            LightUpLineRend(null);
            ToggleLights();
            if (linkedTrigger) linkedTrigger.ObjectEvent();
        }
    }

    public override void ObjectOffEvent()
    {
        ToggleLights();
        if (linkedTrigger)
        {
            LightUpLineRend(null);
            linkedTrigger.ObjectOffEvent();
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

        if(particleObj.activeSelf)
        {
            particleObj.SetActive(false);
        }
        else
        {
           particleObj.SetActive(true);
        }
    }
}
