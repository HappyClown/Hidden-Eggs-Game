using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	public float musicVolume, sfxVolume, panningLevel;
	public int musicMute, sfxMute;
	public float minVolumeReset;
	public AudioVolumeSettings myAudio;
	public LevelTapMannager myTap;
	public Slider musicSlider, sfxSlider, panningSlider;
	void Awake(){
		myAudio = GameObject.FindGameObjectWithTag("GlobalVariables").GetComponent<AudioVolumeSettings>();
		sfxVolume = PlayerPrefs.GetFloat("sfxVolume",myAudio.SFXVolume);
		musicVolume = PlayerPrefs.GetFloat("musicVolume",myAudio.MusicVolume);
		panningLevel =  PlayerPrefs.GetFloat("panningLevel",myTap.panningSpeed);
		// int tempsfxMute = 0;
		// int tempMusicMute = 0;
		// if(myAudio.Muted){	tempsfxMute = 1;}else{	tempsfxMute = 0;}
		// if(myAudio.Paused){	tempMusicMute = 1;}else{  tempMusicMute = 0;}
		// sfxMute = PlayerPrefs.GetInt("sfxMute",tempsfxMute);
		// musicMute = PlayerPrefs.GetInt("musicMute",tempMusicMute);
		// if(sfxMute == 1){myAudio.Muted = true;}else{myAudio.Muted = false;}
		// if(musicMute == 1){myAudio.Paused = true;}else{myAudio.Paused = false;}
	}
	void Start(){
		myAudio.SFXVolume = sfxVolume;
		myAudio.MusicVolume = musicVolume;
		myTap.panningSpeed = panningLevel;
		if(myAudio.Muted){	sfxSlider.value = 0;}else{	sfxSlider.value = sfxVolume;}
		if(myAudio.Paused){	musicSlider.value = 0;}else{	musicSlider.value = musicVolume;}
		panningSlider.value = panningLevel;
		sfxSlider.onValueChanged.AddListener(delegate {ChangeSFXVolume(); });
		musicSlider.onValueChanged.AddListener(delegate {ChangeMusicVolume(); });
		panningSlider.onValueChanged.AddListener(delegate {ChangePanning(); });
	}
	public void ChangePanning(){
		panningLevel = myTap.panningSpeed = panningSlider.value;
		PlayerPrefs.SetFloat("panningLevel",panningLevel);
		myAudio.sliderSFX(); //slider sound
	}
	public void ChangeSFXVolume(){
		sfxVolume = myAudio.SFXVolume = sfxSlider.value;
		PlayerPrefs.SetFloat("sfxVolume",sfxVolume);
		myAudio.sliderSFX(); //slider sound
		
	}
	public void ChangeMusicVolume(){
		musicVolume = myAudio.MusicVolume = musicSlider.value;
		PlayerPrefs.SetFloat("musicVolume",musicVolume);
		myAudio.sliderSFX(); //slider sound
		
	}
	public void SetMute(bool value){
		//myAudio.Muted = value;
		if(value){		
			sfxMute = 1;
			//sfxSlider.value = 0;
			myAudio.SFXVolume = 0;

		}
		else{
			sfxMute = 0;
			if(sfxVolume <= 0){
				myAudio.SFXVolume = minVolumeReset;
				sfxVolume = minVolumeReset;
				sfxSlider.value = sfxVolume;
				PlayerPrefs.SetFloat("sfxVolume",sfxVolume);
			}else{
				myAudio.SFXVolume = sfxVolume;
			}
		}
		myAudio.muteSFX(); //ui sound
	}
	public void SetPause(bool value){
		//myAudio.Paused = value;
		if(value){		
			musicMute = 1;
			myAudio.MusicVolume = 0;
			//musicSlider.value = 0;
		}
		else{
			musicMute = 0;
			if(musicVolume <= 0){
				myAudio.MusicVolume = minVolumeReset;
				musicVolume = minVolumeReset;
				musicSlider.value = musicVolume;
				PlayerPrefs.SetFloat("musicVolume",musicVolume);
			}else{
				myAudio.MusicVolume = musicVolume;
			}
		}
		myAudio.muteMusicSFX(); //ui sound
	}
}
