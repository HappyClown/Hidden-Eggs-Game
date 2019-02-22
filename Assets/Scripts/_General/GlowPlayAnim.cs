using UnityEngine;

public class GlowPlayAnim : MonoBehaviour {
	public SpriteRenderer spriteRend;
	[Header("Start Variables")]
	public bool setStartAlphaZero = true;
	public bool fadeInOnEnable = true;
	private float newAlpha;
	private bool fadingIn = false;
	private bool decreaseIntensity;
	private bool increasingIntensity;
	[Header("Glow variables")]
	[Tooltip("Duration of a fade in/out in seconds.")]
	public float fadeDuration;
	[Tooltip("Do not put lower then minAlpha.")]
	[Range(0.5f, 1)]
	public float maxAlpha;
	// I want to put [Range(0,maxAlpha)] but im not sure how yet, might have to look into PropertyDrawers
	// to make custom inspector components For now leave maxAlpha above min and min below max.
	[Tooltip("Do not put higher then maxAlpha.")]
	[Range(0, 0.5f)]
	public float minAlpha;

	void OnEnable () {
		if (setStartAlphaZero) { spriteRend.color = new Color (1,1,1, 0); newAlpha = 0f; }
		else { spriteRend.color = new Color (1,1,1, maxAlpha); newAlpha = maxAlpha; }
		if (fadeInOnEnable) { fadingIn = true; }
	}
	
	void Update () {
		// For the initial fade in from 0.
		if (fadingIn) {
			newAlpha += (Time.deltaTime / fadeDuration) * maxAlpha;
			if (newAlpha >= maxAlpha) {
				fadingIn = false;
			}
		}
		// After the initial fade in, alpha intensity varies up and down to simulate glow.
		if (!fadingIn) {
			if (newAlpha >= maxAlpha) {
				decreaseIntensity = true;
				increasingIntensity = false;
			}
			if (newAlpha <= minAlpha) {
				decreaseIntensity = false;
				increasingIntensity = true;
			}
			if (decreaseIntensity) {
				newAlpha -= (Time.deltaTime / fadeDuration) * (maxAlpha-minAlpha);
			}
			if (increasingIntensity) {
				newAlpha += (Time.deltaTime / fadeDuration) * (maxAlpha-minAlpha);
			}
		}

		// Adjust the sprite's alpha to newAlpha value.
		spriteRend.color = new Color(1f, 1f, 1f, newAlpha);
	}

	public void ResetGlow () {
		if (setStartAlphaZero) { spriteRend.color = new Color (1,1,1, 0); newAlpha = 0f;}
		else { spriteRend.color = new Color (1,1,1, maxAlpha); newAlpha = maxAlpha; }
		if (fadeInOnEnable) { fadingIn = true; }
	}
}