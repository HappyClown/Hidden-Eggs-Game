using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGlow : MonoBehaviour {
	public float duration, minAlpha, maxAlpha, startAlpha;
	public AnimationCurve animCurve;
	public SpriteRenderer sprite;
	private bool glowOn, glowOff;
	private float timer, newAlpha;
	public bool alternateFade;

	void Update () {
		if (glowOn) {
			timer += Time.deltaTime / duration;
			newAlpha = Mathf.Lerp(startAlpha, maxAlpha, animCurve.Evaluate(timer));
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, newAlpha);
			if (timer >= 1) {
				glowOn = false;
				timer = 0f;
				startAlpha = maxAlpha;
				if (alternateFade) {
					glowOff = true;
				}
			}
		}
		if (glowOff) {
			timer += Time.deltaTime / duration;
			newAlpha = Mathf.Lerp(startAlpha, minAlpha, animCurve.Evaluate(timer));
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, newAlpha);
			if (timer >= 1) {
				glowOff = false;
				timer = 0f;
				startAlpha = minAlpha;
			}
		}

	}

	public void StartGlow() {
		glowOn = true;
		glowOff = false;
		timer = 0f;
		startAlpha = sprite.color.a;
	}

	public void StopGlow() {
		glowOn = false;
		glowOff = true;
		timer = 0f;
		startAlpha = sprite.color.a;
	}

	public void ResetGlow() {
		glowOn = false;
		glowOff = false;
		timer = 0f;
		newAlpha = 0f;
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
	}
}