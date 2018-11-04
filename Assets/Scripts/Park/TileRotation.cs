using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotation : MonoBehaviour 
{
	#region TileRotation Variables
	[Header("Original Values")]

	public Vector3 oGPos;
	public Vector3 oGRot;
	public List<bool> oGConnections;

	[Header("Values")]

	public int initialRot;

	public float zRotation;

	public bool topConnection;
	public bool rightConnection;
	public bool bottomConnection;
	public bool leftConnection;

	public Vector2 topRay;
	public Vector2 rightRay;
	public Vector2 bottomRay;
	public Vector2 leftRay;

	public float neightborTileDist;

	public KitePuzzEngine gameEngineScript;

	public bool canBeRotated;

	public List<bool> myConnections;

	public bool newTopConnection;
	public bool newRightConnection;
	public bool newBottomConnection;
	public bool newLeftConnection;
	#endregion


	void Awake () 
	{
		if(!this.GetComponent<SpriteRenderer>().enabled)
		{
			topConnection = false;
			rightConnection = false;
			bottomConnection = false;
			leftConnection = false;
		}

		oGPos = this.transform.localPosition;
		oGRot = this.transform.localEulerAngles;
		oGConnections.Add(topConnection); oGConnections.Add(rightConnection); oGConnections.Add(bottomConnection); oGConnections.Add(leftConnection);

		// myConnections.Add(topConnection);
		// myConnections.Add(rightConnection);
		// myConnections.Add(bottomConnection);
		// myConnections.Add(leftConnection);

		// zRotation = this.transform.rotation.z;

		// initialRot = Random.Range(0,4);

		// if (initialRot == 0) { zRotation = 0; } else if (initialRot == 1) { zRotation = 90; } else if (initialRot == 2) { zRotation = 180; } else if (initialRot == 3) { zRotation = 270; }

		// this.transform.eulerAngles = new Vector3(this.transform.rotation.x, this.transform.rotation.y, zRotation);

	}


	public void CheckNeighbors ()
	{
		//mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(topConnection){
			topRay = new Vector2 (this.transform.position.x, this.transform.position.y + neightborTileDist);
			RaycastHit2D topHit = Physics2D.Raycast(topRay, Vector3.forward, 50f);
			if (topHit)
			{
				if (topHit.collider.CompareTag("Tile"))
				{
					GameObject topTile = topHit.collider.gameObject;

					if (topTile.GetComponent<TileRotation>().bottomConnection == true && this.topConnection == true)
					{
						// add one to the connection count yo!
						gameEngineScript.connections += 1;
					}
					else 
					{
						// minus one to connection count but it wont work its ok tho :) maybe maybe
						//gameEngineScript.connections -= 1;
					}
				}
			}
		}
		if(rightConnection){
			rightRay = new Vector2 (this.transform.position.x + neightborTileDist, this.transform.position.y);
			RaycastHit2D rightHit = Physics2D.Raycast(rightRay, Vector3.forward, 50f);
			if (rightHit)
			{
				if (rightHit.collider.CompareTag("Tile"))
				{
					GameObject rightTile = rightHit.collider.gameObject;

					if (rightTile.GetComponent<TileRotation>().leftConnection == true && this.rightConnection == true)
					{
						// add one to the connection count yo!
						gameEngineScript.connections += 1;
					}
					else 
					{
						// minus one to connection count but it wont work its ok tho :) maybe maybe
						//gameEngineScript.connections -= 1;
					}
				}
			}
		}
		if(bottomConnection){
			bottomRay = new Vector2 (this.transform.position.x, this.transform.position.y - neightborTileDist);
			RaycastHit2D bottomHit = Physics2D.Raycast(bottomRay, Vector3.forward, 50f);
			if (bottomHit)
			{
				if (bottomHit.collider.CompareTag("Tile"))
				{
					GameObject bottomTile = bottomHit.collider.gameObject;

					if (bottomTile.GetComponent<TileRotation>().topConnection == true && this.bottomConnection == true)
					{
						// add one to the connection count yo!
						gameEngineScript.connections += 1;
					}
					else 
					{
						// minus one to connection count but it wont work its ok tho :) maybe maybe
						//gameEngineScript.connections -= 1;
					}
				}
			}
		}

		if(leftConnection){
			leftRay = new Vector2 (this.transform.position.x - neightborTileDist, this.transform.position.y);
			RaycastHit2D leftHit = Physics2D.Raycast(leftRay, Vector3.forward, 50f);
			if (leftHit)
			{
				if (leftHit.collider.CompareTag("Tile"))
				{
					GameObject leftTile = leftHit.collider.gameObject;

					if (leftTile.GetComponent<TileRotation>().rightConnection == true && this.leftConnection == true)
					{
						// add one to the connection count yo!
						gameEngineScript.connections += 1;
					}
					else 
					{
						// minus one to connection count but it wont work its ok tho :) maybe maybe
						//gameEngineScript.connections -= 1;
					}
				}
			}
		}

	}



	public void RotateTile()
	{
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


	public void ResetThisTile()
	{
		this.transform.localPosition = oGPos;
		this.transform.localEulerAngles = oGRot;

		topConnection = oGConnections[0];
		rightConnection = oGConnections[1];
		bottomConnection = oGConnections[2];
		leftConnection = oGConnections[3];
	}

}
