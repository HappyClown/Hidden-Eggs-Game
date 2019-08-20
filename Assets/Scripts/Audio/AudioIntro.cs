using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioIntro : MonoBehaviour 
{


    public string introCollisionEvent = "event:/SFX/INTRO/Collision";
    public FMOD.Studio.EventInstance introCollisionSound;
    public string introEggDropBasketEvent = "event:/SFX/INTRO/Eggs drops from basket";
    public FMOD.Studio.EventInstance introEggDropBasketSound;

    public string introEggPopTransformEvent = "event:/SFX/INTRO/Eggs pop transform";
    public FMOD.Studio.EventInstance introEggPopTransformSound;

    public string introGustHoverLoopEvent = "event:/SFX/INTRO/Gust Hover Loop";
    public FMOD.Studio.EventInstance introGustHoverLoopSound;

    public string introGustMishapLoopEvent = "event:/SFX/INTRO/Gust Mishap Loop";
    public FMOD.Studio.EventInstance introGustMishapLoopSound;

    public string introSingleEggSpinEvent = "event:/SFX/INTRO/SingleEggSpin";
    public FMOD.Studio.EventInstance introSingleEggSpinSound;

    public string introTimeDiveEvent = "event:/SFX/INTRO/Time Dive";
    public FMOD.Studio.EventInstance introTimeDiveSound;

    public string introTimeHoverLoopEvent = "event:/SFX/INTRO/Time Hover Loop";
    public FMOD.Studio.EventInstance introTimeHoverLoopSound;

    public string introTimeWooshEvent = "event:/SFX/INTRO/Time Woosh";
    public FMOD.Studio.EventInstance introTimeWooshSound;

    public string introTimeSpinLoopEvent = "event:/SFX/INTRO/Time Spin Loop";
    public FMOD.Studio.EventInstance introTimeSpinLoopSound;

    public string introTimeAppearMapEvent = "event:/SFX/INTRO/TimeAppearMap";
    public FMOD.Studio.EventInstance introTimeAppearMapSound;


	void Start () 
	{
        introTimeHoverLoopSound = FMODUnity.RuntimeManager.CreateInstance(introTimeHoverLoopEvent);
        introTimeSpinLoopSound = FMODUnity.RuntimeManager.CreateInstance(introTimeSpinLoopEvent);
        introGustHoverLoopSound = FMODUnity.RuntimeManager.CreateInstance(introGustHoverLoopEvent);
        introGustMishapLoopSound = FMODUnity.RuntimeManager.CreateInstance(introGustMishapLoopEvent);
        introSingleEggSpinSound = FMODUnity.RuntimeManager.CreateInstance(introSingleEggSpinEvent);
	}
	
	void Update () 
	{

	}

    ///////////////////////////////////
    ///  Play SFX
    ///////////////////////////////////

        //TIME + GUST related
         public void introTimeWooshSFX()
    {
        introTimeWooshSound = FMODUnity.RuntimeManager.CreateInstance(introTimeWooshEvent);
        introTimeWooshSound.start();
    }      
        public void introCollisionSFX()
    {
        introCollisionSound = FMODUnity.RuntimeManager.CreateInstance(introCollisionEvent);
        introCollisionSound.start();
    }
        public void introTimeDiveSFX()
    {
        introTimeDiveSound = FMODUnity.RuntimeManager.CreateInstance(introTimeDiveEvent);
        introTimeDiveSound.start();
    }        
        public void introTimeAppearMapSFX()
    {
        introTimeAppearMapSound = FMODUnity.RuntimeManager.CreateInstance(introTimeAppearMapEvent);
        introTimeAppearMapSound.start();
    }
        //EGGS
        public void introEggDropBasketSFX()
    {
        introEggDropBasketSound = FMODUnity.RuntimeManager.CreateInstance(introEggDropBasketEvent);
        introEggDropBasketSound.start();
    }
        public void introEggPopTransformSFX()
    {
        introEggPopTransformSound = FMODUnity.RuntimeManager.CreateInstance(introEggPopTransformEvent);
        introEggPopTransformSound.start();
    }



    /// PLAY  LOOPS

        public void introTimeHoverLoopSFX()
    {
        introTimeHoverLoopSound.start();
    }   
    
        public void introTimeSpinLoopSFX()
    {
        introTimeSpinLoopSound.start();
    } 
    
        public void introGustHoverLoopSFX()
    {
        introGustHoverLoopSound.start();
    }
        public void introGustMishapLoopSFX()
    {
        introGustMishapLoopSound.start();
    }   
         public void introSingleEggSpinLoopSFX()
    {
        introSingleEggSpinSound.start();
    }
   
    /// STOP LOOPS 

    public void STOP_introTimeHoverLoopSFX()
    {
        introTimeHoverLoopSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }   

        public void STOP_introTimeSpinLoopSFX()
    {
        introTimeSpinLoopSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    } 
    
        public void STOP_introGustHoverLoopSFX()
    {
        introGustHoverLoopSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

        public void STOP_introGusMishapLoopSFX()
    {
        introGustMishapLoopSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
         public void STOP_introSingleEggSpinLoopSFX()
    {
        introSingleEggSpinSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    //////// ADD: WIND LOOP FOR AFTER 
    ///////// ADD: EGG CLICKING SOUND
    /////////ADD: EGG FALLING SOUND
    /////////ADD : FX EGG VILLAGE
    ///////////TRANSITION FROM INTRO TO HUB MUSIC?
    
}
