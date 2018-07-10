using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hub : MonoBehaviour 
{
	[Header("Dissolve")]
	public float dissAmnt;
	public float dissSpeed;
	public List<Material> dissolveMats;
	public List<bool> unlockedSeasons;
	public bool doneDissolving;

	[Header("Other Objects")]
	public GameObject backToMenu;
	public Button backToMenuBtn;

	[Header("Summer Objects")]
	public bool summerUnlocked;
	public List<GameObject> summerObjs;

	[Header("Fall Objects")]
	public bool fallUnlocked;

	[Header("What To Do Bools")]
	public bool dissolving;

	[Header("References")]
	public MainMenu mainMenuScript;

	public class Season 
	{
		public string name;
		public bool unlocked;
		public Material dissolveMat;
		public List<GameObject> seasonObjs;
	}

	

	void Start ()
	{
		for (int i = 0; i < unlockedSeasons.Count; i++)
		{
			if (unlockedSeasons[i]) { dissolveMats[i].SetFloat ("_Threshold", 0f); }

		}

		// foreach(Material dissolveMat in dissolveMats)
		// {
		// 	dissolveMat.SetFloat ("_Threshold", 0f);
		// }
	}



	void Update () 
	{
		if (dissolving)
		{
			foreach(Material dissolveMat in dissolveMats)
			{
				dissolveMat.SetFloat ("_Threshold", dissAmnt);
			}

			if (dissAmnt < 1.01f)
			{
				dissAmnt += Time.deltaTime * dissSpeed;
			}
			else 
			{
				dissolving = false;
				DoneDissolving();
			}
		}
	}



	void DoneDissolving()
	{
		// - ENABLE GAMEOBJECTS - // (Seperation lines, buttons, etc)
		foreach(GameObject summerObj in summerObjs)
		{
			if (!summerObj.activeSelf) { summerObj.SetActive(true); }
		}

		backToMenu.SetActive(true);
		backToMenuBtn.enabled = true;
	}



	public void ResetHubSeasons()
	{
		dissAmnt = 0f;

		foreach(Material dissolveMat in dissolveMats)
		{
			dissolveMat.SetFloat ("_Threshold", 0f);
		}

		foreach(GameObject summerObj in summerObjs)
		{
			if (summerObj.activeSelf) { summerObj.SetActive(false); }
		}
	}
}
