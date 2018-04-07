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
	[SerializeField]
	GameObject currentNeedObject;
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
					currentNeedObject = aI.SelectTarget(aI.boredomObjects);
					if(currentNeedObject)
					{
						nav.isStopped = false;
						nav.destination = currentNeedObject.GetComponent<Buildable>().GetVisInteractPos().position;
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
						currentNeedObject  = aI.SelectTarget(aI.hidingPlaces);
					
						if( aI.hidingPlaces.Count > 0 && currentNeedObject)
						{
							nav.destination = currentNeedObject.GetComponent<Buildable>().GetVisInteractPos().position;
							currentNeed = eNeedTypes.hidden;
							
						}
						else
						{
							currentNeedObject = GameManager.instance.leavePoint;
							nav.destination = currentNeedObject.GetComponent<Buildable>().GetVisInteractPos().position;
							currentNeed = eNeedTypes.hidden;
						}
						aI.gameObject.GetComponent<Visitor>().Scare(100.0f);
						//StartCoroutine(DisableCurrentlyScared(5));
						
					}
				break;

				case eNeedTypes.hunger:
					currentNeedObject  = aI.SelectTarget(aI.hungerObjects);
					if(currentNeedObject)
					{
						nav.isStopped = false;
						nav.destination = currentNeedObject.GetComponent<Buildable>().GetVisInteractPos().position;
						currentNeed = eNeedTypes.hunger;
						aI.UpdateStateUI("Hungry");
						targetingNeed =true;
					}
				break;

				case eNeedTypes.hygiene:
					currentNeedObject  = aI.SelectTarget(aI.hygieneObjects);
					if(currentNeedObject)
					{
						nav.isStopped = false;
						nav.destination = currentNeedObject.GetComponent<Buildable>().GetVisInteractPos().position;
						currentNeed = eNeedTypes.hygiene;
						aI.UpdateStateUI("Dirty");
						targetingNeed =true;
					}
				break;

				case eNeedTypes.tiredness:
					currentNeedObject  = aI.assignedBed;
					if(currentNeedObject)
					{
						nav.destination = currentNeedObject.GetComponent<Buildable>().GetVisInteractPos().position;
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

	void OnDisable()
	{
		TimeManager.MinuteTick -= MinTick;
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
							float newBoredom = aINeeds.GetBoredom() + 0.01f;
							aINeeds.SetBoredom(newBoredom);
						}
				break;

				case eNeedTypes.hidden:
					
					aI.UpdateStateUI("Hiding");
					
					 aI.fearTarget = null;
					DisableScared();
					StartCoroutine(StopHiding(10));
					
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
							float newF = aINeeds.GetHunger() + 0.01f;
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
							float newF = aINeeds.GetHygiene() + 0.01f;
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
						float newTiredness = aINeeds.GetTiredness() + 0.01f;
						aINeeds.SetTiredness(newTiredness);
					}
				break;

				default:
				break;
			}
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
		if(currentNeed == eNeedTypes.hidden  && nav.isStopped == true)
		{
			if(gameObject.GetComponent<Visitor>().GetCurrentFear() > 0)
			{
				float newFear = gameObject.GetComponent<Visitor>().GetCurrentFear() - 0.50f;
				gameObject.GetComponent<Visitor>().SetCurrentFear(newFear);
			}
		}
		Debug.Log(aI.assignedBed);
		if(!aINeeds.NeedReachedZero() && gameObject.GetComponent<Visitor>().GetCurrentFear() < 99 && aI.assignedBed != null )
		{
			DebugConsole.Log(gameObject.GetComponent<Visitor>().GetCurrentFear().ToString());
			CheckForScared();
			if(nav)
			{
				if(nav.isActiveAndEnabled && currentNeed != eNeedTypes.none)
				{
					if( currentNeedObject && currentNeedObject.GetComponent<Buildable>())
					{
					if((Vector3.Distance(this.transform.position, currentNeedObject.GetComponent<Buildable>().GetVisInteractPos().position) < 2)  && targetingNeed == true)
					{
						nav.isStopped = true;
						ArrivalAction(currentNeed);
					}
					}
					else
					{
						ChangeNeedState(aINeeds.FindMostUrgentNeed());
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
		if(GameManager.instance.leavePoint && nav)
		{

			nav.isStopped = false;
			currentNeedObject = GameManager.instance.leavePoint;
			nav.destination = currentNeedObject.transform.position;
			//currentNeed = eNeedTypes.hygiene;
			aI.UpdateStateUI("Leaving");

			if(Vector3.Distance(this.transform.position, GameManager.instance.leavePoint.transform.position)< 5)
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
		if(aI )
		{
		
			aI.eyes.transform.Rotate(0, aI.searchingTurnSpeed*2 * Time.deltaTime, 0);
       		 RaycastHit hit;
         Debug.DrawRay(aI.eyes.position, aI.eyes.forward.normalized *20, Color.green);

        //int layerMask = 1 << 12 | 1<< 11;
	 
      if (Physics.Raycast(aI.eyes.position, aI.eyes.forward.normalized * 20, out hit, 20, LMask
            ))
            {
                Debug.DrawRay(aI.eyes.position, aI.eyes.forward.normalized * 20, Color.red);
            if (hit.transform.gameObject.tag == "scary")
            {
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

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "scary")
        {
                aI.fearTarget = other.transform.gameObject;
                aI.gameObject.GetComponent<Visitor>().SetNextFearObject(other.transform.gameObject);
				currentlyScared = true;
        }
    }

	void HourlyTick()
	{
	}

	IEnumerator StopHiding(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		ChangeNeedState(aINeeds.FindMostUrgentNeed());
	}
	void DisableScared()
	{
		currentlyScared = false;
		targetingNeed = false;
		aI.navAgent.speed = 2;
		aI.GetComponent<Visitor>().TurnOffScareParticle();
	}
	IEnumerator EnableTargetingNeed(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		targetingNeed = true;
			currentlyScared = false;
	}

}
