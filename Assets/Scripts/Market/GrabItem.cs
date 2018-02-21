﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GrabItem : MonoBehaviour 
{
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

	public bool holdingItem;
	public GameObject heldItem;

	public GameObject scaleSnapPos;
	public Transform crateSnapPos;

	[Header("Item Parents")]
	public GameObject lvlOneItmHolder;
	public GameObject lvlTwoItmHolder;
	public GameObject lvlThreeItmHolder;
	public GameObject itemHolder;

	[Header("Scripts")]
	public Scale scaleScript;
	public Items itemsScript;
	public Crate crateScript;
	public ResetItemsButton resetItemsButtonScript;

	[Header("Text")]
	public Text pounds;
	public Text amntOfItems;

	[Header("In Crate")]
	public float curntPounds;
	public float curntAmnt;



	void Start ()
	{
		heldItem = null;
		scaleScript = GameObject.FindGameObjectWithTag("Scale").GetComponent<Scale>();
	}
	

	void FixedUpdate () 
	{
		
	}


	void Update () 
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);

		//Debug.DrawRay(mousePos2D, Vector3.forward, Color.red, 60f);

		// Click //
		if (Input.GetMouseButtonDown(0))
		{
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);

			if (hit.collider.CompareTag("Item"))
			{
				//Debug.Log(hit.collider.name);

				holdingItem = true;
				heldItem = hit.collider.gameObject;
				heldItem.transform.parent = itemHolder.transform;

				if (heldItem == scaleScript.itemOnScale)
				{
					scaleScript.isAnItemOnScale = false;
					scaleScript.itemOnScale = null;
				}

				if (heldItem.GetComponent<Items>().inCrate == true)
				{
					//pounds.text = curntPounds - heldItem.GetComponent<Items>().weight + " /" + crateScript.reqPounds + " pounds";
					curntPounds -= heldItem.GetComponent<Items>().weight;
					//amntOfItems.text = curntAmnt - 1 + " /" + crateScript.reqItems + " items";
					curntAmnt -= 1;
					heldItem.GetComponent<Items>().inCrate = false;
				}
			}
		}

		// Drag //
		if (holdingItem == true && Input.GetMouseButton(0))
		{
			float heldItemObjX = heldItem.transform.position.x;
			float heldItemObjY = heldItem.transform.position.y;

			heldItemObjX = mousePos.x;
			heldItemObjY = mousePos.y;

			heldItem.transform.position = new Vector3(heldItemObjX, heldItemObjY, -5f);
		}

		// Let go and decide where to put the item //
		if (Input.GetMouseButtonUp(0) && holdingItem == true)
		{
			holdingItem = false;

			RaycastHit2D[] hits;
			
			hits = Physics2D.RaycastAll(mousePos2D, Vector3.forward, 50f);


			for (int i =0; i < hits.Length; i++)
			{
				//Debug.Log(hits[i].collider.gameObject.name);
				
				// ON THE SCALE //
				if (hits[i].collider.gameObject.CompareTag("Scale"))
				{
					if (scaleScript.itemOnScale != null)
					{
						scaleScript.itemOnScale.transform.position = scaleScript.itemOnScale.GetComponent<Items>().initialPos;
						scaleScript.itemOnScale.transform.parent = itemHolder.transform;
					}
					heldItem.transform.position = new Vector3(scaleSnapPos.transform.position.x, scaleSnapPos.transform.position.y, -5f);
					heldItem.transform.parent = scaleSnapPos.transform;
					scaleScript.itemOnScale = heldItem;
					scaleScript.isAnItemOnScale = true;
				}

				// ON THE TABLE //
				if (hits[i].collider.gameObject.CompareTag("Table"))
				{
					heldItem.transform.position = heldItem.GetComponent<Items>().initialPos;
				}

				// IN THE CRATE //
				if (hits[i].collider.gameObject.CompareTag("Crate"))
				{
					heldItem.GetComponent<Items>().inCrate = true;
					heldItem.transform.position = new Vector3(crateSnapPos.transform.position.x, crateSnapPos.transform.position.y, -5f);
					//pounds.text = curntPounds + heldItem.GetComponent<Items>().weight + " /" + crateScript.reqPounds + " pounds";
					curntPounds += heldItem.GetComponent<Items>().weight;
					//amntOfItems.text = curntAmnt + 1 + " /" + crateScript.reqItems + " items";
					curntAmnt += 1;
				}
				
			}

			heldItem = null;
			
		}

		// NEXT LEVEL OF THE PUZZLE WHEN REQUIREMENTS ARE MET //
		if (curntPounds == crateScript.reqPounds && curntAmnt == crateScript.reqItems)
		{
			Debug.Log("Next level!");
			crateScript.curntLvl += 1;

			curntPounds = 0;
			curntAmnt = 0;

			if(crateScript.curntLvl > 1) resetItemsButtonScript.ResetItemsToTable();

			if (crateScript.curntLvl == 1) 
			{ 
				itemHolder = lvlOneItmHolder;
			}

			if (crateScript.curntLvl == 2) 
			{
				 itemHolder = lvlTwoItmHolder; 
				 lvlTwoItmHolder.SetActive(true);
				 lvlOneItmHolder.SetActive(false);
			}

			if (crateScript.curntLvl == 3) 
			{
				 itemHolder = lvlThreeItmHolder; 
				 lvlThreeItmHolder.SetActive(true);
				 lvlTwoItmHolder.SetActive(false);
			}

			resetItemsButtonScript.FillItemResetArray();
			
		}
	}
}