using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonDissolve : MonoBehaviour 
{
	public float dissAmnt;
	public float dissSpeed;
	public AnimationCurve animCurve;
	public Material dissShader;

	public bool doneDissolving;
	public MainMenuPlay mainMenuPlayScript;

	public GameObject parkButton;
	public GameObject marketButton;
	public GameObject beachButton;

	public GameObject parkGlow;
	public GameObject marketGlow;
	public GameObject beachGlow; 



	void Start ()
	{
		dissShader.SetFloat ("_Threshold", 0f);
	}


	void Update () 
	{
		if (mainMenuPlayScript.enableLevelSelection)
		{
			dissShader.SetFloat ("_Threshold", dissAmnt);

			if (dissAmnt < 1.01f)
			{
				dissAmnt += Time.deltaTime * dissSpeed;

				if (dissAmnt >= 1.01f)
				{
					doneDissolving = true;
				}
			}
		}

		if (doneDissolving)
		{
			if (!parkButton.activeSelf) { parkButton.SetActive(true); }
			if (!marketButton.activeSelf) { marketButton.SetActive(true); }
			if (!beachButton.activeSelf) { beachButton.SetActive(true); }
			if (!parkGlow.activeSelf) { parkGlow.SetActive(true); }
			if (!marketGlow.activeSelf) { marketGlow.SetActive(true); }
			if (!beachGlow.activeSelf) { beachGlow.SetActive(true); }
		}
	}
}
