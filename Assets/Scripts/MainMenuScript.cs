using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayLevels()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString(ConstStrings.CURRENT_SCENE_STRING, "Level1"));
    }

    public void PlayEndless()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString(ConstStrings.ENDLESS_SCENE_STRING, "endlessScene"));
    }
}
