using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagerMenu : MonoBehaviour
{
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


    [Header("Menu Music")]
    [FMODUnity.EventRef]
    public string MusicEvent;
    public FMOD.Studio.EventInstance SceneMusic;

    [Header("Transition to Hub")]
    public AudioManagerHub AudioHub;


    void Start()
    {
        BtnPlay.onClick.AddListener(PlaySound);
        BtnReset.onClick.AddListener(ResetSound);
        PlayMusic();
    }

    /// ----- MUSIC START  -----///
    public void PlayMusic()
    {
        SceneMusic = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);
        SceneMusic.start();
    }

    /// ----- MUSIC STOP  -----///
    public void StopMusicFade()
    {
        SceneMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    /// ----- TRANSITION TO HUB -----///
    void Transition()
    {
        Debug.Log("Audio: Transition To Hub Music");
        StopMusicFade(); //stop menu music
        AudioHub.PlayMusic(); //start hub music
    }

    /// ----- PLAY BUTTON SOUND -----///
    public void PlaySound()
    {
        PlaySnd = FMODUnity.RuntimeManager.CreateInstance(SFXPlayButton);
        PlaySnd.start();
        Transition();

    }

    /// ----- RESET BUTTON SOUND -----///
    public void ResetSound()
    {
        ResetSnd = FMODUnity.RuntimeManager.CreateInstance(SFXResetButton);
        ResetSnd.start();
    }
	
}
