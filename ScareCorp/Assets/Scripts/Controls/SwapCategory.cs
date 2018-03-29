using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SwapCategory : MonoBehaviour {

    [SerializeField]
    AudioClip UIClick;
    AudioSource audioS;
    public GameObject[] catergories;
    public GameObject[] catButtons;
    public Color highlightedColor;
    public  Color oldColor;

    void Start ()
    {
        catergories[1].SetActive(true);
        audioS = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        audioS.clip = UIClick;
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
       audioS.Play();
        foreach (GameObject cat in catergories)
        {
            cat.SetActive(false);
        } 
       catergories[index].SetActive(true);    
    }
}
