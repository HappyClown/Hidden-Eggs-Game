using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioSceneGeneral : MonoBehaviour 
{
	[Header("Buttons")]

    public Button BackMenuBtn;
    public Button lvlCompleteBtn;



	[Header("Scene Music")]
    [FMODUnity.EventRef]
    public string sceneMusicEvent;
    public FMOD.Studio.EventInstance sceneMusic;

    [Header("Cue Title Card Transition")]
    [FMODUnity.EventRef]
    public string transEvent = "event:/SFX/SFX_General/TransitionsSound";
    public FMOD.Studio.EventInstance transMusic;


	[Header("Egg SFX")]
    [FMODUnity.EventRef]
    public string EggClickEvent = "event:/SFX/SFX_General/Egg_Click";
    public FMOD.Studio.EventInstance EggClickSound;

    [FMODUnity.EventRef]
    public string goldEggClickEvent = "event:/SFX/SFX_General/Egg_Click_Gold";
    public FMOD.Studio.EventInstance goldEggClickSound;

    [FMODUnity.EventRef]
    public string goldEggAnimationEvent = "event:/SFX/SFX_General/GoldenEggAnimation";
    public FMOD.Studio.EventInstance goldEggAnimationSound;

    [FMODUnity.EventRef]
    public string goldEggShimyEvent = "event:/SFX/SFX_General/GoldenEgg_ShimyIdle";
    public FMOD.Studio.EventInstance goldEggShimySound;

	[Header("Silver Eggs Slide to Panel SFX")]
    [FMODUnity.EventRef]
    public string silverEggSlidePanelEvent = "event:/SFX/SFX_General/SilverEggs_SlideToPanel";
    public FMOD.Studio.EventInstance silverEggSlidePanelSound;


	[Header("Panel SFX")]
    [FMODUnity.EventRef]
    public string panelOpenEvent = "event:/SFX/SFX_General/MenuEggOpen";
    public FMOD.Studio.EventInstance panelOpenSound;

    [FMODUnity.EventRef]
    public string panelCloseEvent = "event:/SFX/SFX_General/MenuEggClose";
    public FMOD.Studio.EventInstance panelCloseSound;


    [Header("Puzzle Unlocked")]
    [FMODUnity.EventRef]
    public string puzzlePieceEvent = "event:/SFX/SFX_General/PuzzlePiece";
    public FMOD.Studio.EventInstance puzzlePieceSound;

    [FMODUnity.EventRef]
    public string puzzleUnlockedEvent;
    public FMOD.Studio.EventInstance puzzleUnlockedSound;

    [FMODUnity.EventRef]
    public string puzzleButtonEvent ;
    public FMOD.Studio.EventInstance puzzleButtonSound;

	[Header("Bird Unfrozen FX")]

    [FMODUnity.EventRef]
    public string unfrozenBirdEvent = "event:/SFX/SFX_General/UnfrozenBird";
    public FMOD.Studio.EventInstance unfrozenBirdSound;

    [FMODUnity.EventRef]
    public string frozenBirdShakeEvent = "event:/SFX/SFX_General/FrozenBirdShake";
    public FMOD.Studio.EventInstance frozenBirdShakeSound;


	[Header("Congratulations Lvl Complete")]
    [FMODUnity.EventRef]
    public string lvlCompleteEvent;
    public FMOD.Studio.EventInstance lvlCompleteSound;

    [FMODUnity.EventRef]
    public string buttonEvent = "event:/SFX/SFX_General/Button";
    public FMOD.Studio.EventInstance buttonSound;

	[Header("For tests and reference")]
    // TESTS UNLOCKED SOUND
    public bool SceneIn;
    public float AlphaValue;

    public bool puzzleUnlocked;

    public GameObject puzImage;

    public string currentScene;

    public GameObject egg;

    ////////

    public AudioHelperBird audioHelperBirdScript;

	void Start () 
	{
		BackMenuBtn.onClick.AddListener(TransitionMenu);
        lvlCompleteBtn.onClick.AddListener(TransitionMenu);
        sceneMusic = FMODUnity.RuntimeManager.CreateInstance(sceneMusicEvent);
		transMusic = FMODUnity.RuntimeManager.CreateInstance(transEvent);
        goldEggShimySound = FMODUnity.RuntimeManager.CreateInstance(goldEggShimyEvent);
        puzzleUnlockedSound = FMODUnity.RuntimeManager.CreateInstance(puzzleUnlockedEvent);

        audioHelperBirdScript = GameObject.Find ("Audio").GetComponent<AudioHelperBird>();

        PlaySceneMusic();
	}
	
	void Update () 
	{

        if(egg)
        {
            updateEggSound();
            updateSilverEggPanelSound();
        }

        //FOR PUZZLE UNLOCKED TESTS 

        currentScene = SceneManager.GetActiveScene().name;
        AlphaValue = SceneFade.getSceneFadeAlpha();
        if(AlphaValue ==0)
        {SceneIn =true;}
        else 
        if(AlphaValue ==1)
        {SceneIn =false;}

        if(puzzleUnlocked && currentScene == SceneManager.GetActiveScene().name)
        {
            puzzleUnlockedSound.setParameterValue("fade", AlphaValue);
        }


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
        
        puzzleAnimationStop();
		

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
                
        puzzleAnimationStop();

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
    public void ClickEggsSound(GameObject eggObject)
    {
        egg=eggObject;
        EggClickSound = FMODUnity.RuntimeManager.CreateInstance(EggClickEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject (EggClickSound, eggObject.transform,eggObject.GetComponent<Rigidbody> ());
        EggClickSound.start();
    }

    public void updateEggSound()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(EggClickSound, egg.transform,egg.GetComponent<Rigidbody> ());
    }

    public void silverEggsPanel(GameObject eggObject)
    {
        egg=eggObject;
        silverEggSlidePanelSound = FMODUnity.RuntimeManager.CreateInstance(silverEggSlidePanelEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject (silverEggSlidePanelSound, eggObject.transform,eggObject.GetComponent<Rigidbody> ());
        silverEggSlidePanelSound.start();
    }

    public void updateSilverEggPanelSound()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(silverEggSlidePanelSound, egg.transform,egg.GetComponent<Rigidbody> ());
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

    public void puzzlePieceAnimation()
    {
        puzzlePieceSound.start();
    }


    public void puzzleAnimation()
    {
        puzzleUnlockedSound.start();
    }


    public void puzzleAnimationStart(GameObject puzzleImage)
    {
        //to start the animation only when the scene is visible IF already unlocked
        //if(SceneIn && puzzleUnlocked && currentScene == SceneManager.GetActiveScene().name) //not working as intended

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
    public void unfrozenBirdSnd()
    {
        unfrozenBirdSound = FMODUnity.RuntimeManager.CreateInstance(unfrozenBirdEvent);
        unfrozenBirdSound.start();
    }

    public void frozenBirdShake()
    {
        frozenBirdShakeSound = FMODUnity.RuntimeManager.CreateInstance(frozenBirdShakeEvent);
        audioHelperBirdScript.birdHelpSoundFrozen();
        frozenBirdShakeSound.start();
    }

    //stop puzzle animation sound loop
    public void onDestroy()
    {
        puzzleUnlockedSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //public static void DetachInstanceFromGameObject(FMOD.Studio.EventInstance instance)
    }

/// Lvl complete
    public void lvlCompleteSnd()
    {
        lvlCompleteSound = FMODUnity.RuntimeManager.CreateInstance(lvlCompleteEvent);
        lvlCompleteSound.start();
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
