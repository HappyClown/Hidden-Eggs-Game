using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{
    // Start is called before the first frame update
    public enum StepTypes
    {  
       Tap,Slide 
    }
    public StepTypes stepType;
    public Vector3 pos1, pos2;
    public FadeInOutImage screenFade;
    public GameObject[] masks;
    public GameObject[] messages;
    public bool stepDone = false, loaded = false, loading = false, hasText = false;
    public string textContent;
    
     void Update()
    {
        if(loading){
            if(screenFade.shown){
                foreach (GameObject mask in masks)
                {
                    mask.SetActive(true);
                }
                loading = false;
            }
        }
    }
}
