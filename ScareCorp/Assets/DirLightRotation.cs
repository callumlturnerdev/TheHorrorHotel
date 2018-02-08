using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirLightRotation : MonoBehaviour {
    [SerializeField]
    private float rotationSpeed = 1;
    private GameObject DirectionalLight;
    private Light lightRef;
    bool nightTime = false;

    private Color dayColor;
    private Color nightColor;
	// Use this for initialization
	void Start () {
        DirectionalLight = transform.GetChild(0).gameObject;
        lightRef = DirectionalLight.GetComponent<Light>();
        DirectionalLight.transform.LookAt(this.transform);
        dayColor = new Color(210, 210, 210);
        nightColor = new Color(23, 1, 16);
        TimeManager.HourTick += HourlyTick;
    }
    void HourlyTick()
    {
        DirectionalLight.transform.LookAt(this.transform);
        transform.Rotate(transform.rotation.x + -11.25f, 0, 0);
        if ((Mathf.Floor(TimeManager.instance.GetCurrentTime() / 60) < 07 || Mathf.Floor(TimeManager.instance.GetCurrentTime() / 60) > 23))
        {
            lightRef.intensity = 0.25f;
        }
        else
        {
            lightRef.intensity = 0.710f;
        }
    }


    /*
    void SetToNight()
    {
        if (!nightTime)
        {
            DebugConsole.Log("NIGHTTIME");
            lightRef.color = Color.Lerp(dayColor, nightColor, 0.5f); 
            lightRef.intensity = 0.01f;
            nightTime = true;
        }
    }

    void SetToDay()
    {
        if (nightTime)
        {
            lightRef.color = Color.Lerp(nightColor, dayColor, 0.5f);
            lightRef.intensity = 0.003f;
            nightTime = false;
        }
    }*/
}
