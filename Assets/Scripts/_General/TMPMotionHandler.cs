using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPMotionHandler : MonoBehaviour {
	[Header ("Info")]
	public List<int> leftRightOrder = new List<int>();
	public List<int> randomOrder = new List<int>();
	public TMP_Text m_TextComponent;
	public TMP_TextInfo textInfo;
	public int characterCount, forceUpdateCount;

	// If possible call this somewhere else then in Start().
	void Start() {
		GenerateOrders();
		if (m_TextComponent == null) {
			m_TextComponent = this.gameObject.GetComponent<TMP_Text>();
		}
	}

	void Update() {
		if (forceUpdateCount == characterCount-1) {
			m_TextComponent.ForceMeshUpdate();
		}
	}

	// This method should be called by the ReferenceFinder Script/GameObject, to alleviate some of the burden on start functions.
	public void StartSetup() {
		m_TextComponent = this.gameObject.GetComponent<TMP_Text>();
	}

	// Create a list to be used by other TMP motion scripts.
	public void GenerateOrders() {
		// Store textInfo and characterCount values.
		textInfo = m_TextComponent.textInfo;
		characterCount = textInfo.characterCount;
		// Clear all the order lists.
		leftRightOrder.Clear();
		randomOrder.Clear();
		// Populate an int list in ascending order(0,1,2,3...).
		for (int i = 0; i < characterCount; i++)
		{
			leftRightOrder.Add(i);
			randomOrder.Add(i);
		}
		// Randomize the "character position" list.
		for (int i = characterCount - 1; i >= 0; i--)
		{
			int ran = Random.Range(0, i + 1);
			int num = randomOrder[ran];
			randomOrder[ran] = randomOrder[i];
			randomOrder[i] = num;
		}
	}
}
