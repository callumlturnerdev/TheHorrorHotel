using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buildModes;

// BASE CLASS FOR AN OBJECTY THAT CHECKS FOR A COLLISION
abstract public class TriggerBase : MonoBehaviour {

	[SerializeField]
	protected Material triggerLineMat;
	[SerializeField]
	
	public bool triggered = false;
	protected bool beenUsed = false;
	[SerializeField]
	protected bool canCreateLink = false;
    protected float reTriggerTimer = 5;

	protected TriggerBase linkedTrigger;

	 void  Awake ()
    {
        reTriggerTimer = 10;
	}

    protected virtual void Init()
    {

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
       
    }

	public  void SetTriggerObject(TriggerBase obj)
	{
		if(obj)
		{
			linkedTrigger = obj;
		}
		if(obj == null)
		{
			linkedTrigger= null;
		}
	}

	protected  void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (1))
		{
			if(BuildController.instance.GetCurrentBuildMode() != eBuildMode.trigger)
			{
				BuildController.instance.TrigggerMode(true);
			}
		}
		if(Input.GetMouseButtonDown(2))
		{
			OnCollision();
		}
	}

	public virtual void OnCollision()
	{

	}
	public  virtual void ObjectEvent()
	{
	}

    public virtual void ObjectOffEvent()
    {
   
	}
	protected void LightUpLineRend(Material mat)
	{
		if(gameObject.GetComponent<TriggerSender>())
		{
			gameObject.GetComponent<TriggerSender>().SetTriggerLineMat(mat);
		}
	}

    protected IEnumerator resetTrigger()
    {
        yield return new WaitForSeconds(reTriggerTimer);
        ObjectOffEvent();
    }
}
