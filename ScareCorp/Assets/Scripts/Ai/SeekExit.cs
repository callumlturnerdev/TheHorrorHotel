
using UnityEngine;

using StateAI;

public class SeekExit : State<AI>
{
    private static SeekExit _instance;

    private SeekExit()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static SeekExit Instance
    {
        get
        {
            if (_instance == null)
            {
                new SeekExit();
            }

            return _instance;
        }
    }


    public override void EnterState(AI _owner)
    {
        _owner.UpdateStateUI("Leaving");
        Seek(_owner);
    }

    public override void ExitState(AI _owner)
    {

    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.navAgent.remainingDistance <= _owner.navAgent.stoppingDistance + 1.5f && !_owner.navAgent.pathPending)
        {
            _owner.DestroyOwner();
        }
    }


    void Seek(AI _owner)
    {
        if (_owner != null)
        {
            _owner.navAgent.destination = _owner.exitSeek.position;
            _owner.navAgent.isStopped = false;
        }
    }
}
