using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioManagerHub : MonoBehaviour

{
    [Header("Back To Menu Button")]
    public Button BackMenuBtn;
    [FMODUnity.EventRef]
    public string SFXBackButton;
    public FMOD.Studio.EventInstance BackBtnSnd;

    [Header("SFX Uncover Map")]
    [FMODUnity.EventRef]
    public string SFXMapUncover;
    public FMOD.Studio.EventInstance MapSnd;

    [Header("Hub Music")]
    [FMODUnity.EventRef]
    public string MusicEvent;
    public FMOD.Studio.EventInstance SceneMusic;
    public FMOD.Studio.ParameterInstance Summer;
    public FMOD.Studio.ParameterInstance Autumn;
    public FMOD.Studio.ParameterInstance Winter;
    public FMOD.Studio.ParameterInstance Spring;



    [Header("Transition to Menu")]
    public AudioManagerMenu AudioMenu;

    [Header("TEST Transition to Scene")]
    public AudioTransitionToScene AudioTransition;


    void Start()
    {
        BackMenuBtn.onClick.AddListener(BackBtnSound);
        SceneMusic = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);
        SceneMusic.getParameter("Summer", out Summer);
        SceneMusic.getParameter("Autumn", out Autumn);
        SceneMusic.getParameter("Winter", out Winter);
        SceneMusic.getParameter("Spring", out Spring);
    }

    /// ----- BACK TO MENU BUTTON -----///
    public void BackBtnSound()
    {
        BackBtnSnd = FMODUnity.RuntimeManager.CreateInstance(SFXBackButton);
        BackBtnSnd.start();
		
        Debug.Log("Audio: Transition To Menu Music");
        StopMusicFade(); //stop hub music
        AudioMenu.PlayMusic(); //start menu music
    }
   
    /// ----- MAP UNCOVER SFX  -----///
    public void MapUncover()
    {
        //To script with the hub music system in conjunction with the "Hub.cs" script
        MapSnd = FMODUnity.RuntimeManager.CreateInstance(SFXMapUncover);
        MapSnd.start();
    }

    /// ----- MUSIC START  -----///
    public void PlayMusic()
    {
        SceneMusic.start();
    }
    /// ----- MUSIC STOP -----///
    void OnDestroy() //crude alternative while the custom button isnt working
    {
        Debug.Log(" Audio: TEST - Transition To Scene Music (this is temporary OnDestroy)");
        StopMusicFade();
        // --- TEST ---- //
        AudioTransition.Beach.PlayMusic();
    }

    public void StopMusicFade()
    {
        SceneMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

}
