
using UnityEngine;
using StateAI;

public class ScanState : State<AI>
{

    /// <summary>
    ///  THIS STATE DOES A QUICK 360 SEARCH OF THE ENVIROMENT ADDING THESE TO A POINTS OF 
    ///  INTEREST ARRAY IN GAMEOBJECTS
    /// </summary>
    /// 


    private float gameTimer;
    private float seconds;
    private bool timerReached = false;
    private static ScanState _instance;
    
    private ScanState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static ScanState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ScanState();
            }

            return _instance;
        }
    }
    public override void EnterState(AI _owner)
    {
        _owner.UpdateStateUI("Searching");
         seconds = 0;
        DebugConsole.Log("Entering ScanState");
       
       
    }

    public override void ExitState(AI _owner)
    {
        DebugConsole.Log("Exiting ScanState");
    }

    public override void UpdateState(AI _owner)
    {
        Scan(_owner);



        if (_owner.CheckifCountDownElapsed(2) && _owner.pointsOfInterest.Count > 0)
        {
            _owner.stateMachine.ChangeState(PatrolState.Instance);
        }
        else if (_owner.CheckifCountDownElapsed(5))
        {
          //  _owner.stateMachine.ChangeState(SeekTreasureState.Instance);
        }
    }

    private void Scan(AI _owner)
    {
       
        _owner.navAgent.isStopped = true;
        _owner.transform.Rotate(0, _owner.searchingTurnSpeed * Time.deltaTime, 0);

        RaycastHit hit;
        Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized * _owner.GetLookRange() , Color.magenta);

        int layerMask = 1 << 8;
        if (Physics.SphereCast(_owner.eyes.position, _owner.GetSphereCastRadius(), _owner.eyes.forward, out hit, _owner.GetLookRange(), layerMask
            ))
        {
            if (hit.transform.gameObject.tag == "scary")
            {
                _owner.fearTarget = hit.transform.gameObject;
                _owner.stateMachine.ChangeState(ScaredState.Instance);
            }

            Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized * _owner.GetLookRange(), Color.blue);

            if (hit.transform.gameObject.tag == "Door" || hit.transform.gameObject.tag == "Chest" || hit.transform.gameObject.tag == "scary")
            {
                if (!_owner.pointsExplored.Contains(hit.transform.gameObject))
                {
                    if (!_owner.pointsOfInterest.Contains(hit.transform.gameObject))
                    {
                        _owner.pointsOfInterest.Add(hit.transform.gameObject);
                    }
                }

            }
        }

    }

    
}
