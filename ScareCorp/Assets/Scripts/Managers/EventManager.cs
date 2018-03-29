using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public delegate void ClickAction();
    public static event ClickAction PauseClicked;
    public static event ClickAction FastFClicked;
    public static event ClickAction DeleteClicked;
    public static event ClickAction ToggleTopWalls;
    
    [SerializeField]
    AudioClip UIclick;
    AudioSource audioS;
    private bool wallsHidden = false;

  
    void Awake()
    {
        audioS = gameObject.AddComponent(typeof(AudioSource))as AudioSource;
        audioS.clip = UIclick;
    }
    public void Paused()
    {
        audioS.Play();
        if (PauseClicked != null)
        {
            PauseClicked();
        }
    }

    public void FastF()
    {
         audioS.Play();
        if (FastFClicked != null)
        {
            FastFClicked();
        }
    }

    public void DeleteMode()
    {
         audioS.Play();
        if (DeleteClicked != null)
        {
            DeleteClicked();
        }
    }

    public void ToggleWalls()
    {
         audioS.Play();
        if (ToggleTopWalls != null)
        {
            ToggleTopWalls();
        }
    }
    public bool GetWallsHidden() { return wallsHidden; }
}
