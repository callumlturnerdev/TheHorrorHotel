using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : TriggerBase {

	protected Animator anim;
	protected BoxCollider bCol;

    private void Awake()
    {
        Transform trans;
        trans = this.gameObject.transform;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "visitor" || collision.gameObject.tag == "scary")
        {
           OnCollision();
        }
    }

    public override void OnCollision()
    {
         bCol = GetComponent<BoxCollider>();
            bCol.enabled = false;
            anim = GetComponent<Animator>();
            anim.SetBool("Open", true);
            triggered = true;
            beenUsed = true;

            if (linkedTrigger != null)
            {
                if (triggered == true)
                {
                    LightUpLineRend(triggerLineMat);
                    linkedTrigger.ObjectEvent();
                }
            }
            StartCoroutine(resetTrigger());

            StartCoroutine(CloseDoor());
    }
    
    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(1);
        bCol = GetComponent<BoxCollider>();
        bCol.enabled = true;
        anim = GetComponent<Animator>();
        anim.SetBool("Open", false);
        triggered = false;
        beenUsed = false;
        LightUpLineRend(null);
    }

    public override void ObjectOffEvent()
	{
		if(linkedTrigger)
		{
            LightUpLineRend(null);
            LightUpLineRend(null);
			linkedTrigger.ObjectOffEvent();
		}
	}
    public override void ObjectEvent()
	{
		if (!beenUsed)
		{
            LightUpLineRend(null);
			bCol = GetComponent<BoxCollider> ();
			bCol.enabled = false;
			anim = GetComponent<Animator> ();
			anim.SetBool ("Open", true);
			triggered = true;
			beenUsed = true;
            linkedTrigger.ObjectEvent();
		}
	}	
}
