
using UnityEngine;
using StateAI;

public class ForwardWalkState : State<AI>
{
    private static ForwardWalkState _instance;

    private ForwardWalkState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static ForwardWalkState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ForwardWalkState();
            }

            return _instance;
        }
    }


    public override void EnterState(AI _owner)
    {
        _owner.UpdateStateUI("Door");
        _owner.pointsOfInterest.Clear();
        _owner.nextWayPoint = 0;
        ForwardWalk(_owner);
        DebugConsole.Log("Entering ForwardWalk");
        
    }

    public override void ExitState(AI _owner)
    {
        //DebugConsole.Log("Exiting ForwardWalk");
       
    }

    public override void UpdateState(AI _owner)
    {
        
        if (_owner.CheckifCountDownElapsed(3))
        {
            _owner.stateMachine.ChangeState(ScanState.Instance);
        }
    }


    private void  ForwardWalk(AI _owner)
    {
        _owner.navAgent.isStopped = false;

       Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized * 15, Color.cyan);
        _owner.navAgent.destination = _owner.transform.position + (_owner.transform.forward.normalized * 5);
    }
}
