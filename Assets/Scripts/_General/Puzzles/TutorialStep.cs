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
    public GameObject[] masks;
    public bool stepDone;
}
