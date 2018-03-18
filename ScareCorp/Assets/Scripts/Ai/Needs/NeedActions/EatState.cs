
using UnityEngine;
using StateAI;

public class EatState : State<AI>
{

    /// <summary>
    ///  THIS STATE DOES A QUICK 360 SEARCH OF THE ENVIROMENT ADDING THESE TO A POINTS OF 
    ///  INTEREST ARRAY IN GAMEOBJECTS
    /// </summary>
    /// 
    private float gameTimer;
    private float seconds;
    private bool timerReached = false;
    private static EatState _instance;
    private float currentNeedFullfillment;
    private EatState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static EatState Instance
    {
        get
        {
            if (_instance == null)
            {
                new EatState();
            }
            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        currentNeedFullfillment = 0;
        _owner.UpdateStateUI("Eating");
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
        if (_owner.aiNeeds.GetHunger() < 1 )//&& currentNeedFullfillment)< _owner.currentTarget.GetComponent<Buildable>().needFulfillment)
        {

            float newValue = _owner.aiNeeds.GetHunger() + 0.002f * TimeManager.instance.GetPlayRate();
            _owner.aiNeeds.SetHunger(newValue);
            currentNeedFullfillment += 0.002f;
        }
        else
        {
            _owner.currentTarget.GetComponent<Buildable>().hasBeenUsed();
           // _owner.stateMachine.ChangeState(SeekNextNeedState.Instance);
           
        }
    }


}
