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
        if (collision.gameObject.tag == "visitor")
        {
            bCol = GetComponent<BoxCollider>();
            bCol.enabled = false;
            anim = GetComponent<Animator>();
            anim.SetBool("Open", true);
            triggered = true;
            beenUsed = true;
            StartCoroutine(CloseDoor());
        }
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
    }
    public override void ObjectEvent()
	{
		if (!beenUsed)
		{
			bCol = GetComponent<BoxCollider> ();
			bCol.enabled = false;
			anim = GetComponent<Animator> ();
			anim.SetBool ("Open", true);
			triggered = true;
			beenUsed = true;
		}
	}	
}
