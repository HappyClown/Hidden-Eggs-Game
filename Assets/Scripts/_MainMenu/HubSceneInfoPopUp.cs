using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HubSceneInfoPopUp : MonoBehaviour 
{
	public List<FadeInOutImage> imgFadeScripts;

	public List<Image> imgsToFade;
	public List<TextMeshProUGUI> tMPsToFade;
	public bool popUpOn, popUpOff, imgFadeCalled, tMPsFaded, allFaded; 
	public float alphaValue, fadeInDuration, fadeOutDuration;


	void Update()
	{
		if (Input.GetKeyDown("f"))
		{
			PopUpOn();
		}
		if (Input.GetKeyDown("g"))
		{
			PopUpOff();
		}

		if (popUpOn)
		{
			// if (!imgFadeCalled)
			// {
			// 	foreach(FadeInOutImage fadeScript in imgFadeScripts)
			// 	{
			// 		fadeScript.FadeIn();
			// 	}
			// 	imgFadeCalled = true;
			// }

			// if (!tMPsFaded)
			// {
			// 	alphaValue += Time.deltaTime;
			// 	foreach(TextMeshProUGUI tmp in tMPsToFade)
			// 	{
			// 		tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alphaValue);
			// 	}
			// 	if (alphaValue >= 1)
			// 	{
			// 		tMPsFaded = true;
			// 	}
			// }

			if (!allFaded)
			{
				alphaValue += Time.deltaTime / fadeInDuration;
				foreach(TextMeshProUGUI tmp in tMPsToFade)
				{
					tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alphaValue);
				}
				foreach(Image img in imgsToFade)
				{
					img.color = new Color(img.color.r, img.color.g, img.color.b, alphaValue);
				}
				if (alphaValue >= 1)
				{
					allFaded = true;
				}
			}

			if (allFaded)
			{
				popUpOn = false;
				allFaded = false;
			}
		}

		if (popUpOff)
		{
			// if (!imgFadeCalled)
			// {
			// 	foreach(FadeInOutImage fadeScript in imgFadeScripts)
			// 	{
			// 		fadeScript.FadeOut();
			// 	}
			// 	imgFadeCalled = true;
			// }

			// if (!tMPsFaded)
			// {
			// 	alphaValue -= Time.deltaTime;
			// 	foreach(TextMeshProUGUI tmp in tMPsToFade)
			// 	{
			// 		tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alphaValue);
			// 	}
			// 	if (alphaValue <= 0)
			// 	{
			// 		tMPsFaded = true;
			// 	}
			// }

			// if (tMPsFaded && imgFadeCalled)
			// {
			// 	popUpOff = false;
			// 	imgFadeCalled = false;
			// 	tMPsFaded = false;
			// }

			if (!allFaded)
			{
				alphaValue -= Time.deltaTime / fadeOutDuration;
				foreach(TextMeshProUGUI tmp in tMPsToFade)
				{
					tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alphaValue);
				}
				foreach(Image img in imgsToFade)
				{
					img.color = new Color(img.color.r, img.color.g, img.color.b, alphaValue);
				}
				if (alphaValue <= 0)
				{
					allFaded = true;
				}
			}

			if (allFaded)
			{
				popUpOff = false;
				allFaded = false;
			}
		}

	}


	public void PopUpOn()
	{
		popUpOn = true;
		popUpOff = false;
		imgFadeCalled = false;
		tMPsFaded = false;
		allFaded = false;
		alphaValue = tMPsToFade[0].color.a;
	}

	public void PopUpOff()
	{
		popUpOff = true;
		popUpOn = false;
		imgFadeCalled = false;
		tMPsFaded = false;
		allFaded = false;
		alphaValue = tMPsToFade[0].color.a;
	}

}