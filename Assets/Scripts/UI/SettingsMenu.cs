using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class SettingsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private static readonly string FullScreenPref = "FullScreenPref";
    private static readonly string GraphicsPref = "GraphicsPref";
    private static readonly string ResolutionPref = "ResolutionPref";
    private int firstPlayInt;
    public Toggle toggleFullscreen;
    public Dropdown resolutionDropdown;
    public Slider musicSlider, soundEffectsSlider;
    public Text label_graphics;
    public Dropdown dropwdown_graphics;
    private float musicFloat, soundEffectsFloat;
    private int is_FullScreenOn;
    private int qualityIndex;
    public AudioMixer mixer;
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;

        for(int i = 0; i<resolutions.Length;i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
        
        //FIRST PLAY
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);
        if (firstPlayInt == 0)
        {
            //initial values
            is_FullScreenOn = 0;
            qualityIndex = 2;
            label_graphics.text = dropwdown_graphics.options.ElementAt(qualityIndex).text;
            toggleFullscreen.isOn = false;
            musicFloat = .125f;
            soundEffectsFloat = .75f;
            musicSlider.value = musicFloat;
            soundEffectsSlider.value = soundEffectsFloat;
            PlayerPrefs.SetFloat(MusicPref, musicFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
            PlayerPrefs.SetInt(FullScreenPref, is_FullScreenOn);
            PlayerPrefs.SetInt(GraphicsPref, qualityIndex);
        }
        else //AFTER FIRST PLAY 
        {
            //setting parameters based on prefs
            is_FullScreenOn = PlayerPrefs.GetInt(FullScreenPref);
            if (is_FullScreenOn==0)
            {
                toggleFullscreen.isOn = false;
                SetFullScreen(false);
            }
            else
            {
                toggleFullscreen.isOn = true;
                SetFullScreen(true);
            }
            qualityIndex = PlayerPrefs.GetInt(GraphicsPref);
            dropwdown_graphics.value = qualityIndex;
            label_graphics.text = dropwdown_graphics.options.ElementAt(qualityIndex).text;
            SetQuality(qualityIndex);
            musicFloat = PlayerPrefs.GetFloat(MusicPref);
            musicSlider.value = musicFloat;
            SetMusicVolumeLevel(musicFloat);
            soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            soundEffectsSlider.value = soundEffectsFloat;
            SetSFXVolumeLevel(soundEffectsFloat);
        }

    }
    public void SetQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt(GraphicsPref, qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if (isFullscreen)
        {
            PlayerPrefs.SetInt(FullScreenPref,1);
        }
        else
        {
            PlayerPrefs.SetInt(FullScreenPref,0);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }
    
    public void SaveGraphicsSettings()
    {
        PlayerPrefs.SetInt(FullScreenPref, is_FullScreenOn);
        PlayerPrefs.SetInt(GraphicsPref, qualityIndex);
    }

    public void SaveSettings()
    {
        SaveSoundSettings();
        SaveGraphicsSettings();
    }
    
    private void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            if (gameObject.activeSelf == true)
            {
                SaveSettings(); //save values if no focus (reduce app or go on another window)
            }
        }
    }

    public void SetMusicVolumeLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat(MusicPref, sliderValue);
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
    
    public void SetSFXVolumeLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat(SoundEffectsPref, sliderValue);
        mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }

}
