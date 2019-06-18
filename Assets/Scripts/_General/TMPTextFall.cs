using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPTextFall : MonoBehaviour {
	[Header ("Triggers")]
	public bool fallOn;
	[Header ("Settings")]
	public float timeBetweenChar = 0.1f;
	public bool randomOrder;
	public AnimationCurve fallCurve = new AnimationCurve(new Keyframe(0, 1f), new Keyframe(0.5f, 0.5f), new Keyframe(1, 0f));
	public float fallCurveScale = 1.0f;
	public float fallDur = 1f;
	public AnimationCurve bounceCurve = new AnimationCurve(new Keyframe(0, 0f), new Keyframe(0.5f, 1f), new Keyframe(1, 0f));
	public float bounceCurveScale = 1.0f;
	public float bounceDur = 1f;
	public int bounces = 1;
	[Header ("Info")]
	public bool allFalling;
	// private bool falling, bouncing;
	public TMP_Text m_TextComponent;
	public int curChar = 0;
	private int updateAfterCount = 0;
	TMP_TextInfo textInfo;
	TMP_MeshInfo[] cachedMeshInfo;
	int characterCount;
	public List<int> charOrder;

	void Update () {
		if (fallOn) {
			StartCoroutine(StartFall());
			fallOn = false;
		}
		
		if (allFalling && updateAfterCount >= characterCount * (curChar / characterCount)) {
			m_TextComponent.ForceMeshUpdate();
			updateAfterCount = 0;
		}
	}

	IEnumerator StartFall() {
		//falling = true;
		if (curChar == characterCount) {
			curChar = 0;
		}
		fallCurve.preWrapMode = WrapMode.Clamp;
        fallCurve.postWrapMode = WrapMode.Clamp;
		
		m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
		
		textInfo = m_TextComponent.textInfo;
		characterCount = textInfo.characterCount;
		
		// Get the index of the mesh used by this character.
		int matIndex = textInfo.characterInfo[curChar].materialReferenceIndex;
		// Get the index of the first vertex used by this text element.
		int vertIndex = textInfo.characterInfo[curChar].vertexIndex;
		Vector3[] verts;
		
		// Populate an int list in ascending order(0,1,2,3...).
		charOrder.Clear();
		for (int i = 0; i < characterCount; i++)
		{
			charOrder.Add(i);
		}
		// Randomize the "character position" list.
		if (randomOrder) {
			// New list order.
			for (int i = characterCount - 1; i >= 0; i--)
			{
				int ran = Random.Range(0, i + 1);
				int num = charOrder[ran];
				charOrder[ran] = charOrder[i];
				charOrder[i] = num;
			}
		}

		// Start randomly falling characters.
		while (curChar < characterCount)
		{
			m_TextComponent.renderMode = TextRenderFlags.DontRender; // Instructing TextMesh Pro not to upload the mesh as we will be modifying it.
			
			// Get the index of the mesh used by this character.
			matIndex = textInfo.characterInfo[charOrder[curChar]].materialReferenceIndex;
			// Get the index of the first vertex used by this text element.
			vertIndex = textInfo.characterInfo[charOrder[curChar]].vertexIndex;

			verts = textInfo.meshInfo[matIndex].vertices;

			if (textInfo.characterInfo[charOrder[curChar]].character.ToString() != " ") {
				StartCoroutine(FallOnce(verts, matIndex, vertIndex, charOrder[curChar]));
			}
			curChar++;
			// Delay between every text character.
			yield return new WaitForSeconds(timeBetweenChar);
		}
		allFalling = true;
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
			m_TextComponent.UpdateVertexData();
				
			// When to force update the vertices.
			updateAfterCount++;

			if (timer >= 1) {
				falling = false;
				bouncing = true;
				timer = 0f;
			}
			// Update every x seconds.
			yield return null;
		}

		while (bouncing) 
		{
			if (timer >= 1f) {
				bounceCount ++;
				timer = 0f;
			}
			timer += Time.deltaTime / bounceDur;
			// Compute the baseline mid point for each character
			float yOffset = ((bounceCurve.Evaluate(timer) * bounceCurveScale) / bounceCount);
			// Apply offset to adjust our pivot point.
			vertices[vertexIndex + 0].y = originalY0 + yOffset;
			vertices[vertexIndex + 1].y = originalY1 + yOffset;
			vertices[vertexIndex + 2].y = originalY2 + yOffset;
			vertices[vertexIndex + 3].y = originalY3 + yOffset;
			// Upload the mesh with the revised information
			m_TextComponent.UpdateVertexData();
			
			// When to force update the vertices.
			updateAfterCount++;

			if (bounceCount >= bounces) {
				bouncing = false;
			}
			// Update every x seconds.
			yield return null;
		}
	}

	public void StartSetup() {
		m_TextComponent = this.gameObject.GetComponent<TMP_Text>();
	}
}