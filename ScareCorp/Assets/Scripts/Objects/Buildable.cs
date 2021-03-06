﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using needTypes;
using fearTypes;
using UnityEngine.UI;
using UnityEngine.AI;
public class Buildable : MonoBehaviour {

    [SerializeField]
    private Transform visInteractPos;
    NavMeshObstacle navMeshObstacle;
    bool hasBeenActivated;
    public GameObject floatingTextObj;
    public float particleTimer;
    public int objectID;
    public int index;
    public eNeedTypes needtype;
    public eFearTypes fearType;

    [Header("NeedItem")]
    public float uses =999999;
    [Range(0,1)]
    public float needFulfillment;
    public List<GameObject> currentUsers; //Keeps track of current user count.
    public float itemCost;
    [Range(0,10)]
    public int priority;
    [SerializeField]
    private bool largeObject = false; // TEMP VARIABLE FOR DETERMINING HOW MANY GRID SLOTS TO USE WHEN BUILDING
    Rigidbody rb;
    public GameObject particleSmoke;
    AudioSource audioS;
    // Use this for initialization
    void Start() {
        currentUsers = new List<GameObject>();
        hasBeenActivated = false;
        /* 
        if ( transform.childCount > 0 )
        {
            if (transform.GetChild(0).GetComponent<Rigidbody>() != null)
            {
                rb = transform.GetChild(0).GetComponent<Rigidbody>();
                StartCoroutine(DestroyRigidBodies());
            }
        }
        */
	}

    public void ActivateGravity()
    {
       switch (needtype)
        {
            case eNeedTypes.boredom:
                if (!GameManager.instance.boredomObjects.Contains(this.gameObject)) { GameManager.instance.boredomObjects.Add(this.gameObject); }
                break;
            case eNeedTypes.hunger:
                if (!GameManager.instance.hungerObjects.Contains(this.gameObject)) { GameManager.instance.hungerObjects.Add(this.gameObject); }
                break;
            case eNeedTypes.hygiene:
                if (!GameManager.instance.hygieneObjects.Contains(this.gameObject)) { GameManager.instance.hygieneObjects.Add(this.gameObject); }
                break;
            case eNeedTypes.tiredness:
                if (!GameManager.instance.tirednessObjects.Contains(this.gameObject)) { GameManager.instance.tirednessObjects.Add(this.gameObject);
                    GameManager.instance.AddToBedCount(1);
                 }
                break;
            case eNeedTypes.hidden:
                if (!GameManager.instance.hidingPlaces.Contains(this.gameObject)) { GameManager.instance.hidingPlaces.Add(this.gameObject); }
                break;
        }
         //obj.transform.position = transform.position;

        if(gameObject.GetComponent<Rigidbody>())
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            StartCoroutine(DestroyRigidBodies());
        }
        else
        {
            if(transform.childCount > 0)
            {
                if (transform.GetChild(0).GetComponent<Rigidbody>() != null)
                {
                    rb = transform.GetChild(0).GetComponent<Rigidbody>();
                    rb.useGravity = true;
                    StartCoroutine(DestroyRigidBodies());
                }
            }
             
        }
        StartCoroutine(PlaySmokeParticle(particleTimer));
          
        
    }

    public void AddCurrentUser(GameObject obj)
    {
        if(!currentUsers.Contains(obj))
        {
            currentUsers.Add(obj);
        }
    }

    public void RemoveCurrentUser(GameObject obj)
    {
        if(currentUsers.Contains(obj))
        {
            currentUsers.Remove(obj);
        }
    }
    IEnumerator DestroyRigidBodies()
    {
        yield return new WaitForSeconds(3.0f);
           
        Destroy(rb);
    }
    IEnumerator PlaySmokeParticle(float t)
    {
        yield return new WaitForSeconds(t);
        Vector3 trans = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
        GameObject obj = Instantiate(particleSmoke) as GameObject;
        GameObject floatText = Instantiate(floatingTextObj) as GameObject;
        
        floatText.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().text = "-" + itemCost;
        floatText.transform.parent = this.transform;
        if(GetComponent<NavMeshObstacle>())
        {
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            navMeshObstacle.enabled = true;
        }
        if(GetComponent<SphereCollider>())
        {
            GetComponent<SphereCollider>().enabled = true;
        }

        Vector3 floatTrans = new Vector3(gameObject.transform.position.x-20,gameObject.transform.position.y,gameObject.transform.position.z);
          floatText.transform.position = floatTrans;
        floatText.transform.rotation = Quaternion.Euler(0,0,0);
        obj.transform.parent = this.transform;
        obj.transform.position = this.gameObject.transform.position;
        hasBeenActivated = true;
    }
    public bool GetHasBeenActivated(){return hasBeenActivated;}
    void UnassignBed()
    {
       if (!GameManager.instance.tirednessObjects.Contains(this.gameObject)) 
       { 
           GameManager.instance.tirednessObjects.Add(this.gameObject);
       }
    }
  
    void OnDestroy()
    {
        if(GameManager.instance)
        {
            GameManager.instance.RemoveObject(this.gameObject);
           // GameManager.instance.AddToBedCount(-1);
        }
    }
    public void hasBeenUsed()
    {
        uses -= 1;
        if (uses < 1)
        {
            GameManager.instance.RemoveObject(this.gameObject);
            Destroy(gameObject);
        }
    }

    public eFearTypes GetFearType()
    {
        return fearType;
    }
    public float GetCost()
    {
        return itemCost;
    }

    public void SetIndex(int newIndex)
    {
        objectID =newIndex;
    }

    public bool IsLargeObject() {return largeObject;}
    public Transform GetVisInteractPos()
    {
        if(visInteractPos)
        {
            return visInteractPos;
        }
        else
        {
            return null;
        }
    }
}
