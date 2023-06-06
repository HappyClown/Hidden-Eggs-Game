using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellShuffleTutorial : PuzzTutorial
{
    public  BeachClam[] step1Clams, step2Clams, step3Clams;
    public List<BeachClam> currentStepClams = new List<BeachClam>(); 
	public TutorialStep currentStepScript;
    public TutorialUiAnimation tapItemAnimation;
    void Start()
    {
        StartTutorial();
        maxStep = tutorialSteps.Length;
        currentStepScript = tutorialSteps[currentStep];
        tapItemAnimation =  tapIcon.GetComponent<TutorialUiAnimation>();
        finalStep = false;
        firstStep = loadingStep = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!tutOpen){
            StartTutorialInitialSequence();            
        }else if(!finalStep){
            if(currentStepScript.loaded && !loadingStep){
                if(!currentStepScript.stepDone){
                    if(currentStepClams[0].Tapped){
                        tapItemAnimation.myFade.FadeOut();
                        currentStepClams[0].canTap = false;
                    }
                    if(currentStepClams[0].open && !currentStepClams[1].open && tapItemAnimation.myFade.hidden){
                        tapIcon.transform.position = currentStepClams[1].gameObject.transform.position;
                        tapItemAnimation.myFade.FadeIn();
                        currentStepClams[1].canTap = true;
                        audioHelperBirdScript.birdHelpSound();
                    }
                    if(currentStepClams[1].Tapped){
                        tapItemAnimation.myFade.FadeOut();
                        currentStepClams[1].canTap = false;
                        currentStepScript.stepDone = true;
                    }                    
                }else{
                    currentStepScript.loaded = false;
                    currentStepClams.Clear();
                    loadingStep = true;
                }
            }else if(currentStep == 0 && !loadingStep){
                LoadNextStep();
            }else if(loadingStep){
                stepTimer += Time.deltaTime;
                if(stepTimer >= stepTimeDelay){
                    if(firstStep){
                       LoadNextStep();
                       firstStep = false;
                    }else{
                        stepTimer = 0;
                        loadingStep = false;                    
                        currentStepScript.gameObject.SetActive(false);
                        currentStep ++;
                        if(currentStep >= tutorialSteps.Length ){
                            finalStep = true;
                        }else{
                            currentStepScript = tutorialSteps[currentStep];
                        }
                        LoadNextStep();
                    }
                    
                }
            }
        }else{

        }        
        
    }
    private void LoadNextStep(){
        currentStepScript.gameObject.SetActive(true);
        if(currentStep == 0){
            foreach (BeachClam clam in step1Clams)
            {
                currentStepClams.Add(clam);
            }
        }
            if(currentStep == 1){
            foreach (BeachClam clam in step2Clams)
            {
                currentStepClams.Add(clam);
            }
        }
            if(currentStep == 2){
            foreach (BeachClam clam in step3Clams)
            {
                currentStepClams.Add(clam);
            }
        }
        currentStepScript.loaded = true;
        currentStepScript.masks[0].gameObject.transform.position = currentStepClams[0].gameObject.transform.position;
        currentStepScript.masks[1].gameObject.transform.position = currentStepClams[1].gameObject.transform.position;
        tapIcon.transform.position = currentStepClams[0].gameObject.transform.position;
        tapItemAnimation.myFade.FadeIn();
        currentStepClams[0].canTap = true;
        mainPuzzScript.canPlay = true;
        audioHelperBirdScript.birdHelpSound();
    }
}
