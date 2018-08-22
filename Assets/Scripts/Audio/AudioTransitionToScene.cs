using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioTransitionToScene : MonoBehaviour
{
    public AudioManagerHub AudioHub;
    public SceneFade AudioScene;
    //private string AudioSceneToLoad;

    [Header("SFX Transition")]
    [FMODUnity.EventRef]
    public string SFXTransition;
    public FMOD.Studio.EventInstance TranSnd;

	[Header("Beach")]
	public AudioMusicScene Beach;
	[Header("Park")]
	public AudioMusicScene Park;
	[Header("Market")]
	public AudioMusicScene Market;
	
    
    void update()
    {
        Debug.Log(SceneFade.fadeSceneOut);
        if(SceneFade.fadeSceneOut)
        {
			Debug.Log("FadeSceneOut true");
            //start transition sound
			TranSnd = FMODUnity.RuntimeManager.CreateInstance(SFXTransition);
			TranSnd.start();
            //fade out hub music
            HubStop();
            //fade in scene music ?? how to select the good one ?
            ScenePlay();
        }
    }
     void ScenePlay()
    {
        Beach.PlayMusic(); //test
    }

    void SceneStop()
    {
        Beach.StopMusicFade(); //test
    }
    void HubStop()
    {
        AudioHub.StopMusicFade(); //test
    }
}
	
	