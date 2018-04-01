using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using buildModes;


public class BuildOnGrid : MonoBehaviour {

    [SerializeField]
    GameObject[] gridNeighbours;
    private int neighbourInd;
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

    AudioSource audioS;
    [SerializeField]
    AudioClip[] clips;
	// Use this for initialization

    //Deals with multigrid objects
    GameObject linkedGrid;

    int materialID;
	void Start () {
        materialID = 0;
        neighbourInd = 0;
        gridNeighbours = new GameObject[4];
		myRend = GetComponent<Renderer> ();
		originalMat = myRend.material;
        builtObjectRotationZ = 0;
        audioS = GetComponent<AudioSource>();
        switch (BuildController.instance.GetCurrentBuildMode())
        {
            case eBuildMode.building:
                break;
            case eBuildMode.deleting:
                break;
            case eBuildMode.trigger:
                break;
            case eBuildMode.waypoints:
                break;
        }
        FindConnectedGrids();
	}

    void FindConnectedGrids()  // used to find neighbours used for big object placement
    {
        // this function will find the grids connected to this one.
        gridNeighbours[0] = GameObject.Find("" + (gridX-1) + " " + gridY);
        gridNeighbours[1] = GameObject.Find("" + (gridX+1) + " " + gridY);
        gridNeighbours[2] = GameObject.Find("" + gridX + " " + (gridY+1));
        gridNeighbours[3] = GameObject.Find("" + gridX + " " + (gridY-1));
    }
    // Update is called once per frame
    void Update()
    {
        SelectNeighbourBasedOnRotation();
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

                                case eBuildMode.trigger:
                                    Triggers();
                                break;

                                case eBuildMode.deleting:
                                    Deleting();
                                break;

                                case eBuildMode.waypoints:
                                    WayPoints();
                                break;

                                case eBuildMode.materials:
                                    Materials(null);
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
                    if(tempToBuild.GetComponent<Buildable>().IsLargeObject())
                    {
                        
                        if(gridNeighbours[neighbourInd])
                        {
                            if(gridNeighbours[neighbourInd].GetComponent<BuildOnGrid>().canBuildOn || !gridNeighbours[neighbourInd].GetComponent<BuildOnGrid>().beenBuiltOn )
                            {
                                linkedGrid = gridNeighbours[neighbourInd];
                                linkedGrid.GetComponent<BuildOnGrid>().canBuildOn = false;
                                linkedGrid.GetComponent<BuildOnGrid>().beenBuiltOn = true;
                                BuildObject(tempToBuild);      
                            }
                        }
                    }
                    else
                    {
                        BuildObject(tempToBuild);  
                    }
                }

            }
        }
    }
    private void BuildObject(GameObject tempToBuild)
    {
        GameObject obj = Instantiate(tempToBuild) as GameObject;
                    ObjectBuiltOnGrid = obj;
                    obj.transform.parent = this.transform;
                    obj.GetComponent<Buildable>().ActivateGravity();
                    audioS.clip = clips[1];
                    audioS.Play();
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

    private void Triggers()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {

            
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

    public Material GetMaterial()
    {
        return myRend.sharedMaterial;
    }
    public void SetMaterial(Material _mat)
    {
        originalMat = _mat;
        myRend.material = _mat;
    }

    public void Materials(GameObject calledGrid)
    {
        originalMat = BuildController.instance.GetCurrentObjectMaterial();
       
        myRend.material = originalMat;
        isHighlighted = false;
        foreach(GameObject gridN in gridNeighbours)
        {
            if(gridN)
            {
                if(gridN != calledGrid)
                {
                if(gridN.GetComponent<BuildOnGrid>() != null)
                { 
                    if(gridN.GetComponent<BuildOnGrid>().originalMat != BuildController.instance.GetCurrentObjectMaterial())
                    {
                        if(gridN.transform.childCount < 1 || ((gridN.transform.GetChild(0).tag != "wall") && (gridN.transform.GetChild(0).tag != "Door")))
                        {
                             gridN.GetComponent<BuildOnGrid>().Materials(this.gameObject);
                        }
                        else
                        {
                            gridN.GetComponent<MeshRenderer>().material = originalMat;
                        }
                    }
                }
                }
            }
        }
    }
    public void LoadObject(GameObject objToLoad, float Zrotation)
    {
        if (objToLoad != null)
        {
            GameObject obj = Instantiate(objToLoad) as GameObject;
            ObjectBuiltOnGrid = obj;
            obj.transform.parent = this.transform;
            GameManager.instance.AddObject(this.gameObject);
            obj.GetComponent<Buildable>().ActivateGravity();
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
        if(beenBuiltOn == false || BuildController.instance.GetCurrentBuildMode() != eBuildMode.building)
        {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            
            if (a)
            {
                
                if(tempToBuild.GetComponent<Buildable>().IsLargeObject() && gridNeighbours[neighbourInd])
                {
                gridNeighbours[neighbourInd].GetComponent<Renderer>().material = highlightText;
                }
                else if(BuildController.instance.paintMode)
                {
                 foreach(GameObject neighbour in gridNeighbours)
                    {
                        if(neighbour)
                        {
                            neighbour.GetComponent<Renderer>().material = highlightText;
                            neighbour.GetComponent<BuildOnGrid>().isHighlighted = true;
                        }
                    }
                }
                    myRend.material = highlightText;
                    if(!isHighlighted)
                    {
                        audioS.clip = clips[0];
                        audioS.Play();
                    }
                    isHighlighted = true;
                   
            }
				}
            else {
					myRend.material = originalMat;
					isHighlighted = false;
				}
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
        if(linkedGrid != null)
        {
            linkedGrid.GetComponent<BuildOnGrid>().SetCanBuildOnGrid(true);
            linkedGrid = null;
        }
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



    void  SelectNeighbourBasedOnRotation() // Gets build rotation and uses this to determine what neighbour to also build on for larger objects
    {
        float rot = BuildController.instance.GetRotation();
        
        if(rot == 0)
        {
            neighbourInd = 0; 
        }
        if(rot == 90)
        {
            neighbourInd = 2; 
        }
        if(rot == 180)
        {
            neighbourInd = 1; 
        }
        if(rot == 270)
        {
            neighbourInd = 3; 
        }
        
    }
}