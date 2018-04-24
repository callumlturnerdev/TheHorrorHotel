using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScore : Objective {


     /// ObjectiveScored will check for a certain score to be acheived to result in a win

  
	[SerializeField]
	float requiredScore;
	protected override void Awake()
	{
		base.Awake();
		TimeManager.MinuteTick += ScoreObjectiveCheck;
		
	}
	void OnDisable()
	{
		TimeManager.MinuteTick -= ScoreObjectiveCheck;
	}
	private void ScoreObjectiveCheck()
	{
		if(BuildController.instance.GetScreamPoints() >= requiredScore)
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
