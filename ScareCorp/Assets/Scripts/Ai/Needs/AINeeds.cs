//#TODO2 Link How quickly needs drop with timemanager playrate
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using needTypes;


public class AINeeds : MonoBehaviour
{

    private float decreaseRate =0.005f; //0.01f;
    [Range(0,1)]
    private float needHunger = 1.0f;
    [Range(0, 1)]
    private float needHygiene = 1.0f;
    [Range(0, 1)]
    private float needBoredom = 1.0f;
    [Range(0, 1)]
    private float needTiredness = 0.4f;

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
         needTiredness = 0.4f;
      //  StartCoroutine(WaitFor(1.0f *  TimeManager.instance.GetPlayRate()));
        TimeManager.MinuteTick += MinTick;
    }
    void OnDisable()
    {
        TimeManager.MinuteTick -= MinTick;
    }
    private void DayChange()
    {
        daysStayed++;
    }
    private void MinTick()
    {
        UpdateNeeds();
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
       // StartCoroutine(WaitFor(1.0f *  TimeManager.instance.GetPlayRate()));
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
      //  UpdateNeeds();
    }

    public bool NeedReachedZero()
    {
        if(GetTiredness() <= 0 || GetBoredom() <= 0 || GetHunger() <=0 || GetHygiene() <=0 )
        {
           return true; 
        }
        return false;
    }
   public eNeedTypes FindMostUrgentNeed()
	{
        AI ai = gameObject.GetComponent<AI>();
		if(GetTiredness() <= lowestNeedValue() && ai.assignedBed)
		{
            DebugConsole.Log("tiredIsLowest");
			return eNeedTypes.tiredness;
		}
        if(GetBoredom() <= lowestNeedValue() && ai.boredomObjects.Count > 0)
        {
            DebugConsole.Log("boredomLowest");
            return eNeedTypes.boredom;
        }
        if(GetHygiene() <= lowestNeedValue() && ai.hygieneObjects.Count > 0)
        {
            DebugConsole.Log("hygieneLowest");
            return eNeedTypes.hygiene;
        }
        if(GetHunger() <= lowestNeedValue() && ai.hungerObjects.Count >0)
        {
             DebugConsole.Log("hungerlowest");
            return eNeedTypes.hunger;
        }
         DebugConsole.Log("nonlowest");
        return eNeedTypes.none;
	}
    public float lowestNeedValue()
    {
        AI ai = gameObject.GetComponent<AI>();
        float tiredness;
        float hygiene;
        float boredom;
        float hunger;

        if (ai.hungerObjects.Count > 0 && ai.hungerObjects[0]) { hunger = needHunger; } else { hunger = 10; }
        if (ai.hygieneObjects.Count > 0 && ai.hygieneObjects[0]) { hygiene = needHygiene; } else { hygiene = 10; }
        if (ai.boredomObjects.Count > 0 && ai.boredomObjects[0]) { boredom = needBoredom ; } else { boredom = 10; }
        if (ai.assignedBed) { tiredness = needTiredness; } else { tiredness = 10; }
        return Mathf.Min(tiredness, hygiene, boredom, hunger);
    }

}
