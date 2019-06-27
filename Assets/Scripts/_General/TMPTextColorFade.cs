using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPTextColorFade : MonoBehaviour {
	[Header ("Triggers")]
	public bool startFadeIn;
	public bool startFadeOut;
	[Header ("Settings")]
	public float fadeInDur = 1.0f;
	public float fadeOutDur = 1.0f;
	public float timeBetweenCharsIn = 0.5f;
	public float timeBetweenCharsOut = 0.5f;
	public Color iniCol;
	public bool fadeOutRightLeft, fadeInFromMid, fadeInLeftRight, fadeInRandom;
	public bool stayWarped;
	[Header ("References")]
	public TMPMotionHandler handlerScript;
	public TMPWarpText warpScript;
	public TMPTextFall fallScript;
	public TMP_Text m_TextComponent;
	public TextMeshProUGUI tmp;
	[Header ("Info")]
	public bool lastCharacterDone;
	public bool updateMaxAlpha = false;
	public TextState textState;
	public enum TextState {
		blank, fadingIn, fadingOut, fullyVisible
	}
	public List<int> charOrder;

	void Update () {
		if (startFadeIn) {
			if (warpScript) {
				warpScript.StartAnimatedWarp = true;
			}
			StartCoroutine(StartTextFadeIn());
			startFadeIn = false;
		}
		if (startFadeOut) {
			if (warpScript) {
				warpScript.StartAnimatedWarp = true;
			}
			StartCoroutine(StartTextFadeOut());
			startFadeOut = false;
		}
		if (updateMaxAlpha) {
			tmp.color = new Color(iniCol.r, iniCol.g, iniCol.b, 1);
			updateMaxAlpha = false;
		}
	}

	public void StartSetup() {
		textState = TextState.blank;
		m_TextComponent = this.GetComponent<TMP_Text>();
		tmp = this.GetComponent<TextMeshProUGUI>();
		iniCol = tmp.color;
		// Set the alpha to 0 while keeping its RGB values the same.
		tmp.color = new Color(iniCol.r, iniCol.g, iniCol.b, 0);
		if (this.gameObject.GetComponent<TMPWarpText>()) {
			warpScript = this.gameObject.GetComponent<TMPWarpText>();
		}
		if (this.gameObject.GetComponent<TMPMotionHandler>()) {
			handlerScript = this.gameObject.GetComponent<TMPMotionHandler>();
		}
	}

	public void Reset() {
		textState = TextState.blank;
		// Set the alpha to 0 while keeping its RGB values the same.
		tmp.color = new Color(iniCol.r, iniCol.g, iniCol.b, 0);
	}

	/// <summary>
	/// When started, gradually fades in text.
	/// </summary>
	/// <returns></returns>
	public IEnumerator StartTextFadeIn() {
		textState = TextState.fadingIn;
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		Color32[] newVertexColors;
		Color32 c0 = m_TextComponent.color;
		lastCharacterDone = false;

		if (fadeInRandom) {
			charOrder = handlerScript.randomOrder;
		}
		else {
			charOrder = handlerScript.leftRightOrder;
		}
		
		// Fade the text in from the middle of the text (will probably not work if the text is spread on 2+ lines).
		// If the text has an odd number of characters it will start from the middle character if it has an even number
		// it will start with both middle characters.
		if (fadeInFromMid) {
			int currentCharacter = 0;
			int currentCharacterLeft = 0;
			int currentCharacterRight = 0;
			//Debug.Log("Even if 0: " + textInfo.characterCount%2);
			// Check if even with modulo.
			// Even.
			if (textInfo.characterCount%2 == 0) {
				currentCharacterLeft = textInfo.characterCount/2 - 1;
				currentCharacterRight = currentCharacterLeft + 1;
				//Debug.Log("Cur char left: " + currentCharacterLeft + "  Cur char right: " + currentCharacterRight);
				//Debug.Log("Tots num of char: " + textInfo.characterCount);
			}
			// Odds.
			else {
				currentCharacter = Mathf.CeilToInt(textInfo.characterCount/2);
				int characterCount = textInfo.characterCount;
				// Get the index of the material used by the current character.
				int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
				// Get the vertex colors of the mesh used by this text element (character or sprite).
				newVertexColors = textInfo.meshInfo[materialIndex].colors32;
				// Get the index of the first vertex used by this text element.
				int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

				StartCoroutine(FadeInCharacter(materialIndex, vertexIndex, currentCharacter));
				currentCharacterLeft = currentCharacter - 1;
				currentCharacterRight = currentCharacter + 1;
			}
			// Fade in the rest of the characters. To the Left and to the Right at the same time.
			while (currentCharacterRight < textInfo.characterCount)
			{
				int characterCount = textInfo.characterCount;
				// For the character to the left.
				// Get the index of the material used by the current character.
				int materialIndexLeft = textInfo.characterInfo[currentCharacterLeft].materialReferenceIndex;
				// Get the vertex colors of the mesh used by this text element (character or sprite).
				newVertexColors = textInfo.meshInfo[materialIndexLeft].colors32;
				// Get the index of the first vertex used by this text element.
				int vertexIndexLeft = textInfo.characterInfo[currentCharacterLeft].vertexIndex;
				if (textInfo.characterInfo[currentCharacterLeft].character.ToString() != " ") {
					StartCoroutine(FadeInCharacter(materialIndexLeft, vertexIndexLeft, currentCharacterLeft));
				}
				//Debug.Log(currentCharacterLeft);
				currentCharacterLeft--;

				// For the character to the right.
				// Get the index of the material used by the current character.
				int materialIndexRight = textInfo.characterInfo[currentCharacterRight].materialReferenceIndex;
				// Get the vertex colors of the mesh used by this text element (character or sprite).
				newVertexColors = textInfo.meshInfo[materialIndexRight].colors32;
				// Get the index of the first vertex used by this text element.
				int vertexIndexRight = textInfo.characterInfo[currentCharacterRight].vertexIndex;
				if (textInfo.characterInfo[currentCharacterRight].character.ToString() != " ") {
					StartCoroutine(FadeInCharacter(materialIndexRight, vertexIndexRight, currentCharacterRight));
				}
				//Debug.Log(currentCharacterRight);
				currentCharacterRight++;

				yield return new WaitForSeconds(timeBetweenCharsIn);
			}
		}
		// Fade in with the same random order as the fallScript order.
		else {
			int curChar = 0;
			while (curChar < textInfo.characterCount)
			{
				// Get the index of the mesh used by this character.
				int matIndex = textInfo.characterInfo[charOrder[curChar]].materialReferenceIndex;
				// Get the index of the first vertex used by this text element.
				int vertIndex = textInfo.characterInfo[charOrder[curChar]].vertexIndex;
				// Get the vertex colors of the mesh used by this text element (character or sprite).
				newVertexColors = textInfo.meshInfo[matIndex].colors32;

				if (textInfo.characterInfo[charOrder[curChar]].character.ToString() != " ") {
					StartCoroutine(FadeInCharacter(matIndex, vertIndex, charOrder[curChar]));
				}
				curChar++;
				// Delay between every text character.
				yield return new WaitForSeconds(timeBetweenCharsIn);
			}
		}
		// // From left to right.
		// else {
		// 	int currentCharacter = 0;
		// 	while (currentCharacter < textInfo.characterCount)
		// 	{
		// 		int characterCount = textInfo.characterCount;
		// 		// Get the index of the material used by the current character.
		// 		int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
		// 		// Get the vertex colors of the mesh used by this text element (character or sprite).
		// 		newVertexColors = textInfo.meshInfo[materialIndex].colors32;
		// 		// Get the index of the first vertex used by this text element.
		// 		int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

		// 		StartCoroutine(FadeInCharacter(materialIndex, vertexIndex, currentCharacter));
		// 		currentCharacter++;
		// 		yield return new WaitForSeconds(timeBetweenCharsIn);
		// 	}
		// }
	}

	/// <summary>
	/// When started, gradually fades out text. (starting with the last character?)
	/// </summary>
	/// <returns></returns>
	public IEnumerator StartTextFadeOut() {
		textState = TextState.fadingOut;
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		Color32[] newVertexColors;
		Color32 c0 = m_TextComponent.color;
		lastCharacterDone = false;

		if (fadeOutRightLeft) {
			int currentCharacter = textInfo.characterCount - 1;
			while (currentCharacter > -1)
			{
				int characterCount = textInfo.characterCount;
				// Get the index of the material used by the current character.
				int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
				// Get the vertex colors of the mesh used by this text element (character or sprite).
				newVertexColors = textInfo.meshInfo[materialIndex].colors32;
				// Get the index of the first vertex used by this text element.
				int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

				StartCoroutine(FadeOutCharacter(materialIndex, vertexIndex, currentCharacter));
				currentCharacter--;
				yield return new WaitForSeconds(timeBetweenCharsOut);
			}
		}
		else {
			int currentCharacter = 0;
			while (currentCharacter < textInfo.characterCount)
			{
				int characterCount = textInfo.characterCount;
				// Get the index of the material used by the current character.
				int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
				// Get the vertex colors of the mesh used by this text element (character or sprite).
				newVertexColors = textInfo.meshInfo[materialIndex].colors32;
				// Get the index of the first vertex used by this text element.
				int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

				StartCoroutine(FadeOutCharacter(materialIndex, vertexIndex, currentCharacter));
				currentCharacter += 1;
				yield return new WaitForSeconds(timeBetweenCharsOut);
			}
		}
		
	}

	IEnumerator FadeInCharacter(int materialIndex, int vertexIndex, int currentCharacter) {
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		Color32[] newVertexColors;
		Color32 c0 = m_TextComponent.color;

		// Get the vertex colors of the mesh used by this text element (character or sprite).
		newVertexColors = textInfo.meshInfo[materialIndex].colors32;

		float alpha = 0f;
		float fadeToOne = 0f;
		while (alpha < 255)
		{
			fadeToOne += Time.deltaTime / fadeInDur;
			alpha = fadeToOne * 255;
			if (alpha >= 255) {
				alpha = 255;
			}

			if (textInfo.characterInfo[currentCharacter].isVisible)
			{
				c0 = new Color32(255, 255, 255, (byte)alpha);

				newVertexColors[vertexIndex + 0] = c0;
				newVertexColors[vertexIndex + 1] = c0;
				newVertexColors[vertexIndex + 2] = c0;
				newVertexColors[vertexIndex + 3] = c0;

				// Push all updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
				m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			}
			yield return null;
		}
		c0 = new Color32(255, 255, 255, 255);

		newVertexColors[vertexIndex + 0] = c0;
		newVertexColors[vertexIndex + 1] = c0;
		newVertexColors[vertexIndex + 2] = c0;
		newVertexColors[vertexIndex + 3] = c0;

		// Push all updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
		m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

		// After all the characters have fully faded in.
		if (currentCharacter == charOrder[textInfo.characterCount - 1]) {
			// Set the TMP font settings color back to 1 in order to be able to set it back to 0, making the whole text invisible again;
			// This is done in the update to allow other vertex modifying scripts to update their position after in the same frame. 
			updateMaxAlpha = true;
			tmp.color = new Color(iniCol.r, iniCol.g, iniCol.b, 1);
			// To make sure the text stays warped after the complete fade in.
			if (stayWarped && warpScript) {
				// Stop the infinite animated warp loop.
				if (warpScript.warping) {
					//warpScript.stopWarping = true;
				}
				// Make it warp one last time after the colors have been changed.
				warpScript.WarpText();
			}
			textState = TextState.fullyVisible;
			lastCharacterDone = true;
		}

		// Keep setting their alpha value until the last character has faded out.
		while (!lastCharacterDone)
		{
			c0 = new Color32(255, 255, 255, 255);

			newVertexColors[vertexIndex + 0] = c0;
			newVertexColors[vertexIndex + 1] = c0;
			newVertexColors[vertexIndex + 2] = c0;
			newVertexColors[vertexIndex + 3] = c0;

			// Push all updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
			m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			yield return null;
		}
		yield return null;
	}

	IEnumerator FadeOutCharacter(int materialIndex, int vertexIndex, int currentCharacter) {
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		Color32[] newVertexColors;
		Color32 c0 = m_TextComponent.color;

		// Get the vertex colors of the mesh used by this text element (character or sprite).
		newVertexColors = textInfo.meshInfo[materialIndex].colors32;

		float alpha = 255f;
		float fadeToOne = 1f;
		while (alpha > 0)
		{
			fadeToOne -= Time.deltaTime / fadeOutDur;
			alpha = fadeToOne * 255;
			if (alpha <= 0) {
				alpha = 0;
			}

			if (textInfo.characterInfo[currentCharacter].isVisible)
			{
				c0 = new Color32(255, 255, 255, (byte)alpha);

				newVertexColors[vertexIndex + 0] = c0;
				newVertexColors[vertexIndex + 1] = c0;
				newVertexColors[vertexIndex + 2] = c0;
				newVertexColors[vertexIndex + 3] = c0;

				// Push all updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
				m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			}
			yield return null;
		}

		c0 = new Color32(255, 255, 255, 0);

		newVertexColors[vertexIndex + 0] = c0;
		newVertexColors[vertexIndex + 1] = c0;
		newVertexColors[vertexIndex + 2] = c0;
		newVertexColors[vertexIndex + 3] = c0;

		// Push all updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
		m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

		// After all the characters have fully faded out.
		if (currentCharacter == 0 && fadeOutRightLeft || currentCharacter == textInfo.characterCount - 1) {
			// Set the TMP font settings color back to 1 in order to be able to set it back to 0, making the whole text invisible again;
			tmp.color = new Color(iniCol.r, iniCol.g, iniCol.b, 0);
			// To make sure the text stays warped after the complete fade in.
			if (warpScript && stayWarped) {
				// Stop the infinite animated warp loop.
				if (warpScript.warping) {
					warpScript.stopWarping = true;
				}
				// Make it warp one last time after the colors have been changed.
				warpScript.WarpText();
			}
			textState = TextState.blank;
			lastCharacterDone = true;
		}
		// Keep setting their alpha value until the last character has faded out.
		while (!lastCharacterDone)
		{
			c0 = new Color32(255, 255, 255, 0);

			newVertexColors[vertexIndex + 0] = c0;
			newVertexColors[vertexIndex + 1] = c0;
			newVertexColors[vertexIndex + 2] = c0;
			newVertexColors[vertexIndex + 3] = c0;

			// Push all updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
			m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			yield return null;
		}
		
		yield return null;
	}
}
