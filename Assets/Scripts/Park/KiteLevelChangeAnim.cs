using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteLevelChangeAnim : MonoBehaviour {
	[Header("Values")]
	public int myLvl;
	[Header("Animation")]
	private bool animTrigg, animEnabled;
	public Animator anim;
	public ParticleSystem leafsFalling;
	private Vector3 kiteOrigPos, kiteOrigScale, kiteOrigRot;
	public List<GameObject> kitePieces;
	private List<Vector3> kitePiecesOrigPos = new List<Vector3>();
	private List<Vector3> kitePiecesOrigScale = new List<Vector3>();
	private List<Vector3> kitePiecesOrigRot = new List<Vector3>();
	private bool resetKite, waitFrame;
	public SpriteRenderer kiteSprite;
	[Header("Script")]
	public KiteLevelChangeEvent kiteLevelChangeScript;
	public MainPuzzleEngine mainPuzzEngScript;

	void Start () {
		kiteOrigPos = this.transform.position;
		kiteOrigScale = this.transform.localScale;
		kiteOrigRot = this.transform.localEulerAngles;
		if (kitePieces.Count > 0) {
			for(int i = 0; i < kitePieces.Count; i++) {
				Debug.Log(kitePieces[i].transform.position);
				kitePiecesOrigPos.Add(kitePieces[i].transform.position);
				kitePiecesOrigScale.Add(kitePieces[i].transform.localScale);
				kitePiecesOrigRot.Add(kitePieces[i].transform.localEulerAngles);
			}
		}
	}

	void Update () {
		// So that the Kite does not start fading out at the end of the level. (The Animator component takes over the object's transform.)
		if (kiteLevelChangeScript.endEventOn && !animEnabled && myLvl == mainPuzzEngScript.curntLvl) {
			animEnabled = true;
			anim.enabled = true;
		}
		// When to actually start the flying away animation.
		if (kiteLevelChangeScript.animStartB && !animTrigg && myLvl == mainPuzzEngScript.curntLvl) {
			animTrigg = true;
			anim.SetTrigger("StartAnim");
		}
		// Reset everything to be ready to fade back in and animate if the player goes back to the level.
		if (resetKite) {
			anim.enabled = false;
			animEnabled = false;
			animTrigg = false;
			this.transform.position = kiteOrigPos;
			this.transform.localScale = kiteOrigScale;
			this.transform.eulerAngles = kiteOrigRot;
			kiteSprite.color = new Color(kiteSprite.color.r, kiteSprite.color.g, kiteSprite.color.b, 0);
			if (kitePieces.Count > 0) {
				SetChildrenValues();
			}
			resetKite = false;
		}
		// Wait a frame to make sure the Animator component switches to the "Empty" animation to avoid a flicker when re-enabling
		// the object due to it finishing the last frames of its animation before going back to its original position 
		// at the end of the animation. The animation event calling the method to end the animation and reset its position cannot 
		// be realiably put on the last frame when using mobile devices, it will sometimes skip the last few frames of an animation.
		if (waitFrame) {
			resetKite = true;
			waitFrame = false;
		}
	}

	public void SetChildrenValues() {
		for (int i = 0; i < kitePieces.Count; i++) {
			kitePieces[i].transform.position = kitePiecesOrigPos[i];
			kitePieces[i].transform.localScale = kitePiecesOrigScale[i];
			kitePieces[i].transform.localEulerAngles = kitePiecesOrigRot[i];
			Color pieceColor = kitePieces[i].GetComponent<SpriteRenderer>().color;
			pieceColor = new Color(pieceColor.r, pieceColor.g, pieceColor.b, 0);
		}
	}

	#region In Animation Events
	public void LeafParticles() {
		leafsFalling.Play(true);
	}

	public void EndAnim() {
		anim.Play("Empty", 0);
		waitFrame = true;
	}
	#endregion
}
