using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioBeachBootAnim : MonoBehaviour 
{

    [FMODUnity.EventRef]
    public string bootShakeEvent = "event:/SFX/Puzzle_Beach/BootShake";
    public FMOD.Studio.EventInstance bootShakeSound;

	void Start () 
	{

	}
	
	void Update () 
	{

	}

    ///////////////////////////////////
    ///  Play SFX
    ///////////////////////////////////

    public void bootShakeSFX()
    {
        bootShakeSound = FMODUnity.RuntimeManager.CreateInstance(bootShakeEvent);
        bootShakeSound.start();
    }

}
