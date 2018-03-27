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
	bool currentlyScared;
	GameObject possibleFearTarget;
	[SerializeField]
	LayerMask LMask;
	void Awake()
	{
		currentlyScared = false;
		currentNeed = eNeedTypes.none;
		TimeManager.HourTick += HourlyTick;
		TimeManager.MinuteTick+= MinTick;
		aINeeds = GetComponent<AINeeds>();
		nav = GetComponent<NavMeshAgent>();
		aI = GetComponent<AI>();
		targetingNeed = true;
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
					if(currentNeed != eNeedTypes.hidden)
					{
						aI.UpdateStateUI("Scared");
						aI.navAgent.isStopped = false;
						aI.navAgent.speed = 4;
						DebugConsole.Log("ddd");
						if(aI.hidingPlaces.Count > 0)
						{
							nav.destination = aI.hidingPlaces[0].transform.position;
						}
						else
						{
						nav.destination = GameManager.instance.wayPointList[0].transform.position;
						}
						aI.gameObject.GetComponent<Visitor>().Scare(10.0f);
						StartCoroutine(DisableCurrentlyScared(5));
						currentNeed = eNeedTypes.hidden;
					}
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
						StartCoroutine(EnableTargetingNeed(0.1f));
						aI.UpdateStateUI("Tired");
					}
					
				break;

				default:
				break;
			}
		
		
			// WHEN SCARE

		
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
					DebugConsole.Log("dwdw");
					aI.UpdateStateUI("Hiding");
					float newFear = gameObject.GetComponent<Visitor>().GetCurrentFear() - 5.0f;
					gameObject.GetComponent<Visitor>().SetCurrentFear(newFear);
					
				break;

				case eNeedTypes.hunger:
					
					if(aINeeds.GetHunger() > 0.9f)
						{
							
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

	void CheckForScared()
	{
		if(currentlyScared && currentNeed != eNeedTypes.hidden)
		{
			ChangeNeedState(eNeedTypes.hidden);
		}
	}
	void Update()
	{
		ScareScan();
	}
	void MinTick()
	{
		
			CheckForScared();
		
		if(!aINeeds.NeedReachedZero())
		{
			if(nav)
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
				GameManager.instance.AddBed(aI.assignedBed);
				Destroy(gameObject);
			}
		}
	}

	 void ScareScan()
    {
		if(aI && currentNeed != eNeedTypes.hidden)
		{
			
			aI.eyes.transform.Rotate(0, aI.searchingTurnSpeed*2 * Time.deltaTime, 0);
       		 RaycastHit hit;
         Debug.DrawRay(aI.eyes.position, aI.eyes.forward.normalized *20, Color.green);

        //int layerMask = 1 << 12 | 1<< 11;
	 
      if (Physics.Raycast(aI.eyes.position, aI.eyes.forward.normalized * 20, out hit, 20, LMask
            ))
            {
				DebugConsole.Log("HITTTTT");
                Debug.DrawRay(aI.eyes.position, aI.eyes.forward.normalized * 20, Color.red);
            if (hit.transform.gameObject.tag == "scary")
            {
                DebugConsole.Log("scary hit");
                aI.fearTarget = hit.transform.gameObject;
                aI.gameObject.GetComponent<Visitor>().SetNextFearObject(hit.transform.gameObject);
				currentlyScared = true;
            }
        }
		}
    }

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(aI.eyes.position,15);
		if(aI.fearTarget)
		{
			Gizmos.DrawLine(aI.eyes.position,aI.fearTarget.transform.position);
		}
	}


	void HourlyTick()
	{
	}

	IEnumerator DisableCurrentlyScared(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		//currentlyScared = false;
		//targetingNeed = false;
		//ChangeNeedState(aINeeds.FindMostUrgentNeed());
		aI.GetComponent<Visitor>().TurnOffScareParticle();
	}
	IEnumerator EnableTargetingNeed(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		targetingNeed = true;
	}

}
