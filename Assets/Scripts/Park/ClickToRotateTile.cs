using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToRotateTile : MonoBehaviour 
{

	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;


	
	void FixedUpdate () 
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);

		hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
	}


	void Update()
	{
		if (hit.collider != null && Input.GetMouseButtonDown(0) && hit.collider.CompareTag("Tile"))
		{
			Debug.Log(hit.collider.name);
			hit.collider.transform.eulerAngles = new Vector3(hit.collider.transform.eulerAngles.x, hit.collider.transform.eulerAngles.y, hit.collider.transform.eulerAngles.z - 90);
		}
	}
}
