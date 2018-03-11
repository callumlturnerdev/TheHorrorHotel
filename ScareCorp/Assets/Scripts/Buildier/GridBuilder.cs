using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// 
/// INITS AT START OF THE GAME AND IS USED TO SETUP THE BUILDABLE GRID FOR THE PLAYER.
/// 
/// 
/// 
/// </summary>
[ExecuteInEditMode]
public class GridBuilder : MonoBehaviour {


    public List<GameObject> currentVisitors;
	public GameObject cube;
	public int XgridSize;
	public int YgridSize;
    private SavingLoading saveSys;
	public Material[] mats;
	private Renderer rend;

	private int matIndex = 0;
	private float xPos,zPos;
	private int xNum,zNum;
    bool hasBeenBuilt = false;

    public List<GameObject> grids;
	// Use this for initialization
	void Start () {
        saveSys = GameObject.FindGameObjectWithTag("save").GetComponent<SavingLoading>();
		xPos = this.transform.position.x;
		zPos = this.transform.position.z;

		xNum = 0;
		zNum = 0;
        if(grids.Count < 1)
        {
		CreateGrid ();
        }
        currentVisitors = new List<GameObject>();
	}


    public void AddVisitor(GameObject visitor)
    {
        currentVisitors.Add(visitor);
    }

    public void RemoveVisitor(GameObject visitor)
    {
        currentVisitors.Remove(visitor);
    }

    private void DestroyAllVisitors()
    {
        if (currentVisitors.Count > 0)
        {
            foreach (GameObject vis in currentVisitors)
            {
                Destroy(vis);
            }
            currentVisitors.Clear();
        }
    }

	void SetMaterial()
	{
		if (matIndex < 1) {
			matIndex++;
		} 
		else 
		{
			matIndex = 0;
		}
	}

   public  void ResetGrid()
    {
        foreach(GameObject grid in grids)
        {
            if (grid.GetComponent<BuildOnGrid>())
            {
                if (grid.GetComponent<BuildOnGrid>().GetCurrentlyBuiltObject() != null)
                {
                    Destroy(grid.GetComponent<BuildOnGrid>().GetCurrentlyBuiltObject());
                    grid.GetComponent<BuildOnGrid>().SetCanBuildOnGrid(true);
                    grid.transform.SetParent(this.transform);
                }
            }

        }
        DestroyAllVisitors();
    }


	void CreateGrid()
	{
        if (!hasBeenBuilt)
        {
            for (int i = 0; i < XgridSize; i++)
            {

                for (int y = 0; y < YgridSize; y++)
                {
                    GameObject grid = Instantiate(cube) as GameObject;
                    grids.Add(grid);
                    grid.transform.position = new Vector3(xPos, 0, zPos);
                    SetMaterial();
                    grid.GetComponent<Renderer>().material = mats[matIndex];
                    grid.GetComponent<BuildOnGrid>().SetGridPos(xNum, zNum);
                    grid.name = "" + xNum + " " + zNum;
                    zPos += 2.0f;
                    zNum++;
                }
                xNum++;
                zNum = 0;
                SetMaterial();
                zPos = this.transform.position.z;
                xPos += 2.0f;
            }
            hasBeenBuilt = true;
            saveSys.AddGrid(grids);
        }  
	}
}
