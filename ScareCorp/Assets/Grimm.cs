using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grimm : MonoBehaviour {

	Animator animator;
	AudioSource audioS;
	// Use this for initialization

	void Awake()
	{
		animator = GetComponent<Animator>();
		audioS = GetComponent<AudioSource>();
	}
	public void Talking(bool t)
	{
		if(t)
		{
			audioS.Play();
		}
		else
		{
			audioS.Stop();
		}
		if(!animator)
		{
			animator = GetComponent<Animator>();
		}
		animator.SetBool("talking", t);
	}

	public void Leave(bool t)
	{
		if(!animator)
		{
			animator = GetComponent<Animator>();
		}
		animator.SetBool("leave", t);
	}
	// Update is called once per frame
}
