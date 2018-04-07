using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTimed : Objective {

	[SerializeField]
	int DaysToWait;
	protected override void Awake()
	{
		base.Awake();
		TimeManager.DayChanged += DayObjectiveCheck;
	}

	void OnDisable()
	{
		GameManager.DayChanged -= DayObjectiveCheck;
	}
	private void DayObjectiveCheck()
	{
		if(TimeManager.instance.GetCurrentDay() == DaysToWait)
		{
			ObjectiveComplete();
		}
	}
	protected override void ObjectiveComplete()
	{
		Debug.Log("Objective Complete");
		base.ObjectiveComplete();

	} 


}
