
using UnityEngine;
using StateAI;
using needTypes;
public class SeekNextNeedState : State<AI>
{
    private static SeekNextNeedState _instance;
    private GameObject currentTarget;
    private string UiState;
    private AI aiRef;
    private SeekNextNeedState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }
    public static SeekNextNeedState Instance
    {
        get
        {
            if (_instance == null)
            {
                new SeekNextNeedState();
            }
            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        aiRef = _owner;
        GameManager.ObjectAdd += seeker;
        _owner.UpdateStateUI("Going to thing");
        _owner.switchState = false;
        seeker();
    }

    public override void ExitState(AI _owner)
    {
    }

    public override void UpdateState(AI _owner)
    {
        ScareScan(_owner);
        if (_owner.navAgent.remainingDistance <= _owner.navAgent.stoppingDistance + 1.3f && !_owner.navAgent.pathPending)
        {
            if (currentTarget != null)
            {
                switch (currentTarget.GetComponent<Buildable>().needtype)
                {
                    case eNeedTypes.boredom:
                        _owner.stateMachine.ChangeState(EntertainmentState.Instance);
                        break;

                    case eNeedTypes.hunger:
                        _owner.stateMachine.ChangeState(EatState.Instance);
                        break;

                    case eNeedTypes.hygiene:
                        _owner.stateMachine.ChangeState(CleanState.Instance);
                        break;

                    case eNeedTypes.tiredness:
                    
                        _owner.stateMachine.ChangeState(SleepState.Instance);
                        break;
                }
            }
        }
    }


    void seeker()
    {
        Seek(aiRef);
    }
    void ScareScan(AI _owner)
    {
        _owner.eyes.transform.Rotate(0, _owner.searchingTurnSpeed * Time.deltaTime, 0);
        RaycastHit hit;
         Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized *8, Color.magenta);

        int layerMask = 1 << 8 | 1<< 11;
       
        if (Physics.SphereCast(_owner.eyes.position, _owner.GetSphereCastRadius(), _owner.eyes.forward, out hit, 8, layerMask
            ))
            {
                Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized * 8, Color.red);
            if (hit.transform.gameObject.tag == "scary")
            {
                
                _owner.fearTarget = hit.transform.gameObject;
                _owner.gameObject.GetComponent<Visitor>().SetNextFearObject(hit.transform.gameObject);
                if (!_owner.usedScares.Contains(_owner.fearTarget))
                {
                    _owner.stateMachine.ChangeState(ScaredState.Instance);
                }
            }
        }
    }



    private void Seek(AI _owner)
    {
        float lowestNeed = _owner.aiNeeds.lowestNeedValue();
        if(_owner.aiNeeds.lowestNeedValue() < 0.51f)
        {
        if (_owner.hygiene <= _owner.aiNeeds.lowestNeedValue() )
        {
            if (_owner.hygieneObjects.Count > 0)
            {
                _owner.navAgent.isStopped = false;
                currentTarget = _owner.SelectTarget(_owner.hygieneObjects);
                _owner.currentTarget = currentTarget;
                _owner.navAgent.destination = currentTarget.transform.position;
                _owner.UpdateStateUI("Dirty");

            }
        }
        if (_owner.hunger <= _owner.aiNeeds.lowestNeedValue())
        {
            if (_owner.hungerObjects.Count > 0)
            {
                _owner.navAgent.isStopped = false;
                currentTarget = _owner.SelectTarget(_owner.hungerObjects);
                _owner.currentTarget = currentTarget;
                _owner.navAgent.destination = currentTarget.transform.position;
                _owner.UpdateStateUI("Hungry");
            }
        }

        if (_owner.boredom <= _owner.aiNeeds.lowestNeedValue() )
        {
            if (_owner.boredomObjects.Count > 0)
            {
                _owner.navAgent.isStopped = false;
                currentTarget = _owner.SelectTarget(_owner.boredomObjects);
                _owner.currentTarget = currentTarget;
                _owner.navAgent.destination = currentTarget.transform.position;
                _owner.UpdateStateUI("Bored");
            }
        }

        if (_owner.tiredness <= _owner.aiNeeds.lowestNeedValue() )
        {
            if (_owner.assignedBed)
            {
                _owner.navAgent.isStopped = false;
                currentTarget = _owner.assignedBed;
                _owner.currentTarget = currentTarget;
                _owner.navAgent.destination = currentTarget.transform.position;
                _owner.UpdateStateUI("Tired");
            }
        }
        }
        else
        {
             if (_owner.boredomObjects.Count > 0)
            {
                _owner.navAgent.isStopped = false;
                currentTarget = _owner.SelectTarget(_owner.boredomObjects);
                _owner.currentTarget = currentTarget;
                _owner.navAgent.destination = currentTarget.transform.position;
                _owner.UpdateStateUI("Bored");
            }
        }
        if (currentTarget != null)
        {
        }
        else
        {
            _owner.navAgent.isStopped = true;
            _owner.UpdateStateUI("No Target");
        }
    }






}
