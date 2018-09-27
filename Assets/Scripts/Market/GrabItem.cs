using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GrabItem : MonoBehaviour 
{
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;
	public string sceneName;

	public bool canPickUpItems;
	public bool inBetweenLvls;

	public bool holdingItem;
	public GameObject heldItem;

	public float itemScaleMult;

	public GameObject inCrateCollider;

	[Header("Crate Movement")]
	public bool crateToRight;
	public bool crateToDown;
	public bool crateStop;
	public Transform crateRightTransform;
	public Transform crateTopTransform;
	public Transform crateInSceneTransform;
	public float crateMoveSpeed;
	public AnimationCurve crateMoveCurve;
	public float curveTimer;
	public float totalDist;
	public float timeToMoveCrate;
	public float tempTimeToMove;
	public Animator crateAnim;

	[Header("Tagged Colliders & Position Snaps")]
	public GameObject scaleSnapPos;
	public Transform crateSnapPos;
	public Collider2D scaleCol;
	public Collider2D crateCol;

	[Header("Item Parents")]
	public List<GameObject> lvlItemHolders;
	public GameObject itemHolder;
	public GameObject crateParent;

	[Header("Scripts")]
	public Scale scaleScript;
	public Items itemsScript;
	public Crate crateScript;
	public ResetItemsButton resetItemsButtonScript;

	[Header("Text")]
	public TextMeshProUGUI pounds;
	public TextMeshProUGUI amntOfItems;

	[Header("In Crate")]
	public float curntPounds;
	public float curntAmnt;

	[Header("Silver Eggs")]
	public List<GameObject> lvlSilverEggs;
	public int silverEggsPickedUp;


	void Start ()
	{
		heldItem = null;
		scaleScript = GameObject.FindGameObjectWithTag("Scale").GetComponent<Scale>();
		canPickUpItems = true;
		scaleCol = GameObject.FindGameObjectWithTag("Scale").GetComponent<Collider2D>();
		crateCol = GameObject.FindGameObjectWithTag("Crate").GetComponent<Collider2D>();

		totalDist = Vector3.Distance(crateParent.transform.position, crateRightTransform.position);
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
					heldItem.transform.localScale = heldItem.transform.localScale * itemScaleMult;

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
					silverEggsPickedUp += 1;
					Debug.Log("Thats Silver Egg #" + silverEggsPickedUp +" mate");
					hit.collider.enabled = false;
					hit.collider.gameObject.GetComponent<SilverEggs>().StartSilverEggAnim();

					SaveSilverEggsToCorrectFile();
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

			heldItem.transform.localScale = heldItem.transform.localScale / itemScaleMult;

			RaycastHit2D[] hits;
			
			hits = Physics2D.RaycastAll(mousePos2D, Vector3.forward, 50f);

			for (int i =0; i < hits.Length; i++)
			{
				//Debug.Log(hits[i].collider.gameObject.name);
					
				// ON THE SCALE AREA//
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

				// ON THE TABLE AREA//
				if (hits[i].collider.gameObject.CompareTag("Table"))
				{
					heldItem.transform.position = heldItem.GetComponent<Items>().initialPos;
				}

				// IN THE CRATE DIRECTLY//
				if (hits[i].collider.gameObject.CompareTag("InCrate"))
				{
					heldItem.GetComponent<Items>().inCrate = true;
					heldItem.transform.position = new Vector3(heldItem.transform.position.x, heldItem.transform.position.y, -5f);
					heldItem.transform.parent = crateParent.transform;
					//pounds.text = curntPounds + heldItem.GetComponent<Items>().weight + " /" + crateScript.reqPounds + " pounds";
					curntPounds += heldItem.GetComponent<Items>().weight;
					//amntOfItems.text = curntAmnt + 1 + " /" + crateScript.reqItems + " items";
					curntAmnt += 1;
					return;
				}

				// IN THE CRATE AREA//
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

			crateScript.curntLvl += 1;	

			inCrateCollider.transform.position = new Vector3(inCrateCollider.transform.position.x, inCrateCollider.transform.position.y, inCrateCollider.transform.position.z + 1);
			//curveTimer = 0f;
			totalDist = Vector3.Distance(crateParent.transform.position, crateRightTransform.position);

			curntPounds = 0;
			curntAmnt = 0;
		}

			// SET UP LEVEL 1
			if (crateScript.curntLvl == 1 && lvlSilverEggs[0].activeSelf == false && inBetweenLvls == true )
			{ 
				Debug.Log("Setting up level 1");
				itemHolder = lvlItemHolders[0];
				lvlSilverEggs[0].SetActive(true);
				lvlItemHolders[0].SetActive(true);
				inBetweenLvls = false;
				resetItemsButtonScript.FillItemResetArray();
				inCrateCollider.transform.position = new Vector3(inCrateCollider.transform.position.x, inCrateCollider.transform.position.y, inCrateCollider.transform.position.z - 1);
			}

			// IN BETWEEN LEVEL 1 - 2
			if (crateScript.curntLvl == 2 && lvlSilverEggs[1].activeSelf == false && inBetweenLvls == true && silverEggsPickedUp < 1)// after level 1 is done but before level 2 gets set up
			{
				Items[] childrenItemScripts;

				childrenItemScripts = lvlItemHolders[0].transform.GetComponentsInChildren<Items>();

				for (int i = 0; i <childrenItemScripts.Length; i++)
				{
					childrenItemScripts[i].FadeOut();
				}

				if (crateToRight == true)
				{
					//crateAnim.SetTrigger("MoveRight");
					StartCoroutine(MoveCrateRight());
				}
				crateToRight = false;
			}

			// SET UP LEVEL 2
			if (crateScript.curntLvl == 2 && lvlSilverEggs[1].activeSelf == false && inBetweenLvls == true && silverEggsPickedUp == 1 && !crateToRight)
			{
				Debug.Log("Setting up level 2");
				crateAnim.SetTrigger("MoveDown");
				StartCoroutine(MoveCrateDown());
				itemHolder = lvlItemHolders[1]; // would it be wiser to put if statements before these ie: if itemholder != lvlTwoItmHolder -> itemHolder = lvlTwoItmHolder
				lvlItemHolders[0].SetActive(false);
				lvlItemHolders[1].SetActive(true);
				//lvlTwoItmHolder.SetActive(true);
				//lvlOneItmHolder.SetActive(false);
				inBetweenLvls = false;
				inCrateCollider.transform.position = new Vector3(inCrateCollider.transform.position.x, inCrateCollider.transform.position.y, inCrateCollider.transform.position.z - 1);
			}

			// IN BETWEEN LEVEL 2 - 3
			if (crateScript.curntLvl == 3 && lvlSilverEggs[2].activeSelf == false && inBetweenLvls == true && silverEggsPickedUp < 3 && silverEggsPickedUp >= 1)// after level 2 is done but before level 3 gets set up
			{
				Items[] childrenItemScripts;

				childrenItemScripts = lvlItemHolders[1].transform.GetComponentsInChildren<Items>();

				for (int i = 0; i <childrenItemScripts.Length; i++)
				{
					childrenItemScripts[i].FadeOut();
				}

				if (crateToRight == true)
				{
					//crateAnim.SetTrigger("MoveRight");
					StartCoroutine(MoveCrateRight());
				}
				crateToRight = false;
			}


			if (crateScript.curntLvl == 3 && lvlSilverEggs[2].activeSelf == false && inBetweenLvls == true && silverEggsPickedUp == 3 && !crateToRight)
			{
				Debug.Log("Setting up level 3");
				crateAnim.SetTrigger("MoveDown");
				StartCoroutine(MoveCrateDown());
				itemHolder = lvlItemHolders[2]; 
				lvlItemHolders[1].SetActive(false);
				lvlItemHolders[2].SetActive(true);
				//lvlThreeItmHolder.SetActive(true);
				//lvlTwoItmHolder.SetActive(false);
				inBetweenLvls = false;
				inCrateCollider.transform.position = new Vector3(inCrateCollider.transform.position.x, inCrateCollider.transform.position.y, inCrateCollider.transform.position.z - 1);
			}


			if (crateScript.curntLvl == 4 && lvlSilverEggs[2].activeSelf == true && inBetweenLvls == true && silverEggsPickedUp < 6 && silverEggsPickedUp >= 3)// after level 3 is done
			{
				Items[] childrenItemScripts;

				childrenItemScripts = lvlItemHolders[2].transform.GetComponentsInChildren<Items>();

				for (int i = 0; i <childrenItemScripts.Length; i++)
				{
					childrenItemScripts[i].FadeOut();
				}

				if (crateToRight == true)
				{
					//crateAnim.SetTrigger("MoveRight");
					StartCoroutine(MoveCrateRight());
				}
				crateToRight = false;
			}


			// Last level completed and all 6 eggs picked up.
			if (crateScript.curntLvl == 4 && silverEggsPickedUp == 6)
			{
				StartCoroutine(PuzzleComplete());
			}
	}


	// Move crate to the right.
	public IEnumerator MoveCrateRight ()
	{
		//Make it skip a frame to make sure that the animation has time to start.
		yield return new WaitForSeconds(0.0001f);
		//yield return new WaitUntil(!crateAnim.IsInTransition(0));
		

		while (crateAnim.transform.parent.rotation != crateInSceneTransform.rotation)
		{
			// crateAnim.transform.parent.eulerAngles = Vector3.Lerp(crateAnim.transform.parent.eulerAngles, crateInSceneTransform.eulerAngles, Time.deltaTime);
			float Zangle = crateAnim.transform.parent.eulerAngles.z;
			Zangle = Mathf.LerpAngle(crateAnim.transform.parent.eulerAngles.z, 0f, Time.deltaTime * crateMoveSpeed);
			crateAnim.transform.parent.eulerAngles = new Vector3(0, 0, Zangle);
			//Debug.Log(crateAnim.transform.parent.eulerAngles);

			if (Vector3.Distance(crateAnim.transform.parent.eulerAngles, crateInSceneTransform.eulerAngles) <= 0.1f)
			{
				crateAnim.transform.parent.rotation = crateInSceneTransform.rotation;
			}

			yield return null;
		}
		crateAnim.SetTrigger("MoveRight");

		yield return new WaitForSeconds(0.0001f);

		while (crateAnim.GetCurrentAnimatorStateInfo(0).IsName("CrateMoveRight"))
		{
			//Debug.Log("Playing anim move right.");
			yield return null;
		}
		// while (Vector3.Distance(crateParent.transform.position, crateRightTransform.position) > 0.25f)
		// {
		// 	// crateMoveSpeed = (totalDist / timeToMoveCrate)  * (crateMoveCurve.Evaluate(curveTimer));
			
		// 	// curveTimer = (totalDist - Vector3.Distance(crateParent.transform.position, crateRightTransform.position)) / totalDist; // dist travld / totalDist = % done from 0-1

		// 	// crateParent.transform.position = Vector3.MoveTowards(crateParent.transform.position, crateRightTransform.position, crateMoveSpeed * Time.deltaTime); //  * crateMoveCurve.Evaluate(curveTimer)

		// 	// //Debug.Log("Not at Right position yet.");	
		// 	//  tempTimeToMove += Time.deltaTime;
		// 	//  Debug.Log(tempTimeToMove);



		// 	yield return null;
		// }

		Debug.Log("How many times do I pass here?");

		foreach (Transform item in crateParent.transform)
		{
			//Debug.Log("Going through the keeds");
			if(item.gameObject.CompareTag("Item"))
			{
				item.gameObject.SetActive(false);
			}
		}	
		crateParent.transform.parent.position = crateTopTransform.position;
		crateParent.transform.parent.rotation = crateTopTransform.rotation;
	}


	// Move crate down.
	public IEnumerator MoveCrateDown ()
	{
		yield return new WaitUntil(() => crateParent.transform.parent.position == crateTopTransform.position);

		//yield return new WaitForSeconds(1);

		// while (Vector3.Distance(crateParent.transform.position, crateInSceneTransform.position) > 0.25f)
		// {
		// 	crateParent.transform.position = Vector3.MoveTowards(crateParent.transform.position, crateInSceneTransform.position, crateMoveSpeed * Time.deltaTime);

		// 	yield return null;
		// }

		while (crateAnim.GetCurrentAnimatorStateInfo(0).IsName("CrateMoveDown"))
		{
			//Debug.Log("Playing anim move down.");
			yield return null;
		}

		crateParent.transform.parent.position = crateAnim.transform.position;
		crateParent.transform.parent.rotation = crateAnim.transform.rotation;

		lvlSilverEggs[crateScript.curntLvl - 2].SetActive(false);
		lvlSilverEggs[crateScript.curntLvl - 1].SetActive(true);
		resetItemsButtonScript.FillItemResetArray();
	}


	// All silver eggs picked up, what happenes?
	public IEnumerator PuzzleComplete ()
	{
		yield return new WaitForSeconds(0.5f);

		Debug.Log("Puzzle Completed cognraturations!!!");

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene(sceneName);
	}



	public void SaveSilverEggsToCorrectFile()
	{
		if (silverEggsPickedUp > GlobalVariables.globVarScript.marketSilverEggsCount) 
		{ 
			GlobalVariables.globVarScript.marketTotalEggsFound += 1;
			GlobalVariables.globVarScript.marketSilverEggsCount = silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}
		
		
	}
}