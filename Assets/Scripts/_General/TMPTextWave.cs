using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPTextWave : MonoBehaviour {
	[Header ("Triggers")]
	public bool waveOn;
	[Header ("Settings")]
	public AnimationCurve waveCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1.0f), new Keyframe(1, 0f));
	public float curveScale = 1.0f;
	public float timeBetweenChar = 0.1f;
	public float waveDur = 1f;
	public bool randomOrder;
	public int firstChar, lastChar;
	[Header ("References")]
	public TMPMotionHandler handlerScript;
	[Header ("Info")]
	public bool fullyWaving;
	private bool waving;
	public TMP_Text m_TextComponent;
	public int curChar = 0;
	private int updateAfterCount = 0;
	TMP_TextInfo textInfo;
	int characterCount;
	public List<int> charOrder;

	void Update () {
		if (waveOn) {
			StartCoroutine(StartWave());
			waveOn = false;
		}
		
		if (waving && updateAfterCount >= lastChar * (curChar / lastChar)) {
			m_TextComponent.ForceMeshUpdate();
			updateAfterCount = 0;
		}
	}

	IEnumerator StartWave() {
		//fullyWaving = true;
		waving = true;
		
		//firstChar = 0;
		//lastChar = characterCount;

		//if (curChar == lastChar) {
			curChar = firstChar;
		//}
		waveCurve.preWrapMode = WrapMode.Loop;
        waveCurve.postWrapMode = WrapMode.Loop;
		
		m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
		
		textInfo = m_TextComponent.textInfo;
		characterCount = textInfo.characterCount;
		
		if (randomOrder) {
			charOrder = handlerScript.randomOrder;
		}
		else {
			charOrder = handlerScript.leftRightOrder;
		}
		
		// Get the index of the mesh used by this character.
		int matIndex = textInfo.characterInfo[charOrder[curChar]].materialReferenceIndex;
		// Get the index of the first vertex used by this text element.
		int vertIndex = textInfo.characterInfo[charOrder[curChar]].vertexIndex;
		Vector3[] verts;

		// Start the wave from Left to Right.
		while (curChar < lastChar)
		{
			// Instructing TextMesh Pro not to upload the mesh as we will be modifying it.
			m_TextComponent.renderMode = TextRenderFlags.DontRender;
			// Get the index of the mesh used by this character.
			matIndex = textInfo.characterInfo[charOrder[curChar]].materialReferenceIndex;
			// Get the index of the first vertex used by this text element.
			vertIndex = textInfo.characterInfo[charOrder[curChar]].vertexIndex;

			verts = textInfo.meshInfo[matIndex].vertices;

			if (textInfo.characterInfo[charOrder[curChar]].character.ToString() != " ") {
				StartCoroutine(ContinuousWave(verts, matIndex, vertIndex, charOrder[curChar]));
			}
			curChar++;
			// Delay between every text character.
			yield return new WaitForSeconds(timeBetweenChar);
		}
		//fullyWaving = true;
	}

	/// <summary>
	///  Method that makes a single character "wave" along a Unity animation curve.
	/// </summary>
	/// <param name="textComponent"></param>
	/// <returns></returns>
	IEnumerator ContinuousWave(Vector3[] vertices, int materialIndex, int vertexIndex, int currentCharacter) {
		float timer = 0f;

		float originalY0 = vertices[vertexIndex + 0].y;
		float originalY1 = vertices[vertexIndex + 1].y;
		float originalY2 = vertices[vertexIndex + 2].y;
		float originalY3 = vertices[vertexIndex + 3].y;

		while (waving)
		{
			timer += Time.deltaTime / waveDur;
			// Compute the Y offset for each character.
			float yOffset = (waveCurve.Evaluate(timer) * curveScale);
			// Apply offset to adjust our pivot point.
			vertices[vertexIndex + 0].y = originalY0 + yOffset;
			vertices[vertexIndex + 1].y = originalY1 + yOffset;
			vertices[vertexIndex + 2].y = originalY2 + yOffset;
			vertices[vertexIndex + 3].y = originalY3 + yOffset;
			// Upload the mesh with the revised information.
			m_TextComponent.UpdateVertexData();
			// When to force update the vertices.
			updateAfterCount++;
			// Update every x seconds.
			yield return null;
		}
	}

	public void StartSetup() {
		m_TextComponent = this.gameObject.GetComponent<TMP_Text>();
		if (lastChar == 0) lastChar = m_TextComponent.textInfo.characterCount;
	}
}
