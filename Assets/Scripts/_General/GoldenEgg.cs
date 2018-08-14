using UnityEngine;
using UnityEngine.UI;

public class GoldenEgg : MonoBehaviour 
{
	private bool inGoldenEggSequence;

	[Header("Egg Animation")]
	public Animator anim; 
	public float eggAnimStart;
	private float eggAnimTimer;
	private bool eggAnimPlayed;

	[Header("Screen Cover")]
	public Image coverScreen;
	public float coverMaxAlpha;
	private float coverAlpha;

	[Header("Particles")]
	public ParticleSystem partGlow;
	public ParticleSystem partShafts;
	public ParticleSystem partSparkles;
	public ParticleSystem partPop;
	public ParticleSystem partTrail;
	private float partGlowA, partGlowInitA;
	private float partShaftsA;
	private float partTrailSize;



	void Update () 
	{
		// - FOR TESTING - //
		if (Input.GetKeyDown("space"))
		{
			if (!inGoldenEggSequence) 
			{ 
				inGoldenEggSequence = true; 
			}
			else 
			{ 
				inGoldenEggSequence = false;
				coverAlpha = 0;
				coverScreen.color = new Color (coverScreen.color.r, coverScreen.color.g, coverScreen.color.b, coverAlpha);
				anim.Play("Nothing", 0);
				eggAnimTimer = 0;
				eggAnimPlayed = false;
				partGlow.Stop(true);
				partShafts.Stop(true);
				partSparkles.Stop(true);
				partPop.Stop(true);
				partTrail.Stop(true);
				partGlow.Clear(true);
				partShafts.Clear(true);
				partSparkles.Clear(true);
				partPop.Clear(true);
				partTrail.Clear(true);
			}
		}

		// -- START GOLDEN EGG SEQUENCE -- //
		if (inGoldenEggSequence)
		{
			if (coverAlpha < coverMaxAlpha) 
			{ 
				DarkenScreen(); 
			}

			if (!eggAnimPlayed) 
			{ 
				eggAnimTimer += Time.deltaTime;

				if (eggAnimTimer > eggAnimStart)
				{
					StartAnim();
				}
			}

			if (partGlow.isPlaying)
			{
				var main = partGlow.main;
				if (partGlowInitA <= 0) { partGlowInitA = main.startColor.color.a; }

				if (partGlowA < partGlowInitA)
				{
					partGlowA += Time.deltaTime * partGlowInitA;
					main.startColor = new Color (main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, partGlowA);
				}
			} else { partGlowA = 0; partGlowInitA = 0; }

			if (partShafts.isPlaying)
			{
				if (partShaftsA < 1)
				{
					partShaftsA += Time.deltaTime;
					var main = partShafts.main;
					main.startColor = new Color (main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, partShaftsA);
				}
			} else { partShaftsA = 0; }

			if (partTrail.isPlaying)
			{
				partTrailSize += Time.deltaTime;
				var main = partTrail.main;
				main.startSizeMultiplier = partTrailSize;
			} else { partTrailSize = 0; }
		}
	}



	void DarkenScreen ()
	{
		coverAlpha += Time.deltaTime; 
		coverScreen.color = new Color (coverScreen.color.r, coverScreen.color.g, coverScreen.color.b, coverAlpha);
	}



	void StartAnim ()
	{
		anim.SetTrigger("StartAnim");
		eggAnimPlayed = true;
	}



	public void EndGoldenEggSequences ()
	{
		// play click anim
		// play click FX for the egg
		// egg flies to egg panel
		// let particle systems die out (stop them) or forcefully fade the particles out (https://docs.unity3d.com/ScriptReference/ParticleSystem.GetParticles.html)
		// take the banner / bird talking out
		// fade the screen cover away as the egg approaches the egg panel
	}



	// - CALLED DURING ANIMATION - // (Animation Events)
	void StartGlow ()
	{
		if (!partGlow.isPlaying) { partGlow.Play(true); }
	}



	void StartShafts()
	{
		if (!partShafts.isPlaying) { partShafts.Play(true); }
	}



	void StartSparkles()
	{
		if (!partSparkles.isPlaying) { partSparkles.Play(true); }
	}


	
	void StartPop()
	{
		if (!partPop.isPlaying) { partPop.Play(true); }
	}



	void StartTrail()
	{
		if (!partTrail.isPlaying) { partTrail.Play(true); }
	}
}
