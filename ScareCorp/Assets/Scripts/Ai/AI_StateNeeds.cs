using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using needTypes;
using UnityEngine.AI;
using UnityEngine.UI;
public class AI_StateNeeds : MonoBehaviour {

	NavMeshAgent nav;
	eNeedTypes currentNeed;
	AI aI;
	AINeeds aINeeds;
	bool targetingNeed;
	void Awake()
	{
		currentNeed = eNeedTypes.none;
		TimeManager.HourTick += HourlyTick;
		TimeManager.MinuteTick+= MinTick;
		aINeeds = GetComponent<AINeeds>();
		nav = GetComponent<NavMeshAgent>();
		aI = GetComponent<AI>();
		targetingNeed = false;
	} 
	
	
	
	void ChangeNeedState(eNeedTypes newNeed)
	{
		aI.GetNeedObjects();
		switch(newNeed)
		{
			case eNeedTypes.none:
			break;

			case eNeedTypes.boredom:
				if(aI.boredomObjects[0])
				{
					nav.isStopped = false;
					nav.destination = aI.boredomObjects[0].transform.position;
					currentNeed = eNeedTypes.boredom;
					aI.UpdateStateUI("Bored");
					targetingNeed =true;
				}
			break;

			case eNeedTypes.hidden:
				nav.destination = aI.hidingPlaces[0].transform.position;
				currentNeed = eNeedTypes.hidden;
			break;

			case eNeedTypes.hunger:
				if(aI.hungerObjects[0])
				{
					nav.isStopped = false;
					nav.destination = aI.hungerObjects[0].transform.position;
					currentNeed = eNeedTypes.hunger;
					aI.UpdateStateUI("Hungry");
					targetingNeed =true;
				}
			break;

			case eNeedTypes.hygiene:
				if(aI.hygieneObjects[0])
				{
					nav.isStopped = false;
					nav.destination = aI.hygieneObjects[0].transform.position;
					currentNeed = eNeedTypes.hygiene;
					aI.UpdateStateUI("Dirty");
					targetingNeed =true;
				}
			break;

			case eNeedTypes.tiredness:
				if(aI.assignedBed)
				{
					nav.destination = aI.assignedBed.transform.position;
					nav.isStopped = false;	
					currentNeed = eNeedTypes.tiredness;
					StartCoroutine(EnableTargetingNeed(2));
					aI.UpdateStateUI("Tired");
				}
				
			break;

			default:
			break;
		}
	}
	void OnEnable()
	{
		nav.enabled = true;
		targetingNeed = false;
		ChangeNeedState(eNeedTypes.tiredness);
	}
	void ArrivalAction(eNeedTypes currentNeed)
	{
		switch(currentNeed)
		{
			case eNeedTypes.none:
			aI.UpdateStateUI("Nothing");
			break;

			case eNeedTypes.boredom:
				if(aINeeds.GetBoredom() > 0.9f)
					{
						DebugConsole.Log("Test");
						targetingNeed = false;
						ChangeNeedState(aINeeds.FindMostUrgentNeed());
					}
					else
					{
						aI.UpdateStateUI("Reading");
						float newBoredom = aINeeds.GetBoredom() + 0.05f;
						aINeeds.SetBoredom(newBoredom);
					}
			break;

			case eNeedTypes.hidden:

			break;

			case eNeedTypes.hunger:
				if(aINeeds.GetHunger() > 0.9f)
					{
						DebugConsole.Log("Test");
						targetingNeed = false;
						ChangeNeedState(aINeeds.FindMostUrgentNeed());
					}
					else
					{
						aI.UpdateStateUI("Eating");
						float newF = aINeeds.GetHunger() + 0.05f;
						aINeeds.SetHunger(newF);
					}
			break;

			case eNeedTypes.hygiene:
				if(aINeeds.GetHygiene() > 0.9f)
					{
						DebugConsole.Log("Test");
						targetingNeed = false;
						ChangeNeedState(aINeeds.FindMostUrgentNeed());
					}
					else
					{
						aI.UpdateStateUI("Washing");
						float newF = aINeeds.GetHygiene() + 0.05f;
						aINeeds.SetHygiene(newF);
					}
			break;

			case eNeedTypes.tiredness:
				if(aINeeds.GetTiredness() > 0.9f)
				{
					DebugConsole.Log("Test");
					targetingNeed = false;
					ChangeNeedState(aINeeds.FindMostUrgentNeed());
				}
				else
				{
					aI.UpdateStateUI("Sleeping");
					float newTiredness = aINeeds.GetTiredness() + 0.05f;
					aINeeds.SetTiredness(newTiredness);
				}
			break;

			default:
			break;
		}
	}

	
	void OnDisable()
	{
		TimeManager.HourTick -= HourlyTick;
	}


	void MinTick()
	{
		if(!aINeeds.NeedReachedZero())
		{
			if(nav.isActiveAndEnabled && currentNeed != eNeedTypes.none)
			{
				if(nav.remainingDistance < 2 && targetingNeed == true)
				{
					nav.isStopped = true;
					ArrivalAction(currentNeed);
				}
			}
		}
		else
		{
			Leave();
		}
	}
	
	void Leave()
	{
		// Add code so that they leave.
		if(GameManager.instance.wayPointList.Count > 0 && nav)
		{

			nav.isStopped = false;
			nav.destination = GameManager.instance.wayPointList[0].transform.position;
			//currentNeed = eNeedTypes.hygiene;
			aI.UpdateStateUI("Leaving");

			if(Vector3.Distance(this.transform.position, GameManager.instance.wayPointList[0].position)< 10)
			{
				nav.isStopped = true;
				ArrivalAction(currentNeed);
				Destroy(gameObject);
			}
		}
	}

	void HourlyTick()
	{
	}

	IEnumerator EnableTargetingNeed(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		targetingNeed = true;
	}

}
