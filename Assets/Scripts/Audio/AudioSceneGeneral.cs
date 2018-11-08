using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioSceneGeneral : MonoBehaviour 
{
	[Header("Scene Music")]
    [FMODUnity.EventRef]
    public string sceneMusicEvent;
    public FMOD.Studio.EventInstance sceneMusic;

	[Header("Cue Title Card Transition")]
    [FMODUnity.EventRef]
    public string transEvent;
    public FMOD.Studio.EventInstance transMusic;


	[Header("Egg SFX")]
    [FMODUnity.EventRef]
    public string EggClickEvent;
    public FMOD.Studio.EventInstance EggClickSound;

    [FMODUnity.EventRef]
    public string goldEggClickEvent;
    public FMOD.Studio.EventInstance goldEggClickSound;

    [FMODUnity.EventRef]
    public string goldEggAnimationEvent;
    public FMOD.Studio.EventInstance goldEggAnimationSound;

    [FMODUnity.EventRef]
    public string goldEggShimyEvent;
    public FMOD.Studio.EventInstance goldEggShimySound;

	[Header("Panel SFX")]
    [FMODUnity.EventRef]
    public string panelOpenEvent;
    public FMOD.Studio.EventInstance panelOpenSound;

    [FMODUnity.EventRef]
    public string panelCloseEvent;
    public FMOD.Studio.EventInstance panelCloseSound;


	[Header("Puzzle Unlocked Animation")]
    [FMODUnity.EventRef]
    public string puzzleUnlockedEvent;
    public FMOD.Studio.EventInstance puzzleUnlockedSound;

	[Header("Bird Help")]
    [FMODUnity.EventRef]
    public string birdEvent;
    public FMOD.Studio.EventInstance birdSound;

	[Header("Buttons")]

    public Button BackMenuBtn;
    public Button BirdHelpBtn;
    public Button RiddleBtn;
    public Button HintBtn;

    [FMODUnity.EventRef]
    public string puzzleButtonEvent;
    public FMOD.Studio.EventInstance puzzleButtonSound;

    [FMODUnity.EventRef]
    public string buttonEvent;
    public FMOD.Studio.EventInstance buttonSound;

    // TESTS UNLOCKED SOUND
    public bool SceneIn;
    public float AlphaValue;

    public bool puzzleUnlocked;

    public GameObject puzImage;

    public string currentScene;

	void Start () 
	{
		BackMenuBtn.onClick.AddListener(TransitionMenu);
        BirdHelpBtn.onClick.AddListener(birdHelpSound);
        RiddleBtn.onClick.AddListener(birdHelpSound);
        HintBtn.onClick.AddListener(birdHelpSound);
        sceneMusic = FMODUnity.RuntimeManager.CreateInstance(sceneMusicEvent);
		transMusic = FMODUnity.RuntimeManager.CreateInstance(transEvent);
        goldEggShimySound = FMODUnity.RuntimeManager.CreateInstance(goldEggShimyEvent);
        puzzleUnlockedSound = FMODUnity.RuntimeManager.CreateInstance(puzzleUnlockedEvent);
        PlaySceneMusic();
	}
	
	void Update () 
	{
        /* 
        FOR PUZZLE UNLOCKED TESTS 
        
        currentScene = SceneManager.GetActiveScene().name;
        AlphaValue = SceneFade.getSceneFadeAlpha();
        if(AlphaValue ==0)
        {SceneIn =true;}
        else 
        if(AlphaValue ==1)
        {SceneIn =false;}

        */


        //TEST
		/*
        EggByScene()
        sceneMusic.setParameterValue("nbEggsByScene",nbEggsByScene);
        currentScene = SceneManager.GetActiveScene().name;
		*/

	}
    /////////////////////////////////////////
    //  Music Play / Stop
    ///////////////////////////////////////

    public void PlaySceneMusic()
    {
        sceneMusic.start();
    }

    public void StopSceneMusic()
    {
        sceneMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

	 public void PlayTransitionMusic()
    {
        transMusic.start();
    }

    /////////////////////////////////////////
    //  Transitions  &  Buttons


    public void TransitionPuzzle()
    {
        //puzzle button sound
        puzzleButtonSound = FMODUnity.RuntimeManager.CreateInstance(puzzleButtonEvent);
        puzzleButtonSound.start();

        //stop current music
        StopSceneMusic();
        Debug.Log("ASG - Current Music Stopped :");
		PlayTransitionMusic();
		

    }


    public FMOD.Studio.EventInstance  getSceneMusic()
    {
        return sceneMusic;
    }


        public void TransitionMenu()
    {
        //button sound
        buttonSFX();

        //stop current music
        StopSceneMusic();
        //Debug.Log("Current Music Stopped :");
		PlayTransitionMusic();

    }

    ///////////////////////////////////
    ///  Play SFX
    ///////////////////////////////////

    //Button generic
    public void buttonSFX()
    {
        //button sound
        buttonSound = FMODUnity.RuntimeManager.CreateInstance(buttonEvent);
        buttonSound.start();
    }

    //EGGS
    public void ClickEggsSound()
    {
        EggClickSound = FMODUnity.RuntimeManager.CreateInstance(EggClickEvent);
        EggClickSound.start();
    }

    public void goldEggSound()
    {
        goldEggClickSound = FMODUnity.RuntimeManager.CreateInstance(goldEggClickEvent);
        goldEggClickSound.start();
    }

    public void goldEggAnimSound()
    {
        goldEggAnimationSound = FMODUnity.RuntimeManager.CreateInstance(goldEggAnimationEvent);
        goldEggAnimationSound.start();
    }

    public void goldEggShimmerStartSound()
    {
        goldEggShimySound.start();
    }
    public void goldEggShimmerStopSound()
    {
        goldEggShimySound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    //Puzzle Animation
    public void puzzleAnimation()
    {
        puzzleUnlockedSound.start();
    }

    public void puzzleAnimationStart(GameObject puzzleImage)
    {
        //to start the animation only when the scene is visible IF already unlocked
        //if(SceneIn && puzzleUnlocked && currentScene == SceneManager.GetActiveScene().name) //not working

        puzImage = puzzleImage;
        puzzleUnlocked = true;
        FMODUnity.RuntimeManager.AttachInstanceToGameObject (puzzleUnlockedSound, puzzleImage.transform,puzzleImage.GetComponent<Rigidbody> ());
        puzzleUnlockedSound.start();
        Debug.Log("puzzle Unlocked Sound is playing");
    }


        public void puzzleAnimationStop()
    {
        puzzleUnlockedSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Debug.Log("puzzle Unlocked Sound is stopped");
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


    //Bird SFX

    public void birdHelpSound()
    {
        birdSound = FMODUnity.RuntimeManager.CreateInstance(birdEvent);
        birdSound.start();
    }

    //stop puzzle animation sound loop
    public void onDestroy()
    {
        puzzleUnlockedSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

///////////////////////////////////////////
	
// public void EggByScene()
// {
//     if(currentScene == GlobalVariables.globVarScript.beachName)
//         nbEggsByScene = GlobalVariables.beachTotalEggsFound;
//     if(currentScene == GlobalVariables.globVarScript.parkName)
//         nbEggsByScene = GlobalVariables.park.TotalEggsFound;
//     if(currentScene == GlobalVariables.globVarScript.marketName)
//         nbEggsByScene = GlobalVariables.marketTotalEggsFound;)

    //TEmporary. must be a better system than that

//}
}
