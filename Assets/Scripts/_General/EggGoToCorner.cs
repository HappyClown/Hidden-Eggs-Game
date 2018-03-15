using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggGoToCorner : MonoBehaviour 
{
	public Vector3 cornerPos;

	public ClickOnEggs clickOnEggsScript;

	public Vector3 cornerRot;

	public Vector3 cornerEggScale;

	public float timeToMove;

	public bool moveThisEgg;

	public GameObject mySpotInPanel;

	public Animator eggAnim;



	void Start () 
	{
		eggAnim = this.GetComponent<Animator>();
	}


	void Update ()
	{
		if (moveThisEgg == true)
		{
			this.transform.position = Vector3.Lerp(this.transform.position, mySpotInPanel.transform.position, timeToMove * Time.deltaTime);

			this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, cornerRot, timeToMove * Time.deltaTime);

			this.transform.localScale = Vector3.Lerp(this.transform.localScale, cornerEggScale, timeToMove * Time.deltaTime);

			if (Vector3.Distance(this.transform.position, mySpotInPanel.transform.position) <= 0.005f)
			{
				this.transform.position = mySpotInPanel.transform.position;
				this.transform.eulerAngles = cornerRot;
				this.transform.localScale = cornerEggScale;
				moveThisEgg = false;
				clickOnEggsScript.eggMoving -= 1;
				this.transform.parent = clickOnEggsScript.eggPanel.transform;
			}
		}
	}
	


	public void StartEggAnim () 
	{
		eggAnim.SetTrigger("EggPop");
		
		if (mySpotInPanel == null)
		{
			mySpotInPanel = clickOnEggsScript.eggSpots[clickOnEggsScript.eggsFound];
		}
		//StartCoroutine(MakeEggPop());
	}



	public void GoToCorner()
	{	
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -4 + (clickOnEggsScript.eggsFound * -0.1f));

		moveThisEgg = true;

		eggAnim.enabled = false;
	}
}
