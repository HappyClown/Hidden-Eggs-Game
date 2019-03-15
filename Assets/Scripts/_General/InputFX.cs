using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFX : MonoBehaviour {
	public ParticleSystem touchFX;
	public inputDetector inputDetScript;
	public bool isPhoneDevice;

	void Awake() {
		isPhoneDevice = false;
		if (SystemInfo.deviceType == DeviceType.Handheld){
            isPhoneDevice = true;
        }
	}
	
	void Update () {
		if (isPhoneDevice) {
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
				touchFX.transform.position = new Vector3(mousePos.x, mousePos.y, -1);
				touchFX.Play();
			}
		}
		else {
			if (Input.GetMouseButtonDown(0)) {
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				touchFX.transform.position = new Vector3(mousePos.x, mousePos.y, -1);
				touchFX.Play();
			}
		}
	}
}
