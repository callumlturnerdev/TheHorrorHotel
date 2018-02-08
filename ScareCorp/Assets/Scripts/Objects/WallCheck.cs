using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour {

	public int collisionCount = 0;
	public bool isOnWall = false;
	// Placed on object under cursor when building to check for walls for specialised items that can 
	// only be built on walls
	// Use this for initialization
	Renderer rend;

	void Start () {

		rend = GetComponent<Renderer> ();
		if (isOnWall) {
			rend.enabled = false;
		}
	}

	public void SetIsOnWall(bool b)
	{
		isOnWall = b;
		if (isOnWall) {
			rend.enabled = false;
		} else 
		{
			rend.enabled = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "wall") {
			collisionCount++;
				rend.enabled = true;

		}

	}

	public void ChangeMesh(Mesh newMesh)
	{
		rend.GetComponent<MeshFilter> ().sharedMesh = newMesh;
	

	}

	void Update()
	{
		if(collisionCount > 0)rend.enabled = true;
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "wall") {
			collisionCount--;

			if (collisionCount < 1 && isOnWall) 
			{
				rend.enabled = false;
			}
		}
	}
    // Update is called once per frame
    public void SetCollisionCount(int count)
    {
         collisionCount = count;
    }

    public int GetCollisionCount()
	{
		return collisionCount;
	}


	IEnumerator CheckCollisions()
	{

		yield return new WaitForSeconds (1);
	}
}
