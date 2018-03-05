
using UnityEngine;
using StateAI;
using needTypes;
using UnityEngine.AI;
using fearTypes;

public class ScaredState : State<AI>
{
    private static ScaredState _instance;

    private ScaredState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static ScaredState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ScaredState();
            }
            return _instance;
        }
    }


    public override void EnterState(AI _owner)
    {
        _owner.UpdateStateUI("Scared");
        _owner.switchState = false;
        Scared(_owner);
    }

    public override void ExitState(AI _owner)
    {
        _owner.gameObject.GetComponent<Visitor>().TurnOffScareParticle();
        _owner.usedScares.Add(_owner.fearTarget);
        _owner.navAgent.speed = 2;
        _owner.switchState = false;
        
    }

    public override void UpdateState(AI _owner)
    {
        HidingPlaceScan(_owner);
        if ( _owner.CheckifCountDownElapsed(10))
           _owner.stateMachine.ChangeState(SeekNextNeedState.Instance);
    }


    void Scared(AI _owner)
    {
            _owner.gameObject.GetComponent<Visitor>().Scare(50.0f);
            _owner.navAgent.isStopped = false;
            Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized * -10, Color.red);
            _owner.navAgent.speed = 4;
            _owner.navAgent.destination = _owner.transform.position + (_owner.transform.forward.normalized * -30);
     }

    void HidingPlaceScan(AI _owner)
    {
        _owner.eyes.transform.Rotate(0, _owner.searchingTurnSpeed * Time.deltaTime, 0);
        RaycastHit hit;
        Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized * 8, Color.magenta);

        int layerMask = 1 << 8 | 1 << 11;

        if (Physics.SphereCast(_owner.eyes.position, _owner.GetSphereCastRadius(), _owner.eyes.forward, out hit, 8, layerMask
            ))
        {
            Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized * 8, Color.red);

            if (hit.transform.gameObject.GetComponent<Buildable>())
            {
                if (hit.transform.gameObject.GetComponent<Buildable>().needtype == eNeedTypes.hidden)
                {
                    if(hit.transform.gameObject.GetComponent<NavMeshObstacle>())
                    {
                        hit.transform.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
                        if (hit.transform.gameObject.GetComponent<wardrobeTrigger>().hasZombie != true)
                        {
                            _owner.navAgent.destination = hit.transform.gameObject.transform.position;
                        }
                    }
                }
            }
        }

    }
            
   }


