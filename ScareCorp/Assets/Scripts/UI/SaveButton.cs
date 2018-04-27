using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveButton : MonoBehaviour {

	// Use this for initialization
	public string sceneToLoad;
	public bool loadSave = true;
	private string SavePathName;
	private SavingLoading saveSys;
	 public UnityEngine.UI.InputField slot1;
	void Awake()
	{
		saveSys = GameObject.FindGameObjectWithTag("save").GetComponent<SavingLoading>();
	}

	public void SaveData()
	{
		saveSys.Save("/"+sceneToLoad+".dat");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}
	public void LoadData()
	{
		if(!loadSave)
		{
			saveSys.LoadLevel("", sceneToLoad);
		}
		else
		{
		//saveSys.Load("/"+slot1.text.ToString()+".dat");
			saveSys.LoadLevel("/"+sceneToLoad+".dat", sceneToLoad);
		}
	}
	public void SetSavePath(UnityEngine.UI.InputField str)
	{
		SavePathName = str.text.ToString();
	}
}
