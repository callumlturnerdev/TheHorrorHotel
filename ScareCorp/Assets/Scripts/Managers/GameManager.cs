using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using needTypes;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    public List<Transform> wayPointList;
    public static GameManager instance = null;

    public delegate void ClickAction();
    public static event ClickAction ObjectAdd;
    public static event ClickAction DayChanged;

    [Header("NeedObjects")]
    public List<GameObject> hungerObjects;
    public List<GameObject> tirednessObjects;
    public List<GameObject> boredomObjects;
    public List<GameObject> hygieneObjects;
    public List<GameObject> hidingPlaces;
    public static event ClickAction ToggleTopWalls;

    private bool wallsHidden = false;
    public bool GetWallsHidden() { return wallsHidden; }

    // Player Stats for saving
    public float screamPoints;
    public float time;
    public float day;
    [Header("UI")]
    public Text bedCountUI;
    int maxBeds;
    int takenBeds;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        ObjectsAddedEvent();
        screamPoints = 2000;
        //UI stuff
        maxBeds =0;
        takenBeds = 0;
        UpdateBedCountUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            wallsHidden = !wallsHidden;
            ToggleWalls();
        }
    }
    public GameObject GetABed() // USed by visitor to assign bed
    {
        GameObject bed = null;
        foreach(GameObject obj in tirednessObjects)
        {
            if(obj != null)
            {
                bed = obj;
                break;
            }
        }
        tirednessObjects.Remove(bed);
         UpdateBedCountUI();
        return bed;
    }
    
    public void AddBed(GameObject bed)
    {
        if(!tirednessObjects.Contains(bed))
        {
            tirednessObjects.Add(bed);
            
        }
    }
    public void ClearObjects()
    {
        boredomObjects.Clear();
        hungerObjects.Clear();
        hygieneObjects.Clear();
        tirednessObjects.Clear();
    }

    public void AddObject(GameObject obj)
    { 
        if (obj.GetComponent<Buildable>())
        {
            switch (obj.GetComponent<Buildable>().needtype)
            {
                case eNeedTypes.boredom:
                    boredomObjects.Add(obj);
                    break;
                case eNeedTypes.hidden:
                    
                    break;
                case eNeedTypes.hunger:
                    hungerObjects.Add(obj);
                    break;
                case eNeedTypes.hygiene:
                    hygieneObjects.Add(obj);
                    break;
                case eNeedTypes.tiredness:
                    tirednessObjects.Add(obj);
                    break;
                case eNeedTypes.none:
                    break;
                default:
                    break;
            }
            UpdateBedCountUI();
        }
        StartCoroutine(WaitforSeconds(1));
    }

    public void RemoveObject(GameObject obj)
    {
        if (obj.GetComponent<Buildable>())
        {
            switch (obj.GetComponent<Buildable>().needtype)
            {
                case eNeedTypes.boredom:
                    if(boredomObjects.Contains(obj))
                        boredomObjects.Remove(obj);
                    break;
                case eNeedTypes.hidden:

                    break;
                case eNeedTypes.hunger:
                    if(hungerObjects.Contains(obj))
                        hungerObjects.Remove(obj);
                    break;
                case eNeedTypes.hygiene:
                    if(hygieneObjects.Contains(obj))
                        hygieneObjects.Remove(obj);
                    break;
                case eNeedTypes.tiredness:
                    if(tirednessObjects.Contains(obj))
                        tirednessObjects.Remove(obj);
                    break;
                case eNeedTypes.none:
                    break;
                default:
                    break;
            }
        }
        StartCoroutine(WaitforSeconds(1));
    }


    IEnumerator WaitforSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        ObjectsAddedEvent();
    }

    public void ToggleWalls()
    {
        if (ToggleTopWalls != null)
        {
            ToggleTopWalls();
        }
    }
    public bool FreeBedCheck()
    {
        if ((maxBeds - takenBeds) > 0)
        {
            return true;
        }
        return false;
        
    }
    public void AddToBedCount(int n)
    {
        maxBeds += n;
        UpdateBedCountUI();
    }
    public void AddToTakenBeds(int n)
    {
        takenBeds += n;
        UpdateBedCountUI();
    }
    void ObjectsAddedEvent()
    {
        if (ObjectAdd != null)
        {
            ObjectAdd();
        }
    }

    public void AddToWayPointList(Transform trans)
    {
        wayPointList.Add(trans);
    }

    public List<Transform> GetWayPoints()
    {
        if (wayPointList != null)
        {
            return wayPointList;
        }
        else
        {
            return null;
        }
    }

    public void UpdateBedCountUI()
    {
       
        DebugConsole.Log(tirednessObjects.Count.ToString());
       // maxBeds = tirednessObjects.Count;
        bedCountUI.text = "Beds: " + takenBeds + "/" + maxBeds + "";
    }






}
