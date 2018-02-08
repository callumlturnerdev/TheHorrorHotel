using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HUDObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {


    // This script is used on the hud versions of objects to allow the player to click them and 
    // and set that to the currentnyl held object for the player.
    // Use this for initialization

    public bool isOver = false;
    private UIInfoPanel infoPanel;
	public GameObject itemToBuild;
    private Text itemCost;
    private float cost;

    //
    public string name;
    public string description;
    private Image image;

    void Awake()
    {
        infoPanel = GameObject.FindGameObjectWithTag("UIInfoPanel").GetComponent<UIInfoPanel>();
        image = GetComponent<Image>();
        itemCost = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        cost = itemToBuild.GetComponent<Buildable>().GetCost();
        itemCost.text = " " + cost;
    }

    public void OnMouseDown()
	{
		BuildController.instance.SetBuildObject (itemToBuild);
		Debug.Log ("Item Selected =" + itemToBuild.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
        infoPanel.UpdateUIInfoPanel(image.sprite, name, description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
    }
}
