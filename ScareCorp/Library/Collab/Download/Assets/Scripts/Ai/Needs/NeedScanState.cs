﻿
/// <summary>
/// 
///   This state scans a huge area of the enviroment through walls to find all the need objects.
///   Saving these to a list for use when the visitors needs to replenish one of these.
/// 
/// </summary>

using UnityEngine;
using StateAI;
using needTypes;

public class NeedScanState : State<AI>
{

    private float gameTimer;
    private float seconds;
    private bool timerReached = false;
    private static NeedScanState _instance;

    private NeedScanState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static NeedScanState Instance
    {
        get
        {
            if (_instance == null)
            {
                new NeedScanState();
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



        if (_owner.CheckifCountDownElapsed(2) )
        {
            _owner.stateMachine.ChangeState(SeekNextNeedState.Instance);
        }
      
    }

    private void Scan(AI _owner)
    {

        _owner.navAgent.isStopped = true;
        _owner.transform.Rotate(0, _owner.searchingTurnSpeed * Time.deltaTime, 0);

        RaycastHit hit;
        Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized * _owner.GetLookRange() , Color.magenta);

        int layerMask = 1 << 8;
        if (Physics.SphereCast(_owner.eyes.position, _owner.GetSphereCastRadius(), _owner.eyes.forward, out hit, _owner.GetLookRange() , layerMask
            ))
        {
            Debug.DrawRay(_owner.eyes.position, _owner.eyes.forward.normalized * _owner.GetLookRange() , Color.blue);

            if (hit.transform.gameObject.GetComponent<Buildable>())
            {
                GameObject objHit = hit.transform.gameObject;
                eNeedTypes objectsNeed;
                objectsNeed = objHit.GetComponent<Buildable>().needtype;

                switch (objectsNeed)
                {
                    case eNeedTypes.hunger:
                        if (!_owner.hungerObjects.Contains(objHit)) { _owner.hungerObjects.Add(objHit); }
                        break;
                    case eNeedTypes.boredom:
                        if (!_owner.boredomObjects.Contains(objHit)) { _owner.boredomObjects.Add(objHit); }
                        break;
                    case eNeedTypes.hygiene:
                        if (!_owner.hygieneObjects.Contains(objHit)) { _owner.hygieneObjects.Add(objHit); }
                        break;
                    case eNeedTypes.tiredness:
                        if (!_owner.tirednessObjects.Contains(objHit)) { _owner.tirednessObjects.Add(objHit); }
                        break;
                    case eNeedTypes.none:
                        break;
                    default:
                        break;

                }

            }

        }

    } 


}
