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

    public string introEggFallingEvent = "event:/SFX/INTRO/Egg Falling";
    public FMOD.Studio.EventInstance introEggFallingSound;
    public string introGustHoverEvent = "event:/SFX/INTRO/Gust Hover Loop";
    public FMOD.Studio.EventInstance introGustHoverSound;

    public string introGustMishapEvent = "event:/SFX/INTRO/Gust Mishap Loop";
    public FMOD.Studio.EventInstance introGustMishapSound;

    public string introSingleEggSpinEvent = "event:/SFX/INTRO/SingleEggSpin";
    public FMOD.Studio.EventInstance introSingleEggSpinSound;

    public string introTimeDiveLoopEvent = "event:/SFX/INTRO/Time Dive Loop";
    public FMOD.Studio.EventInstance introTimeDiveLoopSound;

    public string introTimeDiveRescueEvent = "event:/SFX/INTRO/Time Dive Rescue";
    public FMOD.Studio.EventInstance introTimeDiveRescueSound;

    public string introTimeHoverLoopEvent = "event:/SFX/INTRO/Time Hover Loop";
    public FMOD.Studio.EventInstance introTimeHoverLoopSound;

    
    public string introTimeHoverLoopFarEvent = "event:/SFX/INTRO/Time Hover Loop Far";
    public FMOD.Studio.EventInstance introTimeHoverLoopFarSound;

    public string introTimeWooshEvent = "event:/SFX/INTRO/Time Woosh";
    public FMOD.Studio.EventInstance introTimeWooshSound;

    public string introTimeSpinLoopEvent = "event:/SFX/INTRO/Time Spin Loop";
    public FMOD.Studio.EventInstance introTimeSpinLoopSound;

    public string introTimeAppearMapEvent = "event:/SFX/INTRO/TimeAppearMap";
    public FMOD.Studio.EventInstance introTimeAppearMapSound;

    public string introWindLoopEvent = "event:/SFX/INTRO/Wind Loop";
    public FMOD.Studio.EventInstance introWindLoopSound;

    public string silverEggClickEvent = "event:/SFX/SFX_General/Egg_Click_Silver";
    public FMOD.Studio.EventInstance silverEggClickSound;
    public string silverEggTrailEvent = "event:/SFX/SFX_General/SilverEggs_SlideToPanel";
    public FMOD.Studio.EventInstance silverEggTrailSound;

    
	//audio
	//public bool audioIntro_on =true;

	void Start () 
	{
        introTimeHoverLoopSound = FMODUnity.RuntimeManager.CreateInstance(introTimeHoverLoopEvent);
        introGustHoverSound = FMODUnity.RuntimeManager.CreateInstance(introGustHoverEvent);
        introTimeHoverLoopFarSound = FMODUnity.RuntimeManager.CreateInstance(introTimeHoverLoopFarEvent);
        introTimeSpinLoopSound = FMODUnity.RuntimeManager.CreateInstance(introTimeSpinLoopEvent);
        introWindLoopSound = FMODUnity.RuntimeManager.CreateInstance(introWindLoopEvent);
        introSingleEggSpinSound = FMODUnity.RuntimeManager.CreateInstance(introSingleEggSpinEvent);
        introEggFallingSound = FMODUnity.RuntimeManager.CreateInstance(introEggFallingEvent);
        introTimeDiveLoopSound = FMODUnity.RuntimeManager.CreateInstance(introTimeDiveLoopEvent);
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

        public void introGustSFX()
    {
        introGustMishapSound = FMODUnity.RuntimeManager.CreateInstance(introGustMishapEvent);
        introGustMishapSound.start();
    }  
        public void introCollisionSFX()
    {
        introCollisionSound = FMODUnity.RuntimeManager.CreateInstance(introCollisionEvent);
        introCollisionSound.start();
    }

        public void introTimeDiveSFX()
    {
        introTimeDiveRescueSound = FMODUnity.RuntimeManager.CreateInstance(introTimeDiveRescueEvent);
        introTimeDiveRescueSound.start();
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
        public void introEggFallingSFX()
    {
        introEggFallingSound.start();
    }


     public void silverEggSnd()
    {
       silverEggClickSound = FMODUnity.RuntimeManager.CreateInstance(silverEggClickEvent);
       silverEggClickSound.start();
    }
	public void SilverEggTrailSFX () 
	{
		silverEggTrailSound = FMODUnity.RuntimeManager.CreateInstance(silverEggTrailEvent);
        silverEggTrailSound.start();
	}


    /// PLAY  LOOPS

        public void introTimeHoverLoopSFX()
    {
        introTimeHoverLoopSound.start();
    }   
        public void introTimeHoverLoopFarSFX()
    {
        introTimeHoverLoopFarSound.start();
    }  
    
        public void introGustHoverSFX()
    {
        introGustHoverSound.start();
    }
        public void introTimeSpinLoopSFX()
    {
        introTimeSpinLoopSound.start();
    } 

        public void introTimeDiveLoopSFX()
    {
        introTimeDiveLoopSound.start();
    }       
    
        public void introWindLoopSFX()
    {
        introWindLoopSound.start();
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
    public void STOP_introTimeHoverLoopFarSFX()
    {
        introTimeHoverLoopFarSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
        public void STOP_introTimeSpinLoopSFX()
    {
        introTimeSpinLoopSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    } 
    
        public void STOP_introWindLoopSFX()
    {
        introWindLoopSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
        public void STOP_introGustHoverSFX()
    {
        introGustHoverSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
         public void STOP_introSingleEggSpinLoopSFX()
    {
        introSingleEggSpinSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


        public void STOP_introEggFallingSFX()
    {
        introEggFallingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

        public void STOP_introTimeDiveLoopSFX()
    {
        introTimeDiveLoopSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }  
    

}