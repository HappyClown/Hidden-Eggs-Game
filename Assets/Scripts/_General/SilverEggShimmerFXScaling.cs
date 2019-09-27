using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverEggShimmerFXScaling : MonoBehaviour 
{
	public ParticleSystem myPartSys;
	public ParticleSystem.MainModule myPartSysMain;

	public Vector3 iniPos, iniScale;
	public Vector3 refScale;
	public Transform silEggTrans;
	public float minSize, maxSize, iniMinSize, iniMaxSize;
	[Tooltip("Should be higher then the maximum Start Lifetime value opf the particle system.")]
	public float refreshDelay;
	private bool refreshSelf;
	private float refreshTimer;

	void Awake()
	{
		iniPos = this.transform.localPosition;
		iniScale = this.transform.localScale;
	}


	void Start () 
	{
		myPartSysMain = myPartSys.main;
		refreshTimer = refreshDelay;
	}
	
	void Update () 
	{
		refScale = silEggTrans.localScale;

		minSize = iniMinSize * refScale.x/*  * (1/refScale.x) */;
		maxSize = iniMaxSize * refScale.x/*  * (1/refScale.x) */;

		myPartSysMain.startSize = Random.Range(minSize, maxSize);

		if (refreshSelf)
		{
			refreshTimer -= Time.deltaTime;
			if (myPartSysMain.simulationSpeed < 3f) {
				myPartSysMain.simulationSpeed += 0.032f;
			}

			if (refreshTimer <= 0)
			{
				var myPartSysEm = myPartSys.emission;
				myPartSysEm.enabled = true;

				refreshSelf = false;
				refreshTimer = refreshDelay;

				myPartSysMain.simulationSpeed = 1f;

				this.transform.parent = silEggTrans;
				this.transform.localPosition = iniPos;
				this.transform.localScale = iniScale;
			}
		}
	}

	public void Refresh()
	{
		refreshSelf = true;
	}
}
