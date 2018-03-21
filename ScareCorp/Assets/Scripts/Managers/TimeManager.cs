using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// CLass dedicated to Days and in game timer.


public class TimeManager : MonoBehaviour {

    public Text UItime;
    public Text UIDay;
    public delegate void ClickAction();
    public static TimeManager instance = null;

    float currentDay;
    [SerializeField] [Range(0,1440)]
    private float time;
    float seconds;
    float minutes;
    float baseplayRate = 0.25f;

    float playRate = 0.25f;
    float defaultPlayRate = 0.25f;
    float FFplayRate = 0.125f;
    bool timeIsStopped = false;

    private float arrivalTimef = 7;
    private float departureTimef = 6;
    float currentHour = 0;
    public static event ClickAction DayChanged; // Tracks when a day changes 
    public static event ClickAction ArriveTime; 
    public static event ClickAction DepartTime;
    public static event ClickAction PlayRateChange;
    public static event ClickAction timeStopped;
    public static event ClickAction HourTick;
    public static event ClickAction MinuteTick;

    void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        EventManager.FastFClicked += TogglePlayRate;
        EventManager.PauseClicked += ToggleTime;
        time = 0000;
        currentDay = 1;
        InterateTime();
    }

    IEnumerator WaitforSeconds(float time)
    {
        yield return new WaitForSeconds(playRate);
        MinutePassed();
        InterateTime();
    }

    public float GetPlayRate(){ if(playRate == 0) {return  0;} else if(playRate == FFplayRate) {return 2;} else return 1; }

    void InterateTime()
    {
        if (!timeIsStopped)
        {
            time++;
            if (currentHour != Mathf.Floor(time / 60))
            {
                HourPassed();
                currentHour = Mathf.Floor(time / 60);
            }
            if (Mathf.Floor(time / 60) > 23)
            {
                time = 0;
                currentDay++;
                DayChange();
            }
            string hours = Mathf.Floor(time / 60).ToString("00");
            string minutes = (time % 60).ToString("00");
            UIDay.text = "Day " + currentDay;
            UItime.text = hours + ":" + minutes;


            if(GetCurrentTime() / 60 == arrivalTimef) // Used for determining when a visitor should arrive
            {
                ArriveT();
            }
            if(GetCurrentTime() / 60 == departureTimef) // Used for determining when a visitor should arrive
            {
                DepartT();
            }
        }
        StartCoroutine(WaitforSeconds(0.01f));
    }
    public float GetCurrentTime() { return time; }
    public void SetCurrentTime(float newtime) { time = newtime; }
    public float GetCurrentDay() { return currentDay; }
    public void  SetCurrentDay(float day) { currentDay = day; }

    public void TogglePlayRate()
    {
        if (playRate == defaultPlayRate) { playRate = FFplayRate; }
        else if (playRate == FFplayRate) { playRate = defaultPlayRate; }
        if (PlayRateChange != null)
        {
            PlayRateChange();
        }
    }
    public void ToggleTime()
    {
        Debug.Log("Pause");
        if (playRate > 0 || playRate < 0) { playRate = 0; }
        else if (playRate == 0) { playRate = defaultPlayRate; }
        timeIsStopped = !timeIsStopped;
        if (timeStopped != null)
        {
            timeStopped();
        }
    }

    private void MinutePassed()
    {
        if(MinuteTick != null)
        {
            MinuteTick();
        }
    }
    private void HourPassed()
    {
        if (HourTick != null)
        {
            HourTick();
        }
    }
    public void DayChange()
    {
        if (DayChanged != null)
        {
            DayChanged();
        }
    }

    public void ArriveT()
    {
        if(ArriveTime != null)
        {
            ArriveTime();
        }
    }
        public void DepartT()
    {
        if(DepartTime != null)
        {
            DepartTime();
        }
    }

}
