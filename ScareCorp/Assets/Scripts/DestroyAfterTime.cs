using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

	[SerializeField]
	private float timer;
	// Use this for initialization
	void Awake () {
		StartCoroutine(DestroyOnWait(timer));
	}
	IEnumerator DestroyOnWait(float t)
	{
		yield return new WaitForSeconds(t);
		Destroy(this.gameObject);
	}
	// Update is called once per frame
}
