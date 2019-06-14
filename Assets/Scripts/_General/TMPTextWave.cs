using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPTextWave : MonoBehaviour {
	[Header ("Triggers")]
	public bool waveOn;
	[Header ("Settings")]
	public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1.0f), new Keyframe(1, 0f));
	public float yMultiplier = 1.0f;
	//public float SpeedMultiplier = 1.0f;
	//public float CurveScale = 1.0f;
	public float timeBetweenChar = 0.1f;
	public float waveDur = 1f;
	public float charWaveUpdateTime = 0.025f;
	[Header ("Info")]
	public bool waving;
	public TMP_Text m_TextComponent;
	private int curChar = 0;
	private int updateAfterCount = 0;
	TMP_TextInfo textInfo;
	TMP_MeshInfo[] cachedMeshInfo;
	int characterCount;
	
	void Start() {
		textInfo = m_TextComponent.textInfo;
		characterCount = textInfo.characterCount;
	}

	void Update () {
		if (waveOn) {
			StartCoroutine(StartWave());
			waveOn = false;
		}
	}

	IEnumerator StartWave() {
		waving = true;
		
		m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
		
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		int characterCount = textInfo.characterCount;

		// Cache the vertex data of the text object as the Jitter FX is applied to the original position of the characters.
		cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
		

		while (curChar < characterCount)
		{
			// Get the index of the mesh used by this character.
			int matIndex = textInfo.characterInfo[curChar].materialReferenceIndex;
			// Get the index of the first vertex used by this text element.
			int vertIndex = textInfo.characterInfo[curChar].vertexIndex;

			StartCoroutine(ContinuousWave(matIndex, vertIndex, curChar));
			curChar++;
			// Delay between every text character.
			yield return new WaitForSeconds(timeBetweenChar);
		}
	}

	/// <summary>
	///  Method to animate a single character along a Unity animation curve.
	/// </summary>
	/// <param name="textComponent"></param>
	/// <returns></returns>
	IEnumerator ContinuousWave(int materialIndex, int vertexIndex, int currentCharacter)
	{
		float timer = 0f;

		Vector3[] vertices;

		vertices = textInfo.meshInfo[materialIndex].vertices;

		m_TextComponent.havePropertiesChanged = true; // Need to force the TextMeshPro Object to be updated.

		float originalY0 = vertices[0].y;
		float originalY1 = vertices[1].y;
		float originalY2 = vertices[2].y;
		float originalY3 = vertices[3].y;

		while (waving)
		{
			timer += Time.deltaTime / waveDur;

			// Compute the baseline mid point for each character
			Vector3 offsetToMidBaseline = new Vector2(0f, /* originalY + */ (VertexCurve.Evaluate(timer) * yMultiplier));

			// Apply offset to adjust our pivot point.
			vertices[vertexIndex + 0] = new Vector3(vertices[vertexIndex+0].x, originalY0 + VertexCurve.Evaluate(timer) * yMultiplier, 0);
			vertices[vertexIndex + 1] = new Vector3(vertices[vertexIndex+1].x, originalY1 + VertexCurve.Evaluate(timer) * yMultiplier, 0);
			vertices[vertexIndex + 2] = new Vector3(vertices[vertexIndex+2].x, originalY2 + VertexCurve.Evaluate(timer) * yMultiplier, 0);
			vertices[vertexIndex + 3] = new Vector3(vertices[vertexIndex+3].x, originalY3 + VertexCurve.Evaluate(timer) * yMultiplier, 0);
			// Upload the mesh with the revised information
			// m_TextComponent.UpdateVertexData();
			// Push changes into meshes
			for (int i = 0; i < textInfo.meshInfo.Length; i++)
			{
				textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
				m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
			}
			//updateAfterCount++;

			// After 100 waves reset timer to 0, makes me feel safer having this here.
			if (timer >= 100f) {
				timer = 0f;
			}
			// Update every x seconds.
			yield return new WaitForSeconds(charWaveUpdateTime);
		}
	}

	public void StartSetup() {
		m_TextComponent = this.gameObject.GetComponent<TMP_Text>();
	}
}
