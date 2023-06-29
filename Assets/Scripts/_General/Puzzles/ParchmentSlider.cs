using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParchmentSlider : MonoBehaviour
{
    
	public Transform insidePos;
    public float slideTime;
    public AnimationCurve slideCurve;
    public FadeInOutTMP textFade;
    public bool hidden, inpos;
    public float slideTimer;
	private Transform initialPos, currentPos, targetPos;
    private FadeInOutImage myFade;
    private bool moving, movingIn;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = this.gameObject.transform;
        myFade = this.gameObject.GetComponent<FadeInOutImage>();
        hidden = true;
        moving = movingIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(moving){
            slideTimer += Time.deltaTime;
            float posValue = slideCurve.Evaluate(slideTimer/slideTime);
            Debug.Log(posValue);
            this.transform.position = Vector3.Lerp(currentPos.position,targetPos.position,posValue);
            if(slideTimer >= slideTime){
                moving = false;
                this.transform.position = targetPos.position;
                if(movingIn){
                    textFade.FadeIn();
                    inpos = true;
                }                
            }
        }
    }
    public void SlideIn(){
        moving = true;
        myFade.FadeIn();
        currentPos = this.gameObject.transform;
        targetPos = insidePos;
        slideTimer = 0;
        movingIn = true;
    }
    public void SlideOut(){
        currentPos = this.gameObject.transform;
        targetPos = initialPos;
        moving = true;
        myFade.FadeOut();
        textFade.FadeOut();
        slideTimer = 0;
        movingIn = false;
        inpos = false;
    }
}
