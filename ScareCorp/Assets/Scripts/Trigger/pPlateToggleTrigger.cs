using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pPlateToggleTrigger : TriggerBase
{

    private bool toggle = true;

    void Start()
    {
        canCreateLink = true;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "visitor"  || other.tag == "scary" )
        {
            OnCollision();
        }
    }

    public override void OnCollision()
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
