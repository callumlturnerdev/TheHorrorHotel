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
    private float time;
    float seconds;
    float minutes;
    float baseplayRate = 1.5f;

    float playRate = 1;
    float defaultPlayRate = 1.0f;
    float FFplayRate = 2.0f;
    bool timeIsStopped = false;


    float currentHour = 0;
    public static event ClickAction DayChanged;
    public static event ClickAction PlayRateChange;
    public static event ClickAction timeStopped;
    public static event ClickAction HourTick;
    // Use this for initialization
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
        yield return new WaitForSeconds(baseplayRate - playRate);
        InterateTime();
    }

    public float GetPlayRate(){ return playRate; }

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
        }
        StartCoroutine(WaitforSeconds(0.01f));
    }
    // USed for saving information with savingloading script
    public float GetCurrentTime() { return time; }
    public void SetCurrentTime(float newtime) { time = newtime; }
    public float GetCurrentDay() { return currentDay; }
    public void  SetCurrentDay(float day) { currentDay = day; }
    //
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
        if (playRate > 0) { playRate = 0; }
        else if (playRate == 0) { playRate = defaultPlayRate; }
        timeIsStopped = !timeIsStopped;
        if (timeStopped != null)
        {
            timeStopped();
        }
    }


    private void HourPassed()
    {
        DebugConsole.Log("HOUR PASSED.........................................");
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

}
