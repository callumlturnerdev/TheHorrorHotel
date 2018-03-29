﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnfinityGames.uNote;
using buildModes;

namespace buildModes
{
    public enum eBuildMode { trigger, building, deleting, waypoints, materials };
}
/// <summary>
/// 
/// BUILD CONTROLLER IS IN CHARGE OF ALL THINGS RELATED TO BUILDING AND UPDATING BUILD RELATED HUD ELEMENTS FOR THE GAME
/// 
/// </summary>
public class BuildController : MonoBehaviour {
    
    [SerializeField]
    AudioClip rotateSound;
    AudioSource audioS;
    public Text buildModeUI;
    public GameObject CursorObj;
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
    private bool UINeedsEnabled;
    bool SnapMode = true;
    public bool paintMode = false;
    // UI elements
    Text ScreamPointsUI;
     float screamPoints = 2000;

    // Use this for initialization
    void Awake () {
        audioS = gameObject.AddComponent(typeof(AudioSource))as AudioSource;
        audioS.clip = rotateSound;
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

    void SetBuildModeUI(string t)
    {
        buildModeUI.text = t;
    }
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
            ScreamPointsUI.text = " "  + screamPoints;
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

        if(Input.GetMouseButtonDown(0) && GetCurrentBuildMode() == eBuildMode.trigger)
        {
            currentBuildMode = eBuildMode.building;
            SetBuildObject(null);
            CursorObj.GetComponent<FollowCursor>().SetHeldObject(null);
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            UINeedsEnabled = !UINeedsEnabled;
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
         currentObject = defaultBuildObject;
         CursorObj.GetComponent<FollowCursor>().SetHeldObject(null);
        inDeleteMode = !inDeleteMode;
         SetBuildModeUI("Delete Mode");
        heldObjectRef.GetComponent<WallCheck>().SetCollisionCount(0);
        if (inDeleteMode == false)
        {
            CursorObj.GetComponent<FollowCursor>().SetHeldObject(currentObject);
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

    public void TrigggerMode(bool t)
    {
        if(t)
        {
            CursorObj.GetComponent<FollowCursor>().SetHeldObject(null);
            SetBuildModeUI("Trigger Mode");
            currentBuildMode = eBuildMode.trigger;
            DebugConsole.Log("In Trigger Mode");
        }
        else
        {
             currentObject = lastBuildObject;
             CursorObj.GetComponent<FollowCursor>().SetHeldObject(currentObject);
            SetBuildObject(currentObject);
            currentBuildMode = eBuildMode.building;
        }
    }
    public void WayPointMode(bool t)
    {
        if (t)
        {
            CursorObj.GetComponent<FollowCursor>().SetHeldObject(null);
             SetBuildModeUI("Waypoint Mode");
            currentBuildMode = eBuildMode.waypoints;
        }
        else
        {
            currentObject = lastBuildObject;
            CursorObj.GetComponent<FollowCursor>().SetHeldObject(lastBuildObject);
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
		if (currentObject && currentObject.tag == "onWall") {
			
			if (heldObjectRef.GetComponent<WallCheck> ().GetCollisionCount () < 1) {
				rotZ += 90;
			} 
		} 
		if (Input.GetKeyDown (KeyCode.R)) 
        { 
            audioS.Play();
            if(rotZ >= 270)
            { rotZ = 0;}
            else
            {
            rotZ += 90; 
            } 
            rotated = !rotated; 
             CursorObj.GetComponent<FollowCursor>().SetRotation(rotZ);
        }
	}


	public void SetBuildObject(GameObject newObject)
	{
        lastBuildObject = currentObject;
         SetBuildModeUI("Build Mode");
        if (newObject == null) { currentObject = defaultBuildObject; }
		currentObject = newObject;
        CursorObj.GetComponent<FollowCursor>().SetHeldObject(currentObject);
        if (currentObject && currentObject.tag == "onWall")
        {
            currentBuildMode = eBuildMode.building;
            heldObjectRef.GetComponent<WallCheck>().SetIsOnWall(true);

            if (heldObjectRef.GetComponent<WallCheck>().GetCollisionCount() < 1)
            {
                rotZ += 90;
            }
        }
        else if (currentObject && currentObject.tag == "groundTexture")
        {
            currentBuildMode = eBuildMode.materials;
            heldObjectRef.GetComponent<WallCheck>().SetIsOnWall(false);
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

    public bool UINeedsOn()
    {return UINeedsEnabled;}

    public void AddObjectForDeletion(GameObject grid)
    {
        objectsToDelete.Add(grid);
    }


}
