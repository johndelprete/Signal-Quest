using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SettingsController : MonoBehaviour
{
    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    private const string VolumeKey = "MasterVolume";

    void Start () {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>(); //need strings because AddOptions only accepts strings as params

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && 
                resolutions[i].height == Screen.height) 
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        LoadMixerVolume("MasterVolume", "SavedMasterVolume", masterVolumeSlider);
        LoadMixerVolume("BGMVolume", "SavedBGMVolume", bgmVolumeSlider);
        LoadMixerVolume("SFXVolume", "SavedSFXVolume", sfxVolumeSlider);
    }

    public void SetResolution (int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Back() {
        if (SettingsState.openedFromGame) {
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        SceneManager.UnloadSceneAsync("Settings");
    }

    public AudioMixer audioMixer;

    public void SetMasterVolume(float volume)
{
    SetMixerVolume("MasterVolume", "SavedMasterVolume", volume);
}

public void SetBGMVolume(float volume)
{
    SetMixerVolume("BGMVolume", "SavedBGMVolume", volume);
}

public void SetSFXVolume(float volume)
{
    SetMixerVolume("SFXVolume", "SavedSFXVolume", volume);
}

void SetMixerVolume(string mixerParameter, string saveKey, float volume)
{
    float safeVolume = Mathf.Clamp(volume, 0.0001f, 1f);

    float decibels = Mathf.Log10(safeVolume) * 20f;
    audioMixer.SetFloat(mixerParameter, decibels);

    PlayerPrefs.SetFloat(saveKey, safeVolume);
    PlayerPrefs.Save();
}

    public void SetFullscreen (bool isFullscreen) {
        Resolution resolution = resolutions[resolutionDropdown.value];

        Screen.SetResolution(resolution.width, resolution.height, isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }

    void OnEnable()
    {
        SettingsState.isOpen = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnDisable()
    {
        SettingsState.isOpen = false;
    }

    void LoadMixerVolume(
        string mixerParameter,
        string saveKey,
        Slider slider
    )
    {
        float savedVolume = PlayerPrefs.GetFloat(saveKey, 1f);

        slider.SetValueWithoutNotify(savedVolume);

        float decibels = Mathf.Log10(savedVolume) * 20f;
        audioMixer.SetFloat(mixerParameter, decibels);
    }
}
