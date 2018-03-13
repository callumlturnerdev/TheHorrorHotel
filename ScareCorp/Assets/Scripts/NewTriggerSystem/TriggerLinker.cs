using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLinker : MonoBehaviour {

	public static TriggerLinker instance = null;
	[SerializeField]
	private GameObject tSender;
	[SerializeField]
	private GameObject tReciever;
	private bool canLink = false;

	// Use this for initialization
	void Awake () {
		if(instance == null)
		{
			instance =  this;
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
		
	}
	
	public void SetCurrentSender(GameObject sender)
	{
		if(sender.GetComponent<TriggerSender>())
		{
			tSender = sender;
			canLink = true;
		}
	}

	public void SetCurrentReceiver(GameObject receiver)
	{
		if(receiver.GetComponent<TriggerReceiver>())
		{
			tReciever = receiver;
			AttemptConnection();
		}
	}


	private void AttemptConnection()
	{
		if(tSender != null && tReciever != null)
		{
			tSender.GetComponent<TriggerSender>().SetLinkedReceiver(tReciever);
			tReciever.GetComponent<TriggerReceiver>().SetLinkedSender(tSender);
			tSender= null;
			tReciever = null;
		}
	}





	// Update is called once per frame
	void Update () {
		
	}
}
