using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSceneParkPuzzle: AudioScenePuzzleGeneric
{

    [FMODUnity.EventRef]
    public string pickupTileEvent;
    public FMOD.Studio.EventInstance pickupTileSound;

    [FMODUnity.EventRef]
    public string dropTileEvent;
    public FMOD.Studio.EventInstance dropTileSound;

    [FMODUnity.EventRef]
    public string rotateTileEvent;
    public FMOD.Studio.EventInstance rotateTileSound;

    [FMODUnity.EventRef]
    public string connectSparkEvent = "event:/SFX/Puzzle_Park/Puzzle_Tile_ConnectFX";
    public FMOD.Studio.EventInstance connectSparkSound;

 
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
    //  SFX
    //////////////////


     public void connectSparkSnd()
    {
        connectSparkSound = FMODUnity.RuntimeManager.CreateInstance(connectSparkEvent);
        connectSparkSound.start();
    }
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



}
