using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class AudioVolumeSettings : MonoBehaviour
{
    //public Button muteMusicBtn;
    //public Button muteSoundBtn;

    //public Slider sliderMusic;
   // public Slider sliderSound;

   // public Slider sliderTouch;

   // public Button XBtn;


    [FMODUnity.EventRef]
    public string buttonEvent = "event:/SFX/SFX_General/Button";
    public FMOD.Studio.EventInstance buttonSound;

    [FMODUnity.EventRef]
    public string muteEvent_perc = "event:/SFX/SFX_General/muteButton_perc";
    public FMOD.Studio.EventInstance muteSound_perc;

    [FMODUnity.EventRef]
    public string unMuteEvent_perc = "event:/SFX/SFX_General/unMuteButton_perc";
    public FMOD.Studio.EventInstance unMuteSound_perc;

    [FMODUnity.EventRef]
    public string muteEvent_harp = "event:/SFX/SFX_General/muteButton_harp";
    public FMOD.Studio.EventInstance muteSound_harp;

    [FMODUnity.EventRef]
    public string unMuteEvent_harp = "event:/SFX/SFX_General/unMuteButton_harp";
    public FMOD.Studio.EventInstance unMuteSound_harp;

    
    [FMODUnity.EventRef]
    public string sliderEvent = "event:/SFX/SFX_General/sliderSound";
    public FMOD.Studio.EventInstance sliderSnd;

//  [FMODUnity.EventRef]
//  public string PausebuttonEvent = "event:/SFX/SFX_General/PauseButton";
//  public FMOD.Studio.EventInstance PausebuttonSound;

    

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;


    //On the slider keep the values between 0 and 1 ;
    [Range(0, 1)]
    public float MusicVolume = 0.5f;
    [Range(0, 1)]
    public float SFXVolume = 0.5f;

    float MasterVolume = 1.2f;


    public bool Muted;
    public bool Paused;




    void Awake(){
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/MUSIC");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");

    }

    void Update() 
    {
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
        Master.setVolume(MasterVolume);

       FMODUnity.RuntimeManager.PauseAllEvents(Paused);

       FMODUnity.RuntimeManager.MuteAllEvents(Muted);
    }

    	void Start () 
	{
        /* if(muteMusicBtn){
            muteMusicBtn.onClick.AddListener(muteMusicSFX);
            }
        if(muteSoundBtn){
            muteSoundBtn.onClick.AddListener(muteSFX);
            }
        if(sliderSound){
            sliderSound.onValueChanged.AddListener(delegate {sliderSFX();});

            }
        if(sliderMusic){
           sliderMusic.onValueChanged.AddListener(delegate {sliderSFX();}); 
        }

        if(sliderTouch){
           sliderTouch.onValueChanged.AddListener(delegate {sliderSFX();}); 
        }
        if(XBtn){
               XBtn.onClick.AddListener(buttonSFX);
        }*/
	}
        public void buttonSFX()
    {
        //button sound
        buttonSound = FMODUnity.RuntimeManager.CreateInstance(buttonEvent);
        buttonSound.start();
    }
    public void muteSFX()
    {
        if(SFXVolume == 0){
            unMuteSound_perc = FMODUnity.RuntimeManager.CreateInstance(unMuteEvent_perc);
            unMuteSound_perc.start();
        }
        else if(SFXVolume != 0){
            muteSound_perc = FMODUnity.RuntimeManager.CreateInstance(muteEvent_perc);
            muteSound_perc.start();
        }
    }

        public void muteMusicSFX()
    {
        if(MusicVolume == 0){
            unMuteSound_harp = FMODUnity.RuntimeManager.CreateInstance(unMuteEvent_harp);
            unMuteSound_harp.start();
        }
        else if(MusicVolume != 0){
            muteSound_harp = FMODUnity.RuntimeManager.CreateInstance(muteEvent_harp);
            muteSound_harp.start();
        }
    }



    public void sliderSFX()
    {
        //sliding sound
       // sliderSnd = FMODUnity.RuntimeManager.CreateInstance(sliderEvent);
       // sliderSnd.start();

    }

}