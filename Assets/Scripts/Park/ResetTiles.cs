using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetTiles : MonoBehaviour 
{
	public Button resetButton;
	public List<GameObject> tiles;
	public KitePuzzEngine kitePuzzEngineScript;
	public SceneTapEnabler sceneTapEnaScript;

	void Start () 
	{
		resetButton = this.GetComponent<Button>();
		resetButton.onClick.AddListener(ResetTilePos);
	}

	void Update () {
		if (!sceneTapEnaScript.canTapPauseBtn) {
			resetButton.interactable = false;
		}
		else {
			resetButton.interactable = true;
		}
	}

	public void FillTileResetArray ()
	{
		tiles.Clear();
		foreach (Transform tile in kitePuzzEngineScript.itemHolder.transform)
		{
			tiles.Add(tile.gameObject);
		}
	}

	public void ResetTilePos () // Used for the reset items button
	{
		if (kitePuzzEngineScript.canPlay)
		{
			kitePuzzEngineScript.connections = 0;

			for (int i = 0; i < tiles.Count; i ++)
			{
				tiles[i].GetComponent<TileRotation>().ResetThisTile();
			}
		}
	}

	public void EndOfLevelReset() // Used in new level setup
	{
		for (int i = 0; i < tiles.Count; i ++)
		{
			//Debug.Log("Should reset items");
			tiles[i].GetComponent<TileRotation>().ResetThisTile();
		}
	}
}
