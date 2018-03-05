using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirLightRotation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 1;
    public GameObject DayLight;
    public GameObject NightLight;
    private Light lightRef;
    bool nightTime = false;


    private Color dayColor;
    private Color nightColor;
    // Use this for initialization
    void Start()
    {
     
  
        NightLight.transform.LookAt(this.transform);
        DayLight.transform.LookAt(this.transform);
        NightLight.SetActive(true);
        DayLight.SetActive(false);

        TimeManager.HourTick += HourlyTick;
    }
    void HourlyTick()
    {
        NightLight.transform.LookAt(this.transform);
        DayLight.transform.LookAt(this.transform);
        transform.Rotate(transform.rotation.x + -11.25f, 0, 0);
        if ((Mathf.Floor(TimeManager.instance.GetCurrentTime() / 60) < 07 || Mathf.Floor(TimeManager.instance.GetCurrentTime() / 60) > 23))
        {
          DayLight.SetActive(false);
            NightLight.SetActive(true);
        }
        else
        {
            
            DayLight.SetActive(true);
            //NightLight.SetActive(false);
        }
    }
}
