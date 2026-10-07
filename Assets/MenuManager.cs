using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject SettingsMenu;
    //public AudioSource gameAudio;

    private bool paused;

    void Start()
    {
        SettingsMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        //gameAudio.Pause();
        Time.timeScale = 0f;
        paused = true;
    }

    public void Resume()
    {
        SettingsMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //gameAudio.Play();
        Time.timeScale = 1f;
        paused = false;
    }
}