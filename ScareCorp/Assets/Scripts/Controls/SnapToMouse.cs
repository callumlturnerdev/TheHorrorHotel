using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buildModes;
public class SnapToMouse : MonoBehaviour
{

    private LineRenderer line;
    public Material[] lineRendMats;
    Ray cameraRay;
    RaycastHit cameraRayHit;
    public GameObject boxType;
    bool mouseIsDown = false;
    int startX, startY, endX, endY, currentX, currentY;
    int maxX, maxY, minX, minY;
    public Transform closestGrid;
    public bool SelectSystem = false;
    bool deleteModeOn = false;
    public GameObject currentObjectOnCursor;

    bool snapX = false;
    bool currentlySelecting = false;
    // Use this for initialization
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        EventManager.DeleteClicked += DeleteMode;
        line = GetComponent<LineRenderer>();
    }

    void DeleteMode()
    {
      
       
    }

   // public void SetCurrentObjectOnCursor(GameObject )

    void OnTriggerStay(Collider other)
    {
        if (boxType.GetComponent<WallCheck>().GetCollisionCount() > 0 || boxType.GetComponent<WallCheck>().isOnWall == false)
        {
            DragSelection(other, true);
        }
        else if (other.tag == "grid")
        {
            closestGrid = other.transform;
            transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(BuildController.instance.GetCurrentBuildMode() == eBuildMode.deleting)
        {
            line.material = lineRendMats[1];
        }
        else if(BuildController.instance.GetCurrentBuildMode() == eBuildMode.waypoints)
        {
            line.material = lineRendMats[2];
        }
        else if(BuildController.instance.GetCurrentBuildMode() == eBuildMode.trigger)
        {
            line.material = lineRendMats[3];
        }
        else
        {
            line.material = lineRendMats[0];
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SelectSystem = !SelectSystem;
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseIsDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseIsDown = false;
        }
        boxType.transform.rotation = Quaternion.Euler(0, BuildController.instance.GetRotation(), 0);
        //boxType.GetComponent<WallCheck>().ChangeMesh(BuildController.instance.GetCurrentObject().GetComponent<MeshFilter>().sharedMesh);
        //boxType.transform.localScale = BuildController.instance.GetCurrentObject().transform.localScale;
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer_mask;
        layer_mask = LayerMask.GetMask("Grid", "Builder");
        if (Physics.Raycast(cameraRay, out cameraRayHit, Mathf.Infinity, layer_mask))
        {
            Vector3 targetPos;
            if (currentlySelecting)
            {
                
            }
            if (BuildController.instance.GetSnapMode())
            {
                targetPos = new Vector3(cameraRayHit.transform.position.x, transform.position.y, cameraRayHit.transform.position.z);
            }
            else
            {
                targetPos = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
            }
            transform.position = targetPos;
        }
    }

    void DragSelection(Collider other, bool enabled)
    {
        if (enabled)
        {
            if (other.GetComponent<BuildOnGrid>())
            {
                if (Input.GetMouseButtonDown(0) && other.tag == "grid")
                {
                    startX = other.GetComponent<BuildOnGrid>().GetGridPos()[0];
                    startY = other.GetComponent<BuildOnGrid>().GetGridPos()[1];
                    other.GetComponent<BuildOnGrid>().HighlightGird(true);
                }

                if (other.tag == "grid" && mouseIsDown)
                {
                    if (other.GetComponent<BuildOnGrid>())
                    {
                        currentlySelecting = true;
                        currentX = other.GetComponent<BuildOnGrid>().GetGridPos()[0];
                        currentY = other.GetComponent<BuildOnGrid>().GetGridPos()[1];
                        //  Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, currentY);
                        //   transform.position = pos;
                        if (currentX == startX || currentY == startY)
                        {
                            other.GetComponent<BuildOnGrid>().HighlightGird(true);
                        }
                    }
                }
                else
                {
                    currentlySelecting = false;
                }
            }

        }
    }

    public void HighlightGrid()
    {
        GameObject[] grid = GameObject.FindGameObjectsWithTag("grid");
        foreach (GameObject cell in grid)
        {
            if (cell.GetComponent<BuildOnGrid>())
            {
                if ((cell.GetComponent<BuildOnGrid>().gridX >= minX && cell.GetComponent<BuildOnGrid>().gridX <= maxX) || (cell.GetComponent<BuildOnGrid>().gridX <= maxX && cell.GetComponent<BuildOnGrid>().gridX >= minX))
                {                  
                    if ((cell.GetComponent<BuildOnGrid>().gridY >= minY && cell.GetComponent<BuildOnGrid>().gridY <= maxY) || (cell.GetComponent<BuildOnGrid>().gridY <= maxY && cell.GetComponent<BuildOnGrid>().gridY >= minY))
                    {
                        cell.GetComponent<BuildOnGrid>().HighlightGird(true);       
                    }
                    else
                    {
                        cell.GetComponent<BuildOnGrid>().HighlightGird(false);
                    }
                }
                else
                {
                    cell.GetComponent<BuildOnGrid>().HighlightGird(false);
                }
            }
        }
    }
}