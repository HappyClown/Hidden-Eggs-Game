using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggMovement : MonoBehaviour
{
	public float moveDuration, startMove, becomeWhite, becomePlain, startShake, glowToMax;
	public float bagExplodeDelay;
    public float turnOffEgg;
    public bool shake;
	public AnimationCurve animCurve;
	public Transform endTrans;
	public RotateFX rotateFXScript;
    public AudioLevelCompleteAnim audioLvlComp;
	public LevelCompleteEggSpawner lvlCompEggSpawnScript;
	public LevelCompEggCounter levelCompEggCounterScript;
	public LevelCompleteEggBag levelCompleteEggbagScript;
	public IncreasePartSysSimulationSpeed FXSpeedScript;

    public IEnumerator SpinMoveEggs(LevelCompleteEggVariables eggVars, bool amIFirst = false, bool amILast = false) {
        Transform eggTransform = eggVars.transform.GetChild(0);
        Vector3 startPos = eggTransform.position;
        audioLvlComp.circleEggsSoloSnd();
        // Fade in the egg and the glow behind it.
        eggVars.myFadeScript.FadeIn();
        eggVars.myGlowFadeScript.FadeIn();
        // Activate and play the egg spawn FX.
        eggVars.spawnFX.gameObject.SetActive(true);
        eggVars.spawnFX.Play();
        // Get a random number between -1 and 1 and apply it to the spin direction.
		int spinDir = Random.Range(0, 2) * 2 - 1;
        eggVars.rotateXYZ.rotations.rotationSpeedZ *= spinDir;
        float timer = 0f;
        // Make the egg glow appear.
        while (timer < glowToMax) {
            timer += Time.deltaTime;
            yield return null;
        }
        eggVars.myGlowFadeScript.maxAlpha = 1f;
        eggVars.myGlowFadeScript.FadeIn(eggVars.myGlowFadeScript.sprite.color.a);
        audioLvlComp.circleEggsSoloGoldSnd();
        // Fade out the scene egg.
        while (timer < becomeWhite) {
            timer += Time.deltaTime;
            yield return null;
        }
        eggVars.myFadeScript.FadeOut();
        audioLvlComp.circleEggsGlowSnd();
        // Plain egg fades in.
        while (timer < becomePlain) {
            timer += Time.deltaTime;
            yield return null;
        }
        eggVars.myGlowFadeScript.FadeOut(0.25f);
        eggVars.plainEggFadeScript.FadeIn();
        audioLvlComp.circleEggsSoloPlainSnd();
        // Make the egg shake?
        while (timer < startShake) {
            timer += Time.deltaTime;
            yield return null;
        }
        shake = true;
        // Move the egg to the bag.
        while (timer < startMove) {
            timer += Time.deltaTime;
            yield return null;
        }
        // If this is the first egg to move to the bag, start the FX that rotates around the eggs.
        if (amIFirst) {
            rotateFXScript.RotatePlayFX(lvlCompEggSpawnScript.allEggToBagDuration);
        }
		audioLvlComp.eggsMoveInBagSnd();
        if (!eggVars.trailFX.isPlaying) {
            eggVars.trailFX.gameObject.SetActive(true);
            eggVars.trailFX.Play(true);
        }
        float lerp = 0f;
        // Move the egg to the middle of the bag.
        while (lerp < 1f) {
            lerp += Time.deltaTime / moveDuration;
            eggTransform.position = Vector3.Lerp(startPos, endTrans.position, animCurve.Evaluate(lerp));
            yield return null;
        }
        // Egg has reached the bag.
		levelCompEggCounterScript.AddEgg();
        audioLvlComp.eggsCounterSnd();
        eggVars.plainEggFadeScript.FadeOut();
        eggVars.myGlowFadeScript.gameObject.SetActive(false);
        eggVars.arrivalFX.gameObject.SetActive(true);
        eggVars.arrivalFX.Play(true);
        eggVars.trailFX.Stop(true);
        eggVars.trailFX.gameObject.SetActive(false);
        levelCompleteEggbagScript.bagAnim.SetTrigger("Scale");
        if (amIFirst) {
            levelCompleteEggbagScript.StartCurrentBagGlow();
            FXSpeedScript.myPartSys.Play();
        }
        if (amILast) {
            FXSpeedScript.IncreaseSimulationSpeed();
            audioLvlComp.particulesInBagSnd();
            StartCoroutine(BagExplosionDelay());
        }
        // The last timer, the timer to end this egg forever.
        timer = 0f;
        while (timer < turnOffEgg) {
            timer += Time.deltaTime;
            yield return null;
        }
        eggVars.gameObject.SetActive(false);
    }

    IEnumerator BagExplosionDelay() {
        while (bagExplodeDelay > 0f) {
            bagExplodeDelay -= Time.deltaTime;
            yield return null;
        }
        levelCompleteEggbagScript.bagAnim.SetTrigger("Explode");
    }
}
