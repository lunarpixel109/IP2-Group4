using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
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
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
