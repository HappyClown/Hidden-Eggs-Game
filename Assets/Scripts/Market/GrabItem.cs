using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GrabItem : MonoBehaviour 
{
	#region GrabItem Script Variables
	[Header("General")]
	public float itemScaleMult;
	public int winLvl;
	[HideInInspector]
	public bool canPlay;
	[HideInInspector]
	public float curntPounds, curntAmnt;
	private bool initialSetupOn;
	public int maxLvl;
	public Items refItemScript;

	private bool setupChsnLvl;
	private float setupLvlWaitTime;
	private int lvlToLoad;
	private float chngLvlTimer;
	private bool holdingItem;
	private GameObject heldItem;

	private RaycastHit2D hit;
	private Vector2 mousePos2D;
	private Vector3 mousePos;

	[Header("Crate")]
	public float crateMoveSpeed;
	public Animator crateAnim;
	public Transform crateTopTransform, crateInSceneTransform;
	public GameObject inCrateCollider;

	[Header("Tagged Colliders & Position Snaps")]
	public GameObject scaleSnapPos;
	public Transform crateSnapPos;

	[Header("Item Parents")]
	public List<GameObject> lvlItemHolders;
	public GameObject itemHolder, crateParent;

	[Header("Scripts")]
	public Scale scaleScript;
	public Crate crateScript;
	public ResetItemsButton resetItemsButtonScript;

	[Header("Silver Eggs")]
	public int silverEggsPickedUp;
	public Sprite hollowSilEgg;
	public List<GameObject> lvlSilverEggs, activeSilverEggs;
	private bool silverEggsActive;
	#endregion

	void Start ()
	{
		canPlay = false;
		initialSetupOn = true;
		heldItem = null;
		maxLvl = GlobalVariables.globVarScript.marketPuzzMaxLvl;
		silverEggsPickedUp = GlobalVariables.globVarScript.marketSilverEggsCount;
		setupLvlWaitTime = refItemScript.fadeDuration;
	}

	void Update () 
	{
		if (canPlay)
		{
			//CHANGE LEVEL BUTTONS
			if (chngLvlTimer < setupLvlWaitTime) { chngLvlTimer += Time.deltaTime; }
			else
			{
				if (Input.GetKeyDown("1") && crateScript.curntLvl != 1) { chngLvlTimer = 0f; lvlToLoad = 1; ChangeLevelSetup();}
				if (Input.GetKeyDown("2") && crateScript.curntLvl != 2) { chngLvlTimer = 0f; lvlToLoad = 2; if (maxLvl >= 2) { ChangeLevelSetup();} }
				if (Input.GetKeyDown("3") && crateScript.curntLvl != 3) { chngLvlTimer = 0f; lvlToLoad = 3; if (maxLvl >= 3) { ChangeLevelSetup();} }
			}


			// Current level complete.
			if (curntPounds == crateScript.reqPounds && curntAmnt == crateScript.reqItems) { SilverEggsSetup(); }

			#region Click
			// Click //
			if (Input.GetMouseButtonDown(0) && !holdingItem)
			{
				UpdateMousePos();
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);

				if (hit)
				{
					if (hit.collider.CompareTag("Item"))
					{
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
							curntPounds -= heldItem.GetComponent<Items>().weight;
							curntAmnt -= 1;
							heldItem.GetComponent<Items>().inCrate = false;
						}
					}
				}
			}
			#endregion

			#region Drag
			// Drag //
			else if (Input.GetMouseButton(0) && holdingItem == true)
			{
				UpdateMousePos();
				float heldItemObjX = heldItem.transform.position.x;
				float heldItemObjY = heldItem.transform.position.y;

				heldItemObjX = mousePos.x;
				heldItemObjY = mousePos.y;

				heldItem.transform.position = new Vector3(heldItemObjX, heldItemObjY, -5f);
			}
			#endregion

			#region Drop
			// Let go and decide where to put the item //
			if (Input.GetMouseButtonUp(0) && holdingItem == true)
			{
				UpdateMousePos();
				holdingItem = false;
				heldItem.transform.localScale = heldItem.transform.localScale / itemScaleMult;

				RaycastHit2D[] hits;
				hits = Physics2D.RaycastAll(mousePos2D, Vector3.forward, 50f);
				for (int i =0; i < hits.Length; i++)
				{
					//Debug.Log(hits[i].collider.name);
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
						break;
					}

					// ON THE TABLE AREA//
					else if (hits[i].collider.gameObject.CompareTag("Table"))
					{
						heldItem.transform.position = new Vector3(mousePos.x, mousePos.y, -5f);
						break;
					}

					// IN THE CRATE DIRECTLY//
					else if (hits[i].collider.gameObject.CompareTag("InCrate"))
					{
						heldItem.GetComponent<Items>().inCrate = true;
						heldItem.transform.position = new Vector3(mousePos.x, mousePos.y, -5f);
						heldItem.transform.parent = crateParent.transform;
						curntPounds += heldItem.GetComponent<Items>().weight;
						curntAmnt += 1;
						break;
					}

					// IN THE CRATE AREA//
					else if (hits[i].collider.gameObject.CompareTag("Crate"))
					{
						heldItem.GetComponent<Items>().inCrate = true;
						heldItem.transform.position = new Vector3(crateSnapPos.transform.position.x, crateSnapPos.transform.position.y, -5f);
						heldItem.transform.parent = crateParent.transform;
						curntPounds += heldItem.GetComponent<Items>().weight;
						curntAmnt += 1;
						break;
					}

					// Cannot drop items outside of the screen. If item held does Not hit any of the areas send it back to its initial position on the table.
					else 
					{
						heldItem.transform.position = heldItem.GetComponent<Items>().initialPos;
					}
				}
				heldItem = null;
			}
		#endregion
		}
		else
		{
			// When this Scene is loaded.
			if (initialSetupOn) { InitialSetup(); }

			if (setupChsnLvl) { ChosenLevelSetup(lvlToLoad);}

			#region Click On SilverEggs
			// Clicking on a silver egg.
			if (Input.GetMouseButtonDown(0))
			{		
				UpdateMousePos();
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
				if (hit)
				{
					//Debug.Log(hit.collider.name);
					if (hit.collider.CompareTag("Egg"))
					{
						if (silverEggsPickedUp < GlobalVariables.globVarScript.marketSilverEggsCount) { silverEggsPickedUp += 1; }
						Debug.Log("Thats Silver Egg #" + silverEggsPickedUp +" mate");
						hit.collider.gameObject.GetComponent<SilverEggs>().StartSilverEggAnim();
						hit.collider.enabled = false;
						
						SaveSilverEggsToCorrectFile();

						SilverEggsCheck(); // Check if the Silver Eggs have all been collected.
					}
				}
			}
			#endregion
		}
	}

	#region Level Change Methods
	void InitialSetup()
	{
		//Debug.Log("Initial Setup");
		if(maxLvl > 3 || maxLvl < 1) { crateScript.curntLvl = 1; }
		else { crateScript.curntLvl = maxLvl; }
		curntPounds = 0;
		curntAmnt = 0;
		itemHolder = lvlItemHolders[crateScript.curntLvl - 1];
		lvlItemHolders[crateScript.curntLvl - 1].SetActive(true);
		resetItemsButtonScript.FillItemResetArray();
		canPlay = true;
		initialSetupOn = false;
		crateScript.UpdateRequirements();
	}

	void SilverEggsSetup()
	{
		//Debug.Log("New Level Setup");
		canPlay = false;
	
		//Set the silver egg sprites to Hollow if level was completed previously.
		if (maxLvl > crateScript.curntLvl)
		{ 
			foreach (Transform silEgg in lvlSilverEggs[crateScript.curntLvl - 1].transform)
			{
				silEgg.GetComponent<SpriteRenderer>().sprite = hollowSilEgg;
				Debug.Log(silEgg.name + "has been set to hollow, ooouuuhhhh. Like a ghost. A nice ghost. Yeeah.");
			}
		}

		lvlSilverEggs[crateScript.curntLvl - 1].SetActive(true);
		if (lvlSilverEggs[crateScript.curntLvl - 1].transform.childCount > 0)
		{
			//List<GameObject> activeSilverEggs = new List<GameObject>();
			foreach (Transform silEgg in lvlSilverEggs[crateScript.curntLvl - 1].transform)
			{
				activeSilverEggs.Add(silEgg.gameObject);
				//Debug.Log(silEgg.name + "has been added to the active Silver Egg List!");
			}
		}
		silverEggsActive = true;

		// Fade out the finished level's items. (Except the ones in the crate.)
		if (scaleScript.itemOnScale != null) { scaleScript.itemOnScale.transform.parent = itemHolder.transform; }
		
		// for (int i = 0; i < resetItemsButtonScript.items.Length; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		// { resetItemsButtonScript.items[i].GetComponent<Items>().FadeOut(); }

		Items[] childrenItemScripts; // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		childrenItemScripts = lvlItemHolders[crateScript.curntLvl - 1].transform.GetComponentsInChildren<Items>(); 
		for (int i = 0; i < childrenItemScripts.Length; i++)
		{ childrenItemScripts[i].FadeOut(); }

		StartCoroutine(MoveCrateRight());

		curntPounds = 0;
		curntAmnt = 0;
	}

	void SilverEggsCheck()
	{
		int amntSilEggsTapped = 0;
		if (activeSilverEggs.Count > 0)
		{
			foreach(GameObject silEgg in activeSilverEggs)
			{
				if (silEgg.GetComponent<CircleCollider2D>().enabled == false) { amntSilEggsTapped++; }
				Debug.Log("Amount of SilverEggs loaded: " + activeSilverEggs.Count + "& amount of Silver Eggs tapped: " + amntSilEggsTapped);
			}

			if (amntSilEggsTapped == activeSilverEggs.Count) 
			{
				activeSilverEggs.Clear();
				NextLevelSetup();
				silverEggsActive = false;
			}
		}
	}

	void NextLevelSetup()
	{
		resetItemsButtonScript.EndOfLevelReset();
		itemHolder.SetActive(false);
		crateAnim.SetTrigger("MoveDown");
		StartCoroutine(MoveCrateDown());
		chngLvlTimer = 0f;

		crateScript.curntLvl++;
		if (crateScript.curntLvl > maxLvl) { maxLvl = crateScript.curntLvl; }
		SaveMaxLvl();

		if (crateScript.curntLvl >= winLvl) 
		{
			StartCoroutine(PuzzleComplete());
			return;
		}

		itemHolder = lvlItemHolders[crateScript.curntLvl - 1];
		itemHolder.SetActive(true);

		resetItemsButtonScript.FillItemResetArray();
		canPlay = true;
		crateScript.UpdateRequirements();
	}
	
	void ChangeLevelSetup()
	{
		// Close up current level.
		canPlay = false;

		for (int i = 0; i < resetItemsButtonScript.items.Length; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeOut(); }
		
		setupChsnLvl = true;
		// RESET SILVER EGGs ///////////////////////
		

	}

	void ChosenLevelSetup(int lvlToLoad)
	{
		// Setup chosen level.
		chngLvlTimer += Time.deltaTime;
		if (chngLvlTimer > setupLvlWaitTime)
		{
			lvlItemHolders[crateScript.curntLvl - 1].SetActive(false);
			resetItemsButtonScript.EndOfLevelReset();
			crateScript.curntLvl = lvlToLoad;
			curntPounds = 0;
			curntAmnt = 0;
			itemHolder = lvlItemHolders[crateScript.curntLvl - 1];
			lvlItemHolders[crateScript.curntLvl - 1].SetActive(true);

			resetItemsButtonScript.FillItemResetArray();
			for (int i = 0; i < resetItemsButtonScript.items.Length; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
			{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeIn(); }

			//inCrateCollider.transform.position = new Vector3(inCrateCollider.transform.position.x, inCrateCollider.transform.position.y, inCrateCollider.transform.position.z - 1);
			canPlay = true;
			crateScript.UpdateRequirements();

			setupChsnLvl = false;
			chngLvlTimer = 0;
		}
	}
	#endregion

	#region General Methods
	public void SaveSilverEggsToCorrectFile()
	{
		if (silverEggsPickedUp > GlobalVariables.globVarScript.marketSilverEggsCount) 
		{ 
			GlobalVariables.globVarScript.marketTotalEggsFound += 1;
			GlobalVariables.globVarScript.marketSilverEggsCount = silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}
	}

	public void SaveMaxLvl()
	{
		if (maxLvl > GlobalVariables.globVarScript.marketPuzzMaxLvl)
		{
			GlobalVariables.globVarScript.marketPuzzMaxLvl = maxLvl;
			GlobalVariables.globVarScript.SaveEggState();
		}
	}

	//public void LoadMaxLvl()
	//{
	//	GlobalVariables.globVarScript.LoadCorrectPuzz();
	//}

	void UpdateMousePos()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}
	#endregion

	#region Coroutines
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

		foreach (Transform item in crateParent.transform) // DONT THINK ITS NEEDED 
		{
			//Debug.Log("Going through the keeds");
			if(item.gameObject.CompareTag("Item"))
			{
				SpriteRenderer sprRen = item.GetComponent<SpriteRenderer>();
				sprRen.color = new Color(sprRen.color.r, sprRen.color.g, sprRen.color.b, 0f);
			}
		}
		scaleScript.itemOnScale = null; // Deleted both scale lines if we want scale arrow to reset after silver eggs have been clicked. 
		scaleScript.isAnItemOnScale = false; //

		crateParent.transform.parent.position = crateTopTransform.position;
		crateParent.transform.parent.rotation = crateTopTransform.rotation;
	}


	// Move crate down.
	public IEnumerator MoveCrateDown ()
	{
		//Debug.Log("Entered Coroutine MoveCrateDown. Yo.");
		yield return new WaitUntil(() => crateParent.transform.parent.position == crateTopTransform.position);
		//Debug.Log("CrateParent pos = crateTop pos.");
		while (crateAnim.GetCurrentAnimatorStateInfo(0).IsName("CrateMoveDown"))
		{
			//Debug.Log("MoveCrateDown Animating.");
			yield return null;
		}

		crateParent.transform.parent.position = crateAnim.transform.position;
		crateParent.transform.parent.rotation = crateAnim.transform.rotation;

		resetItemsButtonScript.FillItemResetArray();
	}

	// All silver eggs picked up, what happenes?
	public IEnumerator PuzzleComplete ()
	{
		yield return new WaitForSeconds(0.5f);

		Debug.Log("Puzzle Completed cognraturations!!!");

		yield return new WaitForSeconds(0.5f);

		SceneFade.SwitchScene(GlobalVariables.globVarScript.marketName);
	}
	#endregion
}