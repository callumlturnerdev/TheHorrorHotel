using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour {

	// Use this for initialization
	private string SavePathName;
	private SavingLoading saveSys;
	 public UnityEngine.UI.InputField slot1;
	void Awake()
	{
		saveSys = GameObject.FindGameObjectWithTag("save").GetComponent<SavingLoading>();
	}

	public void SaveData()
	{
		saveSys.Save("/"+slot1.text.ToString()+".dat");
	}

	public void LoadData()
	{
		saveSys.Load("/"+slot1.text.ToString()+".dat");
	}
	public void SetSavePath(UnityEngine.UI.InputField str)
	{
		SavePathName = str.text.ToString();
	}
}
