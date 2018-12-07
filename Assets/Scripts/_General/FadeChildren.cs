using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeChildren : MonoBehaviour {
	[Header ("What am I?")]
	public Image myImg;
	public SpriteRenderer mySprt;
	public TextMeshProUGUI myTMP;
	public bool haveImg, haveSprt, haveTMP;
	private Color myColor;
	[Header ("Children")]
	public bool fadeAllChildren; // Could use recursion to get all the children's childs ++
	public List<GameObject> children;
	public List<Image> childImages;
	public List<SpriteRenderer> childSprites;
	public List <TextMeshProUGUI> childTMP;
	[Header ("Other")]
	private float curAlpha, prevAlpha;

	void Start () {
		//children = this.gameObject.GetComponentsInChildren<Transform>();
		if (fadeAllChildren) {

			GetChildren(this.transform);
		}
	}
	
	void Update () {
		SetColor();
		curAlpha = myColor.a;
		if (curAlpha != prevAlpha) {
			ChangeChildrenAlpha(myColor.a);
		}
		prevAlpha = myColor.a;
	}

	void GetChildren (Transform parent)
	{
		foreach (Transform child in parent)
		{
			children.Add(child.gameObject);
		}

		foreach (GameObject child in children)
		{
			if(child.GetComponent<Image>()) {
				childImages.Add(child.GetComponent<Image>());
			}
			else if(child.GetComponent<SpriteRenderer>()) {
				childSprites.Add(child.GetComponent<SpriteRenderer>());
			}
			else if(child.GetComponent<TextMeshProUGUI>()) {
				childTMP.Add(child.GetComponent<TextMeshProUGUI>());
			}
		}
	}

	void ChangeChildrenAlpha (float newAlpha)
	{
		if (childImages.Count > 0) {
			foreach (Image img in childImages)
			{
				img.color = new Color(img.color.r, img.color.g, img.color.b, newAlpha);
			}
		}
		if (childSprites.Count > 0) {
			foreach (SpriteRenderer sprt in childSprites)
			{
				sprt.color = new Color(sprt.color.r, sprt.color.g, sprt.color.b, newAlpha);
			}
		}
		if (childTMP.Count > 0) {
			foreach (TextMeshProUGUI tmp in childTMP)
			{
				tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, newAlpha);
			}
		}
	}

	Color SetColor ()
	{
		if (haveImg) {
			myColor = myImg.color;
		}
		else if (haveSprt) {
			myColor = mySprt.color;
		}
		else if (haveTMP) {
			myColor = myTMP.color;
		}
		return myColor;
	}
}
