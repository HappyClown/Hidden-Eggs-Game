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
	public float CurveScale = 1.0f;
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
		int loopCount = 0;

        VertexCurve.preWrapMode = WrapMode.Loop;
        VertexCurve.postWrapMode = WrapMode.Loop;

		Vector3[] vertices;
		Vector3[] newVertexPositions;

		while (true)
		{
			m_TextComponent.renderMode = TextRenderFlags.DontRender; // Instructing TextMesh Pro not to upload the mesh as we will be modifying it.
            m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
			
			TMP_TextInfo textInfo = m_TextComponent.textInfo;
            int characterCount = textInfo.characterCount;
			// Get the index of the mesh used by this character.
			int matIndex = textInfo.characterInfo[0].materialReferenceIndex;
			// Get the index of the first vertex used by this text element.
			int vertIndex = textInfo.characterInfo[0].vertexIndex;
            
            newVertexPositions = textInfo.meshInfo[matIndex].vertices;

			for(int i = 0; i < characterCount; i++)
			{
				if (!textInfo.characterInfo[i].isVisible)
                    continue;
				// Get the index of the mesh used by this character.
				matIndex = textInfo.characterInfo[i].materialReferenceIndex;
				// Get the index of the first vertex used by this text element.
				vertIndex = textInfo.characterInfo[i].vertexIndex;
				
				vertices = textInfo.meshInfo[matIndex].vertices;

				float offsetY = VertexCurve.Evaluate((float)i / characterCount + loopCount / 50f) * CurveScale; // Random.Range(-0.25f, 0.25f);
				// Compute the baseline mid point for each character
				// Vector3 offsetToMidBaseline = new Vector2(0f, VertexCurve.Evaluate(timer) + (timeBetweenChar * (i + 1)) * yMultiplier);
				// Apply offset to adjust our pivot point.
				vertices[vertIndex + 0].y += offsetY;
				vertices[vertIndex + 1].y += offsetY;
				vertices[vertIndex + 2].y += offsetY;
				vertices[vertIndex + 3].y += offsetY;
			}
			loopCount++;
			// Upload the mesh with the revised information
			m_TextComponent.UpdateVertexData();
			// Delay between every text character.
			//yield return null;
			yield return new WaitForSeconds(0.025f);
		}
	}
}