
using UnityEngine;
using StateAI;

public class PatrolState : State<AI>
{
    private static PatrolState _instance;

    private PatrolState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static PatrolState Instance
    {
        get {
            if (_instance == null)
            {
                new PatrolState();
            }

            return _instance;
        }
    }


    public override void EnterState(AI _owner)
    {
        _owner.UpdateStateUI("Patroling");
        _owner.switchState = false;
    }

    public override void ExitState(AI _owner)
    {
        _owner.switchState = false;
    }

    public override void UpdateState(AI _owner)
    {
        Patrol(_owner);
        if(_owner.switchState)
         _owner.stateMachine.ChangeState(ForwardWalkState.Instance);
    }


    void Patrol(AI _owner)
    {
        if ((_owner.nextWayPoint < _owner.pointsOfInterest.Count))
        {
            _owner.navAgent.destination = _owner.pointsOfInterest[_owner.nextWayPoint].transform.position;
            _owner.navAgent.isStopped = false;

            if (_owner.navAgent.remainingDistance <= _owner.navAgent.stoppingDistance + 1 && !_owner.navAgent.pathPending)
            {
                _owner.pointsExplored.Add(_owner.pointsOfInterest[_owner.nextWayPoint]);
                _owner.nextWayPoint += 1;

                


            }
        }
        else
        {
            //_owner.stateMachine.ChangeState(SeekTreasureState.Instance);
        }
        

    }
}
