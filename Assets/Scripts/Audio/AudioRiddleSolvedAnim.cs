using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioRiddleSolvedAnim : MonoBehaviour 
{

    [FMODUnity.EventRef]
    public string zoomEggEvent = "event:/SFX/ANIMS/Riddle Solved/Zoom egg";
    public FMOD.Studio.EventInstance zoomEggSound;

    [FMODUnity.EventRef]
    public string eggImploseFeathersEvent = "event:/SFX/ANIMS/Riddle Solved/EggImposeFeathers";
    public FMOD.Studio.EventInstance eggImploseFeathersSound;

    [FMODUnity.EventRef]
    public string zoomEggMidEvent = "event:/SFX/ANIMS/Riddle Solved/ZoomEggFromMid";
    public FMOD.Studio.EventInstance zoomEggMidSound;
    [FMODUnity.EventRef]    public string riddleSolvedTextEvent = "event:/SFX/ANIMS/Riddle Solved/textPopSfx";
    public FMOD.Studio.EventInstance riddleSolvedTextSound;
    [FMODUnity.EventRef]
    public string spiningStarsEvent = "event:/SFX/ANIMS/Riddle Solved/SpinningStars";
    public FMOD.Studio.EventInstance spiningStarsSound;

    [FMODUnity.EventRef]
    public string fireworkTrailEvent = "event:/SFX/ANIMS/Riddle Solved/Firework Trail";
    public FMOD.Studio.EventInstance fireworkTrailSound;

    [FMODUnity.EventRef]
    public string fireworkExplosionEvent = "event:/SFX/ANIMS/Riddle Solved/Firework Explosion";
    public FMOD.Studio.EventInstance fireworkExplosionSound;

    
    [FMODUnity.EventRef]
    public string fireworkTrailBurstEvent = "event:/SFX/ANIMS/Riddle Solved/FireworkTrailBurst";
    public FMOD.Studio.EventInstance fireworkTrailBurstSound;

    [FMODUnity.EventRef]
    public string goldenEggIdleEvent = "event:/SFX/ANIMS/Riddle Solved/IdleSquish";
    public FMOD.Studio.EventInstance goldenEggIdleSound;

        [FMODUnity.EventRef]
    public string goldenEggIdleShimmyEvent = "event:/SFX/ANIMS/Riddle Solved/IdleShimmy";
    public FMOD.Studio.EventInstance goldenEggIdleShimmySound;


	void Start () 
	{


	}
	
	void Update () 
	{

	}

    ///////////////////////////////////
    ///  Play SFX
    ///////////////////////////////////



    public void zoomEggSnd(){
        zoomEggSound = FMODUnity.RuntimeManager.CreateInstance(zoomEggEvent);
        zoomEggSound.start();

    }

        public void eggImploseFeathersSnd(){
        eggImploseFeathersSound = FMODUnity.RuntimeManager.CreateInstance(eggImploseFeathersEvent);
        eggImploseFeathersSound.start();

    }

        public void zoomEggMidSnd(){
        zoomEggMidSound = FMODUnity.RuntimeManager.CreateInstance(zoomEggMidEvent);
        zoomEggMidSound.start();

    }
    public void riddleSolvedTextSnd(){
        riddleSolvedTextSound = FMODUnity.RuntimeManager.CreateInstance(riddleSolvedTextEvent);
        riddleSolvedTextSound.start();

    }

    public void spiningStarsSnd(){
        spiningStarsSound = FMODUnity.RuntimeManager.CreateInstance(spiningStarsEvent);
        spiningStarsSound.start();    
    }
        public void fireworkTrailSnd(){
        fireworkTrailSound = FMODUnity.RuntimeManager.CreateInstance(fireworkTrailEvent);
        fireworkTrailSound.start();    
    }        
        public void fireworkExplosionSnd(){
        fireworkExplosionSound = FMODUnity.RuntimeManager.CreateInstance(fireworkExplosionEvent);
        fireworkExplosionSound.start();    
    }
    
        public void fireworkTrailBurstSnd(){
        fireworkTrailBurstSound = FMODUnity.RuntimeManager.CreateInstance(fireworkTrailBurstEvent);
        fireworkTrailBurstSound.start();    
    }        


    
        public void goldenEggIdleSnd(){
        goldenEggIdleSound = FMODUnity.RuntimeManager.CreateInstance(goldenEggIdleEvent);
        goldenEggIdleSound.start();    
    }

        public void goldenEggIdleShimmySnd(){
        goldenEggIdleShimmySound = FMODUnity.RuntimeManager.CreateInstance(goldenEggIdleShimmyEvent);
        goldenEggIdleShimmySound.start();    
    }





}
