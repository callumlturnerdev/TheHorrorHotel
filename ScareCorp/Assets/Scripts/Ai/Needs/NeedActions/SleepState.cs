
using UnityEngine;
using StateAI;

public class SleepState : State<AI>
{

    /// <summary>
    ///  THIS STATE DOES A QUICK 360 SEARCH OF THE ENVIROMENT ADDING THESE TO A POINTS OF 
    ///  INTEREST ARRAY IN GAMEOBJECTS
    /// </summary>
    /// 


    private float gameTimer;
    private float seconds;
    private bool timerReached = false;
    private static SleepState _instance;
    private float currentNeedFullfillment;

    private SleepState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static SleepState Instance
    {
        get
        {
            if (_instance == null)
            {
                new SleepState();
            }

            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        currentNeedFullfillment = 0;
        _owner.UpdateStateUI("Sleeping");
        seconds = 0;
        DebugConsole.Log("Entering ScanState");
        _owner.navAgent.speed = 0;

    }

    public override void ExitState(AI _owner)
    {
        DebugConsole.Log("Exiting ScanState");
        _owner.navAgent.speed = 1 * (TimeManager.instance.GetPlayRate() * 3);
    }

    public override void UpdateState(AI _owner)
    {
        Sleep(_owner);


    }

    private void Sleep(AI _owner)
    {
        if (_owner.currentTarget)
        {
            if (_owner.aiNeeds.GetTiredness() < 1 && currentNeedFullfillment < _owner.currentTarget.GetComponent<Buildable>().needFulfillment)
            {

                float newValue = _owner.aiNeeds.GetTiredness() + 0.001f;
                _owner.aiNeeds.SetTiredness(newValue);
                currentNeedFullfillment += 0.001f;

            }
        }
        else
        {

            _owner.stateMachine.ChangeState(SeekNextNeedState.Instance);
        }
    }


}
