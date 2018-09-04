using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseMenu : MonoBehaviour 
{
	public Button closeMenuOnClickButton;
	//public GameObject closeMenuOnClick;

	public SlideInHelpBird birdScript;



	void Start () 
	{
		closeMenuOnClickButton = this.GetComponent<Button>();
		closeMenuOnClickButton.onClick.AddListener(birdScript.MoveBirdUp);
		this.gameObject.SetActive(false);
	}
}
