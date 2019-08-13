using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RainbowRiddle : MonoBehaviour 
{
	RaycastHit2D hit;
	RaycastHit2D hitFX;
	Vector2 mousePos2D;
	Vector3 mousePos;

    [Header("Rainbow Riddle")]
	public List<GameObject> fruitBaskets;
	public int basketNumber;
	public GameObject goldenEgg;
	public GoldenEgg goldenEggScript;
    public LayerMask layerMask;
	public LayerMask layerMaskFX;
	public GameObject appleBasket;
	[Header ("Script References")]
	public SceneTapEnabler scenTapEnabScript;
	public LevelTapMannager lvlTapManScript;
	public inputDetector inputDetScript;
	public ClickOnEggs clickOnEggsScript;
	public AudioSceneMarket audioSceneMarket;

    void Start ()
    {
        if (GlobalVariables.globVarScript.riddleSolved == true)
		{
			foreach (GameObject basket in fruitBaskets)
			{
				basket.SetActive(false);
			}
			goldenEgg.SetActive(true);
		}
    }

    void Update ()
    {
		if (!GlobalVariables.globVarScript.riddleSolved && Input.GetMouseButtonDown(0) && scenTapEnabScript.canTapEggRidPanPuz){
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
			hitFX = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMaskFX);

			if (hitFX) {
				//Debug.Log(hitFX.collider.gameObject.name);
				// - PLAY BASKET FX - // 
				if (hitFX.collider.CompareTag("OnClickFX")) {
					inputDetScript.cancelDoubleTap = true;
					hitFX.collider.GetComponent<OnClickFX>().PlayFX();

					//SFX Hit Baskets
					audioSceneMarket.goldenEggBasketsSFX();
				}
			}

			if (hit) {
				// -- HIT ACTIVE FRUITBASKET -- //
				if (hit.collider.CompareTag("FruitBasket")) {
					if (basketNumber == 0 && hit.collider.gameObject == appleBasket) {
						basketNumber++;
					}
					else if (basketNumber >= 0 && hit.collider.gameObject != appleBasket) {
						basketNumber++;
					}

					if (hit.collider.gameObject != appleBasket) {
						hit.collider.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
					}
					
					// - PUZZLE SOLVED - //
					if (basketNumber >= 6) {
						basketNumber = 0;
						RainbowRiddleSolved();
						// Activate the Golden Egg sequence.
						goldenEgg.SetActive(true);
						goldenEggScript.waitingToStartSeq = true;
						goldenEggScript.CannotTaps();
						// Disable all basket colliders.
						foreach (GameObject basket in fruitBaskets)
						{
							basket.SetActive(false);
						}	
						return;
					}
					else {
						fruitBaskets[basketNumber].GetComponent<PolygonCollider2D>().enabled = true;
					}
				}
				
				if (basketNumber > 1 && hit.collider.gameObject == appleBasket) {
					basketNumber = 1;
				}

				// - DID NOT HIT BASKET - //
				if (!hit.collider.CompareTag("FruitBasket")) {
					basketNumber = 0;
					foreach (GameObject basket in fruitBaskets)
					{
						basket.GetComponent<PolygonCollider2D>().enabled = false;
					}	
					fruitBaskets[0].GetComponent<PolygonCollider2D>().enabled = true;
				}
			}
		}   
    }

    public void RainbowRiddleSolved () {
		if (clickOnEggsScript.goldenEggFound == 0) {
			clickOnEggsScript.goldenEggFound = 1;
			clickOnEggsScript.AddEggsFound();
		}
		GlobalVariables.globVarScript.riddleSolved = true;
		GlobalVariables.globVarScript.SaveEggState();
	}

}
