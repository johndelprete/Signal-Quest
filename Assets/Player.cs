using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int level = 1;
    public int health = 10;

    IEnumerator Start()
    {
        if (LoadManager.loadSavedGame)
        {
            yield return null; // Wait one frame for map/player setup.

            LoadPlayer();
            LoadManager.loadSavedGame = false;
        }
    }

    public void SavePlayer () {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer () {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null) {
            return;
        }

        level = data.level;
        health = data.health;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        Debug.Log("Loading player to: " + position);
        transform.position = position;
    }
}
