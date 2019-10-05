using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioLevelCompleteAnim : MonoBehaviour 
{

    public string eggsCounterEvent = "event:/SFX/ANIMS/Level Complete/EggsCounter";
    public FMOD.Studio.EventInstance eggsCounterSound;

    public string bagAppearEvent = "event:/SFX/ANIMS/Level Complete/BagAppear";
    public FMOD.Studio.EventInstance bagAppearSound;

    public string bagHoverEvent = "event:/SFX/ANIMS/Level Complete/BagHover";
    public FMOD.Studio.EventInstance bagHoverSound;

    public string circleEggsGlowEvent = "event:/SFX/ANIMS/Level Complete/CircleEggsGlow";
    public FMOD.Studio.EventInstance circleEggsGlowSound;

    public string circleEggsSoloEvent = "event:/SFX/ANIMS/Level Complete/CircleEggsSolo";
    public FMOD.Studio.EventInstance circleEggsSoloSound;

    public string circleEggsSoloPlainEvent = "event:/SFX/ANIMS/Level Complete/CircleEggsSoloPlain";
    public FMOD.Studio.EventInstance circleEggsSoloPlainSound;

    
    public string circleEggsSoloSilverEvent = "event:/SFX/ANIMS/Level Complete/CircleEggsSoloSilver";
    public FMOD.Studio.EventInstance circleEggsSoloSilverSound;

    
    public string circleEggsSoloGoldEvent = "event:/SFX/ANIMS/Level Complete/CircleEggsSoloGold";
    public FMOD.Studio.EventInstance circleEggsSoloGoldSound;

    public string particulesInBagEvent = "event:/SFX/ANIMS/Level Complete/ParticulesInBag";
    public FMOD.Studio.EventInstance particulesInBagSound;

    
    public string eggsMoveInBagEvent = "event:/SFX/ANIMS/Level Complete/EggsMoveInBag";
    public FMOD.Studio.EventInstance eggsMoveInBagSound;
    public string bagExplodeEvent = "event:/SFX/ANIMS/Level Complete/BagExplode";
    public FMOD.Studio.EventInstance bagExplodeSound;
    public string bagRumbleEvent = "event:/SFX/ANIMS/Level Complete/BagRumble";
    public FMOD.Studio.EventInstance bagRumbleSound;

    public string congratsTxtEvent = "event:/SFX/ANIMS/Level Complete/CongratsTxt";
    public FMOD.Studio.EventInstance congratsTxtSound;

    public string levelComplete_ALLEvent = "event:/SFX/ANIMS/FakeAnim_LevelComplete";
    public FMOD.Studio.EventInstance levelComplete_ALLSound;

   bool audioWholeSequencePlay = true;

	void Start () 
	{
	}
	
	void Update () 
	{
	}

    ///////////////////////////////////
    ///  Play SFX
    ///////////////////////////////////


    public void eggsCounterSnd(){
        if(!audioWholeSequencePlay){
        eggsCounterSound = FMODUnity.RuntimeManager.CreateInstance(eggsCounterEvent);
        eggsCounterSound.start();
        }

    }
    public void bagAppearSnd(){
        if(!audioWholeSequencePlay){
        bagAppearSound = FMODUnity.RuntimeManager.CreateInstance(bagAppearEvent);
        bagAppearSound.start();
        }


        //CHEAT AND PLAY THE WHOLE ANIM SEQUENCE SOUND
        if(audioWholeSequencePlay){
            levelComplete_ALLSound = FMODUnity.RuntimeManager.CreateInstance(levelComplete_ALLEvent);
            levelComplete_ALLSound.start();    
         }

    }

    public void circleEggsSoloSnd(){
    if(!audioWholeSequencePlay){
        circleEggsSoloSound = FMODUnity.RuntimeManager.CreateInstance(circleEggsSoloEvent);
        circleEggsSoloSound.start();  
        }
    }

    public void circleEggsSoloPlainSnd(){
                if(!audioWholeSequencePlay){
        circleEggsSoloPlainSound = FMODUnity.RuntimeManager.CreateInstance(circleEggsSoloPlainEvent);
        circleEggsSoloPlainSound.start();   }
    }
    
    public void circleEggsSoloSilverSnd(){
                if(!audioWholeSequencePlay){
        circleEggsSoloSilverSound = FMODUnity.RuntimeManager.CreateInstance(circleEggsSoloSilverEvent);
        circleEggsSoloSilverSound.start();    }
    }
    
    public void circleEggsSoloGoldSnd(){
                if(!audioWholeSequencePlay){
        circleEggsSoloGoldSound = FMODUnity.RuntimeManager.CreateInstance(circleEggsSoloGoldEvent);
        circleEggsSoloGoldSound.start();   }
    }


        public void circleEggsGlowSnd(){
                    if(!audioWholeSequencePlay){
        circleEggsGlowSound = FMODUnity.RuntimeManager.CreateInstance(circleEggsGlowEvent);
        circleEggsGlowSound.start();    }
    }        
        public void particulesInBagSnd(){
                    if(!audioWholeSequencePlay){
        particulesInBagSound = FMODUnity.RuntimeManager.CreateInstance(particulesInBagEvent);
        particulesInBagSound.start();    }
    }

    
        public void eggsMoveInBagSnd(){
                    if(!audioWholeSequencePlay){
        eggsMoveInBagSound = FMODUnity.RuntimeManager.CreateInstance(eggsMoveInBagEvent);
        eggsMoveInBagSound.start();    }
    }
        public void bagRumbleSnd(){
                    if(!audioWholeSequencePlay){
        bagRumbleSound = FMODUnity.RuntimeManager.CreateInstance(bagRumbleEvent);
        // bagRumbleSound.start();   
         }
    }
        public void bagExplodeSnd(){
                    if(!audioWholeSequencePlay){
        bagExplodeSound = FMODUnity.RuntimeManager.CreateInstance(bagExplodeEvent);
        bagExplodeSound.start();    }
    }
        public void bagHoverSnd(){
                    if(!audioWholeSequencePlay){
        bagHoverSound = FMODUnity.RuntimeManager.CreateInstance(bagHoverEvent);
        bagHoverSound.start();    }
    }
        public void congratsTxtSnd(){
        congratsTxtSound = FMODUnity.RuntimeManager.CreateInstance(congratsTxtEvent);
        congratsTxtSound.start();    
    }

}