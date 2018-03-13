using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buildModes;
public class TriggerSender : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
	GameObject linkedReceiver;
	[SerializeField]
	private Material defaultMat;
	private Material litMat;
	LineRenderer line;
	private bool lightLine = false;
	void Awake () {
		linkedReceiver = null;
		line = gameObject.GetComponent<LineRenderer>();
		defaultMat = Resources.Load("Materials/lineMat") as Material;
		litMat = Resources.Load("Materials/TriggerMAt") as Material;
		line.material = defaultMat;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(BuildController.instance.GetCurrentBuildMode() != eBuildMode.trigger)
			{
				BuildController.instance.TrigggerMode(true);
			}

			TriggerLinker.instance.SetCurrentSender(this.gameObject);
		}
		if(Input.GetMouseButtonDown(1))
		{
			if(BuildController.instance.GetCurrentBuildMode() != eBuildMode.trigger)
			{
				BuildController.instance.TrigggerMode(true);
			}

			TriggerLinker.instance.SetCurrentSender(this.gameObject);
			if(BuildController.instance.GetCurrentBuildMode() == eBuildMode.trigger)
			{
				
				UnlinkReceiver();
			}
		}
	}

	public TriggerBase GetLinkedTriggerBase()
	{
		if(linkedReceiver)
		{
			if(linkedReceiver.GetComponent<TriggerBase>())
			{
				return linkedReceiver.GetComponent<TriggerBase>();
			}
			return null;
		}
		return null;
	}
	public void SetLinkedReceiver(GameObject receiver)
	{
		if(linkedReceiver != null){UnlinkReceiver();}
		linkedReceiver = receiver;
		line.SetPosition(0,this.transform.position);
		line.SetPosition(1,linkedReceiver.transform.position);
		if(gameObject.GetComponent<TriggerBase>())
		{
			gameObject.GetComponent<TriggerBase>().SetTriggerObject(linkedReceiver.GetComponent<TriggerBase>());
		}
	}

	void UnlinkReceiver()
	{
		if(linkedReceiver != null)
		{
			if(linkedReceiver.GetComponent<TriggerSender>())
			{
				if(gameObject.GetComponent<TriggerBase>())
				{
					gameObject.GetComponent<TriggerBase>().SetTriggerObject(null);
				}
				line.SetPosition(0,this.transform.position);
				line.SetPosition(1,this.transform.position);
				linkedReceiver.GetComponent<TriggerSender>().UnlinkReceiver();
				linkedReceiver.GetComponent<TriggerSender>().RevertToReceiver();
			}
		}
	}

	public void SetTriggerLineMat(Material mat)
	{
		if(lightLine == false)
		{
		line.material = litMat;
		lightLine = true;
		}
		else
		{
			line.material = defaultMat;
			lightLine = false;
		}
	}
	void RevertToReceiver()
	{
		if(gameObject.GetComponent<TriggerReceiver>() == null)
		{
			gameObject.AddComponent<TriggerReceiver>();
			Destroy(this);
		}
	}
	
}
