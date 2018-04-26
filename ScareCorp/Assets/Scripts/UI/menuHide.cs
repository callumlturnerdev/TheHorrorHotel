using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuHide : MonoBehaviour {
    public GameObject menuToHide;
    private bool hidden = true;

    public void ToggleShowMenu()
    {
        if (hidden)
        {
            menuToHide.SetActive(true);
            hidden = false;
        }
        else
        {
            menuToHide.SetActive(false);
            hidden = true;
        }
    }
}
    