using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

	public Camera mainCam;
	public float   speed = 20.0f;
	public float zoomSpeed = 2.0f;
    private float lerpTimer = 0;
    float X, Y, Z;
    private float smooth = 0.01f;
    private Quaternion originalRotation;
    private Quaternion currentRotation;
	// Use this for initialization
	void Start () {
        originalRotation = this.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) 
		{
			transform.Translate (0, zoomSpeed, 0);
			mainCam.orthographicSize += 1;
		}

		if (Input.GetAxis ("Mouse ScrollWheel") > 0) 
		{
			transform.Translate (0, -zoomSpeed, 0);
			mainCam.orthographicSize -= 1;
		}

	    float yPos = -zoomSpeed * Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime ;
		float zPos = speed * Input.GetAxis ("Vertical") * Time.deltaTime;
        float xPos = speed * Input.GetAxis ("Horizontal") * Time.deltaTime;
        Vector3 newPos = new Vector3 (xPos, yPos, zPos);
		transform.Translate(newPos);

		if (Input.GetMouseButton (2)) {
			Vector3 rot = new Vector3 (0, -120, 0);
			float mouseX = Input.GetAxis ("Mouse X");
			transform.Rotate (rot * mouseX *Time.deltaTime, Space.Self);
		}

        if (Input.GetKeyDown(KeyCode.E))
        {
            originalRotation = this.transform.rotation;
            StopAllCoroutines();
            StartCoroutine(Rotate(-45));
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            originalRotation = this.transform.rotation;
            StopAllCoroutines();
            StartCoroutine(Rotate(45));
        }
    }

    IEnumerator Rotate(float rotationAmount)
    {
        Quaternion finalRotation = Quaternion.Euler(0, rotationAmount, 0) * originalRotation;
        while (this.transform.rotation != finalRotation)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, finalRotation, Time.deltaTime * 10);
            originalRotation = this.transform.rotation;
            yield return 0;
        }
    }

    
}
