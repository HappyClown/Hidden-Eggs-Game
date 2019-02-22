using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringConnectFXHandler : MonoBehaviour {
	public List<ParticleSystem> connectionFXs;
	private int currentFX = 0;
	
	public void PlayConnectionFX (GameObject tile, int whatConnection) {
		GameObject go = connectionFXs[currentFX].gameObject;
		ParticleSystem ps = connectionFXs[currentFX];

		if (whatConnection == 1) { // Top
			go.transform.position = new Vector3 (tile.transform.position.x, tile.transform.position.y + 1, go.transform.position.z);
		}
		else if (whatConnection == 2) { // Right
			go.transform.position = new Vector3 (tile.transform.position.x + 1, tile.transform.position.y, go.transform.position.z);
		}
		else if (whatConnection == 3) { // Bottom
			go.transform.position = new Vector3 (tile.transform.position.x, tile.transform.position.y - 1, go.transform.position.z);
		}
		else if (whatConnection == 4) { // Left
			go.transform.position = new Vector3 (tile.transform.position.x - 1, tile.transform.position.y, go.transform.position.z);
		}

		ps.Play(true);

		currentFX++;
		if (currentFX > connectionFXs.Count - 1) {
			currentFX = 0;
		}
	}
}