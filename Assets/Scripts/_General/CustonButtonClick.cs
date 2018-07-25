using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CustonButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	public string sceneName;



	public void OnPointerDown (PointerEventData pointerEventData) 
	{
		Debug.Log("Button pressed on " + pointerEventData.pointerCurrentRaycast.gameObject.name);
		// On click down on an object with collider
	}
	
	public void OnPointerUp (PointerEventData pointerEventData) 
	{
		Debug.Log("Button unpressed from " + pointerEventData.pointerCurrentRaycast.gameObject.name);
		// Maybe not needed seeing as it does not detect if the up was on the same collider/gameobject
	}

	public void OnPointerClick (PointerEventData pointerEventData) 
	{
		Debug.Log("Button clicked on " + pointerEventData.pointerCurrentRaycast.gameObject.name);
		// Load proper scene
		OpenScene();
	}



	public void OpenScene ()
	{
		SceneFade.SwitchScene(sceneName);
	}
}
