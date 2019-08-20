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

    [Header("Intro Music")]
    [FMODUnity.EventRef]
    public string introMusicEvent = "event:/Music/MusicScenes/IntroMusic"; 
    public FMOD.Studio.EventInstance introMusic;

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

    [Header("SFX")]
    [FMODUnity.EventRef]
    public string ButtonSndEvent = "event:/SFX/SFX_General/Button";
    public FMOD.Studio.EventInstance buttonSnd;

    [FMODUnity.EventRef]
    public string SFX_MapUncover = "event:/SFX/SFX_General/HubUncoverFX";
    public FMOD.Studio.EventInstance mapSnd;

    [FMODUnity.EventRef]
    public string SFX_Clouds = "event:/SFX/SFX_General/CloudsMoving";
    public FMOD.Studio.EventInstance CloudsSnd;

    [FMODUnity.EventRef]
    public string SFX_Zoom  = "event:/SFX/SFX_General/Zoom";
    public FMOD.Studio.EventInstance ZoomSnd;

    //-- stat paper --
    [FMODUnity.EventRef]
    public string SFX_statPaperOn  = "event:/SFX/SFX_General/StatsPaperVillageOn";
    public FMOD.Studio.EventInstance SFX_statPaperOnSnd;

    [FMODUnity.EventRef]
    public string SFX_statPaperOff  = "event:/SFX/SFX_General/StatsPaperVillageOff";
    public FMOD.Studio.EventInstance SFX_statPaperOffSnd;

    [Header("Loops Map Glow & Scene Selection")]

    [FMODUnity.EventRef]
    public string SFX_ShimyLoopMenu = "event:/SFX/SFX_General/shimyLoopMenu";
    public FMOD.Studio.EventInstance shimyLoopMenuSnd;

    [FMODUnity.EventRef]
    public string MarketAmbiance = "event:/SFX/Market/AmbianceMarketDreamy";
    public FMOD.Studio.EventInstance marketAmbianceSnd;

    [FMODUnity.EventRef]
    public string ParkAmbiance = "event:/SFX/Park/AmbianceParkDreamy";
    public FMOD.Studio.EventInstance parkAmbianceSnd;

    [FMODUnity.EventRef]
    public string BeachAmbiance = "event:/SFX/Beach/AmbianceBeachDreamy";
    public FMOD.Studio.EventInstance beachAmbianceSnd;
    
    [Header("Buttons")]
    public Button BtnPlay;
    public Button BtnNewGame;
    public Button BackMenuBtn;


    //TEST level glow & ambiance
    private FMOD.Studio.EventInstance currentMusic;

    private FMOD.Studio.EventInstance soundSelectedScene;

    public int currentlevelSelected = -1;

    //TEST
    public int nbEggsBySeason;

    public bool dissolvingSnd = false;

    public int nbUnlockedSeasons = 1; //defaut : summer

    public bool CloudsIn = false;

    
    [Header("Scripts References")]

    public Hub hubscript;

    public FadeInOutTMP fadeInOutStory; //quick fix, find a better system

    public bool audioIntro_ON = false;
    void Start()
    {
        BtnPlay.onClick.AddListener(PlaySound);
        BtnNewGame.onClick.AddListener(NewGameSound);
        BackMenuBtn.onClick.AddListener(BackBtnSound);

        menuMusic = FMODUnity.RuntimeManager.CreateInstance(menuEvent);
        hubMusic = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);
        introMusic = FMODUnity.RuntimeManager.CreateInstance(introMusicEvent);

        UnlockedSeasonMusic(); //to check which seasons are unlocked
        //play the associated hub music
        hubMusic.setParameterValue("Summer", SummerOn);
        hubMusic.setParameterValue("Autumn", AutumnOn);
        hubMusic.setParameterValue("Winter", WinterOn);
        hubMusic.setParameterValue("Spring", SpringOn);

        //hubMusic.setParameterValue("nbEggsBySeason",nbEggsBySeason); //TEST

        if(!audioIntro_ON){
        currentMusic = menuMusic;}
        else{currentMusic = introMusic;}


        if (!GlobalVariables.globVarScript.toHub) { TransitionMenu(); } else { TransitionHub();}

        //---TEST: LOOPS Menu Glow etc

        shimyLoopMenuSnd = FMODUnity.RuntimeManager.CreateInstance(SFX_ShimyLoopMenu);
    }
    void Update()
    {
        //EggBySeason();
        hubMusic.setParameterValue("Summer", SummerOn);
        hubMusic.setParameterValue("Autumn", AutumnOn);
        hubMusic.setParameterValue("Winter", WinterOn);
        hubMusic.setParameterValue("Spring", SpringOn);

        //hubMusic.setParameterValue("nbEggsBySeason",nbEggsBySeason);

        //ajout sfx pour quand les nuages se tassent 
        if(fadeInOutStory.getFadingOut() && !CloudsIn)
        {
            CloudsMoving();
            CloudsIn = true;
        }
        if(!fadeInOutStory.getFadingOut())
        {
            CloudsIn = false;
        }


        //Dissolving Sound when map is uncovered
        if(hubscript.dissolving && !dissolvingSnd)
        {
            MapUncover();
            dissolvingSnd =true;
        }
        if(!hubscript.dissolving)
        {
            dissolvingSnd =false;
        }
    }

    ///////////////////
    // SEASONS
    ////////////////////

    public void UnlockedSeasonMusic()
    {
        
        //CHECK WITH THE UNLOCKED SEASONS / HUB.CS ..not working well for some reason
        /* 
        foreach(bool b in hubscript.unlockedSeasons )
        {
            if(b == true)
            nbUnlockedSeasons++;
        }
        Debug.Log("nb Unlocked seasons "+ nbUnlockedSeasons);

        */

        //DEFAULT : SUMMER

        switch(nbUnlockedSeasons){
            case 1: 
                SpringOn = 0;
                WinterOn = 0;
                AutumnOn = 0;
                SummerOn = 1f;
            break;
            case 2:                    
                SpringOn = 0;
                WinterOn = 0;
                AutumnOn = 1f;
                SummerOn = 0;
            break;
            case 3:
                SpringOn = 0;
                WinterOn = 1f;
                AutumnOn = 0;
                SummerOn = 0;
            break;
            case 4: 
                SpringOn = 1f;
                WinterOn = 0;
                AutumnOn = 0;
                SummerOn = 0;
            break;
        }
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
        AmbianceGlowStop();
        ShimyLoopSoundStop();

    }
        public void StopIntroMusicFade()
    {
        introMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
        StopIntroMusicFade(); //stop intro music
        PlayMenuMusic(); //start menu music
        AmbianceGlowStop();
        ShimyLoopSoundStop();
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
        ButtonSound();
        CloudsMoving();
        TransitionHub();
    }

    /// ----- NEW GAME BUTTON SOUND -----///
    public void NewGameSound()
    {
        nbUnlockedSeasons =1; //return to summer
        ButtonSound();
        TransitionHub();
    }

        /// ----- BACK TO MENU BUTTON -----///
    public void BackBtnSound()
    {
        ButtonSound();
        Debug.Log("Audio: Transition To Menu Music");
        TransitionMenu();
    }
    public void ButtonSound()
    {
        buttonSnd = FMODUnity.RuntimeManager.CreateInstance(ButtonSndEvent);
        buttonSnd.start();
    }


   
    /// ----- MAP UNCOVER SFX  -----///
    public void MapUncover()
    {
        //To script with the hub music system in conjunction with the "Hub.cs" script
        mapSnd = FMODUnity.RuntimeManager.CreateInstance(SFX_MapUncover);
        mapSnd.start();
    }

    public void CloudsMoving()
    {
        CloudsSnd = FMODUnity.RuntimeManager.CreateInstance(SFX_Clouds);
        CloudsSnd.start();
    }

    //------Stats Paper Map------//
    
    public void StatPaperSound_on(){
        SFX_statPaperOnSnd = FMODUnity.RuntimeManager.CreateInstance(SFX_statPaperOn);
        SFX_statPaperOnSnd.start();

    }

    public void StatPaperSound_off(){
        SFX_statPaperOffSnd = FMODUnity.RuntimeManager.CreateInstance(SFX_statPaperOff);
        SFX_statPaperOffSnd.start();

    }


    // -------ZOOM sound ------//

    public void ZoomSound()
    {
        ZoomSnd = FMODUnity.RuntimeManager.CreateInstance(SFX_Zoom);
        ZoomSnd.start();
    }

    //--------LOOP map selection-----//

    public void ShimyLoopSoundStart()
    {
        shimyLoopMenuSnd.start();
    }

        public void ShimyLoopSoundStop()
    {
        shimyLoopMenuSnd.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void AmbianceGlowStart(int levelSelected)
    {
        currentlevelSelected = levelSelected;

        /*
        //QUICK TEST NOT IDEAL
        Debug.Log("AUDIO : int scene selected = "+levelSelected);
        if(levelSelected == 0){
            soundSelectedScene = FMODUnity.RuntimeManager.CreateInstance(MarketAmbiance);
            Debug.Log("AUDIO : scene selected = Market");
        }
        else if(levelSelected == 1){
            soundSelectedScene = FMODUnity.RuntimeManager.CreateInstance(ParkAmbiance);
            Debug.Log("AUDIO : scene selected = Park");
        }
        else if(levelSelected == 2){
            soundSelectedScene = FMODUnity.RuntimeManager.CreateInstance(BeachAmbiance);
            Debug.Log("AUDIO : scene selected = Beach");
            
        }
        */
        
        soundSelectedScene.start();

        //Debug.Log("AUDIO : AMB start");
        ButtonSound();
    }
    public void AmbianceGlowStop()
    {
        soundSelectedScene.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //Debug.Log("AUDIO : AMB stop");
    }

}
