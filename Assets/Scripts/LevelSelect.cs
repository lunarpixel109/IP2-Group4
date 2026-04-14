using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{

    public Button button;
    
    private void Start()
    {
        button.Select();
    }

    public void Easy()
    {
        SceneManager.LoadScene("Scenes/Tracks/Easy Track");
    }

    public void Medium()
    {
        SceneManager.LoadScene("Scenes/Tracks/Medium Track");
    }

    public void Hard()
    {
        SceneManager.LoadScene("Scenes/Tracks/Hard Track");
    }
    public void MainMenu()
    {
        FindFirstObjectByType<MainMenu>().EnterButtons();
        SceneManager.UnloadSceneAsync("Level Select");
    }
}
