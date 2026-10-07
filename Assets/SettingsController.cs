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

    public void SetVolume (float volume) {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen (bool isFullscreen) {
        Resolution resolution = resolutions[resolutionDropdown.value];

        Screen.SetResolution(resolution.width, resolution.height, isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }
}
