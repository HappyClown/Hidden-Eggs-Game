using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXMuteButt : MonoBehaviour {
	public Button button;
	public GameObject onIcon, offIcon;
	public bool mute;
	public PauseMenu myMenu;
	// Use this for initialization
	void Start () {
		button = this.GetComponent<Button>();
		mute = myMenu.myAudio.Muted;
		button.onClick.AddListener(delegate {ButtonPressed(); });
	}
	
	// Update is called once per frame
	void Update(){
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
	public void ButtonPressed () {
		if(mute){
			mute = false;
		}
		else{
			mute = true;
		}
		myMenu.SetMute(mute);
	}
}
