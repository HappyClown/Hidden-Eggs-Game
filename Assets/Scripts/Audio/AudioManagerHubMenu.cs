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
    
    [Header("Buttons")]
    public Button BtnPlay;
    public Button BtnNewGame;
    public Button BackMenuBtn;

    [FMODUnity.EventRef]
    public string ButtonSndEvent;
    public FMOD.Studio.EventInstance ButtonSnd;

    [Header("SFX Uncover Map")]
    [FMODUnity.EventRef]
    public string SFXMapUncover;
    public FMOD.Studio.EventInstance MapSnd;

    [Header("SFX Clouds")]
    [FMODUnity.EventRef]
    public string SFXClouds;
    public FMOD.Studio.EventInstance CloudsSnd;

    //TEST
    private FMOD.Studio.EventInstance currentMusic;


    //TEST
    public int nbEggsBySeason;

    public bool dissolvingSnd = false;

    public int nbUnlockedSeasons = 1; //defaut : summer

    public bool CloudsIn = false;

    
    [Header("Scripts References")]

    public Hub hubscript;

    public FadeInOutTMP fadeInOutStory; //quick fix, find a better system

 
    void Start()
    {
        BtnPlay.onClick.AddListener(PlaySound);
        BtnNewGame.onClick.AddListener(NewGameSound);
        BackMenuBtn.onClick.AddListener(BackBtnSound);

        menuMusic = FMODUnity.RuntimeManager.CreateInstance(menuEvent);
        hubMusic = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);

        UnlockedSeasonMusic(); //to check which seasons are unlocked
        //play the associated hub music
        hubMusic.setParameterValue("Summer", SummerOn);
        hubMusic.setParameterValue("Autumn", AutumnOn);
        hubMusic.setParameterValue("Winter", WinterOn);
        hubMusic.setParameterValue("Spring", SpringOn);

        //hubMusic.setParameterValue("nbEggsBySeason",nbEggsBySeason); //TEST

        currentMusic = menuMusic;

        if (!GlobalVariables.globVarScript.toHub) { TransitionMenu(); } else { TransitionHub();}
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

        if(nbUnlockedSeasons ==4)
        {
            SpringOn = 1f;
            WinterOn = 0;
            AutumnOn = 0;
            SummerOn = 0;
        }
        else
            if(nbUnlockedSeasons ==3)
            {
                SpringOn = 0;
                WinterOn = 1f;
                AutumnOn = 0;
                SummerOn = 0;
            }
            else 
                if(nbUnlockedSeasons ==2)
                {
                    SpringOn = 0;
                    WinterOn = 0;
                    AutumnOn = 1f;
                    SummerOn = 0;}
                else
                    if(nbUnlockedSeasons ==1)
                        {
                            SpringOn = 0;
                            WinterOn = 0;
                            AutumnOn = 0;
                            SummerOn = 1f;
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
        ButtonSnd = FMODUnity.RuntimeManager.CreateInstance(ButtonSndEvent);
        ButtonSnd.start();
    }


   
    /// ----- MAP UNCOVER SFX  -----///
    public void MapUncover()
    {
        //To script with the hub music system in conjunction with the "Hub.cs" script
        MapSnd = FMODUnity.RuntimeManager.CreateInstance(SFXMapUncover);
        MapSnd.start();
    }

        public void CloudsMoving()
    {
        CloudsSnd = FMODUnity.RuntimeManager.CreateInstance(SFXClouds);
        CloudsSnd.start();
    }

    

}
