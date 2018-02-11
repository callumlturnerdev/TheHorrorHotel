using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using needTypes;
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            wallsHidden = !wallsHidden;
            ToggleWalls();
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
                    boredomObjects.Remove(obj);
                    break;
                case eNeedTypes.hidden:

                    break;
                case eNeedTypes.hunger:
                    hungerObjects.Remove(obj);
                    break;
                case eNeedTypes.hygiene:
                    hygieneObjects.Remove(obj);
                    break;
                case eNeedTypes.tiredness:
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








}
