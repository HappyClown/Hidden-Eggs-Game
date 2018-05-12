using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
	[Header("Summer")]
	public bool summerUnlocked;
	public SeasonDissolve summerDissolve;

	public GameObject summerParkBtn;
	public GameObject summerMarketBtn;
	//public Button summerBeachBtn;

	// public string summerParkBtnSceneName;
	// public string summerMarketBtnSceneName;

	[HeaderAttribute("Fall")]
	public bool fallUnlocked;



	void Start () 
	{
		//if (summerParkBtn) { summerParkBtn.onClick.AddListener(OpenScene01); }
		//if (summerMarketBtn) { summerMarketBtn.onClick.AddListener(OpenScene02); }
	}
	


	void Update ()
	{
		if (summerDissolve.doneDissolving)
		{
			summerParkBtn.SetActive(true);
			summerMarketBtn.SetActive(true);
			//summerParkBtn.gameObject.SetActive(true);
			//summerMarketBtn.gameObject.SetActive(true);
			//summerBeachBtn.gameObject.SetActive(true);
		}
	}


	// public void OpenScene01 () 
	// {
	// 	SceneManager.LoadScene(summerParkBtnSceneName);
	// }

	// public void OpenScene02 () 
	// {
	// 	SceneManager.LoadScene(summerMarketBtnSceneName);
	// }
}
