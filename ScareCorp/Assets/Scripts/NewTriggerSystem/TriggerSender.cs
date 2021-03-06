﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buildModes;
public class TriggerSender : MonoBehaviour {

	// Use this for initialization

	GameObject linkedReceiver;
	[SerializeField]
	private Material defaultMat;
	private Material litMat;
	private bool enableLinking;
	LineRenderer line;
	private bool lightLine = false;
	TriggerSender previousSenderRef; // Ref to the object linking this
	void Awake () {
		//enableLinking = false;
		//linkedReceiver = null;
		line = gameObject.GetComponent<LineRenderer>();
		defaultMat = Resources.Load("Materials/lineMat") as Material;
		litMat = Resources.Load("Materials/TriggerMAt") as Material;
		line.material = defaultMat;
		StartCoroutine(EnableTriggerSystem(0.01f));
	}
	

	public void PreviousSenderRef(TriggerSender previousRef) // Used to keep a reference to the object linking this one.
	{
		previousSenderRef = previousRef;
	}

	 void OnDestroy()
	 {
		 if(previousSenderRef)
		 	{previousSenderRef.UnlinkReceiver();}

		if(linkedReceiver)
			{
				if(linkedReceiver.GetComponent<TriggerSender>())
				{
					linkedReceiver.GetComponent<TriggerSender>().RevertToReceiver(); 
				}
			}	
	 }


	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(1) && enableLinking)
		{
			if(BuildController.instance.GetCurrentBuildMode() != eBuildMode.trigger)
			{
				BuildController.instance.TrigggerMode(true);
			}
			TriggerLinker.instance.SetCurrentSender(this.gameObject);
			enableLinking = false;
		}
		if(Input.GetMouseButtonDown(2) && BuildController.instance.GetCurrentBuildMode() != eBuildMode.deleting)
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
		//enableLinking = true;
		line = gameObject.GetComponent<LineRenderer>();
		if(linkedReceiver != null){UnlinkReceiver();}
		
		if(receiver && receiver.GetComponent<TriggerReceiver>())
		{
			linkedReceiver = receiver;
			linkedReceiver.GetComponent<TriggerReceiver>().SetLinkedSender(this.gameObject); //

		}
		
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

	public GameObject GetLinkedReceiver(){return linkedReceiver;}
	IEnumerator  EnableTriggerSystem(float t)
	{
		yield return new WaitForSeconds(t);
		enableLinking = true;
	}
	
}
