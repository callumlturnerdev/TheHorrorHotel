using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimizeButton : MonoBehaviour {	

	[SerializeField]
	AudioClip UIClick;
	AudioSource audioS;
	public List<GameObject> objectsToHide;
	public List<GameObject> objectsToShow;
	public GameObject headerToMove;
	private Vector2 trans;
	private bool isHidden = false;
	
	// Use this for initialization
	void Awake () {
		if(headerToMove)
		{
			trans = headerToMove.transform.position;
		}
		audioS = gameObject.AddComponent(typeof(AudioSource))as AudioSource;
		audioS.clip = UIClick;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void MoveHeader()
	{
		if(isHidden)
		{
		headerToMove.transform.Translate(0, -155,0);
		}
		else
		{
		headerToMove.transform.Translate(0, 155,0);		
		}
	}
	public  void MinimizePressed()
	{
		audioS.Play();
		foreach(GameObject obj in objectsToHide)
		{
			if(isHidden)
			{
			obj.SetActive(true);
			}
			else
			{
				obj.SetActive(false);
				if(objectsToShow.Count > 0)
				{
					foreach(GameObject ob in objectsToShow)
					{
						if(ob)
						{
							ob.SetActive(true);
						}
					}
				}
			}
		}
		isHidden = !isHidden;

	}
}
