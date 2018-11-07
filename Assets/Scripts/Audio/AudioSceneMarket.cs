using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSceneMarket : MonoBehaviour 
{
	[Header("Golden Egg Baskets SFX")]
    [FMODUnity.EventRef]
    public string basketsEvent;
    public FMOD.Studio.EventInstance basketsSound;
    
    void Start () 
	{
		
	}
	
	void Update () 
	{
		
	}

    public void goldenEggBasketsSFX()
    {
        basketsSound = FMODUnity.RuntimeManager.CreateInstance(basketsEvent);
        basketsSound.start();

        //random sounds ftm but it could be nice to have a sequence depending of the colors ? ..
    }

}
