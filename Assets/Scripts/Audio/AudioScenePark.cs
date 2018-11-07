using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioScenePark : MonoBehaviour 
{
	[Header("Golden Egg Children Game SFX")]
    [FMODUnity.EventRef]
    public string childrenGameEvent;
    public FMOD.Studio.EventInstance childrenGameSound;
    
    void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

    public void goldenEggGameSFX()
    {
        childrenGameSound = FMODUnity.RuntimeManager.CreateInstance(childrenGameEvent);
        childrenGameSound.start();

        //random sounds ftm but it could be nice to have a sequence depending of the numbers ..
    }


}
