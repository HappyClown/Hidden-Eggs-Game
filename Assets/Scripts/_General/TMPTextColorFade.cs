using UnityEngine;
using System.Collections;
using TMPro;

public class TMPTextColorFade : MonoBehaviour {
	[Header ("Triggers")]
	public bool startFadeIn;
	[Header ("Settings")]
	public float fadeInDur = 1.0f;
	public float timeBetweenChars = 0.5f;
	public Color iniCol;
	[Header ("Info")]
	public TextState textState;
	public enum TextState {
		blank, fadingIn, fullyVisible
	}
	public TMP_Text m_TextComponent;
	public TextMeshProUGUI tmp;
	public TMPWarpText warpTextScript;

	void Update () {
		if (startFadeIn) {
			if (warpTextScript) {
				warpTextScript.StartAnimatedWarp = true;
			}
			//StartSetup();
			StartCoroutine(StartTextFade());
			startFadeIn = false;
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
			warpTextScript = this.gameObject.GetComponent<TMPWarpText>();
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
	public IEnumerator StartTextFade() {
		textState = TextState.fadingIn;
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		int currentCharacter = 0;
		Color32[] newVertexColors;
		Color32 c0 = m_TextComponent.color;

		while (currentCharacter < textInfo.characterCount)
		{
			int characterCount = textInfo.characterCount;
			// Get the index of the material used by the current character.
			int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
			// Get the vertex colors of the mesh used by this text element (character or sprite).
			newVertexColors = textInfo.meshInfo[materialIndex].colors32;
			// Get the index of the first vertex used by this text element.
			int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

			StartCoroutine(FadeInCharacter(materialIndex, vertexIndex, currentCharacter));
			currentCharacter += 1;
			yield return new WaitForSeconds(timeBetweenChars);
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
			if (alpha > 255) {
				alpha = 255;
			}
			//Debug.Log(alpha);

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
		if (currentCharacter == textInfo.characterCount - 1) {
			// Set the TMP font settings color back to 1 in order to be able to set it back to 0, making the whole text invisible again;
			tmp.color = new Color(iniCol.r, iniCol.g, iniCol.b, 1);
			// To make sure the text stays warped after the complete fade in.
			if (warpTextScript) {
				// Stop the infinite animated warp loop.
				if (warpTextScript.warping) {
					warpTextScript.stopWarping = true;
				}
				// Make it warp one last time after the colors have been changed.
				warpTextScript.WarpText();
			}
			Debug.Log("Text fully faded in! No really, all of it!!");
		}

		yield return null;
	}

}
