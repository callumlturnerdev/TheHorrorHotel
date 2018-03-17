
using UnityEngine;
using StateAI;

public class EntertainmentState : State<AI>
{

    /// <summary>
    ///  THIS STATE DOES A QUICK 360 SEARCH OF THE ENVIROMENT ADDING THESE TO A POINTS OF 
    ///  INTEREST ARRAY IN GAMEOBJECTS
    /// </summary>
    /// 
    private float gameTimer;
    private float seconds;
    private bool timerReached = false;
    private static EntertainmentState _instance;
    private float currentNeedFullfillment;

    private EntertainmentState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static EntertainmentState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EntertainmentState();
            }

            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        currentNeedFullfillment = 0;
        _owner.UpdateStateUI("Reading");
        seconds = 0;
        _owner.navAgent.speed = 0;
    }

    public override void ExitState(AI _owner)
    {
        _owner.navAgent.speed = 2;
    }

    public override void UpdateState(AI _owner)
    {
        Eat(_owner);
    }

    private void Eat(AI _owner)
    {
        if (_owner.hygiene > 0.49f && _owner.tiredness > 0.49f && _owner.hunger > 0.49f)
        {
            float newValue = _owner.aiNeeds.GetBoredom() + 0.002f *  TimeManager.instance.GetPlayRate();
            _owner.aiNeeds.SetBoredom(newValue);
            currentNeedFullfillment += 0.002f;
        }
        else
        {
            _owner.stateMachine.ChangeState(SeekNextNeedState.Instance);
        }
    }


}
