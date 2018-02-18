using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BASE CLASS FOR AN OBJECTY THAT CHECKS FOR A COLLISION
abstract public class TriggerBase : MonoBehaviour {

    public TriggerBase parentTrigger;
	public TriggerBase otherTrigger;
	public GameObject triggerObject;
	protected LineRenderer line;
	protected bool lineToMouse;
	protected GameObject cursor;
	public bool triggered = false;
	protected bool beenUsed = false;

    protected float reTriggerTimer = 5;

	 void  Awake ()
    {
		line = GetComponent<LineRenderer> ();
		cursor = GameObject.FindGameObjectWithTag ("cursor");
        reTriggerTimer = 10;
	}

    protected virtual void Init()
    {

    }
	  void Update ()
    {
		LineRendMouse ();
	}

	public   bool GetTriggered()
	{return triggered;}

	public  void SetTriggered(bool newValue)
	{
		triggered = newValue;
		beenUsed = true;
	}
    public void SetTriggerParent(GameObject trigger)
    {
        lineToMouse = false;
        triggerObject = trigger;
        parentTrigger = triggerObject.GetComponent<TriggerBase>();
        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, parentTrigger.transform.position);
    }

	public  void SetTriggerObject(GameObject trigger)
	{
		lineToMouse = false;
		triggerObject = trigger;
		otherTrigger = triggerObject.GetComponent<TriggerBase> ();
		line.SetPosition(0, this.transform.position);
		line.SetPosition (1, triggerObject.transform.position);
	}

	 void LineRendMouse()
	{
		if (lineToMouse)
		{
			line.SetPosition(0, this.transform.position);
			line.SetPosition (1, cursor.transform.position);
		}
	}

	protected  void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (1)) 
		{
			lineToMouse = true;
	
			TriggerController.instance.SetCurrentTrigger (this.gameObject);
		}
	}


	public  virtual void ObjectEvent()
	{
	}

    public virtual void ObjectOffEvent()
    {
   
    }


    protected IEnumerator resetTrigger()
    {
        yield return new WaitForSeconds(reTriggerTimer);
        ObjectOffEvent();
    }
}
