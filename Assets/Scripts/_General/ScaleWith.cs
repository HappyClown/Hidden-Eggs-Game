using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWith : MonoBehaviour {
	public GameObject objectToCopy;
	public bool matchScaleRatio, sameXYZ;
	private float otherIniXScale, myIniXScale;
	private float myNewXScale;
	private Vector3 otherIniScale, myIniScale;
	private Vector3 myNewScale;

	void Start () {
		GetScaleRefs();
	}
	
	void Update () {
		if (matchScaleRatio) {
			if (sameXYZ) {
				float otherCurXScale = objectToCopy.transform.localScale.x;
				myNewXScale = (myIniXScale * otherCurXScale) / otherIniXScale;
				this.transform.localScale = new Vector3(myNewXScale, myNewXScale, myNewXScale);
			}
			else {
				Vector3 otherCurScale = objectToCopy.transform.localScale;
				myNewScale = new Vector3(myIniScale.x * otherIniScale.x / otherCurScale.x, myIniScale.y * otherIniScale.y / otherCurScale.y, myIniScale.z * otherIniScale.z / otherCurScale.z);
			}
		}
	}

	public void GetScaleRefs() {
		if (sameXYZ) {
			otherIniXScale = objectToCopy.transform.localScale.x;
			myIniXScale = this.transform.localScale.x;
		}
		else {
			otherIniScale = objectToCopy.transform.localScale;
			myIniScale = this.transform.localScale;
		}
	}
}
