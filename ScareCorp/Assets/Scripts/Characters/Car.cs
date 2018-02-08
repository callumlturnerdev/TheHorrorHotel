
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

   public List<GameObject> wheels;
    private float wheelSpeed = 10*100;
    private float carSpeed = 20;
    public bool driving = true;
    public bool fastForward = false;
    public float passengers = 1;
	// Use this for initialization
	void Start () {
        TimeManager.PlayRateChange += SetCarSpeed;
        TimeManager.timeStopped += SetCarSpeed;
        foreach (Transform child in transform)
        {
            wheels.Add(child.gameObject);
        }
        SetCarSpeed();
	}



    void SetCarSpeed()
    {

        
        carSpeed = 20 * TimeManager.instance.GetPlayRate();
        wheelSpeed = carSpeed * 100;
    }

    void CarMove()
    {
        this.gameObject.transform.Translate(0, carSpeed * Time.deltaTime, 0);
        foreach (GameObject wheel in wheels)
        {
            if (wheel.GetComponent<MeshRenderer>())
            {
                wheel.transform.Rotate(0, 0, wheelSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "carPark")
        {
            driving = false;
            Spawn();
        }

        if (other.tag == "carDestroy")
        {
            Destroy(this.gameObject);
        }
    }

    private void Spawn()
    {
        if (passengers > 0)
        {
            gameObject.GetComponent<VisitorSpawner>().SpawnMoreVisitors(1);
            passengers--;
            StartCoroutine(WaitFor(4));
        }
        else
        {
            StartCoroutine(StartUpCar());
        }
    }

    IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Spawn();

    }
    IEnumerator StartUpCar()
    {

        yield return new WaitForSeconds(2);
        SetIsDriving(true);
    }

    public void SetIsDriving(bool b)
    {
        driving = b;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (driving == true)
        {
            CarMove();
        }
	}
}
