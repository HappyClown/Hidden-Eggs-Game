using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellShuffleTutorial : PuzzTutorial
{
    // Start is called before the first frame update
    public 
    void Start()
    {
        StartTutorial();
        maxStep = tutorialSteps.Length;
        currentStepScript = tutorialSteps[currentStep];
    }

    // Update is called once per frame
    void Update()
    {
        if(!tutOpen){
            StartTutorialInitialSequence();            
        }else{
            if(!currentStepScript.loaded){
                currentStepScript.gameObject.SetActive(true);
                currentStepScript.loaded = true;
            }else{
                if(!currentStepScript.stepDone){

                }
            }
        }        
        
    }
}
