using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioTransitions : MonoBehaviour , IPointerClickHandler

{
    [Header("Custom Button")]
    public GameObject Beach;
    public GameObject Market;
    public GameObject Park;
	
    [Header("SFX Custom Button")]
    [FMODUnity.EventRef]
    public string SFXCustomButton;
    public FMOD.Studio.EventInstance CustomBtnSnd;

    [Header("Transition to Scene")]
    public SceneFade TransitionScene;

       
    void Start()
    {

    }
	
    /// ----- SCENES CUSTOM BUTTON  -----///
    public void TransitionScenes()
    {
        //select music of the next scene depending of the clicked custom button
        //reference to Scenefade.cs 
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        ///not working
        Debug.Log("Bouton Custom");
        CustomBtnSnd = FMODUnity.RuntimeManager.CreateInstance(SFXCustomButton);
        CustomBtnSnd.start();

        //when clicked stop hub music
    }

}
