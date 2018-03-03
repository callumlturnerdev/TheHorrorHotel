using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class HUDGetElements : MonoBehaviour {
    public string itemsLocation;
	public GameObject[] items;
	private float xPos = 0;
    private RectTransform rectT;
	// Use this for initialization
	void Start () {
        rectT = GetComponent<RectTransform>();
		xPos = this.transform.position.x - 10;
        items = Resources.LoadAll(itemsLocation, typeof(GameObject)).Cast<GameObject>().ToArray();

		foreach (Object item in items) 
		{
            xPos += 90f;
           
            GameObject obj = Instantiate (item) as GameObject;
            obj.transform.SetParent(this.transform); 
	
			obj.transform.position = new Vector3 (xPos, this.transform.position.y- 30, this.transform.position.z);
            
            obj.GetComponent<HUDMouseCheck>().Init();
            Debug.Log(obj.name);
			Debug.Log ("Spawned");
		}
	}
}
