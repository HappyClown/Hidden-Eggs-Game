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

	public bool eggFound;

	public GameObject eggTrail;

	public ParticleSystem eggClickFX;

	public bool eggClickFXPlayed;



	void Awake ()
	{

	}



	void Start () 
	{
		eggAnim = this.GetComponent<Animator>();

		if (GlobalVariables.globVarScript.eggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)])
		{
			eggFound = GlobalVariables.globVarScript.eggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)];
		}

		if (eggFound)
		{
			eggAnim.enabled = false;
			this.transform.position = mySpotInPanel.transform.position;
			this.transform.eulerAngles = cornerRot;
			this.transform.localScale = cornerEggScale;
			this.GetComponent<Collider2D>().enabled = false;
			clickOnEggsScript.eggsFound += 1;
			//moveThisEgg = true;
			//clickOnEggsScript.eggMoving -= 1;
			this.transform.parent = clickOnEggsScript.eggPanel.transform;
			Debug.Log(this.gameObject.name + " has been loaded as found already.");
		}
	}


	void Update ()
	{
		if (moveThisEgg == true)
		{
			this.transform.position = Vector3.Lerp(this.transform.position, mySpotInPanel.transform.position, timeToMove * Time.deltaTime);

			this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, cornerRot, timeToMove * Time.deltaTime);

			this.transform.localScale = Vector3.Lerp(this.transform.localScale, cornerEggScale, timeToMove * Time.deltaTime);

			// Arrived at corner spot.
			if (Vector3.Distance(this.transform.position, mySpotInPanel.transform.position) <= 0.005f)
			{
				this.transform.position = mySpotInPanel.transform.position;
				this.transform.eulerAngles = cornerRot;
				this.transform.localScale = cornerEggScale;
				moveThisEgg = false;
				clickOnEggsScript.eggMoving -= 1;
				this.transform.parent = clickOnEggsScript.eggPanel.transform;
				eggTrail.SetActive(false);
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

		eggTrail.SetActive(true);

		eggFound = true;

		GlobalVariables.globVarScript.eggToSave = this.eggFound;
		Debug.Log(GlobalVariables.globVarScript.eggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)]);
		GlobalVariables.globVarScript.eggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)] = this.eggFound;
		GlobalVariables.globVarScript.SaveEggState();
	}



	public void GoToCorner()
	{	
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -4 + (clickOnEggsScript.eggsFound * -0.1f));

		moveThisEgg = true;

		eggAnim.enabled = false;
	}

	public void PlayEggClickFX()
	{
		if (!eggClickFXPlayed) 
			{ 
				eggClickFX.Play(true); 
				eggClickFXPlayed = true;
			}
	}
}
