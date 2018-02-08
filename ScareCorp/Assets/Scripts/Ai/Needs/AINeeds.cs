//#TODO2 Link How quickly needs drop with timemanager playrate
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using needTypes;


public class AINeeds : MonoBehaviour
{

    private float decreaseRate = 0.01f;
    [Range(0,1)]
    private float needHunger = 1.0f;
    [Range(0, 1)]
    private float needHygiene = 1.0f;
    [Range(0, 1)]
    private float needBoredom = 1.0f;
    [Range(0, 1)]
    private float needTiredness = 1.0f;

    public float daysStayed;
    private float daysToStay;

    public float searchDuration = 4f;
    public float searchingTurnSpeed = 120f;
    private void Start()
    {
        daysToStay = 0;
        daysStayed = 0;

        TimeManager.DayChanged += DayChange;
        needHunger = 1.0f;
         needHygiene = 1.0f;
         needBoredom = 1.0f;
         needTiredness = 0.50f;
        StartCoroutine(WaitFor(3.0f / TimeManager.instance.GetPlayRate()));
    }

    private void DayChange()
    {
        daysStayed++;
    }

    public bool CheckIfTimeToLeave()
    {
        if (daysStayed >= daysToStay)
            return true;
        return false;
    }

    private void UpdateNeeds()
    {

        needHunger =  Mathf.Clamp(needHunger -= decreaseRate, 0,1);
        needHygiene = Mathf.Clamp(needHygiene -= decreaseRate, 0, 1);
        needBoredom = Mathf.Clamp(needBoredom -= decreaseRate, 0, 1);
        needTiredness = Mathf.Clamp(needTiredness -= decreaseRate, 0, 1);
        StartCoroutine(WaitFor(3.0f / TimeManager.instance.GetPlayRate()));
    }

    public float GetHunger() { return needHunger; }
    public float GetBoredom() { return needBoredom; }
    public float GetHygiene() { return needHygiene; }
    public float GetTiredness() { return needTiredness; }

    public void SetHunger(float i) { needHunger = i; }
    public void SetBoredom(float i) { needBoredom = i; }
    public void SetHygiene(float i) { needHygiene = i; }
    public void SetTiredness(float i) { needTiredness = i; }


    IEnumerator WaitFor(float duration)
    {
        yield return new WaitForSeconds(duration);
        UpdateNeeds();
    }

    public float lowestNeedValue()
    {
        AI ai = gameObject.GetComponent<AI>();
        float tiredness;
        float hygiene;
        float boredom;
        float hunger;


        if (ai.hungerObjects.Count > 0) { hunger = needHunger; } else { hunger = 10; }
        if (ai.hygieneObjects.Count > 0) { hygiene = needHygiene; } else { hygiene = 10; }
        if (ai.boredomObjects.Count > 0) { boredom = needBoredom ; } else { boredom = 10; }
        if (ai.tirednessObjects.Count > 0) { tiredness = needTiredness; } else { tiredness = 10; }

        return Mathf.Min(tiredness, hygiene, boredom, hunger);
    }

}
