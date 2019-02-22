using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSceneBeachPuzzle : AudioScenePuzzleGeneric 
{
    /* 
    [Header("Buttons")]
    public Button BackBeachBtn;
    public Button ToggleLevelPuz1;
	public Button ToggleLevelPuz2;
	public Button ToggleLevelPuz3;
    public Button ResetItemBtn;

    [Header("Music")]

    [FMODUnity.EventRef]
    public string sceneMusicEvent;
    public FMOD.Studio.EventInstance sceneMusic;

    [FMODUnity.EventRef]
    public string transEvent = "event:/SFX/SFX_General/TransitionsSound";
    public FMOD.Studio.EventInstance transMusic;

*/
	[Header("SFX")]

    [FMODUnity.EventRef]
    public string ambiantEvent;
    public FMOD.Studio.EventInstance ambiantSound;


/* 
    [FMODUnity.EventRef]
    public string silverEggClickEvent = "event:/SFX/SFX_General/Egg_Click_Silver";
    public FMOD.Studio.EventInstance silverEggClickSound;

    [FMODUnity.EventRef]
    public string silverEggTrailEvent = "event:/SFX/SFX_General/FX_trail";
    public FMOD.Studio.EventInstance silverEggTrailSound;

	

    [FMODUnity.EventRef]
    public string buttonEvent = "event:/SFX/SFX_General/Button";
    public FMOD.Studio.EventInstance buttonSound;
*/
	[Header("SFX")]
    [FMODUnity.EventRef]
    public string failEvent;
    public FMOD.Studio.EventInstance failSound;

    [FMODUnity.EventRef]
    public string bubblesEvent;
    public FMOD.Studio.EventInstance bubblesSound;

    [FMODUnity.EventRef]
    public string bubblePopEvent;
    public FMOD.Studio.EventInstance bubblePopSound;


    public static List<string> listOceanSounds;
    public static List<string> listMusicalSounds;

    public FMOD.Studio.EventInstance currentClamSound;
    public List<string> listSoundsUsed;

    public bool musicalSounds = true;
    void Start () 
	{
        BackBtn.onClick.AddListener(Transition);
        ResetItemBtn.onClick.AddListener(buttonSFX);
        ToggleLevelPuz1.onClick.AddListener(levelSFX);
		ToggleLevelPuz2.onClick.AddListener(levelSFX);
		ToggleLevelPuz3.onClick.AddListener(levelSFX);
		
		sceneMusic = FMODUnity.RuntimeManager.CreateInstance(sceneMusicEvent);
		transMusic = FMODUnity.RuntimeManager.CreateInstance(transEvent);
        ambiantSound = FMODUnity.RuntimeManager.CreateInstance(ambiantEvent);

		PlaySceneMusic();

        init_listMusicalSounds();
        init_listOceanSounds();
	}

	void Update () 
	{
		
	}

    //////////////////
    //  MUSIC
    //////////////////


///ADD A UNDERWATER TRANSITION PARAMETER FOR THE MUSIC ?
    public override void PlaySceneMusic()
    {
        sceneMusic.start();
        ambiantSound.start();
    }

    public override void StopSceneMusic()
    {
        sceneMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambiantSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
     public override void Transition()
    {
        //puzzle button sound
        buttonSFX();

        //stop current music
        StopSceneMusic();
        Debug.Log("Current Music Stopped :");
		transMusic.start();
		PlayTransitionMusic();

        //clear the list of Used Ocean Sound
        listSoundsUsed.Clear();

    }
	
    /* 
	public void PlayTransitionMusic()
    {
        transMusic.start();
    }

    //////////////////
    //  SFX
    //////////////////

    //GENERAL 
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

    public void buttonSFX()
    {
        //button sound
        buttonSound = FMODUnity.RuntimeManager.CreateInstance(buttonEvent);
        buttonSound.start();
    }

*/
    public void failSFX()
    {
        //button sound
        failSound = FMODUnity.RuntimeManager.CreateInstance(failEvent);
        failSound.start();
    }

    public void levelSFX()
    {
        //button sound
        buttonSFX();
        //reset the list of Used Ocean Sounds
        listSoundsUsed.Clear();
    }

    public void BubblesSFX()
    {
        bubblesSound = FMODUnity.RuntimeManager.CreateInstance(bubblesEvent);
        bubblesSound.start();
    }
    public void BubblePopSFX()
    {
        bubblePopSound = FMODUnity.RuntimeManager.CreateInstance(bubblePopEvent);
        bubblePopSound.start();
    }

/////////////////// TESTS  ///////////////////////////

public void init_listOceanSounds()
{
        listOceanSounds = new List<string>();
        listSoundsUsed = new List<string>();
        listOceanSounds.Add("event:/SFX/Puzzle_Beach/SoundsPuzzle/Boat");
        listOceanSounds.Add("event:/SFX/Puzzle_Beach/SoundsPuzzle/Dolphin");
        listOceanSounds.Add("event:/SFX/Puzzle_Beach/SoundsPuzzle/Frog");
        listOceanSounds.Add("event:/SFX/Puzzle_Beach/SoundsPuzzle/SeaGull");
        listOceanSounds.Add("event:/SFX/Puzzle_Beach/SoundsPuzzle/Seal");
        listOceanSounds.Add("event:/SFX/Puzzle_Beach/SoundsPuzzle/Walrus");
        listOceanSounds.Add("event:/SFX/Puzzle_Beach/SoundsPuzzle/Whale");
        listOceanSounds.Add("event:/SFX/Puzzle_Beach/SoundsPuzzle/Wave");

        /* 
        foreach (string str  in listOceanSounds)
        {
            Debug.Log("list of Ocean Sounds contains"+str);
        }
        */

    }

public void init_listMusicalSounds()
{
        listMusicalSounds = new List<string>();
        listSoundsUsed = new List<string>();
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/F");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/G");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/A");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/B");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/C");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/D");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/E");
        //listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/F2");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/G2");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/A2");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/B2");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/C2");
        listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/D2");
        //listMusicalSounds.Add("event:/SFX/Notes_SFX_InC/E2");
    }



public string chooseRandomSound()
{
    int randPosition;
    string oceanSoundEvent;
    if(musicalSounds){
        randPosition = Random.Range(0,listMusicalSounds.Count-1);
        oceanSoundEvent = (string)listMusicalSounds[randPosition];

        while(listSoundsUsed.Contains(oceanSoundEvent)){
            randPosition = Random.Range(0,listMusicalSounds.Count-1);
            oceanSoundEvent = (string)listMusicalSounds[randPosition];
        }
        listSoundsUsed.Add(oceanSoundEvent);
    }
    else{
        randPosition = Random.Range(0,listOceanSounds.Count-1);
        oceanSoundEvent = (string)listOceanSounds[randPosition];

        while(listSoundsUsed.Contains(oceanSoundEvent)){
            randPosition = Random.Range(0,listOceanSounds.Count-1);
            oceanSoundEvent = (string)listOceanSounds[randPosition];
        }
        listSoundsUsed.Add(oceanSoundEvent);
    }

    return oceanSoundEvent;
}

public void playOceanSound(string oceanSoundEvent)
{
    //play the random chosen ocean sound
    currentClamSound = FMODUnity.RuntimeManager.CreateInstance(oceanSoundEvent);
    currentClamSound.start();
}

public void newLevel()
{
    listSoundsUsed.Clear();
}

}