using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using needTypes;
using fearTypes;
public class Buildable : MonoBehaviour {
    public int objectID;
    public int index;
    public eNeedTypes needtype;
    public eFearTypes fearType;
    public float uses =999999;
    [Range(0,1)]
    public float needFulfillment;
    public float itemCost;
    [Range(0,10)]
    public int priority;
    Rigidbody rb;
    // Use this for initialization
    void Start() {
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
                if (!GameManager.instance.tirednessObjects.Contains(this.gameObject)) { GameManager.instance.tirednessObjects.Add(this.gameObject); }
                break;
            case eNeedTypes.hidden:
                if (!GameManager.instance.hidingPlaces.Contains(this.gameObject)) { GameManager.instance.hidingPlaces.Add(this.gameObject); }
                break;
        }
        if ( transform.childCount > 0 )
        {
            if (transform.GetChild(0).GetComponent<Rigidbody>() != null)
            {
                rb = transform.GetChild(0).GetComponent<Rigidbody>();
                StartCoroutine(DestroyRigidBodies());
            }
        }
	}
    IEnumerator DestroyRigidBodies()
    {
        yield return new WaitForSeconds(3);
        Destroy(rb);
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
}
