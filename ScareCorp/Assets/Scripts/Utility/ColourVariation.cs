using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColourVariation : MonoBehaviour {

    public Color[] clothesColors;
    public GameObject[] ColorChangeParts;

    private void Awake()
    {
        int randNum = Random.Range(0, clothesColors.Length);
        foreach (GameObject clothing in ColorChangeParts)
        {
            clothing.GetComponent<SpriteRenderer>().color= clothesColors[randNum];
        }
    }
}
