using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioMusicScene : MonoBehaviour

{
    [Header("Music Scene")]
	public CustonButtonClick CustomBtn;
    [FMODUnity.EventRef]
    public string MusicEvent;
    public FMOD.Studio.EventInstance SceneMusic;
	
	public void PlayMusic()
    {
		Debug.Log("Scene Start");
        SceneMusic = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);
        SceneMusic.start();
    }
	
    public void StopMusicFade()
    {
		Debug.Log("Scene Stop");
        SceneMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}