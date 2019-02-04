using UnityEngine;

public class SplineWalker : MonoBehaviour {
	public enum SplineWalkerMode {
		Once,
		Loop,
		PingPong
	}
	public enum DirectionMode {
		LookForward,
		FlatZForward,
		NoRotation
	}
	public BezierSpline spline;
	public float duration;
	public DirectionMode dirMode;
	public SplineWalkerMode mode;
	public bool isPlaying;
	public bool IsPlaying {
		get { return isPlaying; } set { isPlaying = value; }
	}
	private bool goingForward = true;
	private float progress;
	public float Progress {
		get { return progress; }
	}
	
	private void Update () {
		if (isPlaying) {
			if (goingForward) {
				progress += Time.deltaTime / duration;
				if (progress > 1f) {
					if (mode == SplineWalkerMode.Once) {
						progress = 1f;
					}
					else if (mode == SplineWalkerMode.Loop) {
						progress -= 1f;
					}
					else {
						progress = 2f - progress;
						goingForward = false;
					}
				}
			}
			else {
				progress -= Time.deltaTime / duration;
				if (progress < 0f) {
					progress = -progress;
					goingForward = true;
				}
			}
			Vector3 position = spline.GetPoint(progress);
			transform.localPosition = position;
			if (dirMode == DirectionMode.LookForward) {
				transform.LookAt(position + spline.GetDirection(progress));
			}
			else if (dirMode == DirectionMode.FlatZForward) {
				transform.LookAt(position + spline.GetDirection(progress), Vector3.right);
				this.transform.eulerAngles = new Vector3(0, 0, this.transform.eulerAngles.x * -1);
			}
			else {
				//Nothing yo.
			}
		}
	}

}
