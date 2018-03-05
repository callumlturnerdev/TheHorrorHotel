using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HUDMouseCheck : MonoBehaviour {
    // SIMPLE SCRIPT THAT CHECKS IF THE MOUSE IS COLLIDING WITH THE HUD ELEMENTS AND IF SO SETS THE CURSOR BUILDER TO NOT BUILD STUFF IN THE WORLD
    // Use this for initialization
    public Button mybutton;
    private BoxCollider mycol;
    public Camera camview;

    public void Init()
    {
        mybutton = GetComponent<Button>();
    }
    public void Awake()
    {
        mycol = this.gameObject.GetComponent<BoxCollider>();
    }


    void OnMouseEnter()
	{

        BuildController.instance.NotOnHud = false;
	}

	void OnMouseExit()
	{
        BuildController.instance.NotOnHud = true;
	}
}
