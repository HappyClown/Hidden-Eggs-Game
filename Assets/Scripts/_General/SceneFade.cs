using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class SceneFade : MonoBehaviour 
{
	public static bool fadeSceneOut;
	private bool fadeSceneIn;

	private static string sceneToLoad;

	public GameObject titleCardObj;
	private Image titleCardImg;
	private FadeInOutBoth titleCardFadeScript;
	public TextMeshProUGUI titleCardTxt;
	[TooltipAttribute("The minimum amount of time that the title card will be shown.")]
	public float minTitleCardShowTime;
	private float titleCardTimer;
	public Image fadeImage;
	[TooltipAttribute("The time it takes for the transition to fade in/out in seconds.")]
	public float fadeTime;

	private static float curveTime;
	private static float newAlpha;
	public AnimationCurve animCurve;
	private string currentScene;



	void Awake () 
	{
		titleCardImg = titleCardObj.GetComponent<Image>();
		titleCardFadeScript = titleCardObj.GetComponent<FadeInOutBoth>();
		titleCardImg.color = new Color(titleCardImg.color.r, titleCardImg.color.g, titleCardImg.color.b, 0f);
		titleCardTxt.color = new Color(titleCardTxt.color.r, titleCardTxt.color.g, titleCardTxt.color.b, titleCardImg.color.a);
	}



	void Update () 
	{
		// Keep track of the current active scene.
		currentScene = SceneManager.GetActiveScene().name;

		titleCardTxt.color = new Color(titleCardTxt.color.r, titleCardTxt.color.g, titleCardTxt.color.b, titleCardImg.color.a);

		// -- FADE OUT CURRENT SCENE -- //
		if (fadeSceneOut)
		{
			if (fadeSceneIn) { fadeSceneIn = false; }
			// Set the transition image to raycast target to block the player from tapping on any buttons while it is transitioning.
			if (!fadeImage.raycastTarget) { fadeImage.raycastTarget = true; }
			curveTime += Time.deltaTime / fadeTime;
			newAlpha = animCurve.Evaluate(curveTime);
			fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
			
			if (newAlpha >= 0.8)
			{
				if (titleCardTxt.text != sceneToLoad) { titleCardTxt.text = sceneToLoad; }
				// start playing music for the scene here
				if (!titleCardObj.activeInHierarchy) 
				{
					titleCardObj.SetActive(true); 
					titleCardFadeScript.FadeIn();
				}

				if (titleCardImg.color.a >= 1 && titleCardTimer < minTitleCardShowTime)
				{
					titleCardTimer += Time.deltaTime;
				}
			}

			// If transition and titlecard faded in completely.
			if (newAlpha >= 1 && titleCardImg.color.a >= 1 && currentScene != sceneToLoad)
			{
				curveTime = 1;
				newAlpha = 1;		
				SceneManager.LoadScene(sceneToLoad);
			}
			// If scene has been loaded and title card has been on long enough.
			if (currentScene == sceneToLoad && titleCardTimer >= minTitleCardShowTime)
			{
				fadeSceneIn = true;
				fadeSceneOut = false;
				SceneManager.LoadScene(sceneToLoad);
				titleCardTimer = 0f;
			}
		}

		// -- FADE IN NEW SCENE -- //
		if (fadeSceneIn)
		{
			titleCardFadeScript.FadeOut();
			if (titleCardImg.color.a <= 0.5f);
			{
				curveTime -= Time.deltaTime / fadeTime;
				newAlpha = animCurve.Evaluate(curveTime);
				fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
			}
			if (newAlpha <= 0.2) { if (fadeImage.raycastTarget) { fadeImage.raycastTarget = false; } }
			if (newAlpha <= 0)
			{
				//if (fadeImage.raycastTarget) { fadeImage.raycastTarget = false; }
				curveTime = 0;
				newAlpha = 0;
				fadeSceneIn = false;
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
