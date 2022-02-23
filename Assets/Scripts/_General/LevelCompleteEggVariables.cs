using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggVariables : MonoBehaviour
{
	public Animator eggAnim;
    public RotateXYZ rotateXYZ;
	public FadeInOutSprite myFadeScript, myGlowFadeScript, plainEggFadeScript;
	public SpriteRenderer mySprite, whiteOverlaySprite;
	public ParticleSystem trailFX, arrivalFX, spawnFX;

    public void GetReferences() {
		eggAnim = this.transform.GetComponent<Animator>();
		myFadeScript = this.transform.Find("LvlCompEgg").GetComponent<FadeInOutSprite>();
		rotateXYZ = this.transform.Find("LvlCompEgg").GetComponent<RotateXYZ>();
        mySprite = myFadeScript.gameObject.GetComponent<SpriteRenderer>();
		myGlowFadeScript = myFadeScript.transform.Find("SmallEggGlow").GetComponent<FadeInOutSprite>();
		whiteOverlaySprite = myFadeScript.transform.Find("WhiteOverlay").GetComponent<SpriteRenderer>();
		plainEggFadeScript = myFadeScript.transform.Find("PlainEgg").GetComponent<FadeInOutSprite>();
        trailFX = myFadeScript.transform.Find("EggBag Trail FX").GetComponent<ParticleSystem>();
        arrivalFX = myFadeScript.transform.Find("EggBag Burst FX").GetComponent<ParticleSystem>();
        spawnFX = myFadeScript.transform.Find("EggBag SpawnBurst FX").GetComponent<ParticleSystem>();
		SpriteMask sMask = this.GetComponent<SpriteMask>();
		sMask.sprite = this.GetComponent<SpriteRenderer>().sprite;
    }
}
