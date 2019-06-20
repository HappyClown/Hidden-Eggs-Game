using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPTextFall : MonoBehaviour {
	[Header ("Triggers")]
	public bool fallOn;
	[Header ("Settings")]
	public float timeBetweenChar = 0.1f;
	public bool randomOrder, startFromMid;
	public bool fadeInOnFall, bounceAfterFall, waveAfterFall;
	public AnimationCurve fallCurve = new AnimationCurve(new Keyframe(0, 1f), new Keyframe(0.5f, 0.5f), new Keyframe(1, 0f));
	public float fallCurveScale = 1.0f;
	public float fallDur = 1f;
	public AnimationCurve bounceCurve = new AnimationCurve(new Keyframe(0, 0f), new Keyframe(0.5f, 1f), new Keyframe(1, 0f));
	public float bounceCurveScale = 1.0f;
	public float bounceDur = 1f;
	public float bounceDampner = 1f;
	public int bounces = 1;
	[Header ("References")]
	public TMPMotionHandler handlerScript;
	public TMPTextColorFade colorFadeScript;
	public TMPTextWave waveScript;
	[Header ("Info")]
	public bool allFalling;
	public TMP_Text m_TextComponent;
	public int curChar = 0;
	public int updateAfterCount = 0;
	TMP_TextInfo textInfo;
	TMP_MeshInfo[] cachedMeshInfo;
	int characterCount;
	int space;
	public List<int> charOrder;

	void Update () {
		if (fallOn) {
			if (startFromMid) {
				StartCoroutine(StartFallFromMid());
			}
			else {
				StartCoroutine(StartFall());
			}
			fallOn = false;
		}
		// Force a mesh update only after all the currently moving elements have moved.
		if (allFalling && updateAfterCount >= characterCount * (curChar / (characterCount + space))) {
			m_TextComponent.ForceMeshUpdate();
			updateAfterCount = 0;
		}
	}

	IEnumerator StartFallFromMid() {
		//int currentCharacter = 0;
		int currentCharacterLeft;
		int currentCharacterRight;
		space = 0;
		Vector3[] vertsLeft;
		Vector3[] vertsRight;
		Vector3[] verts;

		allFalling = true;
		// Reset curChar value if all the characters had previously started their motion.
		if (curChar >= characterCount) {
			curChar = 0;
		}
		// Set the curve wrap modes. This can (and should) be done in the inspector.
		fallCurve.preWrapMode = WrapMode.Clamp;
        fallCurve.postWrapMode = WrapMode.Clamp;
		bounceCurve.preWrapMode = WrapMode.Loop;
        bounceCurve.postWrapMode = WrapMode.Loop;
		// Generate the mesh and populate the textInfo with data we can use and manipulate.
		m_TextComponent.ForceMeshUpdate();
		// Store textInfo and characterCount values.
		textInfo = m_TextComponent.textInfo;
		characterCount = textInfo.characterCount;

		// Start fading in the elements.
		if (fadeInOnFall) {
			colorFadeScript.startFadeIn = true;
		}
		// Assign the left to right order for fall to wave purposes.
		charOrder = handlerScript.leftRightOrder;
		// Check if even with modulo.
		// Even.
		if (characterCount%2 == 0) {
			currentCharacterLeft = characterCount/2 - 1;
			currentCharacterRight = currentCharacterLeft + 1;
		}
		// Odds.
		else {
			curChar = Mathf.CeilToInt(characterCount/2);
			// Get the index of the material used by the current character.
			int materialIndex = textInfo.characterInfo[curChar].materialReferenceIndex;
			// Get the array of vertices used by this text element.
			verts = textInfo.meshInfo[materialIndex].vertices;
			// Get the index of the first vertex used by this text element.
			int vertexIndex = textInfo.characterInfo[curChar].vertexIndex;
			if (textInfo.characterInfo[curChar].character.ToString() != " ") {
				StartCoroutine(FallOnce(verts, materialIndex, vertexIndex, curChar));
				curChar++;
			}
			else {
				space++;
			}
			currentCharacterLeft = curChar - 1;
			currentCharacterRight = curChar + 1;
		}
		// Fade in the rest of the characters. To the Left and to the Right at the same time.
		while (currentCharacterRight < characterCount)
		{
			// For the character to the left.
			// Get the index of the material used by the current character.
			int materialIndexLeft = textInfo.characterInfo[currentCharacterLeft].materialReferenceIndex;
			// Get the index of the first vertex used by this text element.
			int vertexIndexLeft = textInfo.characterInfo[currentCharacterLeft].vertexIndex;
			// Get the array of vertices used by this text element.
			vertsLeft = textInfo.meshInfo[materialIndexLeft].vertices;
			if (textInfo.characterInfo[currentCharacterLeft].character.ToString() != " ") {
				StartCoroutine(FallOnce(vertsLeft, materialIndexLeft, vertexIndexLeft, currentCharacterLeft));
				curChar++;
			}
			else {
				space++;
			}
			currentCharacterLeft--;

			// For the character to the right.
			// Get the index of the material used by the current character.
			int materialIndexRight = textInfo.characterInfo[currentCharacterRight].materialReferenceIndex;
			// Get the index of the first vertex used by this text element.
			int vertexIndexRight = textInfo.characterInfo[currentCharacterRight].vertexIndex;
			// Get the array of vertices used by this text element.
			vertsRight = textInfo.meshInfo[materialIndexRight].vertices;
			if (textInfo.characterInfo[currentCharacterRight].character.ToString() != " ") {
				StartCoroutine(FallOnce(vertsRight, materialIndexRight, vertexIndexRight, currentCharacterRight));
				curChar++;
			}
			else {
				space++;
			}
			currentCharacterRight++;

			yield return new WaitForSeconds(timeBetweenChar);
		}
	}

	IEnumerator StartFall() {
		allFalling = true;
		// Reset curChar value if all the characters had previously started their motion.
		if (curChar == characterCount) {
			curChar = 0;
		}
		// Set the curve wrap modes. This can (and should) be done in the inspector.
		fallCurve.preWrapMode = WrapMode.Clamp;
        fallCurve.postWrapMode = WrapMode.Clamp;
		bounceCurve.preWrapMode = WrapMode.Loop;
        bounceCurve.postWrapMode = WrapMode.Loop;
		// Generate the mesh and populate the textInfo with data we can use and manipulate.
		m_TextComponent.ForceMeshUpdate();
		// Store textInfo and characterCount values.
		textInfo = m_TextComponent.textInfo;
		characterCount = textInfo.characterCount;
		// Get the index of the mesh used by this character.
		int matIndex = textInfo.characterInfo[curChar].materialReferenceIndex;
		// Get the index of the first vertex used by this text element.
		int vertIndex = textInfo.characterInfo[curChar].vertexIndex;
		Vector3[] verts;
		
		// Start fading in the elements.
		if (fadeInOnFall) {
			colorFadeScript.startFadeIn = true;
		}
		// Check the correct bools in the TMPTextColorFade script to make it have the correct order.
		if (randomOrder) {
			charOrder = handlerScript.randomOrder;
		}
		else {
			charOrder = handlerScript.leftRightOrder;
		}
		// Start randomly falling characters.
		while (curChar < characterCount)
		{
			// Instructing TextMesh Pro not to upload the mesh as we will be modifying it.
			m_TextComponent.renderMode = TextRenderFlags.DontRender;
			// Get the index of the mesh used by this character.
			matIndex = textInfo.characterInfo[charOrder[curChar]].materialReferenceIndex;
			// Get the index of the first vertex used by this text element.
			vertIndex = textInfo.characterInfo[charOrder[curChar]].vertexIndex;
			// Get the array of vertices used by this text element.
			verts = textInfo.meshInfo[matIndex].vertices;
			// Ignore "spaces" as they do not seem to have vertices they cause problems.
			// But it does count as a character so the delay between characters falling must still be applied.
			if (textInfo.characterInfo[charOrder[curChar]].character.ToString() != " ") {
				StartCoroutine(FallOnce(verts, matIndex, vertIndex, charOrder[curChar]));
			}
			curChar++;
			// Delay between every text character.
			yield return new WaitForSeconds(timeBetweenChar);
		}
		//allFalling = true;
	}

	/// <summary>
	///  Method that makes a character fall.
	/// </summary>
	/// <param name="textComponent"></param>
	/// <returns></returns>
	IEnumerator FallOnce(Vector3[] vertices, int materialIndex, int vertexIndex, int currentCharacter) {
		float timer = 0f;
		int bounceCount = 1;
		bool falling = true;
		bool bouncing = false;

		float originalY0 = vertices[vertexIndex + 0].y;
		float originalY1 = vertices[vertexIndex + 1].y;
		float originalY2 = vertices[vertexIndex + 2].y;
		float originalY3 = vertices[vertexIndex + 3].y;

		while (falling)
		{
			timer += Time.deltaTime / fallDur;
			// Compute the baseline mid point for each character
			float yOffset = (fallCurve.Evaluate(timer) * fallCurveScale);
			// Apply offset to adjust our pivot point.
			vertices[vertexIndex + 0].y = originalY0 + yOffset;
			vertices[vertexIndex + 1].y = originalY1 + yOffset;
			vertices[vertexIndex + 2].y = originalY2 + yOffset;
			vertices[vertexIndex + 3].y = originalY3 + yOffset;
			// Upload the mesh with the revised information
			m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
			// Store textInfo values.
			textInfo = m_TextComponent.textInfo;
				
			// When to force update the vertices.
			updateAfterCount++;

			if (timer >= 1) {
				falling = false;
				if (bounceAfterFall) {
					bouncing = true;
				}
				else if (waveAfterFall && currentCharacter == handlerScript.leftRightOrder[characterCount-1]) {
					waveScript.waveOn = true;
				}
				timer = 0f;
			}
			yield return null;
		}

		while (bouncing) 
		{
			timer += Time.deltaTime / bounceDur;

			if (timer >= 1f) {
				bounceCount ++;
				timer = 0f;
			}
			// Compute the baseline mid point for each character
			float yOffset = ((bounceCurve.Evaluate(timer) * bounceCurveScale) / (bounceCount * bounceDampner));
			// Apply offset to adjust our pivot point.
			vertices[vertexIndex + 0].y = originalY0 + yOffset;
			vertices[vertexIndex + 1].y = originalY1 + yOffset;
			vertices[vertexIndex + 2].y = originalY2 + yOffset;
			vertices[vertexIndex + 3].y = originalY3 + yOffset;
			// Upload the mesh with the revised information
			m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
			// Store textInfo values.
			textInfo = m_TextComponent.textInfo;
			
			// When to force update the vertices.
			updateAfterCount++;

			if (bounceCount >= bounces) {
				bouncing = false;
				if (startFromMid) {
					curChar--;
				}
			}
			yield return null;
		}
	}
	// This method should be called by the ReferenceFinder Script/GameObject, to alleviate some of the burden on start functions.
	public void StartSetup() {
		m_TextComponent = this.gameObject.GetComponent<TMP_Text>();
	}
}