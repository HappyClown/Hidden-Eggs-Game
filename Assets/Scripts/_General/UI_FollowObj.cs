using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_FollowObj : MonoBehaviour 
{
	public TextMeshProUGUI text;
	public GameObject objToFollow;
	public Vector2 newPos;
	public Camera cam;



	void Start () 
	{
		
	}
	


	void Update () 
	{
		newPos = cam.WorldToScreenPoint(objToFollow.transform.position);

		text.rectTransform.anchoredPosition = newPos;
	}
}
