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

    void ToggleShadows(float shadowStrength)
    {
        DayLight.GetComponent<Light>().shadowStrength = shadowStrength;
    }
   
    void OnDisable()
    {
        TimeManager.HourTick -= HourlyTick;
    }
    void HourlyTick()
    {
        NightLight.transform.LookAt(this.transform);
        DayLight.transform.LookAt(this.transform);
        transform.Rotate(transform.rotation.x + -11.25f, 0, 0);
        if ((Mathf.Floor(TimeManager.instance.GetCurrentTime() / 60) < 07 || Mathf.Floor(TimeManager.instance.GetCurrentTime() / 60) > 23))
        {
             //ToggleShadows(0.01f);
             DayLight.SetActive(false);
             NightLight.SetActive(true);
        }
        else
        {
            if ((Mathf.Floor(TimeManager.instance.GetCurrentTime() / 60) > 10 || Mathf.Floor(TimeManager.instance.GetCurrentTime() / 60) < 20))
            {
                ToggleShadows(0.5f);
            }
            DayLight.SetActive(true);
            //NightLight.SetActive(false);
        }
    }
}
