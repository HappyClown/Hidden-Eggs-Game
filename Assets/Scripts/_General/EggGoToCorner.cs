using UnityEngine;
using UnityEngine.SceneManagement;

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

	private float moveTimer;
	private float curveTime;
	private float moveSpeed;
	public AnimationCurve animCurve;
	private float distToSpot;
	private float constantSpeed;
	private float timeTest;
	public float settleEggDist = 0.005f;
//
	private float animCurveTestTime;
	private float animCurveTestAvVal;
	private float animCurveTestVal;
	private float animCurveTestFrames;
	private float distPercent;
	private float distLeft;
	private Vector3 startSpotInPanel;
	private Vector3 myStartPos, myStartRot, myStartScale;
//
	private float openPanelSpotx, openPanelSpoty, openPanelSpotz;



	void Start () 
	{
		if (!eggAnim) { eggAnim = this.GetComponent<Animator>(); }

		LoadEggFromCorrectScene();

		if (eggFound)
		{
			eggAnim.enabled = false;
			this.transform.position = mySpotInPanel.transform.position;
			this.transform.eulerAngles = cornerRot;
			this.transform.localScale = cornerEggScale;
			this.GetComponent<Collider2D>().enabled = false;
			if (!this.CompareTag("GoldenEgg")) { clickOnEggsScript.eggsFound += 1; }
			// else 
			// { 
			// 	GameObject gEgg = GameObject.Find("GoldenEgg");
			// 	gEgg.transform.localScale
			// }
			//moveThisEgg = true;
			//clickOnEggsScript.eggMoving -= 1;
			this.transform.parent = clickOnEggsScript.eggPanel.transform;
			Debug.Log(this.gameObject.name + " has been loaded as found already.");
			clickOnEggsScript.UpdateEggsString();
		}
		else
		{
			openPanelSpotx = mySpotInPanel.transform.position.x - (clickOnEggsScript.eggPanelHidden.transform.position.x - clickOnEggsScript.eggPanelShown.transform.position.x);
			openPanelSpoty = mySpotInPanel.transform.position.y - (clickOnEggsScript.eggPanelHidden.transform.position.y - clickOnEggsScript.eggPanelShown.transform.position.y);
			openPanelSpotz = mySpotInPanel.transform.position.z - (clickOnEggsScript.eggPanelHidden.transform.position.z - clickOnEggsScript.eggPanelShown.transform.position.z);
			startSpotInPanel = new Vector3(openPanelSpotx, openPanelSpoty, openPanelSpotz);

			myStartPos = new Vector3 (this.transform.position.x, this.transform.position.y, -4 + (clickOnEggsScript.eggsFound * -0.1f));
			myStartRot = new Vector3 (this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
			if (!this.CompareTag("GoldenEgg")) { myStartScale = new Vector3(1.4f ,1.4f ,1); } // Hardcoded, needs to be set to the end scale size of the egg in the "EggPop" animation which all the normal eggs use.
			else { myStartScale = new Vector3 (this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z); }

			// distToSpot = Vector3.Distance(new Vector3 (this.transform.position.x, this.transform.position.y, -4 + (clickOnEggsScript.eggsFound * -0.1f)), startSpotInPanel);
			// constantSpeed = (distToSpot + settleEggDist)/ timeToMove;

			// while (animCurveTestTime < 1)
			// {
			// 	animCurveTestTime += Time.deltaTime;
			// 	animCurveTestVal += animCurve.Evaluate(animCurveTestTime);
			// 	animCurveTestFrames++;
			// }
			// if (animCurveTestTime > 1)
			// {
			// 	animCurveTestAvVal = animCurveTestVal / animCurveTestFrames;
			// }
		}
	}

	// 		curveTime += Time.deltaTime / fadeTime;
	// 		newAlpha = animCurve.Evaluate(curveTime);
	// 		fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);

	void Update ()
	{
		if (moveThisEgg == true)
		{	
			timeTest += Time.deltaTime;
			//distLeft = Vector3.Distance(this.transform.position, startSpotInPanel);

			curveTime += Time.deltaTime / timeToMove;
			//distPercent =  distLeft / distToSpot;
			moveSpeed = animCurve.Evaluate(curveTime);

			this.transform.position = Vector3.Lerp(myStartPos, startSpotInPanel, moveSpeed);

			this.transform.eulerAngles = Vector3.Lerp(myStartRot, cornerRot, moveSpeed);

			this.transform.localScale = Vector3.Lerp(myStartScale, cornerEggScale, moveSpeed);

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
		if (!this.CompareTag("GoldenEgg")) { eggAnim.SetTrigger("EggPop"); }
		else { eggAnim.SetTrigger("TapAnim"); }
		
		if (mySpotInPanel == null)
		{
			mySpotInPanel = clickOnEggsScript.eggSpots[clickOnEggsScript.eggsFound];
		}

		eggTrail.SetActive(true);

		eggFound = true;

		SaveEggToCorrectFile();
	}


	// Called  as an event in the egg animations
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


	public void LoadEggFromCorrectScene()
	{
		if (SceneManager.GetActiveScene().name == "Market")
		{
			if (GlobalVariables.globVarScript.marketEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)])
			{
				eggFound = GlobalVariables.globVarScript.marketEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)];
			}
		}

		if (SceneManager.GetActiveScene().name == "Park")
		{
			if (GlobalVariables.globVarScript.parkEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)])
			{
				eggFound = GlobalVariables.globVarScript.parkEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)];
			}
		}

		if (SceneManager.GetActiveScene().name == "Beach")
		{
			if (GlobalVariables.globVarScript.beachEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)])
			{
				eggFound = GlobalVariables.globVarScript.beachEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)];
			}
		}
	}



	public void SaveEggToCorrectFile()
	{
		if (SceneManager.GetActiveScene().name == "Market")
		{
			GlobalVariables.globVarScript.marketEggToSave = this.eggFound;
			Debug.Log(GlobalVariables.globVarScript.marketEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)]);
			GlobalVariables.globVarScript.marketEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)] = this.eggFound;
			GlobalVariables.globVarScript.SaveEggState();
		}

		if (SceneManager.GetActiveScene().name == "Park")
		{
			GlobalVariables.globVarScript.parkEggToSave = this.eggFound;
			Debug.Log(GlobalVariables.globVarScript.parkEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)]);
			GlobalVariables.globVarScript.parkEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)] = this.eggFound;
			GlobalVariables.globVarScript.SaveEggState();
		}

		if (SceneManager.GetActiveScene().name == "Beach")
		{
			GlobalVariables.globVarScript.beachEggToSave = this.eggFound;
			Debug.Log(GlobalVariables.globVarScript.beachEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)]);
			GlobalVariables.globVarScript.beachEggsFoundBools[clickOnEggsScript.eggs.IndexOf(this.gameObject)] = this.eggFound;
			GlobalVariables.globVarScript.SaveEggState();
		}
	}
}
