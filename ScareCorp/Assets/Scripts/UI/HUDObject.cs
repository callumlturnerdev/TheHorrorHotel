using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HUDObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {


    // This script is used on the hud versions of objects to allow the player to click them and 
    // and set that to the currentnyl held object for the player.
    // Use this for initialization
    public AudioClip OnOverSound;
    public AudioClip OnClick;
    private AudioSource audioS;
    public bool isOver = false;
    private UIInfoPanel infoPanel;
	public GameObject itemToBuild;
    private Text itemCost;
    private float cost;
    public string name;
    public string description;
    private Image image;

    GameObject bg;
    public Color bgColor;
    [SerializeField]
    private bool DeleteModeObj; // Really dodgy way to approach this should replace at some point
    void Awake()
    {
        if(this.transform.childCount > 0)
        {
            bg = this.transform.GetChild(0).gameObject;
            if(bg.GetComponent<Image>())
            {
                bg.GetComponent<Image>().color = bgColor;
            }
        }

       
        audioS = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;     
        infoPanel = GameObject.FindGameObjectWithTag("UIInfoPanel").GetComponent<UIInfoPanel>();
        image = GetComponent<Image>();
        itemCost = this.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        cost = itemToBuild.GetComponent<Buildable>().GetCost();
        if(itemCost)
             itemCost.text = " " + cost;
    }
    public void OnMouseDown()
	{
		BuildController.instance.SetBuildObject (itemToBuild);
        audioS.clip = OnClick;
        audioS.Play();
        if (DeleteModeObj)
        {
            BuildController.instance.DeleteMode();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        audioS.clip = OnOverSound;
        audioS.Play();
        isOver = true;
        infoPanel.UpdateUIInfoPanel(image.sprite, name, description);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
    }
}
