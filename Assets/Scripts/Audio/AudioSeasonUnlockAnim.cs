using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioSeasonUnlockAnim : MonoBehaviour 
{

    public string eggCounterEvent = "event:/SFX/ANIMS/Season Unlocked/Egg Counter tick";
    public FMOD.Studio.EventInstance eggCounterSound;

    public string lockScaleEvent = "event:/SFX/ANIMS/Season Unlocked/Lock Zoom";
    public FMOD.Studio.EventInstance lockScaleSound;

    public string lockImploseEvent = "event:/SFX/ANIMS/Season Unlocked/Implose";
    public FMOD.Studio.EventInstance lockImploseSound;

    public string lockUnlockEvent = "event:/SFX/ANIMS/Season Unlocked/Unlock";
    public FMOD.Studio.EventInstance lockUnlockSound;

    public string lockFireworksTrailBurstEvent = "event:/SFX/ANIMS/Season Unlocked/FireworkTrailBurst";
    public FMOD.Studio.EventInstance lockFireworksTrailBurstSound;
    public FMOD.Studio.EventInstance currentNote;
    public List<string> listNotes;
    int notesIndex = 14;
    int eggIndex =70;
    bool ascending = false;


	void Start () 
	{
        init_listNotes();

	}
	
	void Update () 
	{

	}

    ///////////////////////////////////
    ///  Play SFX
    ///////////////////////////////////



    public void init_listNotes(){
        
        listNotes = new List<string>();
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_01");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_02");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_03");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_04");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_05");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_06");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_07");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_08");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_09");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_10");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_11");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_12");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_13");
        listNotes.Add("event:/SFX/ANIMS/Season Unlocked/Notes/Notes_harp_14");

    }

    public void eggCounterSnd(){
        //eggCounterSound = FMODUnity.RuntimeManager.CreateInstance(eggCounterEvent);  //ticking sound
        //eggCounterSound.start();
        eggIndex--;

        
        //ascending notes
        // if(notesIndex<14 && ascending){
        // currentNote = FMODUnity.RuntimeManager.CreateInstance((string)listNotes[notesIndex]);
        // currentNote.start();
        // notesIndex++;
        // }
        // else if (notesIndex > 0){
        //     ascending =false;
        //     notesIndex--;
        //     currentNote = FMODUnity.RuntimeManager.CreateInstance((string)listNotes[notesIndex]);
        //     currentNote.start();
        // }
        // else{
        //     ascending = true;
        // }

        //descending notes
        if(notesIndex>0 && !ascending){
        notesIndex--;
        currentNote = FMODUnity.RuntimeManager.CreateInstance((string)listNotes[notesIndex]);
        currentNote.start();
        }
        else if (notesIndex == 0 && !ascending){
            ascending =true;
            currentNote = FMODUnity.RuntimeManager.CreateInstance((string)listNotes[notesIndex]);
            currentNote.start();
            notesIndex++;
        }
        else if (notesIndex<14 && ascending){
            currentNote = FMODUnity.RuntimeManager.CreateInstance((string)listNotes[notesIndex]);
            currentNote.start();
            notesIndex++;
        }
        else if (notesIndex == 14 && ascending){
            ascending = false;
        }
        
    }
    public void lockScaleUpSnd(){
        lockScaleSound = FMODUnity.RuntimeManager.CreateInstance(lockScaleEvent);
        lockScaleSound.start();

    }

    public void lockScaleDown(){
        lockScaleSound = FMODUnity.RuntimeManager.CreateInstance(lockScaleEvent);
        lockScaleSound.start();    
    }
        public void lockImploseSnd(){
        lockImploseSound = FMODUnity.RuntimeManager.CreateInstance(lockImploseEvent);
        lockImploseSound.start();    
    }        
        public void lockUnlockSnd(){
        lockUnlockSound = FMODUnity.RuntimeManager.CreateInstance(lockUnlockEvent);
        lockUnlockSound.start();    
    }
        public void lockFireworksTrailBurstSnd(){
        lockFireworksTrailBurstSound = FMODUnity.RuntimeManager.CreateInstance(lockFireworksTrailBurstEvent);
        lockFireworksTrailBurstSound.start();    
    }
}
