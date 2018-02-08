using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishOnKeyPress : MonoBehaviour {

    private MeshRenderer mesh;
    public Material hiddenMat;
    public Material originalMat;
	// Use this for initialization
	void Start () {
        originalMat = GetComponent<MeshRenderer>().material;
        GameManager.ToggleTopWalls += ToggleHidden;
        mesh = GetComponent<MeshRenderer>();
        if (GameManager.instance.GetWallsHidden() == true)
        {
            mesh.material = hiddenMat;
        }
    }

    private void OnDestroy()
    {
        GameManager.ToggleTopWalls -= ToggleHidden;
    }
    void ToggleHidden()
    {
        if (GameManager.instance.GetWallsHidden() == true)
        {
            if (GetComponent<MeshRenderer>())
            {
                mesh = this.gameObject.GetComponent<MeshRenderer>();
                mesh.material = hiddenMat;
            }
        }
        else
        {
            if (GetComponent<MeshRenderer>() != null)
            {
                mesh = this.gameObject.GetComponent<MeshRenderer>();
                mesh.material = originalMat;
            }
        }
    }

}
