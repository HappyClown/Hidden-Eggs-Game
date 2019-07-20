using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXMuteButt : MonoBehaviour {
	public Button button;
	public GameObject onIcon, offIcon;
	public bool mute, inpPuzzle;
	public PauseMenu myMenu;
	public PuzzlePauseMenu MyPuzzleMenu;
	// Use this for initialization
	void Start () {
		button = this.GetComponent<Button>();
		if(inpPuzzle){
			mute = MyPuzzleMenu.myAudio.Muted;
		}
		else{
			mute = myMenu.myAudio.Muted;
		}
		button.onClick.AddListener(delegate {ButtonPressed(); });
	}
	
	// Update is called once per frame
	void Update(){
		if(inpPuzzle){
			if(MyPuzzleMenu.myAudio.SFXVolume == 0){
				onIcon.SetActive(false);
				offIcon.SetActive(true);
				mute = true;
			}else{
				onIcon.SetActive(true);
				offIcon.SetActive(false);
				mute = false;
			}
		}
		else{
			if(myMenu.myAudio.SFXVolume == 0){
				onIcon.SetActive(false);
				offIcon.SetActive(true);
				mute = true;
			}else{
				onIcon.SetActive(true);
				offIcon.SetActive(false);
				mute = false;
			}
		}
	}
	public void ButtonPressed () {
		if(mute){
			mute = false;
		}
		else{
			mute = true;
		}
		if(inpPuzzle){
			MyPuzzleMenu.SetMute(mute);
		}
		else{
			myMenu.SetMute(mute);
		}
	}
}
