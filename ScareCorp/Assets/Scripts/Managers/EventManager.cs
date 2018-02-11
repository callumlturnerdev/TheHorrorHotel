using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public delegate void ClickAction();
    public static event ClickAction PauseClicked;
    public static event ClickAction FastFClicked;
    public static event ClickAction DeleteClicked;
    public static event ClickAction ToggleTopWalls;
    

    private bool wallsHidden = false;
    public void Paused()
    {
        if (PauseClicked != null)
        {
            PauseClicked();
        }
    }

    public void FastF()
    {
        if (FastFClicked != null)
        {
            FastFClicked();
        }
    }

    public void DeleteMode()
    {
        if (DeleteClicked != null)
        {
            DeleteClicked();
        }
    }

    public void ToggleWalls()
    {
        if (ToggleTopWalls != null)
        {
            ToggleTopWalls();
        }
    }
    public bool GetWallsHidden() { return wallsHidden; }
}
