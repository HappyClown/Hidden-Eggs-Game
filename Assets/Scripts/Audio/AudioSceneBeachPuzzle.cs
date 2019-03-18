using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSceneBeachPuzzle : AudioScenePuzzleGeneric 
{

	[Header("SFX")]

    [FMODUnity.EventRef]
    public string ambiantEvent;
    public FMOD.Studio.EventInstance ambiantSound;

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

    //////////// tests pour melodie /////////
	[Header("TESTS MELODIE ")]

    public List<string> listNotesTapped;
    public FMOD.Studio.EventInstance notesTappedMelody;
    public float timer =0;
    public bool timerReached = false;
    public bool playingMelody = false;
    public int indexNotePlaying =0;
    public double randomBeat = 0;
    public AnimationCurve animCurve;
    public List<float> playingPoints;
    private float interval, curntInterval;
    public float playDuration = 1f;

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
        if (Input.GetKeyDown("space")) { // Deleeette MmeeeEEeeEEe!!!...
            setPlayingMelody(true);
        }
        // if(playingMelody){
        //     if(!timerReached){
        //         timer += Time.deltaTime;
        //         randomBeat = Random.Range(0.1f,0.2f);

        //     }
        //     if(!timerReached && timer>randomBeat)
        //     {
        //         Debug.Log("Note Play : "+indexNotePlaying);
        //         playMusicList((string)listNotesTapped[indexNotePlaying]);
        //         if(indexNotePlaying == listNotesTapped.Count-1){
        //             timerReached = true;
        //             indexNotePlaying = 0;
        //             playingMelody = false;
        //         }
        //         else
        //             indexNotePlaying++;
        //             timer = 0;
        //     }
        // }
        if (playingMelody) {
            timer += Time.deltaTime / playDuration;
            if (timer >= playingPoints[indexNotePlaying]) {
                playMusicList((string)listNotesTapped[indexNotePlaying]);
                indexNotePlaying++;
                if (indexNotePlaying >= listNotesTapped.Count - 1) {
                    playingMelody = false;
                    timer = 0;
                    playingPoints.Clear();
                    listNotesTapped.Clear();
                }
            }
        }
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
        listNotesTapped.Clear();
        listNotesTapped = new List<string>();

    }
	

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
    //reset the list of Used Ocean Sounds
    listSoundsUsed.Clear();
	//TEST MELODY 
    //setPlayingMelody(true);
}

///////tests pour melodie/////
public void addToMusicList(string clamSoundUsed)
{   
    //listNotesTapped.Add(clamSoundUsed);

    if(!listNotesTapped.Contains(clamSoundUsed))
        listNotesTapped.Add(clamSoundUsed);
    
}

public void playMusicList(string s)
{
        notesTappedMelody = FMODUnity.RuntimeManager.CreateInstance(s);
        notesTappedMelody.start();
        
}

public void setPlayingMelody(bool isPlaying){
    playingMelody = isPlaying;
    interval = 1f / (listNotesTapped.Count - 1);
    for(int i = 0; i < listNotesTapped.Count; i++) 
    {
        playingPoints.Add(animCurve.Evaluate(interval * i));
    }

    
}
}