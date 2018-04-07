using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {


    public GameObject[] car;
    public int visitorNum;
    public Transform spawnPos;
    public GameObject carType;
    private bool canSpawn;
    private bool spawnFF;
    // Use this for initialization
    void Start()
    {
       // TimeManager.DayChanged += DayChange;
        //TimeManager.ArriveTime += DayChange;
        canSpawn = true;
        spawnFF = false;
        EventManager.PauseClicked += Pause;
        EventManager.FastFClicked += FastForward;
        GameManager.BedAdded += DayChange;
        //SpawnMoreVisitors (0);
    }

    public void DayChange()
    {
        SpawnMoreCars(1);
        
    }


    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (canSpawn)
            {
                SpawnMoreCars(1);
            }
        }


    }


    public void SpawnMoreCars(int num)
    {
        if(GameManager.instance.FreeBedCheck())
        {
            for (int i = 0; i < num; i++)
            {
                GameManager.instance.AddToTakenBeds(1);
                GameObject car = Instantiate(carType) as GameObject;
                car.GetComponent<Car>().passengers = 1;
                car.transform.position = transform.position;
                car.SetActive(true);
                car.transform.position = transform.position;
                if (spawnFF)
                {
                    //car.GetComponent<Car>().FastForward();
                }
            }
        }

    }

    void OnDisable()
    {
        EventManager.PauseClicked -= Pause;
        EventManager.FastFClicked -= FastForward;

    }

    private void Pause()
    {
        canSpawn = !canSpawn;
    }

    private void FastForward()
    {
        spawnFF = !spawnFF;
    }


}
