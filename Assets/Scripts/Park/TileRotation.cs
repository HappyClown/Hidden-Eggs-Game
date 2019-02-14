using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotation : MonoBehaviour 
{
	#region TileRotation Variables
	[Header("Original Values")]
	public List<bool> oGConnections;
	public Vector3 oGPos, oGRot;
	[Header("Values")]
	public int initialRot;
	public float zRotation;
	public bool topConnection, rightConnection, bottomConnection, leftConnection;
	public float neightborTileDist;
	public Vector2 topRay, rightRay, bottomRay, leftRay;
	public List<bool> myConnections;
	public bool newTopConnection, newRightConnection, newBottomConnection, newLeftConnection;
	public bool canBeRotated, isEmpty, movedTile;
	[Header("Scripts")]
	public KitePuzzEngine gameEngineScript;
	public StringConnectFXHandler stringConnectFXScript;
	#endregion


	void Awake () {
		if(!this.GetComponent<SpriteRenderer>().enabled) {
			topConnection = false;
			rightConnection = false;
			bottomConnection = false;
			leftConnection = false;
		}

		oGPos = this.transform.localPosition;
		oGRot = this.transform.localEulerAngles;
		oGConnections.Add(topConnection); oGConnections.Add(rightConnection); oGConnections.Add(bottomConnection); oGConnections.Add(leftConnection);
	}


	public void CheckNeighbors () {
		if(topConnection) {
			topRay = new Vector2 (this.transform.position.x, this.transform.position.y + neightborTileDist);
			RaycastHit2D topHit = Physics2D.Raycast(topRay, Vector3.forward, 50f);
			if (topHit) {
				if (topHit.collider.CompareTag("Tile")) {
					GameObject topTile = topHit.collider.gameObject;
					if (topTile.GetComponent<TileRotation>().bottomConnection == true && this.topConnection == true) {
						gameEngineScript.connections += 1;
						if (movedTile) { 
							stringConnectFXScript.PlayConnectionFX(this.gameObject, 1); 
						}
					}
				}
			}
		}

		if(rightConnection) {
			rightRay = new Vector2 (this.transform.position.x + neightborTileDist, this.transform.position.y);
			RaycastHit2D rightHit = Physics2D.Raycast(rightRay, Vector3.forward, 50f);
			if (rightHit) {
				if (rightHit.collider.CompareTag("Tile")) {
					GameObject rightTile = rightHit.collider.gameObject;
					if (rightTile.GetComponent<TileRotation>().leftConnection == true && this.rightConnection == true) {
						gameEngineScript.connections += 1;
						if (movedTile) { 
							stringConnectFXScript.PlayConnectionFX(this.gameObject, 2); 
						}
					}
				}
			}
		}

		if(bottomConnection) {
			bottomRay = new Vector2 (this.transform.position.x, this.transform.position.y - neightborTileDist);
			RaycastHit2D bottomHit = Physics2D.Raycast(bottomRay, Vector3.forward, 50f);
			if (bottomHit) {
				if (bottomHit.collider.CompareTag("Tile")) {
					GameObject bottomTile = bottomHit.collider.gameObject;
					if (bottomTile.GetComponent<TileRotation>().topConnection == true && this.bottomConnection == true) {
						gameEngineScript.connections += 1;
						if (movedTile) { 
							stringConnectFXScript.PlayConnectionFX(this.gameObject, 3); 
						}
					}
				}
			}
		}

		if(leftConnection) {
			leftRay = new Vector2 (this.transform.position.x - neightborTileDist, this.transform.position.y);
			RaycastHit2D leftHit = Physics2D.Raycast(leftRay, Vector3.forward, 50f);
			if (leftHit) {
				if (leftHit.collider.CompareTag("Tile")) {
					GameObject leftTile = leftHit.collider.gameObject;
					if (leftTile.GetComponent<TileRotation>().rightConnection == true && this.leftConnection == true) {
						gameEngineScript.connections += 1;
						if (movedTile) { 
							stringConnectFXScript.PlayConnectionFX(this.gameObject, 4); 
						}
					}
				}
			}
		}

		movedTile = false;
	}

	public void RotateTile() {
		this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z - 90);

		if (topConnection) { newRightConnection = true; } else { newRightConnection = false; }
		if (rightConnection) { newBottomConnection = true; } else { newBottomConnection = false; }
		if (bottomConnection) { newLeftConnection = true; } else { newLeftConnection = false; }
		if (leftConnection) { newTopConnection = true; } else { newTopConnection = false; }

		topConnection = newTopConnection;
		rightConnection = newRightConnection;
		bottomConnection = newBottomConnection;
		leftConnection = newLeftConnection;
	}

	public void ResetThisTile() {
		this.transform.localPosition = oGPos;
		this.transform.localEulerAngles = oGRot;

		topConnection = oGConnections[0];
		rightConnection = oGConnections[1];
		bottomConnection = oGConnections[2];
		leftConnection = oGConnections[3];
	}
}