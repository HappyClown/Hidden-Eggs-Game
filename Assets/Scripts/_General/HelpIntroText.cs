using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpIntroText : MonoBehaviour {
	public TextMeshProUGUI introTMP;
	void Start () {
		
	}
	
	void Update () {
		
	}

	public void ShowIntroText() {
		introTMP.gameObject.SetActive(true);
		
	}
}
