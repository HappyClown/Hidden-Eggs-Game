using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GrabItem : MonoBehaviour 
{
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;
	public bool canPickUpItems;
	public bool inBetweenLvls;

	public bool holdingItem;
	public GameObject heldItem;

	[Header("Crate Movement")]
	public bool crateToRight;
	public bool crateToDown;
	public bool crateStop;
	public Transform crateRightTransform;
	public Transform crateTopTransform;
	public Transform crateInSceneTransform;
	public float crateMoveSpeed;

	[Header("Tagged Colliders & Position Snaps")]
	public GameObject scaleSnapPos;
	public Transform crateSnapPos;
	public Collider2D scaleCol;
	public Collider2D crateCol;

	[Header("Item Parents")]
	public GameObject lvlOneItmHolder;
	public GameObject lvlTwoItmHolder;
	public GameObject lvlThreeItmHolder;
	public GameObject itemHolder;
	public GameObject crateParent;

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

	[Header("Silver Eggs")]
	public List<GameObject> silverEggs;
	public GameObject lvlOneSlvrEggs;
	public GameObject lvlTwoSlvrEggs;
	public GameObject lvlThreeSlvrEggs;


	void Start ()
	{
		heldItem = null;
		scaleScript = GameObject.FindGameObjectWithTag("Scale").GetComponent<Scale>();
		canPickUpItems = true;
		scaleCol = GameObject.FindGameObjectWithTag("Scale").GetComponent<Collider2D>();
		crateCol = GameObject.FindGameObjectWithTag("Crate").GetComponent<Collider2D>();
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

			if (hit)
			{
				if (hit.collider.CompareTag("Item") && !inBetweenLvls)
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
			

				// Pick up silver Eggs //
				if (hit.collider.CompareTag("Egg") && inBetweenLvls)
				{
					silverEggs.Add(hit.collider.gameObject);
					Debug.Log("Thats Silver Egg #" + silverEggs.Count +" mate");
					hit.collider.enabled = false;
					hit.collider.gameObject.SetActive(false); // redundant with the previous line but what happens to the eggs remains to be thought of.
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
					heldItem.transform.parent = crateParent.transform;
					//pounds.text = curntPounds + heldItem.GetComponent<Items>().weight + " /" + crateScript.reqPounds + " pounds";
					curntPounds += heldItem.GetComponent<Items>().weight;
					//amntOfItems.text = curntAmnt + 1 + " /" + crateScript.reqItems + " items";
					curntAmnt += 1;
				}

				// Cannot drop items outside of the screen. If item held dosnt hit any of the 4 tags it goes back to its original position.
				if (!hits[i].collider.gameObject.CompareTag("Scale") && !hits[i].collider.gameObject.CompareTag("Table") && !hits[i].collider.gameObject.CompareTag("Crate") && !hits[i].collider.gameObject.CompareTag("Item")) 
				{
					heldItem.transform.position = heldItem.GetComponent<Items>().initialPos;
				}
			}

			heldItem = null;
		}


 
		// NEXT LEVEL OF THE PUZZLE WHEN REQUIREMENTS ARE MET //
		if (curntPounds == crateScript.reqPounds && curntAmnt == crateScript.reqItems)
		{
			inBetweenLvls = true;

			crateToRight = true;

			crateCol.enabled = false; //disable crate collider (cannot drop items on crate anymore)
			//tell crate to do moveright animation (MoveRight settrigger)
			

			//Debug.Log("Next level!");
			crateScript.curntLvl += 1;	

			//if(crateScript.curntLvl > 1) resetItemsButtonScript.ResetItemsToTable();

			curntPounds = 0;
			curntAmnt = 0;
		}


			if (crateScript.curntLvl == 1 && lvlOneSlvrEggs.activeSelf == false && inBetweenLvls == true )
			{ 
				Debug.Log("Setting up level 1");
				itemHolder = lvlOneItmHolder;
				lvlOneSlvrEggs.SetActive(true); //set active the level 1 silver eggs
				crateCol.enabled = true; //re-enable crate collider
				inBetweenLvls = false;
				resetItemsButtonScript.FillItemResetArray();
			}


			if (crateScript.curntLvl == 2 && lvlTwoSlvrEggs.activeSelf == false && inBetweenLvls == true && silverEggs.Count < 1)// after level 1 is done but before level 2 gets set up
			{
				Items[] childrenItemScripts;

				childrenItemScripts = lvlOneItmHolder.transform.GetComponentsInChildren<Items>();

				for (int i = 0; i <childrenItemScripts.Length; i++)
				{
					childrenItemScripts[i].FadeOut();
				}

				if (crateToRight == true)
				{
					StartCoroutine(MoveCrateRight());
				}
				crateToRight = false;
			}


			if (crateScript.curntLvl == 2 && lvlTwoSlvrEggs.activeSelf == false && inBetweenLvls == true && silverEggs.Count == 1) // and silver egg is picked up 
			{
				Debug.Log("Setting up level 2");
				//resetItemsButtonScript.ResetItemsToTable();
				StartCoroutine(MoveCrateDown());	
				 itemHolder = lvlTwoItmHolder; // would it be wiser to put if statements before these ie: if itemholder != lvlTwoItmHolder -> itemHolder = lvlTwoItmHolder
				 lvlTwoItmHolder.SetActive(true);
				 lvlOneItmHolder.SetActive(false);
				 lvlTwoSlvrEggs.SetActive(true); //set active the new level 2 silver eggs WHEN THE CRATE IS BACK IN POSITION
				 lvlOneSlvrEggs.SetActive(false);
				 crateCol.enabled = true; //re-enable crate collider
				 resetItemsButtonScript.FillItemResetArray();
				 inBetweenLvls = false;
				 
			}


			if (crateScript.curntLvl == 3 && lvlThreeSlvrEggs.activeSelf == false && inBetweenLvls == true && silverEggs.Count < 3 && silverEggs.Count >= 1)// after level 2 is done but before level 3 gets set up
			{
				Items[] childrenItemScripts;

				childrenItemScripts = lvlTwoItmHolder.transform.GetComponentsInChildren<Items>();

				for (int i = 0; i <childrenItemScripts.Length; i++)
				{
					childrenItemScripts[i].FadeOut();
				}

				if (crateToRight == true)
				{
					StartCoroutine(MoveCrateRight());
				}
				crateToRight = false;
			}


			if (crateScript.curntLvl == 3 && lvlThreeSlvrEggs.activeSelf == false && inBetweenLvls == true && silverEggs.Count == 3) // and silver eggs are picked up
			{
				Debug.Log("Setting up level 3");
				//resetItemsButtonScript.ResetItemsToTable();
				StartCoroutine(MoveCrateDown());
				 itemHolder = lvlThreeItmHolder; 
				 lvlThreeItmHolder.SetActive(true);
				 lvlTwoItmHolder.SetActive(false);
				 lvlThreeSlvrEggs.SetActive(true); //set active the new level 3 silver eggs WHEN THE CRATE IS BACK IN POSITION
				 lvlTwoSlvrEggs.SetActive(false);
				 crateCol.enabled = true; //re-enable crate collider
				 resetItemsButtonScript.FillItemResetArray();
				 inBetweenLvls = false;
			}


			if (crateScript.curntLvl == 4 && lvlThreeSlvrEggs.activeSelf == true && inBetweenLvls == true && silverEggs.Count < 6 && silverEggs.Count >= 3)// after level 2 is done but before level 3 gets set up
			{
				Items[] childrenItemScripts;

				childrenItemScripts = lvlThreeItmHolder.transform.GetComponentsInChildren<Items>();

				for (int i = 0; i <childrenItemScripts.Length; i++)
				{
					childrenItemScripts[i].FadeOut();
				}

				if (crateToRight == true)
				{
					StartCoroutine(MoveCrateRight());
				}
				crateToRight = false;
			}
	}


	// public void MoveCrateRight ()
	// {
	// 	if (crateToRight == true)
	// 	{
	// 		crateParent.transform.position = Vector3.MoveTowards(crateParent.transform.position, crateRightTransform.position, crateMoveSpeed * Time.deltaTime);
	// 	}

	// 	if (crateParent.transform.position.x >= crateRightTransform.position.x - 0.25f)
	// 	{
	// 		foreach (Transform item in crateParent.transform)
	// 		{
	// 			//Debug.Log("Going through the keeds");
	// 			if(item.gameObject.CompareTag("Item"))
	// 			{
	// 				item.gameObject.SetActive(false);
	// 			}
	// 		}
	// 		crateToRight = false;
	// 		crateStop = true;
	// 		crateParent.transform.position = crateTopTransform.position;
	// 	}
	// }


	public IEnumerator MoveCrateRight ()
	{
			while (Vector3.Distance(crateParent.transform.position, crateRightTransform.position) > 0.25f)
			{
				crateParent.transform.position = Vector3.MoveTowards(crateParent.transform.position, crateRightTransform.position, crateMoveSpeed * Time.deltaTime);

				//Debug.Log("Not at Right position yet.");

				yield return null;
			}

			foreach (Transform item in crateParent.transform)
			{
				//Debug.Log("Going through the keeds");
				if(item.gameObject.CompareTag("Item"))
				{
					item.gameObject.SetActive(false);
				}
			}
		crateParent.transform.position = crateTopTransform.position;
	}


	public IEnumerator MoveCrateDown ()
	{
		crateParent.transform.position = crateTopTransform.position;

			while (Vector3.Distance(crateParent.transform.position, crateInSceneTransform.position) > 0.25f)
			{
				crateParent.transform.position = Vector3.MoveTowards(crateParent.transform.position, crateInSceneTransform.position, crateMoveSpeed * Time.deltaTime);

				yield return null;
			}
	}
}