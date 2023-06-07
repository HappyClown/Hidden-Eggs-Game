using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MarketPuzzleEngine : MainPuzzleEngine {
	#region MarketPuzzleEngine Script Variables
	[Header("General")]
	public List<Items> currentLvlItems;
	public float maxZPos, ammontZ, currentZPos;
	public float itemScaleMult;
	private bool holdingItem;
	private GameObject heldItem;
	[Header("Crate")]
	public float crateMoveSpeed;
	public Animator crateAnim;
	public Transform crateTopTransform, crateInSceneTransform;
	public float waitBeforeCrteDown;
	[Header("Tagged Colliders & Position Snaps")]
	public GameObject scaleSnapPos;
	public Transform crateSnapPos;
	[Header("Item Parents")]
	public GameObject crateParent;
	[Header("Scripts")]
	public Scale scaleScript;
	public Crate crateScript;
	public ResetItemsButton resetItemsButtonScript;
	public Items refItemScript;
	public ReqParchmentMove reqParchMoveScript;
	[Header("Hide In Inspector ^_^")]
	public float curntPounds;
	public float curntAmnt;
	#endregion

	public AudioSceneMarketPuzzle audioSceneMarketPuz;

	void Start () {
		StartSetup();	
		//mySilverEggMan.silverEggsPickedUp = GlobalVariables.globVarScript.silverEggsCount;
		if (setupLvlWaitTime < refItemScript.fadeDuration) setupLvlWaitTime = refItemScript.fadeDuration;
		audioSceneMarketPuz =  GameObject.Find ("Audio").GetComponent<AudioSceneMarketPuzzle>();		
		heldItem = null;
	}

	void Update () {
		if(resetLevel){
			ResetLevel();	
			resetLevel = false;
		}
		if(setupLevel){
			SetUpLevel();					
			setupLevel = false;
		}
		if (canPlay) {
			RunBasics(canPlay);
			// Current level complete.
			if (curntPounds == crateScript.reqPounds && curntAmnt == crateScript.reqItems && !holdingItem) { 
				SilverEggsSetup();
				LevelFinishedSequence(); }
			#region Click
			// Click //
			if (myInput.dragStarted && !holdingItem) {
				UpdateMousePos(myInput.draggingPosition);
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
				Debug.Log(mousePos2D);

				if (hit) {
					Debug.Log(hit.collider.tag);
					if (hit.collider.CompareTag("Item")) {
						holdingItem = true;
						heldItem = hit.collider.gameObject;
						heldItem.transform.parent = itemHolder.transform;
						heldItem.transform.localScale = heldItem.transform.localScale * itemScaleMult;
						heldItem.GetComponent<SpriteRenderer>().sprite = heldItem.GetComponent<Items>().item;
						currentZPos =  heldItem.GetComponent<Items>().zPos;	
						foreach (Items theItems in currentLvlItems)
						{
							if(theItems.zPos < currentZPos)
							{
								theItems.zPos += ammontZ;
								theItems.gameObject.transform.position = new Vector3(theItems.gameObject.transform.position.x,theItems.gameObject.transform.position.y, theItems.zPos);
							}
						}					

						if (heldItem == scaleScript.itemOnScale) {
							scaleScript.isAnItemOnScale = false;
							scaleScript.itemOnScale = null;
						}

						if (heldItem.GetComponent<Items>().inCrate) {
							curntPounds -= heldItem.GetComponent<Items>().weight;
							curntAmnt -= 1;
							heldItem.GetComponent<Items>().inCrate = false;
						}
						audioSceneMarketPuz.pickupFruit();
					}
				}
			}
			#endregion

			#region Drag
			// Drag //
			else if (myInput.isDragging && holdingItem) {
				UpdateMousePos(myInput.draggingPosition);
				float heldItemObjX = heldItem.transform.position.x;
				float heldItemObjY = heldItem.transform.position.y;

				heldItemObjX = mousePos.x;
				heldItemObjY = mousePos.y;

				heldItem.transform.position = new Vector3(heldItemObjX, heldItemObjY, maxZPos);
			}
			#endregion

			#region Drop
			// Let go and decide where to put the item //
			if (myInput.dragReleased && holdingItem) {
				UpdateMousePos(myInput.releaseDragPos);
				holdingItem = false;
				heldItem.transform.localScale = heldItem.transform.localScale / itemScaleMult;
				heldItem.GetComponent<SpriteRenderer>().sprite = heldItem.GetComponent<Items>().itemWithShadow;

				RaycastHit2D[] hits;
				hits = Physics2D.RaycastAll(mousePos2D, Vector3.forward, 50f);
				for (int i =0; i < hits.Length; i++) {
					// ON THE SCALE AREA//
					if (hits[i].collider.gameObject.CompareTag("Scale")) {
						if (scaleScript.itemOnScale != null) {
							scaleScript.itemOnScale.transform.position = scaleScript.itemOnScale.GetComponent<Items>().initialPos;
							scaleScript.itemOnScale.transform.parent = itemHolder.transform;
						}
						heldItem.transform.position = new Vector3(scaleSnapPos.transform.position.x, scaleSnapPos.transform.position.y, -5f);
						heldItem.transform.parent = scaleSnapPos.transform;
						
						scaleScript.itemOnScale = heldItem;
						scaleScript.isAnItemOnScale = true;

						audioSceneMarketPuz.dropFruitScale();
						break;
					}

					// ON THE TABLE AREA//
					else if (hits[i].collider.gameObject.CompareTag("Table")) {
						heldItem.transform.position = new Vector3(mousePos.x, mousePos.y, maxZPos);
						heldItem.GetComponent<Items>().zPos = maxZPos;
						//SFX DROP ON WOOD
						audioSceneMarketPuz.dropFruitCrate();
						break;
					}

					// IN THE CRATE DIRECTLY//
					else if (hits[i].collider.gameObject.CompareTag("InCrate")) {
						heldItem.GetComponent<Items>().inCrate = true;
						heldItem.transform.position = new Vector3(mousePos.x, mousePos.y, maxZPos);
						heldItem.GetComponent<Items>().zPos = maxZPos;
						heldItem.transform.parent = crateParent.transform;
						curntPounds += heldItem.GetComponent<Items>().weight;
						curntAmnt += 1;
						//SFX DROP ON WOOD
						audioSceneMarketPuz.dropFruitCrate();
						break;
					}

					// IN THE CRATE AREA//
					else if (hits[i].collider.gameObject.CompareTag("Crate")) {
						heldItem.GetComponent<Items>().inCrate = true;
						heldItem.transform.position = new Vector3(crateSnapPos.transform.position.x, crateSnapPos.transform.position.y, maxZPos);
						heldItem.GetComponent<Items>().zPos = maxZPos;
						heldItem.transform.parent = crateParent.transform;
						curntPounds += heldItem.GetComponent<Items>().weight;
						curntAmnt += 1;
						//SFX DROP ON WOOD
						audioSceneMarketPuz.dropFruitCrate();
						break;
					}

					// Cannot drop items outside of the screen. If item held does Not hit any of the areas send it back to its initial position on the table.
					else {
						heldItem.transform.position = heldItem.GetComponent<Items>().initialPos;
					}
				}
				heldItem = null;
			}
		#endregion
		}
		else {
			RunBasics(canPlay);
			// After the initial set up run the first sequence.		
		}
	}	
	#region Level Methods
	private void ResetLevel(){
		
		resetItemsButtonScript.FillItemResetArray();
		if(setupChsnLvl){
			for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
			{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeOut(); }
			reqParchMoveScript.moveToHidden = true;
			audioSceneMarketPuz.closePanel();
		}
		curntPounds = 0;
		curntAmnt = 0;			
		resetItemsButtonScript.EndOfLevelReset();	
		crateScript.UpdateRequirements();		
		currentLvlItems.Clear();
		itemHolder.gameObject.GetComponentsInChildren<Items>(currentLvlItems);
	}
	private void SetUpLevel(){
		
		crateDownB = true; 
		crateAnim.SetTrigger("MoveDown"); 
		audioSceneMarketPuz.crateSlideDown(); 
		StartCoroutine(MoveCrateDown()); 
		
		lvlItemHolders[curntLvl - 1].SetActive(true); 
		for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		{ 
			resetItemsButtonScript.items[i].GetComponent<Items>().FadeIn(); 
		}
		crateScript.UpdateRequirements();
		reqParchMoveScript.moveToShown = true;
		audioSceneMarketPuz.openPanel();
		currentLvlItems.Clear();
		itemHolder.gameObject.GetComponentsInChildren<Items>(currentLvlItems); 
	}
	private void LevelFinishedSequence(){
		// Fade out the finished level's items. (Except the ones in the crate.)
		if (scaleScript.itemOnScale != null) { scaleScript.itemOnScale.transform.parent = itemHolder.transform; }
		Items[] childrenItemScripts; // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		childrenItemScripts = lvlItemHolders[curntLvl - 1].transform.GetComponentsInChildren<Items>(); 
		for (int i = 0; i < childrenItemScripts.Length; i++)
		{ childrenItemScripts[i].FadeOut(); }
		StartCoroutine(MoveCrateRight());
		reqParchMoveScript.moveToHidden = true;
		audioSceneMarketPuz.closePanel();
	}
	#endregion
	#region Coroutines
	// Move crate to the right.
	public IEnumerator MoveCrateRight () {
		//Make it skip a frame to make sure that the animation has time to start.
		yield return new WaitForSeconds(0.0001f);

		while (crateAnim.transform.parent.rotation != crateInSceneTransform.rotation)
		{
			float Zangle = crateAnim.transform.parent.eulerAngles.z;
			Zangle = Mathf.LerpAngle(crateAnim.transform.parent.eulerAngles.z, 0f, Time.deltaTime * crateMoveSpeed);
			crateAnim.transform.parent.eulerAngles = new Vector3(0, 0, Zangle);

			if (Vector3.Distance(crateAnim.transform.parent.eulerAngles, crateInSceneTransform.eulerAngles) <= 0.1f)
			{
				crateAnim.transform.parent.rotation = crateInSceneTransform.rotation;
			}

			yield return null;
		}
		crateAnim.SetTrigger("MoveRight");

		//SFX MOVE CRATE
		audioSceneMarketPuz.crateSlideRight();

		yield return new WaitForSeconds(0.0001f);

		while (crateAnim.GetCurrentAnimatorStateInfo(0).IsName("CrateMoveRight"))
		{
			//Debug.Log("Playing anim move right.");
			yield return null;
		}
		
		scaleScript.itemOnScale = null; // Deleted both scale lines if we want scale arrow to reset after silver eggs have been clicked. 
		scaleScript.isAnItemOnScale = false; //

		crateParent.transform.parent.position = crateTopTransform.position;
		crateParent.transform.parent.rotation = crateTopTransform.rotation;

		mySilverEggMan.lvlSilverEggs[curntLvl - 1].SetActive(true);
		resetItemsButtonScript.EndOfLevelReset();
		itemHolder.SetActive(false);
		scrnDarkImgScript.FadeIn();
		crateAnim.SetTrigger("MoveDown");
		//SFX MOVE CRATE
		audioSceneMarketPuz.crateSlideDown();
		StartCoroutine(MoveCrateDown());
	}
	

	// Move crate down.
	public IEnumerator MoveCrateDown () {
		//Debug.Log("Entered Coroutine MoveCrateDown. Yo.");
		yield return new WaitUntil(() => crateParent.transform.parent.position == crateTopTransform.position);
		//Debug.Log("CrateParent pos = crateTop pos.");
		while (crateAnim.GetCurrentAnimatorStateInfo(0).IsName("CrateMoveDown"))
		{
			//Debug.Log("MoveCrateDown Animating.");
			yield return null;
		}
		//Debug.Log("should take anim pos");
		crateParent.transform.parent.position = crateAnim.transform.position;
		crateParent.transform.parent.rotation = crateAnim.transform.rotation;

		// foreach activesilvereggs getcomponenet silvereggsequence startsequence = true
		foreach(GameObject silEgg in mySilverEggMan.activeSilverEggs)
		{
			silEgg.GetComponent<SilverEggSequence>().StartSequence();
		}
	}

	#endregion
}