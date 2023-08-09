using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInOutUI : MonoBehaviour
{
    private Transform InPos, outPos;
    private Vector3 currentTargetPos;
    public GameObject target;
    public bool startIn = false;
    private bool revertOnFinish = false, moving = false, isIn;
    private float timer = 0.0f;
    public float animationTime;
    public AnimationCurve moveAnimationCurve;
    // Start is called before the first frame update
    void Start()
    {
        if(startIn){
            InPos = this.gameObject.transform;
            outPos = target.transform;
            isIn = true;
        }else{
            InPos = target.transform;
            outPos = this.gameObject.transform;
            isIn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(moving){
            timer += Time.deltaTime/animationTime;
            this.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,currentTargetPos,moveAnimationCurve.Evaluate(timer));
            if(timer >= 1){
                timer = 0;
                moving = false;
                this.transform.position = currentTargetPos;
                if(isIn){
                    isIn = false;
                }else{
                    isIn = true;
                }
                if(revertOnFinish){
                    revertOnFinish = false;
                    MoveInOut();
                }
            }
        }
    }
    public void MoveInOut(){
        if(moving){
            revertOnFinish = true;
        }
        if(isIn){
            currentTargetPos = outPos.position;
        }else{
            currentTargetPos = InPos.position;
        }
        moving = true;
    }
}
