using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HelpButton : MonoBehaviour 
{
	private Button button;

	public SlideInHelpBird birdScript;


	void Start () 
	{
		button = this.GetComponent<Button>();
		button.onClick.AddListener(birdScript.MoveBirdUp);
	}

}
