using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSilverEggs : MonoBehaviour 
{
	[Header("SilverEgg SFX")]
    [FMODUnity.EventRef]
    public string silverEggClickEvent;
    public FMOD.Studio.EventInstance silverEggClickSound;

	
	public void SilverEggTrailSFX () 
	{
		silverEggClickSound = FMODUnity.RuntimeManager.CreateInstance(silverEggClickEvent);
        silverEggClickSound.start();
	}
}
