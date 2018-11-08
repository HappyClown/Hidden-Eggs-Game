using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioManagerHubMenu : MonoBehaviour

{
    ////////////////////////////
    //  MUSIC
    ///////////////////////////

    [Header("Menu Music")]
    [FMODUnity.EventRef]
    public string menuEvent;
    public FMOD.Studio.EventInstance menuMusic;

    
    [Header("Hub Music")]
    [FMODUnity.EventRef]
    public string MusicEvent;
    public FMOD.Studio.EventInstance hubMusic;
    public FMOD.Studio.ParameterInstance Summer;
    public FMOD.Studio.ParameterInstance Autumn;
    public FMOD.Studio.ParameterInstance Winter;
    public FMOD.Studio.ParameterInstance Spring;

    [Header("Pour tests")]
    public float SummerOn;
    public float AutumnOn;
    public float WinterOn;
    public float SpringOn;

    ////////////////////////////
    //  SFX & BUTTONS
    ///////////////////////////
    
    [Header("Play Button")]
    public Button BtnPlay;
    [FMODUnity.EventRef]
    public string SFXPlayButton;
    public FMOD.Studio.EventInstance PlaySnd;

    [Header("Reset Button")]
    public Button BtnReset;
    [FMODUnity.EventRef]
    public string SFXResetButton;
    public FMOD.Studio.EventInstance ResetSnd;

    [Header("Back To Menu Button")]
    public Button BackMenuBtn;
    [FMODUnity.EventRef]
    public string SFXBackButton;
    public FMOD.Studio.EventInstance BackBtnSnd;

    [Header("SFX Uncover Map")]
    [FMODUnity.EventRef]
    public string SFXMapUncover;
    public FMOD.Studio.EventInstance MapSnd;

    //TEST
    private FMOD.Studio.EventInstance currentMusic;


    //TEST
    public int nbEggsBySeason;

 
    void Start()
    {
        BtnPlay.onClick.AddListener(PlaySound);
        BtnReset.onClick.AddListener(ResetSound);
        BackMenuBtn.onClick.AddListener(BackBtnSound);

        menuMusic = FMODUnity.RuntimeManager.CreateInstance(menuEvent);
        hubMusic = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);

        //FOR NOW THE INITIAL VALUE OF SUMMER IS 1 , CHECK WITH THE UNLOCKED SEASONS / HUB.CS
        SummerOn = 1;
        hubMusic.setParameterValue("Summer", SummerOn);
        hubMusic.setParameterValue("Autumn", AutumnOn);
        hubMusic.setParameterValue("Winter", WinterOn);
        hubMusic.setParameterValue("Spring", SpringOn);

        //hubMusic.setParameterValue("nbEggsBySeason",nbEggsBySeason); //TEST

        currentMusic = menuMusic;
        if (!GlobalVariables.globVarScript.toHub) { PlayMenuMusic(); } else { PlayHubMusic(); }
    }
    void Update()
    {

        //EggBySeason();
        hubMusic.setParameterValue("Summer", SummerOn);
        hubMusic.setParameterValue("Autumn", AutumnOn);
        hubMusic.setParameterValue("Winter", WinterOn);
        hubMusic.setParameterValue("Spring", SpringOn);

        //hubMusic.setParameterValue("nbEggsBySeason",nbEggsBySeason);
    }

    ///////////////////////
    //    MUSIC SWITCH
    ///////////////////////

    /// ----- MUSIC START  -----///
    public void PlayHubMusic()
    {
        hubMusic.start();
        currentMusic = hubMusic;
    }

    public void PlayMenuMusic()
    {
        menuMusic.start();
        currentMusic = menuMusic;
    }

    /// ----- MUSIC STOP  -----///
    public void StopMenuMusicFade()
    {
        //SceneMusicFadeOut(alphafade,fadePara);
       menuMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StopHubMusicFade()
    {
        //SceneMusicFadeOut(alphafade,fadePara);
        hubMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


    /// ----- TRANSITION TO HUB -----///
    public void TransitionHub()
    {
        StopMenuMusicFade(); //stop menu music
        PlayHubMusic(); //start hub music
    }

    public void TransitionMenu()
    { 
        StopHubMusicFade(); //stop hub music
        PlayMenuMusic(); //start menu music
    }

    public FMOD.Studio.EventInstance getCurrentMusic()
    {
        return currentMusic;
    }


    /////////////////////
    // SFX & BUTTONS
    /////////////////////

    /// ----- PLAY BUTTON SOUND -----///
    public void PlaySound()
    {
        PlaySnd = FMODUnity.RuntimeManager.CreateInstance(SFXPlayButton);
        PlaySnd.start();
        TransitionHub();
        MapUncover();   //eventually to time with the seasons unlocked

    }

    /// ----- RESET BUTTON SOUND -----///
    public void ResetSound()
    {
        ResetSnd = FMODUnity.RuntimeManager.CreateInstance(SFXResetButton);
        ResetSnd.start();
        TransitionHub();

    }

        /// ----- BACK TO MENU BUTTON -----///
    public void BackBtnSound()
    {
        BackBtnSnd = FMODUnity.RuntimeManager.CreateInstance(SFXBackButton);
        BackBtnSnd.start();
		
        Debug.Log("Audio: Transition To Menu Music");
        TransitionMenu();
    }
   
    /// ----- MAP UNCOVER SFX  -----///
    public void MapUncover()
    {
        //To script with the hub music system in conjunction with the "Hub.cs" script
        MapSnd = FMODUnity.RuntimeManager.CreateInstance(SFXMapUncover);
        MapSnd.start();
    }

}
