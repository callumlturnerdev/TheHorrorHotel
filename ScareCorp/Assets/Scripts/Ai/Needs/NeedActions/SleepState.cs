﻿
using UnityEngine;
using StateAI;

public class SleepState : State<AI>
{

    /// <summary>

    /// </summary>
    /// 

    private float gameTimer;
    private float seconds;
    private bool timerReached = false;
    private static SleepState _instance;

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
        _owner.UpdateStateUI("Sleeping");
        
        seconds = 0;
        _owner.navAgent.speed = 0;
    }

    public override void ExitState(AI _owner)
    {
        _owner.navAgent.speed = 1 * ( TimeManager.instance.GetPlayRate() * 3); 
    }

    public override void UpdateState(AI _owner)
    {
        Sleep(_owner);
    }

    private void Sleep(AI _owner)
    {
        if (_owner.currentTarget && _owner.aiNeeds.GetTiredness() < 1)
        {
                float newValue = _owner.aiNeeds.GetTiredness() + 0.001f;
                _owner.aiNeeds.SetTiredness(newValue);
        }
        else
        {
            _owner.stateMachine.ChangeState(SeekNextNeedState.Instance);
        }
    }


}
