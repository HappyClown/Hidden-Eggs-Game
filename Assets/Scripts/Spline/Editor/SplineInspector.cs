using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(Spline))]
public class SplineInspector : Editor {

	private void OnSceneGUI () {
		Spline spline = target as Spline;

		Handles.color = Color.white;
		Handles.DrawLine(spline.p0, spline.p1);
	}
}
