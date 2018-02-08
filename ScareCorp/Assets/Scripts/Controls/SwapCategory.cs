using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SwapCategory : MonoBehaviour {



    public GameObject[] catergories;
    public GameObject[] catButtons;
    public Color highlightedColor;
    public  Color oldColor;

    void Start () {
        catergories[1].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetButtonToActive(GameObject buttonPressed)
    {


        foreach (GameObject but in catButtons)
        {
            but.GetComponent<Image>().color = oldColor;
        }
        buttonPressed.GetComponent<Image>().color = highlightedColor;
    }

    public void SwapCat(int index )
    {

        
        foreach (GameObject cat in catergories)
        {
            cat.SetActive(false);

        }
        
                catergories[index].SetActive(true);
           
    }


}
