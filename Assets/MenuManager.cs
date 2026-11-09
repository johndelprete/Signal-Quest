using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameObject SettingsMenu;
    public AudioSource gameAudio;

    private bool paused;

    void Start()
    {
        SettingsMenu.SetActive(false);
    }

    void Update()
    {
        if (SettingsState.isOpen) {
            return;
        }

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {

            if (paused)
                Resume();
            else
                Pause();
        }
    }

    void Pause()
    {
        SettingsMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (gameAudio != null)
            gameAudio.Pause();

        Time.timeScale = 0f;
        paused = true;
    }

    public void Resume()
    {
        SettingsMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (gameAudio != null)
            gameAudio.UnPause();

        Time.timeScale = 1f;
        paused = false;
    }
}