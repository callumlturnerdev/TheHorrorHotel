using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTrigger : TriggerBase {

	private AudioSource _audio;

	// Use this for initialization
	private void OnEnable() 
	{
		_audio = GetComponent<AudioSource>();
	}
	
	public override void ObjectOffEvent()
	{
	
		if(linkedTrigger)
		{
			LightUpLineRend(null);
			linkedTrigger.ObjectOffEvent();
		}
	}

	public override void ObjectEvent()
	{
		if(!beenUsed)
		{
		
			PlaySound();
			if(linkedTrigger)
			{
				LightUpLineRend(null);
				linkedTrigger.ObjectEvent();
			}
		}
	}

	private void PlaySound()
	{
		if(_audio.clip != null)
		{
		_audio.Play();
		}

	}

}
