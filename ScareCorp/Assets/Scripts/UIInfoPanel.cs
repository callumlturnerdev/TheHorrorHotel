using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour {

    public GameObject ObjectIcon;
    public GameObject ObjectName;
    public GameObject ObjectDescription;

    private Image Icon;
    private Text name;
    private Text description;
	// Use this for initialization
	void Awake () {
        Icon = ObjectIcon.GetComponent<Image>();
        name = ObjectName.GetComponent<Text>();
        description = ObjectDescription.GetComponent<Text>();
	}
	
    public void UpdateUIInfoPanel(Sprite _icon, string _name, string _text)
    {
        Icon.sprite =_icon;
        name.text = _name;
        description.text = _text;
    }
}
