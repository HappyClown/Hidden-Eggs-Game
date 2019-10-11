using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioTransitions : MonoBehaviour

{	
    [Header("SFX Custom Button")]
    [FMODUnity.EventRef]
    public string SFXCustomButton;
    public FMOD.Studio.EventInstance CustomBtnSnd;

    [Header("Transition to Scene")]
    public SceneFade sceneScript;

    [Header("Transition From Hub")]
    public AudioManagerHubMenu audioManHubScript;

    [Header("Cue Title Card Transition")]
    [FMODUnity.EventRef]
    public string transEvent;
    public FMOD.Studio.EventInstance transMusic;


    void Start()
    {
        transMusic = FMODUnity.RuntimeManager.CreateInstance(transEvent);
        //currentScene = SceneManager.GetActiveScene().name; 
        //Init_DictMusicTransitions();
    }

void Update()   
    {
	

    }

    /// ----- TRANSITIONS LINKED TO THE SCENE FADE SCRIPT  -----///
    public void TransitionScenes(string sceneName)
    {


		if(audioManHubScript)
		{
			btnSound(); //for the custom button sound of the hub 
            audioManHubScript.StopHubMusicFade();
            audioManHubScript.StopIntroMusicFade();

            transMusic.start();
		}
		
		/*
        if(sceneName == GlobalVariables.globVarScript.menuName)
        {
            btnSound(); //for the custom button sound of the hub 
            audioManHubScript.StopHubMusicFade();
            transMusic.start();

        }
		
        if(sceneName == GlobalVariables.globVarScript.parkName)
        {
            transMusic.start();
        }
        if(sceneName == GlobalVariables.globVarScript.marketName)
        {
            transMusic.start();
        }
        if(sceneName == GlobalVariables.globVarScript.beachName)
        {
            transMusic.start();
        }
        if(sceneName == GlobalVariables.globVarScript.beachPuzName)
        {
            transMusic.start();
        }
        if(sceneName == GlobalVariables.globVarScript.parkPuzName)
        {
            transMusic.start();
        }
        if(sceneName == GlobalVariables.globVarScript.marketPuzName)
        {
            transMusic.start();
        }
		*/
    }
 
    ////////////////////////////////
    //   MUSIC STOP / PLAY
    ////////////////////////////////

    public void btnSound()
    {
        CustomBtnSnd = FMODUnity.RuntimeManager.CreateInstance(SFXCustomButton);
        CustomBtnSnd.start();
    }




}
