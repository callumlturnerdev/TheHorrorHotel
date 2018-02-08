using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildOnGrid : MonoBehaviour {

	public GameObject tempToBuild;
	public Material highlightText;
	private Material originalMat;
	Renderer myRend;
	bool canBuildOn = false;
	bool beenBuiltOn = false;
	public int gridX,gridY;
	bool mouseIsDown = false;

	bool isHighlighted = false;
	// Use this for initialization
	void Start () {
		myRend = GetComponent<Renderer> ();
		originalMat = myRend.material;
	}
	
	// Update is called once per frame
	void Update () {

		if (isHighlighted) {
			canBuildOn = true;
		}

		tempToBuild = BuildController.instance.GetCurrentObject ();
		if (BuildController.instance.GetCurrentTempObject ().GetComponent<WallCheck> ().GetCollisionCount() > 0 
		   || tempToBuild.tag != "onWall") {
			if (Input.GetMouseButtonUp (0) && canBuildOn && BuildController.instance.NotOnHud == true) {
				tempToBuild = BuildController.instance.GetCurrentObject ();

                if (BuildController.instance.CheckCost(tempToBuild.GetComponent<Buildable>().GetCost()))
                {
                    GameObject obj = Instantiate(tempToBuild) as GameObject;







                    obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
                    canBuildOn = false;
                    beenBuiltOn = true;
                    isHighlighted = false;
                    if (BuildController.instance.GetRotated())
                    {
                        obj.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    obj.transform.rotation = Quaternion.Euler(0, BuildController.instance.GetRotation(), 0);
                }
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			mouseIsDown = true;
		}
		if (Input.GetMouseButtonUp (0)) 
		{
			
			mouseIsDown = false;
			myRend.material = originalMat;
		}

	}

	public void HighlightGird(bool a)
	{
		if (BuildController.instance.NotOnHud == true) {
			if (!beenBuiltOn) {
				if (a) {
					myRend.material = highlightText;
					isHighlighted = true;
				} else {
					myRend.material = originalMat;
					isHighlighted = false;
				}
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "cursor" && beenBuiltOn == false)
		{
			canBuildOn = true;
		}



		if (other.tag == "cursor" && mouseIsDown) 
		{
			
		}

	}


	void OnTriggerExit(Collider other)
	{
		if (other.tag == "cursor") 
		{
			canBuildOn = false;
		}
	}



	public void SetGridPos(int x, int y)
	{
		gridX = x;
		gridY = y;

	}

	public int[] GetGridPos()
	{
		int[] gridArray = new int[2];
		gridArray[0] = gridX;
		gridArray[1] = gridY;
		return gridArray;
	}



}
