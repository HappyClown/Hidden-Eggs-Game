using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScrollingBG : MonoBehaviour {
	public float scrollSpeed, xLimit;
	private float scrollValue, scrollSpeedLerpValue;
	public float fullScrollSpeedDuration;
	public List<GameObject> bGs;
	public bool scroll;
	private int bgInFront = 0;
	public AnimationCurve scrollAnimCurve;

	void Start () {

	}
	
	void Update () {
		if (scroll) {
			if (scrollSpeedLerpValue < 1) {
				scrollSpeedLerpValue += Time.deltaTime / fullScrollSpeedDuration;
				scrollValue = Mathf.Lerp(0, scrollSpeed, scrollAnimCurve.Evaluate(scrollSpeedLerpValue));
			}
			for (int i = 0; i < bGs.Count; i++)
			{
				if (bGs[i].transform.position.x <= xLimit) {
					bgInFront++;
					if (bgInFront >= bGs.Count) {
						bgInFront = 0;
					}
					bGs[i].transform.position = new Vector3(bGs[bgInFront].transform.position.x - xLimit, bGs[i].transform.position.y, bGs[i].transform.position.z);
				}
				bGs[i].transform.Translate(transform.right * Time.deltaTime * scrollValue * -1);
			}
		}
	}
}
