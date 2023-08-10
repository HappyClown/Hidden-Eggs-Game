using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioKiteAnim : MonoBehaviour 
{

    //[FMODUnity.EventRef]
    // public string kiteMovingEvent=  "event:/SFX/Puzzle_Park/KiteMoving";
    // //public FMOD.Studio.EventInstance kiteMovingSound ;

    // [FMODUnity.EventRef]
    // public string kiteFlyingEvent = "event:/SFX/Puzzle_Park/KiteFlying";
    //public FMOD.Studio.EventInstance kiteFlyingSound ;

	void Start () 
	{

	}
	
	void Update () 
	{

	}

    ///////////////////////////////////
    ///  Play SFX
    ///////////////////////////////////

    //Button generic
    public void KiteMovingSFX(){

        //kiteMovingSound = FMODUnity.RuntimeManager.CreateInstance(kiteMovingEvent);
        //kiteMovingSound.start();
    }
    public void KiteFlyingSFX()
    {
        //kiteFlyingSound = FMODUnity.RuntimeManager.CreateInstance(kiteFlyingEvent);
        //kiteFlyingSound.start();
    }

}
