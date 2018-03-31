using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTrigger : TriggerBase {

	private AudioSource _audio;
	private Animator  _anim;
	// Use this for initialization
	private void OnEnable() 
	{
		_audio = GetComponent<AudioSource>();
		_anim = GetComponent<Animator>();
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
			LightUpLineRend(null);
			PlaySound();
			_anim.SetTrigger("ActivateScare");
			if(linkedTrigger)
			{
				linkedTrigger.ObjectEvent();
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
