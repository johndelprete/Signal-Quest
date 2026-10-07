using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Play() {
        SceneManager.LoadScene("Game");
    }

    public void Settings() {
        SettingsState.openedFromGame = false;
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void Menu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit() {
        Application.Quit();
    }

    public void newGame() {
        LoadManager.loadSavedGame = false;
        SceneManager.LoadScene("Game");
    }

    public void loadGame() {
        LoadManager.loadSavedGame = true;
        SceneManager.LoadScene("Game");
    }
}
