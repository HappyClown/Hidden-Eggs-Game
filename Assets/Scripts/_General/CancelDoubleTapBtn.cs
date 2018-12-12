using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelDoubleTapBtn : MonoBehaviour {
	public inputDetector inputDetScript;
	public Button btn;

	void Start () {
		btn.onClick.AddListener(CancelDoubleTap);
	}
	
	public void CancelDoubleTap () {
		inputDetScript.cancelDoubleTap = true;
	}
}
