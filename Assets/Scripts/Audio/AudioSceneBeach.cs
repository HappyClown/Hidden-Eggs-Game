using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSceneBeach : MonoBehaviour
{
	[Header("Crab Walk SFX")]
    [FMODUnity.EventRef]
    public string crabWalkEvent;
    public FMOD.Studio.EventInstance crabWalkSound;

    [Header("Crab Claws SFX")]
    [FMODUnity.EventRef]
    public string crabClawsEvent;
    public FMOD.Studio.EventInstance crabClawsSound;
    
    void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

    public void crabWalkSFX()
    {
        crabWalkSound = FMODUnity.RuntimeManager.CreateInstance(crabWalkEvent);
        crabWalkSound.start();

    }
        public void crabClawsSFX()
    {
        crabClawsSound = FMODUnity.RuntimeManager.CreateInstance(crabClawsEvent);
        crabClawsSound.start();

    }

}
