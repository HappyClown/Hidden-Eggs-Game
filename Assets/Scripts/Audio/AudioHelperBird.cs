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

	void Start () 
	{
        BirdHelpBtn.onClick.AddListener(birdHelpSound);
        RiddleBtn.onClick.AddListener(birdHelpSound);
        HintBtn.onClick.AddListener(birdHelpSound);
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
