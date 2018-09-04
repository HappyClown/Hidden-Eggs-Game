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

	public ParticleSystem firework01; 
	public ParticleSystem firework02;

	public bool fireworksFired;
	public GameObject appleBasket;



    void Start ()
    {
        if (GlobalVariables.globVarScript.rainbowRiddleSolved == true)
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
		if (!GlobalVariables.globVarScript.rainbowRiddleSolved && Input.GetMouseButtonDown(0))
		{
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos2D = new Vector2 (mousePos.x, mousePos.y);

			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);

			hitFX = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMaskFX);

			
			if (hitFX)
			{
				Debug.Log(hitFX.collider.gameObject.name);
				// - PLAY BASKET FX - // 
				if (hitFX.collider.CompareTag("OnClickFX"))
				{
					hitFX.collider.GetComponent<OnClickFX>().PlayFX();
				}
			}  

			if (hit)
    		{
				// -- HIT ACTIVE FRUITBASKET -- //
				if (hit.collider.CompareTag("FruitBasket"))
				{
					if (basketNumber == 0 && hit.collider.gameObject == appleBasket)
					{
						basketNumber++;
					}
					else if (basketNumber >= 0 && hit.collider.gameObject != appleBasket)
					{
						basketNumber++;
					}

					if (hit.collider.gameObject != appleBasket)
					{
						hit.collider.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
					}
					
					// - PUZZLE SOLVED - //
					if (basketNumber >= 6)
					{
						RainbowRiddleSolved ();

						//SpawnGoldenEgg;
						goldenEgg.SetActive(true);
						goldenEggScript.inGoldenEggSequence = true;

						if (!fireworksFired)
						{
							firework01.Play(true);
							firework02.Play(true);
							fireworksFired = true;
						}
						//Disable/destroy all basket colliders;
						foreach (GameObject basket in fruitBaskets)
						{
							basket.SetActive(false);
						}	
						return;
					}
					else
					{
						fruitBaskets[basketNumber].GetComponent<PolygonCollider2D>().enabled = true;
					}
				}
				
				// - DID NOT HIT BASKET - //
				if (!hit.collider.CompareTag("FruitBasket") && !goldenEgg.activeSelf)
				{
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



    public void RainbowRiddleSolved ()
	{
		GlobalVariables.globVarScript.rainbowRiddleSolved = true;
		GlobalVariables.globVarScript.SaveEggState();
	}

}
