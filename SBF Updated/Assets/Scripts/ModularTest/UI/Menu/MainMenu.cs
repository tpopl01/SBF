using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static string level_name = "Scene Geonosis";
    public static GameModeType gameMode = GameModeType.Conquest;
    [SerializeField] GameObject levelSelectPanel = null;

    public void OpenSelectLevel()
    {
        levelSelectPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SelectLevel(string levelName)
    {
        level_name = levelName;
    }

    public void SelectGameMode(int gameModeType)
    {
        gameMode = (GameModeType)gameModeType;
    }

    public void Confirm()
    {
        SceneManager.LoadScene(level_name);
    }
}
