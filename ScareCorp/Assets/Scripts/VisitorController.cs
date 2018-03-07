using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using fearTypes;
public class VisitorController : MonoBehaviour {

	public Image VisImageUI;
	public Sprite[] visSprites;
	public Text VisNameUI,VisDaysUI, VisFearUI;
 	public static VisitorController instance = null;
	private string visitorName;
	private int daysStaying;
	private eFearTypes fearType;
	private bool genderMale;
	// Use this for initialization
	void Awake () 
	{
		
		 if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);


		RandomiseVisitorInfo();


	}
	private void SetVisitorUI()
	{
		VisNameUI.text = "Name:  " + visitorName; VisDaysUI.text =  "Days:  " +daysStaying; VisFearUI.text =  "Fear:  " + fearType.ToString();
		if(genderMale)
		{
			VisImageUI.sprite = visSprites[0];
		}
		else
		{
			VisImageUI.sprite = visSprites[1];
		}
	}
	public bool GetIsGenderMale(){return genderMale;}
	public string GetAVisitorName() {return visitorName;}
	public int GetAVisitorStayDays(){return daysStaying;}
	public eFearTypes GetVisitorFear(){return fearType;}

	public void RandomiseVisitorInfo()
	{
		int randnum = Random.Range(0,2);
		if(randnum == 1){genderMale = true;}else {genderMale = false;}
		
		visitorName = GetRandomName(genderMale);
		daysStaying = 1;
		fearType = GetRandomFear();
		SetVisitorUI();
	}

	private eFearTypes GetRandomFear()
	{
		int randNum = Random.Range(0, 4);
		switch(randNum)
		{
			case 0:
				return eFearTypes.Enviroment;
			case 1:
				return eFearTypes.Gore;
			case 2:
				return eFearTypes.JumpScare;
			case 3:
				return eFearTypes.none;
			case 4:
				return eFearTypes.Seperation;
			default:
				break;
		}
		return eFearTypes.none;
	}
	//public string GetName() {return visitorName.text;}
    private string  GetRandomName(bool man)
    {
        string[] menNames = new string[] {"Bob", "Dave" , "Malcolm" , "Justin" , "Bruce" , "Alan" , "Jerry" , "Mark" , "Jeremy"};
         string[] womenNames = new string[] {"Sarah", "Ripley" , "Clair" , "Jill" , "Barbara" , "Lola" , "Ramona" , "Tracy" , "Margaret"};
        if(man)
        {
            int randnum = Random.Range(0, menNames.Length);
            return menNames[randnum];
        }
        else
        {
            int randnum = Random.Range(0, womenNames.Length);
            return womenNames[randnum];
        }
    }
}
