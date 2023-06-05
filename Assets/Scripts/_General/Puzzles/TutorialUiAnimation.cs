using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUiAnimation : MonoBehaviour
{   
    public enum AnimationType{
        TapAnimation,
        SlideAnimation
    }
    public AnimationType myAnimation;
    public float animationSpeed = 2.0f, animationDuration = 1.0f;
    public float tapMaxScale = 2.0f, tapMinScale = 1.0f, currentScale;
    private float animationTimer = 0;
    public bool animate, goingUp;
    public Transform slidePointA, slidePointB;
    public FadeInOutImage myFade;
    void Start()
    {
        if(myAnimation == AnimationType.TapAnimation){
            this.gameObject.transform.localScale = new Vector3(tapMinScale,tapMinScale,tapMinScale);
            goingUp = true;
            currentScale = tapMinScale;
        }else if(myAnimation == AnimationType.SlideAnimation){
            this.gameObject.transform.position = slidePointA.position;
        }
        animate = false;
        myFade = this.gameObject.GetComponent<FadeInOutImage>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!myFade.hidden && !animate){
            animate = true;
        }
        if(animate){
            if(myAnimation == AnimationType.TapAnimation){
                if(goingUp){
                    currentScale += Time.deltaTime * animationSpeed;
                    if(currentScale >= tapMaxScale){
                        goingUp = false;
                        currentScale = tapMaxScale;
                    }                    
                }else{
                    currentScale -= Time.deltaTime * animationSpeed;
                    if(currentScale <= tapMinScale){
                        goingUp = true;
                        currentScale = tapMinScale;
                    }                    
                }              
                this.gameObject.transform.localScale = new Vector3(currentScale,currentScale,currentScale);                
            }else if(myAnimation == AnimationType.SlideAnimation){

            }
        }
        
    }
}
