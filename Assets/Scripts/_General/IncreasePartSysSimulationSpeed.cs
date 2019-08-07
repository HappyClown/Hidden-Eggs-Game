using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePartSysSimulationSpeed : MonoBehaviour {
	public ParticleSystem myPartSys;
	public float increaseDur;
	public float maxSpeed;
	private bool changingSpeed;
	private float timer;
	private float curSpeed;
	private float iniSpeed;
	
	void Update () {
		if (changingSpeed) {
			timer += Time.deltaTime / increaseDur;
			curSpeed = Mathf.Lerp(iniSpeed, maxSpeed, timer);
			var partSysMain = myPartSys.main;
			partSysMain.simulationSpeed = curSpeed;
			if (timer >= 1f) {
				timer = 0f;
				changingSpeed = false;
			}
		}
	}
	
	public void IncreaseSimulationSpeed() {
		var partSysMain = myPartSys.main;
		iniSpeed = partSysMain.simulationSpeed;
		changingSpeed = true;
	}
}
