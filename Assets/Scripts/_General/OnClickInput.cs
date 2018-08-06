using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickInput : MonoBehaviour 
{
	static Ray2D ray;
	public static RaycastHit2D hit;
	static Vector2 mousePos2D;
	static Vector3 mousePos;
	public LayerMask layerMask;
	

	void Update () 
	{
		if ( Input.GetMouseButtonDown(0))
		{
			Debug.Log("Clickidy click.");
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);

			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
		}
	}
}
