using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public bool keepAlive;
    public AudioClip gemSound;

    bool musicOn;
    bool soundOn;
    float soundVolume = 1.0f;
    AudioSource src;

	void Awake() {
        src = GetComponent<AudioSource>();

        if (keepAlive){
            DontDestroyOnLoad(gameObject);
        }

        if(PlayerPrefs.HasKey("MusicOn") && PlayerPrefs.GetInt("MusicOn") == 1){
            musicOn = false;
        }

        if(PlayerPrefs.HasKey("SoundOn") && PlayerPrefs.GetInt("SoundOn") == 1){
            soundOn = false;
        }

        ToggleMusic();
        ToggleSound();
	}

    public void ToggleMusic(){
        musicOn = !musicOn;
        PlayerPrefs.SetInt("MusicOn",musicOn ? 1 : 0);
        if(musicOn){
            src.Play();
        } else {
            src.Stop();
        }
    }

    public void ToggleSound(){
        soundOn = !soundOn;
        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
        if (soundOn)
        {
            soundVolume = 1.0f;
        }
        else {
            soundVolume = 0.0f;
        }
    }

    public void PlayGemSound()
    {
        src.PlayOneShot(gemSound, soundVolume);
    }

    public bool GetMusicState(){
        return musicOn;
    }

    public bool GetSoundState(){
        return soundOn;
    }
	
}
