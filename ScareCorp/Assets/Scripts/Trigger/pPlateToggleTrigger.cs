using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pPlateToggleTrigger : TriggerBase
{

    private bool toggle = true;

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "visitor"  || other.tag == "scary" )

        {
            if (toggle)
            {
                if (otherTrigger != null)
                {
                    otherTrigger.ObjectEvent();
                    toggle = false;
                }
            }
            else if (!toggle)
            {
                if (otherTrigger != null)
                {
                    otherTrigger.ObjectOffEvent();
                    toggle = true;
                }
            }
            StartCoroutine(resetTrigger());
        }
    }

}
