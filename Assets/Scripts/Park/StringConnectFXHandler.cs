using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringConnectFXHandler : MonoBehaviour {
	public List<ParticleSystem> connectionFXs;
	private int currentFX = 0;
	public AudioSceneParkPuzzle audioScenePuzGenScript;

	void Start() {
		if (!audioScenePuzGenScript) {
			audioScenePuzGenScript = GameObject.Find("Audio").GetComponent<AudioSceneParkPuzzle>();
		}
	}
	
	public void PlayConnectionFX (GameObject tile, int whatConnection) {
		GameObject go = connectionFXs[currentFX].gameObject;
		ParticleSystem ps = connectionFXs[currentFX];

		if (whatConnection == 1) { // Top
			go.transform.position = new Vector3 (tile.transform.position.x, tile.transform.position.y + 1, go.transform.position.z);
			audioScenePuzGenScript.connectSparkSnd();
		}
		else if (whatConnection == 2) { // Right
			go.transform.position = new Vector3 (tile.transform.position.x + 1, tile.transform.position.y, go.transform.position.z);
			audioScenePuzGenScript.connectSparkSnd();
		}
		else if (whatConnection == 3) { // Bottom
			go.transform.position = new Vector3 (tile.transform.position.x, tile.transform.position.y - 1, go.transform.position.z);
			audioScenePuzGenScript.connectSparkSnd();
		}
		else if (whatConnection == 4) { // Left
			go.transform.position = new Vector3 (tile.transform.position.x - 1, tile.transform.position.y, go.transform.position.z);
			audioScenePuzGenScript.connectSparkSnd();
		}

		ps.Play(true);

		currentFX++;
		if (currentFX > connectionFXs.Count - 1) {
			currentFX = 0;
		}
	}
}