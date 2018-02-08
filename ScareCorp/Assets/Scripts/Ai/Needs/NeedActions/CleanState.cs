
using UnityEngine;
using StateAI;

public class CleanState : State<AI>
{

    /// <summary>
    ///  THIS STATE DOES A QUICK 360 SEARCH OF THE ENVIROMENT ADDING THESE TO A POINTS OF 
    ///  INTEREST ARRAY IN GAMEOBJECTS
    /// </summary>
    /// 


    private float gameTimer;
    private float seconds;
    private bool timerReached = false;
    private static CleanState _instance;
    private float currentNeedFullfillment;
    private CleanState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static CleanState Instance
    {
        get
        {
            if (_instance == null)
            {
                new CleanState();
            }

            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        currentNeedFullfillment = 0;
        _owner.UpdateStateUI("Washing");
        seconds = 0;
        DebugConsole.Log("Entering ScanState");
        _owner.navAgent.speed = 0;

    }

    public override void ExitState(AI _owner)
    {
        DebugConsole.Log("Exiting ScanState");
        _owner.navAgent.speed = 2;
    }

    public override void UpdateState(AI _owner)
    {
        Eat(_owner);


    }

    private void Eat(AI _owner)
    {
        if (_owner.aiNeeds.GetHygiene() < 1 && currentNeedFullfillment < _owner.currentTarget.GetComponent<Buildable>().needFulfillment)
        {

                float newValue = _owner.aiNeeds.GetHygiene()   + 0.001f * TimeManager.instance.GetPlayRate();
                _owner.aiNeeds.SetHygiene(newValue);
                    currentNeedFullfillment += 0.001f;
            
        }
        else
        {
            _owner.stateMachine.ChangeState(SeekNextNeedState.Instance);
        }
    }


}
