using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class SavingLoading : MonoBehaviour
{
    [SerializeField]
    private List<Text> slotNames;
    private float scarepoints;
    private float currentTime;
    private float currentDay;
    public static SavingLoading saveLoad;
    public GridBuilder gridBuilderRef;

    public List<GameObject> currentVisitors;
    [SerializeField]
    public GameObject GridContainer;
    
     List<GameObject> grids;
    private ObjectFinder objFind;

    //string path = Application.dataPath + "/Saves";

    public GameObject visitorGameobject;
    private void Awake()
    {
       
       


        currentVisitors = new List<GameObject>();
        if (saveLoad == null)
        {
            DontDestroyOnLoad(gameObject);
            saveLoad = this;
        }
        else if (saveLoad != this)
        {
            Destroy(gameObject);
        }
        objFind = gameObject.GetComponent<ObjectFinder>();
        gridBuilderRef = GameObject.FindGameObjectWithTag("gridbuilder").GetComponent<GridBuilder>();
        
       // grids = gridBuilderRef.grids;
        if (!Directory.Exists(Application.dataPath + "/Saves/"))
          {
            Directory.CreateDirectory(Application.dataPath + "/Saves/");
          }
    }

    public void AddGrid(List<GameObject> _grid)
    {
        grids = _grid;
    }  
    public void SaveVisitor(GameObject vis)
    {
        if (vis == null)
        {
            
        }
        currentVisitors.Add(vis);
    }


    public void DeleteVisitorSave(GameObject vis)
    {
        currentVisitors.Remove(vis);
    }

    public bool validFilepath(string s)
    {
        if (File.Exists(Application.dataPath + "/Saves/" + s))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Load(string filepath)
    {
         grids = gridBuilderRef.GetGrid();
      
        if (File.Exists(Application.dataPath + "/Saves/" + filepath))
        {
            GameManager.instance.ClearObjects();
            gridBuilderRef.ResetGrid();
            //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Saves/" + filepath, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            BuildController.instance.SetScreamPoints(data.scarepoints);
            TimeManager.instance.SetCurrentDay(data.day);
            TimeManager.instance.SetCurrentTime(data.time);

            // currentDay = data.day;
            // currentTime = data.time;
            for (int i = 0; i < grids.Count; i++)
            {
              
                if (grids[i].GetComponent<BuildOnGrid>())
                {
                   // grids[i].GetComponent<BuildOnGrid>().LoadMaterial(data.matIDs[i]);
                    grids[i].GetComponent<BuildOnGrid>().LoadObject(objFind.FindObjectBasedOnID(data.objectIDs[i]), data.objectsRots[i]);
                    grids[i].GetComponent<BuildOnGrid>().SetMaterial(GetComponent<ObjectFinder>().groundMats[data.matIDs[i]]);
                }
            }
            VisitorLoadData(data);
        }
    }



    public void Save(string filepath)
    {
         grids = gridBuilderRef.GetGrid();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/Saves/" + filepath);
        PlayerData data = new PlayerData();
        data.scarepoints = BuildController.instance.GetScreamPoints();
        data.time = TimeManager.instance.GetCurrentTime();
        data.day = TimeManager.instance.GetCurrentDay();

        for (int i = 0; i < grids.Count; i++)
        {

            data.matIDs[i] = GetComponent<ObjectFinder>().GetMaterialIndexInArray(grids[i].GetComponent<BuildOnGrid>().GetMaterial()); // NEW MATERIAL LOADER --------------------------------------------
 
            if (grids[i].GetComponent<BuildOnGrid>())
            {
                //data.matIDs[i] = grids[i].GetComponent<BuildOnGrid>().GetMatID();
                if (grids[i].GetComponent<BuildOnGrid>().GetCurrentlyBuiltObject() != null)
                {
                    GameObject obj = grids[i].GetComponent<BuildOnGrid>().GetCurrentlyBuiltObject();
                    data.objectsRots[i] = grids[i].GetComponent<BuildOnGrid>().GetCurrentlyBuiltObjectRotationZ();
                    if (obj.GetComponent<Buildable>())
                    {
                        data.objectIDs[i] = (obj.GetComponent<Buildable>().objectID);
                    }
                    else
                    {
                        data.objectIDs[i] = -1;
                    }
                }
                else
                {
                    data.objectIDs[i] = -1;
                }
            }
        }
        VisitorDataSave(data);
        bf.Serialize(file, data);
        file.Close();
    }

    void VisitorLoadData(PlayerData data)
    {
        currentVisitors.Clear();
        if (data.visitorCount > 0)
        {
            for (int i = 0; i < data.visitorCount; i++)
            {
                GameObject _vis = Instantiate(visitorGameobject) as GameObject;
                GameManager.instance.AddToTakenBeds(1);
                SaveVisitor(_vis);
                AINeeds _visAI = _vis.GetComponent<AINeeds>();
                Visitor _visScript = _vis.GetComponent<Visitor>();
                AI _AI = _vis.GetComponent<AI>();
                _vis.transform.position =  new Vector3( data.visitorPosX[i], 0, data.visitorPosY[i]);
                _visAI.SetHunger(data.visitorHunger[i]);
                _visAI.SetTiredness(data.visitorTiredness[i]);
                _visAI.SetBoredom(data.visitorBoredom[i]);
                _visAI.SetHygiene(data.visitorHygiene[i]);
                _visScript.SetCurrentFear(data.visitorCurrentFear[i]);
                _AI.GetABed();
                _vis.SetActive(true);

            }
        }
    }

    void VisitorDataSave(PlayerData data)
    {
        data.visitorCount = currentVisitors.Count;
        if (currentVisitors.Count > 0)
        {
            for (int i = 0; i < currentVisitors.Count; i++)
            {
                if (currentVisitors[i].GetComponent<Visitor>())
                {
                    Visitor _visitor = currentVisitors[i].GetComponent<Visitor>();
                    AINeeds _visitorAI = currentVisitors[i].GetComponent<AINeeds>();
                    data.visitorPosX[i] = _visitor.gameObject.transform.position.x;
                    data.visitorPosY[i] = _visitor.gameObject.transform.position.z;
                    data.visitorHunger[i] = _visitorAI.GetHunger();
                    data.visitorTiredness[i] = _visitorAI.GetTiredness();
                    data.visitorBoredom[i] = _visitorAI.GetBoredom();
                    data.visitorHygiene[i] = _visitorAI.GetHygiene();
                    data.visitorCurrentFear[i] = _visitor.GetCurrentFear();
                }
            }
        }
    }


    void ClearVisitorData(PlayerData data)
    {
       // Array.Clear(data.visitorPos, 0, data.visitorPos.Length);
        Array.Clear(data.visitorHunger, 0, data.visitorHunger.Length);
        Array.Clear(data.visitorTiredness, 0, data.visitorTiredness.Length);
        Array.Clear(data.visitorBoredom, 0, data.visitorBoredom.Length);
        Array.Clear(data.visitorHygiene, 0, data.visitorHygiene.Length);
        Array.Clear(data.visitorCurrentFear, 0, data.visitorCurrentFear.Length);
    }


    [Serializable]
    class PlayerData
    {


        public float[] objectsRots = new float[1300];
        public int[] objectIDs = new int[1300];
        public int[] matIDs =  new int[1300];
        public float scarepoints;
        public float time;
        public float day;

        public int visitorCount = 0;
        public float[] visitorPosX = new float[200];
        public float[] visitorPosY = new float[200];
        public float[] visitorHunger = new float[200];
        public float[] visitorTiredness = new float[200];
        public float[] visitorBoredom = new float[200];
        public float[] visitorHygiene = new float[200];
        public float[] visitorCurrentFear = new float[200];
        //
    }
}
