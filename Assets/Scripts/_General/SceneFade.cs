using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneFade : MonoBehaviour 
{
	[HideInInspector]
	public static bool fadeSceneOut;
	private static string sceneToLoad;

	public Image fadeImage;
	[TooltipAttribute("The time it takes for the transition to fade in/out in seconds.")]
	public float fadeTime;
	private static float curveTime;
	private static float newAlpha;

	public AnimationCurve animCurve;



	void Start () 
	{
		
	}


	
	void Update () 
	{
		if (fadeSceneOut)
		{
			curveTime += Time.deltaTime / fadeTime;
			newAlpha = animCurve.Evaluate(curveTime);
			fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
			
			if (newAlpha >= 1)
			{
				fadeSceneOut = false;
				SceneManager.LoadScene("sceneToLoad");
				newAlpha = 0;
			}
		}
	}



	public static void SwitchScene (string sceneName)
	{
		newAlpha = 0f;
		fadeSceneOut = true;
		sceneToLoad = sceneName;
	}
}
