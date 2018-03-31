using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buildModes;
public class TriggerReceiver : MonoBehaviour {

	[SerializeField]
	GameObject linkedSender;

	// Use this for initialization
	void Awake () {
		linkedSender = null;
	}
	
	// Update is called once per frame

	public void SetLinkedSender(GameObject sender)
	{
		linkedSender = sender;
		CreateATriggerSender();
	}

	private void CreateATriggerSender()
	{
		DebugConsole.Log("Making A Sender");
		if(gameObject.GetComponent<TriggerSender>() == null)
		{
			gameObject.AddComponent<TriggerSender>();
			gameObject.GetComponent<TriggerSender>().PreviousSenderRef(linkedSender.GetComponent<TriggerSender>());
			Destroy(this);
		}
	}

	 void OnDestroy()
	 {
		 
	 }
	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(1))
		{
			/*if(BuildController.instance.GetCurrentBuildMode() != eBuildMode.trigger)
			{
				BuildController.instance.TrigggerMode(true);
			}*/
			if(BuildController.instance.GetCurrentBuildMode() == eBuildMode.trigger)
			{
				TriggerLinker.instance.SetCurrentReceiver(this.gameObject);
			}
		}
	}

}
