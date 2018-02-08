using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : TriggerBase {

	protected Animator anim;
	protected BoxCollider bCol;

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
