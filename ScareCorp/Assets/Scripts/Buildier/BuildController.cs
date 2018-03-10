using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnfinityGames.uNote;
using buildModes;

namespace buildModes
{
    public enum eBuildMode { none, building, deleting, waypoints, materials };
}
/// <summary>
/// 
/// BUILD CONTROLLER IS IN CHARGE OF ALL THINGS RELATED TO BUILDING AND UPDATING BUILD RELATED HUD ELEMENTS FOR THE GAME
/// 
/// </summary>
public class BuildController : MonoBehaviour {

	public static BuildController instance = null;
    private eBuildMode currentBuildMode = eBuildMode.building;
    public List<GameObject> objectsToDelete;
	public GameObject defaultBuildObject;
	public GameObject currentObject;
    private GameObject lastBuildObject;
	private int objIndex;

	// USED TO PASS ON MAT INDEX TO BUILT OBJECT
	public int materialIndex = 0;

    [Note("TEst", "other")]
    public NoteData scriptNote;
	private float rotZ = 0;
	private bool rotated = false;

    bool inDeleteMode = false;
    bool inMaterailMode = false;
	bool notOnHud = true;

	private GameObject playerCursor;
	private GameObject heldObjectRef;

    bool SnapMode = true;
    public bool paintMode = false;
    // UI elements
    Text ScreamPointsUI;
     float screamPoints = 9999;

    // Use this for initialization
    void Awake () {
        inDeleteMode = false;
		if (instance == null) {
			instance = this;
		} else if (instance != this) 
		{
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this.gameObject);
		Init ();
	}

    public float GetScreamPoints() { return screamPoints; }
    public void SetScreamPoints(float newVal) { screamPoints = newVal; UpdateHUD(); }
    public eBuildMode GetCurrentBuildMode() { return currentBuildMode; }

	void Init()
	{
        currentObject = defaultBuildObject;
		playerCursor = GameObject.FindGameObjectWithTag ("cursor");
		heldObjectRef = playerCursor.transform.GetChild (0).gameObject;
       // EventManager.DeleteClicked += DeleteMode;
        HUDSetup();
    }

    void HUDSetup()
    {
        if (GameObject.FindGameObjectWithTag("ScarePoints") != null)
        {
            ScreamPointsUI = GameObject.FindGameObjectWithTag("ScarePoints").GetComponent<Text>();
            ScreamPointsUI.text = " "  + screamPoints + " SP";
        }
    }

    void UpdateHUD()
    {
        if (ScreamPointsUI)
        {
            ScreamPointsUI.text = " " + screamPoints ;
        }
    }

    public void AddPoints(float cost)
    {
        screamPoints += cost;
        UpdateHUD();
    }

    public bool CheckCost(float cost)
    {
        if (screamPoints >= cost)
        {
            screamPoints -= cost;
            UpdateHUD();
            return true;
        }
        return false;
    }

	// Update is called once per frame
	void Update () {

		SwapObject ();
        if(Input.GetKeyDown(KeyCode.CapsLock))
        {
            paintMode = !paintMode;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            screamPoints = 0;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            screamPoints += 100;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SnapMode = !SnapMode;
        }
       
    }

    public bool GetSnapMode() { return SnapMode; }
 

	public bool NotOnHud{ get; set;}

	public bool GetNotOnHud()
	{
		return notOnHud;
	}
	public void SetNotOnHud(bool t)
	{
		notOnHud = t;
	}

    public void DeleteMode()
    {
       
        inDeleteMode = !inDeleteMode;
        heldObjectRef.GetComponent<WallCheck>().SetCollisionCount(0);

        if (inDeleteMode == false)
        {
            currentBuildMode = eBuildMode.building;
            if (objectsToDelete.Count > 0)
            {
                foreach (GameObject Obj in objectsToDelete)
                {
                    Obj.GetComponent<BuildOnGrid>().SetCanBuildOnGrid(true);
                    Destroy(Obj.transform.GetChild(0).gameObject);
                }

                objectsToDelete.Clear();
            }
        }
        else
        {
            currentBuildMode = eBuildMode.deleting;
        }
    }

    public void WayPointMode(bool t)
    {
        if (t)
        {
            currentBuildMode = eBuildMode.waypoints;
        }
        else
        {
            currentObject = lastBuildObject;
            SetBuildObject(currentObject);
            currentBuildMode = eBuildMode.building;
           
        }
    }
    public bool GetInDeleteMode(){return inDeleteMode; }
    public bool GetInMaterialMode() { return inMaterailMode; }

    public Material GetCurrentObjectMaterial()
    {
        if (inMaterailMode)
        {
            return currentObject.GetComponent<Renderer>().sharedMaterial;
        }
        else
        {
            return null;
        }
    }

	void SwapObject()
	{
		if (currentObject.tag == "onWall") {
			
			if (heldObjectRef.GetComponent<WallCheck> ().GetCollisionCount () < 1) {
				rotZ += 90;
			} 
		} 
		if (Input.GetKeyDown (KeyCode.R)) 
        { 
            if(rotZ >= 270)
            { rotZ = 0;}
            else
            {
            rotZ += 90; 
            } 
            rotated = !rotated; 
        }
	}


	public void SetBuildObject(GameObject newObject)
	{
        lastBuildObject = currentObject;
        if (newObject == null) { currentObject = defaultBuildObject; }
		currentObject = newObject;
        if (currentObject.tag == "onWall")
        {
            currentBuildMode = eBuildMode.building;
            heldObjectRef.GetComponent<WallCheck>().SetIsOnWall(true);

            if (heldObjectRef.GetComponent<WallCheck>().GetCollisionCount() < 1)
            {
                rotZ += 90;
            }
        }
        else if (currentObject.tag == "groundTexture")
        {
            currentBuildMode = eBuildMode.materials;
            inMaterailMode = true;
        }
        else
        {
            inMaterailMode = false;
            currentBuildMode = eBuildMode.building;
            heldObjectRef.GetComponent<WallCheck>().SetIsOnWall(false);
        }
	}

	public float GetRotation()
	{
		return rotZ;
	}

	public  bool GetRotated()
	{
		return rotated;
	}


	public GameObject GetCurrentTempObject() // Returns the object that is currently ready to be built.
	{
		return heldObjectRef;
	}

	public GameObject GetCurrentObject()
	{
		return currentObject;
	}


    public void AddObjectForDeletion(GameObject grid)
    {
        objectsToDelete.Add(grid);
    }
}
