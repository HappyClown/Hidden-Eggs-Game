using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakALegEngine : MonoBehaviour {

	public BreakALegLvl[] mylvls;
	public int currentLvl;
	// Use this for initialization
	void Start () {
		currentLvl = 0;
		mylvls[currentLvl].SetUpLvl();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q)){
			mylvls[currentLvl].MoveBag1(true);
		}else if(Input.GetKeyDown(KeyCode.A)){
			mylvls[currentLvl].MoveBag1(false);
		}else if(Input.GetKeyDown(KeyCode.W)){
			mylvls[currentLvl].MoveBag2(true);
		}else if(Input.GetKeyDown(KeyCode.S)){
			mylvls[currentLvl].MoveBag2(false);
		}else if(Input.GetKeyDown(KeyCode.E)){
			mylvls[currentLvl].MoveBag3(true);
		}else if(Input.GetKeyDown(KeyCode.D)){
			mylvls[currentLvl].MoveBag3(false);

		}
	}
}
