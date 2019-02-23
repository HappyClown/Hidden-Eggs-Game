using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class AudioVolumeSettings : MonoBehaviour
{

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;


    public float MusicVolume = 0.5f;
    public float SFXVolume = 0.5f;

    public float MasterVolume = 0.8f;

    
    public float volumeTests = 0.8f;

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
        SFX.setVolume(MusicVolume);
        Master.setVolume(MusicVolume);

       FMODUnity.RuntimeManager.PauseAllEvents(Paused);

       FMODUnity.RuntimeManager.MuteAllEvents(Paused);



    }


}