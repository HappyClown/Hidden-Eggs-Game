﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSceneMarketPuzzle : MonoBehaviour 
{
 	[Header("Scene Music")]
    [FMODUnity.EventRef]
    public string sceneMusicEvent;
    public FMOD.Studio.EventInstance sceneMusic;
	
	[Header("Cue Title Card Transition")]
    [FMODUnity.EventRef]
    public string transEvent;
    public FMOD.Studio.EventInstance transMusic;

    [Header(" Silver Egg SFX")]
    [FMODUnity.EventRef]
    public string silverEggClickEvent;
    public FMOD.Studio.EventInstance silverEggClickSound;

	[Header(" Fruits SFX")]
    [FMODUnity.EventRef]
    public string pickupFruitEvent;
    public FMOD.Studio.EventInstance pickupFruitSound;

    [FMODUnity.EventRef]
    public string dropFruitScaleEvent;
    public FMOD.Studio.EventInstance dropFruitScaleSound;

	[FMODUnity.EventRef]
    public string dropFruitCrateEvent;
    public FMOD.Studio.EventInstance dropFruitCrateSound;

    [Header("Panel SFX")]
    [FMODUnity.EventRef]
    public string panelOpenEvent;
    public FMOD.Studio.EventInstance panelOpenSound;

    [FMODUnity.EventRef]
    public string panelCloseEvent;
    public FMOD.Studio.EventInstance panelCloseSound;

    [Header("Animations")]
	[FMODUnity.EventRef]
    public string arrowWiggleEvent;
    public FMOD.Studio.EventInstance arrowWiggleSound;

	[FMODUnity.EventRef]
    public string scaleWiggleEvent;
    public FMOD.Studio.EventInstance scaleWiggleSound;

	[FMODUnity.EventRef]
    public string crateSlideDownEvent;
    public FMOD.Studio.EventInstance crateSlideDownSound;

    [FMODUnity.EventRef]
    public string crateSlideRightEvent;
    public FMOD.Studio.EventInstance crateSlideRightSound;

  
	[Header("Buttons")]

    public Button BackMarketBtn;
    public Button ResetItemBtn;
    public Button ToggleLevelPuz1;
	public Button ToggleLevelPuz2;
	public Button ToggleLevelPuz3;

    
    [FMODUnity.EventRef]
    public string buttonEvent;
    public FMOD.Studio.EventInstance buttonSound;
    
    void Start () 
	{
        BackMarketBtn.onClick.AddListener(TransitionMarket);
        ResetItemBtn.onClick.AddListener(buttonSFX);
        ToggleLevelPuz1.onClick.AddListener(buttonSFX);
		ToggleLevelPuz2.onClick.AddListener(buttonSFX);
		ToggleLevelPuz3.onClick.AddListener(buttonSFX);
		
		sceneMusic = FMODUnity.RuntimeManager.CreateInstance(sceneMusicEvent);
		transMusic = FMODUnity.RuntimeManager.CreateInstance(transEvent);
		PlaySceneMusic();
	}

	void Update () 
	{
		
	}

    //////////////////
    //  MUSIC
    //////////////////

    public void PlaySceneMusic()
    {
        sceneMusic.start();
    }

    public void StopSceneMusic()
    {
        sceneMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
     public void TransitionMarket()
    {
        //puzzle button sound
        buttonSFX();

        //stop current music
        StopSceneMusic();
        Debug.Log("Current Music Stopped :");
		
		PlayTransitionMusic();

    }
	
	public void PlayTransitionMusic()
    {
        transMusic.start();
    }
    //////////////////
    //  SFX
    //////////////////

    //Pick up and Drop Fruits & Veggies
    public void pickupFruit()
    {
       pickupFruitSound = FMODUnity.RuntimeManager.CreateInstance(pickupFruitEvent);
       pickupFruitSound.start();
    }

    public void dropFruitScale()
    {
        dropFruitScaleSound = FMODUnity.RuntimeManager.CreateInstance(dropFruitScaleEvent);
        dropFruitScaleSound.start();
    }

	public void dropFruitCrate()
    {
        dropFruitCrateSound = FMODUnity.RuntimeManager.CreateInstance(dropFruitCrateEvent);
        dropFruitCrateSound.start();
    }

	//Animations
    public void arrowWiggle()
    {
        arrowWiggleSound = FMODUnity.RuntimeManager.CreateInstance(arrowWiggleEvent);
        arrowWiggleSound.start();
    }

    public void scaleWiggle()
    {
    	scaleWiggleSound = FMODUnity.RuntimeManager.CreateInstance(scaleWiggleEvent);
        scaleWiggleSound.start();
    }

    public void crateSlideRight()
    {
        crateSlideRightSound = FMODUnity.RuntimeManager.CreateInstance(crateSlideRightEvent);
        crateSlideRightSound.start();
    }
    public void crateSlideDown()
    {
        crateSlideDownSound = FMODUnity.RuntimeManager.CreateInstance(crateSlideDownEvent);
        crateSlideDownSound.start();
    }

    //GENERAL 
    public void silverEggSnd()
    {
       silverEggClickSound = FMODUnity.RuntimeManager.CreateInstance(silverEggClickEvent);
       silverEggClickSound.start();
    }

    public void buttonSFX()
    {
        //button sound
        buttonSound = FMODUnity.RuntimeManager.CreateInstance(buttonEvent);
        buttonSound.start();
    }

    //PAnel SFX
    public void openPanel()
    {
        panelOpenSound = FMODUnity.RuntimeManager.CreateInstance(panelOpenEvent);
        panelOpenSound.start();
    }

    public void closePanel()
    {
        panelCloseSound = FMODUnity.RuntimeManager.CreateInstance(panelCloseEvent);
        panelCloseSound.start();
    }
}