using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using buildModes;
/// <summary>
/// 
/// 
/// THE SCRIPT THAT DETERMINES HOW BUILDING ON A GRID WORK AND HOW THE HIGHLIGHT SYSTEM WORKS FOR BUILDING ON THE TILES.
/// 
/// 
/// 
/// </summary>
public class BuildOnGrid : MonoBehaviour {

    
     GameObject ObjectBuiltOnGrid;
    private float builtObjectRotationZ;
	public GameObject tempToBuild;
	public Material highlightText;
	private Material originalMat;
	Renderer myRend;
	bool canBuildOn = false;
	bool beenBuiltOn = false;
	public int gridX,gridY;
	bool mouseIsDown = false;
    // The below Material is temp way of showing object thats going to be destroyed
    public Material destroyMat;
	bool isHighlighted = false;
	// Use this for initialization


	void Start () {
		myRend = GetComponent<Renderer> ();
		originalMat = myRend.material;
        builtObjectRotationZ = 0;
        switch (BuildController.instance.GetCurrentBuildMode())
        {
            case eBuildMode.building:
                break;
            case eBuildMode.deleting:
                break;

            case eBuildMode.none:
                break;
            case eBuildMode.waypoints:
                break;

        }
	}
    // Update is called once per frame
    void Update()
    {
        if (isHighlighted)
        {
            canBuildOn = true;
        }
        if (BuildController.instance.GetCurrentObject())
        {
            tempToBuild = BuildController.instance.GetCurrentObject();
            if (BuildController.instance.GetCurrentTempObject().GetComponent<WallCheck>().GetCollisionCount() > 0
               || tempToBuild.tag != "onWall")
            {
                if (isHighlighted)
                {
                        if (Input.GetMouseButtonUp(0))
                        {
                            switch (BuildController.instance.GetCurrentBuildMode())
                            {

                                case eBuildMode.building:
                                 Building();
                                break;

                                case eBuildMode.deleting:
                                    Deleting();
                                break;

                                case eBuildMode.waypoints:
                                    WayPoints();
                                break;

                                case eBuildMode.materials:
                                    Materials();
                                    break;
                        }
                        }
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                mouseIsDown = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseIsDown = false;
            myRend.material = originalMat;
            isHighlighted = false;
        }

    }
    private void Building()
    {
       if (canBuildOn && !beenBuiltOn)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                tempToBuild = BuildController.instance.GetCurrentObject();

                if (BuildController.instance.CheckCost(tempToBuild.GetComponent<Buildable>().GetCost()))
                {
                    GameObject obj = Instantiate(tempToBuild) as GameObject;
                    ObjectBuiltOnGrid = obj;
                    obj.transform.parent = this.transform;
                    GameManager.instance.AddObject(this.gameObject);
                    obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
                    //canBuildOn = false;
                    beenBuiltOn = true;
                    isHighlighted = false;
                    canBuildOn = false;
                    if (BuildController.instance.GetRotated())
                    {
                        obj.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    obj.transform.rotation = Quaternion.Euler(0, BuildController.instance.GetRotation(), 0);
                    builtObjectRotationZ = BuildController.instance.GetRotation();
                }

            }
        }
    }

    private void Deleting()
    {
        if (transform.childCount > 0 && BuildController.instance.GetInDeleteMode()) // IF IN DELETE MODE
        {
            GameObject childObj = this.transform.GetChild(0).gameObject;
            BuildController.instance.AddObjectForDeletion(this.gameObject);
            foreach (Transform child in childObj.transform)
            {
                if (child.transform.childCount > 0)
                {
                    for (int i = 0; i < child.transform.childCount; i++)
                    {
                        if (child.transform.GetChild(i).GetComponent<MeshRenderer>())
                            child.transform.GetChild(i).GetComponent<MeshRenderer>().material = destroyMat;
                    }
                }
                if (child.gameObject.GetComponent<MeshRenderer>())
                {
                    child.gameObject.GetComponent<MeshRenderer>().material = destroyMat;
                }
            }
            myRend.material = highlightText;
            isHighlighted = true;
        }
    }

    private void WayPoints()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            
            if (transform.childCount > 0 && transform.GetChild(0).GetComponent<Waypoint>())
            {
                transform.GetChild(0).GetComponent<Waypoint>().DestroyWaypoint();
            }
            else
            {
                tempToBuild = BuildController.instance.GetCurrentObject();
                GameObject obj = Instantiate(tempToBuild) as GameObject;
                ObjectBuiltOnGrid = obj;
                obj.transform.parent = this.transform;
                GameManager.instance.AddObject(this.gameObject);
                obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);

                beenBuiltOn = true;
                isHighlighted = false;
                canBuildOn = false;
                if (BuildController.instance.GetRotated())
                {
                    obj.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                obj.transform.rotation = Quaternion.Euler(0, BuildController.instance.GetRotation(), 0);
                builtObjectRotationZ = BuildController.instance.GetRotation();
            }
        }
    }
    private void Materials()
    {
        originalMat = BuildController.instance.GetCurrentObjectMaterial();
        myRend.material = originalMat;
        isHighlighted = false;
    }

    public void LoadObject(GameObject objToLoad, float Zrotation)
    {
        if (objToLoad != null)
        {
            GameObject obj = Instantiate(objToLoad) as GameObject;
            ObjectBuiltOnGrid = obj;
            obj.transform.parent = this.transform;
            GameManager.instance.AddObject(this.gameObject);
            obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
            obj.transform.rotation = Quaternion.Euler(0, Zrotation, 0);
            //canBuildOn = false;
            beenBuiltOn = true;
            isHighlighted = false;
            canBuildOn = false;
            // STILL NEEDS TO KEEP ROTATION TO WORK CORRECTLY
        }
    }


   
	public void HighlightGird(bool a)  // USED FOR DRAG SELECTION AND ADDING OBJECTS FOR DELETION IN DELETE MODE
	{
        isHighlighted = false;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (a)
            {
                    myRend.material = highlightText;
                    isHighlighted = true;
            }
				}
            else {
					myRend.material = originalMat;
					isHighlighted = false;
				}
	}

    public GameObject GetCurrentlyBuiltObject() { return ObjectBuiltOnGrid; }

    public float GetCurrentlyBuiltObjectRotationZ() { return builtObjectRotationZ; }

    public void SetCanBuildOnGrid(bool b) // Needs a name change as is used to perform tile delete object
    {
        canBuildOn = b;
        isHighlighted = !b;
        beenBuiltOn = !b;
        ObjectBuiltOnGrid = null;
    }


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "cursor" )
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
