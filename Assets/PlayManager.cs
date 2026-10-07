using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{

public GameObject PlayMenu;
public Button playButton;

    void Start()
    {
        PlayMenu.SetActive(false);
        playButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        PlayMenu.SetActive(true);
    }
}
