using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellShuffleTutorial : PuzzTutorial
{
    public  BeachClam[] step1Clams, step2Clams, step3Calms;
    public TutorialUiAnimation tapItemAnimation;
    void Start()
    {
        StartTutorial();
        maxStep = tutorialSteps.Length;
        currentStepScript = tutorialSteps[currentStep];
        tapItemAnimation =  tapIcon.GetComponent<TutorialUiAnimation>();
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
                if(currentStep == 0){
                    currentStepScript.masks[0].gameObject.transform.position = step1Clams[0].gameObject.transform.position;
                    currentStepScript.masks[1].gameObject.transform.position = step1Clams[1].gameObject.transform.position;
                    tapIcon.transform.position = step1Clams[0].gameObject.transform.position;
                    tapItemAnimation.myFade.FadeIn();
                    step1Clams[0].canTap = true;
                    mainPuzzScript.canPlay = true;
                }                
            }else{
                if(!currentStepScript.stepDone){
                    if(currentStep == 0){
                        if(step1Clams[0].Tapped){
                            tapItemAnimation.myFade.FadeOut();
                            step1Clams[0].canTap = false;
                        }
                        if(step1Clams[0].open && !step1Clams[1].open && tapItemAnimation.myFade.hidden){
                            tapIcon.transform.position = step1Clams[1].gameObject.transform.position;
                            tapItemAnimation.myFade.FadeIn();
                            step1Clams[1].canTap = true;
                        }
                        //currentStepScript.loaded = false;
                        //currentStepScript.stepDone = true;
                    }
                }else{
                    currentStep ++;
                }
            }
        }        
        
    }
}
