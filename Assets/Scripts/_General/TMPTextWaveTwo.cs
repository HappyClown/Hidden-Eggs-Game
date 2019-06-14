using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPTextWaveTwo : MonoBehaviour {
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
	private float timer, originalY;

	void Update () {
		if (waveOn) {
			StartCoroutine(StartWave());
			waveOn = false;
		}
		timer += Time.deltaTime/waveDur;
		// if (timer >= 1) {
		// 	timer = timer - 1;
		// }
	}

	public void StartSetup() {
		m_TextComponent = this.gameObject.GetComponent<TMP_Text>();
	}

	IEnumerator StartWave() {
		waving = true;
		// VertexCurve.preWrapMode = WrapMode.Loop;
		// VertexCurve.postWrapMode = WrapMode.Loop;

		// Vector3[] vertices;
		// Matrix4x4 matrix;

		// m_TextComponent.havePropertiesChanged = true; // Need to force the TextMeshPro Object to be updated.

		Vector3[] vertices;

		m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
		
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		int characterCount = textInfo.characterCount;
		int loop = 0;

		m_TextComponent.havePropertiesChanged = true; // Need to force the TextMeshPro Object to be updated.
		
		// Get the index of the mesh used by this character.
		int matIndex = textInfo.characterInfo[0].materialReferenceIndex;
		// Get the index of the first vertex used by this text element.
		int vertIndex = textInfo.characterInfo[0].vertexIndex;
		//m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
		//Mesh mesh = m_TextComponent.textInfo.meshInfo[0].mesh;

		vertices = textInfo.meshInfo[matIndex].vertices;
		originalY = (vertices[0].y + vertices[2].y) / 2;

		while (loop < 10000)
		{
			for(int i = 0; i < characterCount; i++)
			{
				// Get the index of the mesh used by this character.
				matIndex = textInfo.characterInfo[i].materialReferenceIndex;
				// Get the index of the first vertex used by this text element.
				vertIndex = textInfo.characterInfo[i].vertexIndex;
				//m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
				//Mesh mesh = m_TextComponent.textInfo.meshInfo[0].mesh;
				
				vertices = textInfo.meshInfo[matIndex].vertices;
				// Compute the baseline mid point for each character
				Vector3 offsetToMidBaseline = new Vector2(0f, VertexCurve.Evaluate(timer) + (timeBetweenChar * (i + 1)) * yMultiplier);
				//Debug.Log(timer);
				Debug.Log(VertexCurve.Evaluate(timer));
				// Apply offset to adjust our pivot point.
				vertices[vertIndex + 0] += offsetToMidBaseline;
				vertices[vertIndex + 1] += offsetToMidBaseline;
				vertices[vertIndex + 2] += offsetToMidBaseline;
				vertices[vertIndex + 3] += offsetToMidBaseline;
			}
			// Upload the mesh with the revised information
			m_TextComponent.UpdateVertexData();
			loop++;
			// Delay between every text character.
			yield return null;
			//yield return new WaitForSeconds(timeBetweenChar);
		}
	}

	/// <summary>
	///  Method to animate a single character along a Unity animation curve.
	/// </summary>
	/// <param name="textComponent"></param>
	/// <returns></returns>
	IEnumerator ContinuousWave(int materialIndex, int vertexIndex, int currentCharacter)
	{
		//m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
		//Mesh mesh = m_TextComponent.textInfo.meshInfo[0].mesh;
		float timer = 0f;

		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		int characterCount = textInfo.characterCount;

		Vector3[] vertices;
		// Matrix4x4 matrix;

		vertices = textInfo.meshInfo[materialIndex].vertices;

		m_TextComponent.havePropertiesChanged = true; // Need to force the TextMeshPro Object to be updated.

		float originalY = (vertices[0].y + vertices[2].y) / 2;

		while (waving)
		{
			timer += Time.deltaTime / waveDur;
			m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.

			// Compute the baseline mid point for each character
			Vector3 offsetToMidBaseline = new Vector2(0f, originalY + (VertexCurve.Evaluate(timer) * yMultiplier));
			// float offsetY = VertexCurve.Evaluate((float)i / characterCount + loopCount / 50f); // Random.Range(-0.25f, 0.25f);
			// Debug.Log("Vector2 offset: " + offsetToMidBaseline);

			// Apply offset to adjust our pivot point.
			vertices[vertexIndex + 0] += -offsetToMidBaseline;
			vertices[vertexIndex + 1] += -offsetToMidBaseline;
			vertices[vertexIndex + 2] += -offsetToMidBaseline;
			vertices[vertexIndex + 3] += -offsetToMidBaseline;
			// Upload the mesh with the revised information
			m_TextComponent.UpdateVertexData();
			// for (int i = 0; i < textInfo.meshInfo.Length; i++)
			// {
			// 	textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
			// 	m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
			// }

			// After 100 waves reset timer to 0, makes me feel safer having this here.
			if (timer >= 100f) {
				timer = 0f;
			}
			// Update every x seconds.
			yield return new WaitForSeconds(charWaveUpdateTime);
		}
	}
}
