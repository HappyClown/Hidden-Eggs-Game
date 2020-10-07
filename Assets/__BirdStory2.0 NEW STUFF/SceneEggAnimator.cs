using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEggAnimator : MonoBehaviour {

	public Animator animator;
	public GameObject animDummy;

	// There would need to be a pool of dummy objects.
	// This would be to avoid having an animator component of each scene egg.
	// Request a dummy, put it at the tapped egg's position, make the egg a child of this dummy, animate the dummy thus animating the egg, unchild the egg and start moving it, make dummy available for use again.

}
