using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RainbowRiddle : MonoBehaviour 
{
    Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;

    [Header("Rainbow Riddle")]
	public List<GameObject> fruitBaskets;
	public int basketNumber;
	public GameObject goldenEgg;

    public LayerMask layerMask;

	public ParticleSystem firework01; 
	public ParticleSystem firework02;

	public bool fireworksFired;



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
		hit = OnClickInput.hit;
		
		// if (!GlobalVariables.globVarScript.rainbowRiddleSolved)
		// {
		// 	mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// 	mousePos2D = new Vector2 (mousePos.x, mousePos.y);

		// 	hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f, layerMask);
		// }

        if (hit)
        {
            if (hit.collider.CompareTag("FruitBasket") && Input.GetMouseButtonDown(0))
			{

				basketNumber += 1;

				hit.collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				
				if (basketNumber >= 6)
				{
					RainbowRiddleSolved ();
					//SpawnGoldenEgg;
					goldenEgg.SetActive(true);

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
					fruitBaskets[basketNumber].GetComponent<CircleCollider2D>().enabled = true;
				}
			}
			

			if (Input.GetMouseButtonDown(0) && !hit.collider.CompareTag("FruitBasket") && !goldenEgg.activeSelf)
			{
				basketNumber = 0;
				foreach (GameObject basket in fruitBaskets)
				{
					basket.GetComponent<CircleCollider2D>().enabled = false;
				}	
				fruitBaskets[0].GetComponent<CircleCollider2D>().enabled = true;
			}
        }   
    }



    public void RainbowRiddleSolved ()
	{
		GlobalVariables.globVarScript.rainbowRiddleSolved = true;
		GlobalVariables.globVarScript.SaveEggState();
	}

}
