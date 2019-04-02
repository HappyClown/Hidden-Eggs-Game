using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioHelperBird : MonoBehaviour 
{
	[Header("Buttons")]

    public Button BirdHelpBtn;
    public Button RiddleBtn;
    public Button HintBtn;
    public Button PauseBtn;
    public Button XBtn;


	[Header("Bird Help")]
    [FMODUnity.EventRef]
    public string birdEvent;
    public FMOD.Studio.EventInstance birdSound;

    [FMODUnity.EventRef]
    public string birdFrozenEvent;
    public FMOD.Studio.EventInstance birdFrozenSound;

/* 
	[Header("Congratulations Lvl Complete")]
    [FMODUnity.EventRef]
    public string lvlCompleteEvent;
    public FMOD.Studio.EventInstance lvlCompleteSound;
*/
    [FMODUnity.EventRef]
    public string buttonEvent = "event:/SFX/SFX_General/Button";
    public FMOD.Studio.EventInstance buttonSound;

    [FMODUnity.EventRef]
    public string PausebuttonEvent = "event:/SFX/SFX_General/PauseButton";
    public FMOD.Studio.EventInstance PausebuttonSound;


	void Start () 
	{
        BirdHelpBtn.onClick.AddListener(birdHelpSound);
        RiddleBtn.onClick.AddListener(birdHelpSound);
        HintBtn.onClick.AddListener(birdHelpSound);
        if(PauseBtn){
           PauseBtn.onClick.AddListener(PausebuttonSFX); 
        }
        if(XBtn){
               XBtn.onClick.AddListener(buttonSFX);
        }
	}
	
	void Update () 
	{

	}

    ///////////////////////////////////
    ///  Play SFX
    ///////////////////////////////////

    //Button generic
    public void buttonSFX()
    {
        //button sound
        buttonSound = FMODUnity.RuntimeManager.CreateInstance(buttonEvent);
        buttonSound.start();
    }
    public void PausebuttonSFX()
    {
        //button sound
        PausebuttonSound = FMODUnity.RuntimeManager.CreateInstance(PausebuttonEvent);
        PausebuttonSound.start();
    }

    public void birdHelpSoundFrozen()
    {
        birdFrozenSound = FMODUnity.RuntimeManager.CreateInstance(birdFrozenEvent);
        birdFrozenSound.start();
    }
        public void birdHelpSound()
    {
        birdSound = FMODUnity.RuntimeManager.CreateInstance(birdEvent);
        birdSound.start();
    }





/// Lvl complete
/* 
    public void lvlCompleteSnd()
    {
        lvlCompleteSound = FMODUnity.RuntimeManager.CreateInstance(lvlCompleteEvent);
        lvlCompleteSound.start();
    }
*/
}
