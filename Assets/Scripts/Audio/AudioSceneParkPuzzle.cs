using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSceneParkPuzzle: AudioScenePuzzleGeneric
{
    /* 
	[Header("Buttons")]
    public Button BackParkBtn;
    public Button ResetItemBtn;
    public Button ToggleLevelPuz1;
	public Button ToggleLevelPuz2;
	public Button ToggleLevelPuz3;
    [Header(" Music")]
    [FMODUnity.EventRef]
    public string sceneMusicEvent;
    public FMOD.Studio.EventInstance sceneMusic;
	
    [FMODUnity.EventRef]
    public string transEvent = "event:/SFX/SFX_General/TransitionsSound";
    public FMOD.Studio.EventInstance transMusic;

    [Header(" SFX")]
    [FMODUnity.EventRef]
    public string silverEggClickEvent = "event:/SFX/SFX_General/Egg_Click_Silver";
    public FMOD.Studio.EventInstance silverEggClickSound;

    [FMODUnity.EventRef]
    public string silverEggTrailEvent = "event:/SFX/SFX_General/FX_trail";
    public FMOD.Studio.EventInstance silverEggTrailSound;
*/
    [FMODUnity.EventRef]
    public string pickupTileEvent;
    public FMOD.Studio.EventInstance pickupTileSound;

    [FMODUnity.EventRef]
    public string dropTileEvent;
    public FMOD.Studio.EventInstance dropTileSound;

    [FMODUnity.EventRef]
    public string rotateTileEvent;
    public FMOD.Studio.EventInstance rotateTileSound;

    /* 
    [FMODUnity.EventRef]
    public string buttonEvent = "event:/SFX/SFX_General/Button";
    public FMOD.Studio.EventInstance buttonSound;
    */
    
    void Start () 
	{
        BackBtn.onClick.AddListener(Transition);

        ResetItemBtn.onClick.AddListener(buttonSFX);
        ToggleLevelPuz1.onClick.AddListener(buttonSFX);
		ToggleLevelPuz2.onClick.AddListener(buttonSFX);
		ToggleLevelPuz3.onClick.AddListener(buttonSFX);
		transMusic = FMODUnity.RuntimeManager.CreateInstance(transEvent);
		sceneMusic = FMODUnity.RuntimeManager.CreateInstance(sceneMusicEvent);
		PlaySceneMusic();
	}
	
	void Update () 
	{
		
	}

    //////////////////
    //  MUSIC
    //////////////////

/* 
    public void PlaySceneMusic()
    {
        sceneMusic.start();
    }

    public void StopSceneMusic()
    {
        sceneMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

       public void TransitionPark()
    {
        //puzzle button sound
        buttonSound = FMODUnity.RuntimeManager.CreateInstance(buttonEvent);
        buttonSound.start();

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

    
    public void silverEgg()
    {
       silverEggClickSound = FMODUnity.RuntimeManager.CreateInstance(silverEggClickEvent);
       silverEggClickSound.start();
    }

    public void SilverEggTrailSFX () 
	{
		silverEggTrailSound = FMODUnity.RuntimeManager.CreateInstance(silverEggTrailEvent);
        silverEggTrailSound.start();
	}
    */

    public void pickupTile()
    {
        pickupTileSound = FMODUnity.RuntimeManager.CreateInstance(pickupTileEvent);
        pickupTileSound.start();
    }
    public void dropTile()
    {
        dropTileSound = FMODUnity.RuntimeManager.CreateInstance(dropTileEvent);
        dropTileSound.start();
    }

    public void rotateTile()
    {
        rotateTileSound = FMODUnity.RuntimeManager.CreateInstance(rotateTileEvent);
        rotateTileSound.start();
    }

    /*
        public void buttonSFX()
    {
        //button sound
        buttonSound = FMODUnity.RuntimeManager.CreateInstance(buttonEvent);
        buttonSound.start();
    }

    */

}
