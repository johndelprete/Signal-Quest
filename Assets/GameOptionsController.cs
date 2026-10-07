using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOptionsController : MonoBehaviour
{
    public void OpenSettings() {
        Time.timeScale = 0f;
        SettingsState.openedFromGame = true;
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void QuitToMainMenu () {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
