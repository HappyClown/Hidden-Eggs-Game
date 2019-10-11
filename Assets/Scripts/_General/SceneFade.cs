using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class SceneFade : MonoBehaviour 
{
	public bool fadeSceneOut;
	private bool fadeSceneIn;

	private string sceneToLoad;
	private string currentScene;
	private bool titCardSceneTrans, whtSceneTrans, inTransition;
	private Image fadeImage;
	public AnimationCurve animCurve;
	private float curveTime;
	private float newAlpha;
	//private List<ParticleSystem> cadreParticles;
	

	[Header("Title Card")]
	public GameObject titleCardObj;
	public TextMeshProUGUI titleCardTxt;
	[TooltipAttribute("The minimum amount of time that the title card will be fully shown.")]
	public float minTitleCardShowTime;
	[TooltipAttribute("When to start fading in the title card; based on the darkened background's alpha value.")]
	public float startTitleCardFade;
	private Image titleCardImg;
	public FadeInOutImage titleCardFadeScript;
	private SeasonCadre seasonCadreScript;
	public SeasonCadresManager seasonCadreManScript;
	//private static SeasonCadre seasonCadreScriptStatic;
	//public static SeasonCadresManager seasonCadreManScriptStatic;
	private float titleCardTimer;
	private bool changeCadreStuff;

	[Header("Fade To Black")]
	public Image blckFadeImage;
	[TooltipAttribute("The time it takes for the transition to fade in/out in seconds.")]
	public float fadeTime;
	[TooltipAttribute("When to start fading out the background; Based on the Darkened Titlecard's alpha value.")]
	public float startBackgroundFade;

	[Header("Fade To White")]
	public Image whtFadeImage;
	[TooltipAttribute("The time it takes for the transition to fade in/out in seconds.")]
	public float whtFadeTime;
	[TooltipAttribute("When to start fading out the background; Based on the White Background Alpha value.")]
	public float whtStartBackgroundFade;

	//public List<GameObject> titleCards;
	//public List<TextMeshProUGUI> titleTexts;
	//public bool setupNewCard;

	//public AudioTransitions audioTransScript;
	public AudioTransitions audioTransScript;

	private AsyncOperation myOperation;

	void Awake () 
	{
		titleCardImg = titleCardObj.GetComponent<Image>();
		titleCardImg.color = new Color(titleCardImg.color.r, titleCardImg.color.g, titleCardImg.color.b, 0f);
		titleCardTxt.color = new Color(titleCardTxt.color.r, titleCardTxt.color.g, titleCardTxt.color.b, titleCardImg.color.a);
		//seasonCadreManScriptStatic = seasonCadreManScript;
		//audioTransScript = audioTransScript;
	}

	void Update () 
	{
		// Keep track of the current active scene.
		if (currentScene != SceneManager.GetActiveScene().name) { // This causes GC alloc.
			currentScene = SceneManager.GetActiveScene().name;
		}
			
		titleCardTxt.color = new Color(titleCardTxt.color.r, titleCardTxt.color.g, titleCardTxt.color.b, titleCardImg.color.a);

		#region Title Card & Black Background scene transition.
		if (titCardSceneTrans) {
			if(newAlpha == 0) {
				myOperation = SceneManager.LoadSceneAsync(sceneToLoad);
				myOperation.allowSceneActivation = false;
				//QualitySettings.asyncUploadTimeSlice = 2;
			}
			//if (!setupNewCard) { ChoseTitleCard(); }
			if (fadeImage != blckFadeImage) { fadeImage = blckFadeImage;}
			// -- FADE OUT CURRENT SCENE -- //
			if (fadeSceneOut) {
				if (fadeSceneIn) { fadeSceneIn = false; }
				// Set the transition image to raycast target to block the player from tapping on any buttons while it is transitioning.
				if (!fadeImage.raycastTarget) { fadeImage.raycastTarget = true; }
				curveTime += Time.deltaTime / fadeTime;
				newAlpha = animCurve.Evaluate(curveTime);
				fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
				
				if (newAlpha >= startTitleCardFade)
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
					myOperation.allowSceneActivation = true;
				}
				// If scene has been loaded and title card has been on long enough.
				if (/*currentScene == sceneToLoad*/myOperation.isDone && titleCardTimer >= minTitleCardShowTime)
				{
					inTransition = false;
					fadeSceneIn = true;
					fadeSceneOut = false;
					//SceneManager.LoadScene(sceneToLoad);
					titleCardTimer = 0f;
					curveTime = 0;
				}
			}
		
			// -- FADE IN NEW SCENE -- //
			if (fadeSceneIn)
			{
				titleCardFadeScript.FadeOut();
				if (titleCardImg.color.a <= startBackgroundFade)
				{
					curveTime += Time.deltaTime / fadeTime;
					newAlpha = 1-animCurve.Evaluate(curveTime);
					fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
				}

				if (newAlpha <= 0.2) { if (fadeImage.raycastTarget) { fadeImage.raycastTarget = false; } }
				if (newAlpha <= 0)
				{
					//if (fadeImage.raycastTarget) { fadeImage.raycastTarget = false; }
					curveTime = 0;
					newAlpha = 0;
					fadeSceneIn = false;
					titCardSceneTrans = false;
					//setupNewCard = false;
				}
			}
		}
		#endregion
		
		#region White Background scene transition.
		if (whtSceneTrans)
		{
			if (fadeImage != whtFadeImage) { fadeImage = whtFadeImage;}
			// -- FADE OUT CURRENT SCENE -- //
			if (fadeSceneOut)
			{
				if (fadeSceneIn) { fadeSceneIn = false; }
				// Set the transition image to raycast target to block the player from tapping on any buttons while it is transitioning.
				if (!fadeImage.raycastTarget) { fadeImage.raycastTarget = true; }
				curveTime += Time.deltaTime / fadeTime;
				newAlpha = animCurve.Evaluate(curveTime);
				fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);

				// If transition and titlecard faded in completely.
				if (newAlpha >= 1 && currentScene != sceneToLoad)
				{
					curveTime = 1;
					newAlpha = 1;
					SceneManager.LoadScene(sceneToLoad);
				}

				if (titleCardTimer < minTitleCardShowTime) { titleCardTimer += Time.deltaTime;}

				// If scene has been loaded and title card has been on long enough.
				if (currentScene == sceneToLoad && titleCardTimer >= minTitleCardShowTime)
				{
					fadeSceneIn = true;
					fadeSceneOut = false;
					titleCardTimer = 0f;
					curveTime = 0;
					inTransition = false;
				}
			}
		
			// -- FADE IN NEW SCENE -- //
			if (fadeSceneIn)
			{
				curveTime += Time.deltaTime / fadeTime;
				newAlpha = 1-animCurve.Evaluate(curveTime);
				fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
				if (newAlpha <= 0.2) { if (fadeImage.raycastTarget) { fadeImage.raycastTarget = false; } }
				if (newAlpha <= 0)
				{
					//if (fadeImage.raycastTarget) { fadeImage.raycastTarget = false; }
					curveTime = 0;
					newAlpha = 0;
					fadeSceneIn = false;
					whtSceneTrans = false;
				}
			}
		}
		#endregion

	}

	public void SwitchScene (string sceneName) {
		if (!inTransition) {
			inTransition = true;
			newAlpha = 0f;
			titCardSceneTrans = true;
			fadeSceneOut = true;
			sceneToLoad = sceneName;

			seasonCadreScript = seasonCadreManScript.GetCadreInfo(sceneName);
			titleCardImg.sprite = seasonCadreScript.cadreSprite;
			//titleCardFadeScript.img = titleCardImg;
			foreach (ParticleSystem partSys in seasonCadreScript.cadreParticles)
			{
				partSys.Play();
			}

			if (audioTransScript != null) {
				audioTransScript.TransitionScenes(sceneName);
			}
			else {
				audioTransScript = GameObject.FindWithTag("Audio").GetComponent<AudioTransitions>();
				if (audioTransScript != null) {
					audioTransScript.TransitionScenes(sceneName);
				}
			}
		}
	}

	public void SwitchSceneWhiteFade (string sceneName) {
		if (!inTransition) {
			inTransition = true;
			newAlpha = 0f;
			whtSceneTrans = true;
			fadeSceneOut = true;
			sceneToLoad = sceneName;
			if (audioTransScript != null) {
				audioTransScript.TransitionScenes(sceneName);
			}
			else {
				audioTransScript = GameObject.FindWithTag("Audio").GetComponent<AudioTransitions>();
				if (audioTransScript != null) {
					audioTransScript.TransitionScenes(sceneName);
				}
			}
		}
	}

	public void ChoseTitleCard() {
		if (sceneToLoad == GlobalVariables.globVarScript.marketName) {

		}
	}

	//// FOR UNLOCKED PUZZLE SOUND TESTS

	public float getSceneFadeAlpha()
	{
		return newAlpha;
	}

}
