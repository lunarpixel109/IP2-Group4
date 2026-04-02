using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        SceneManager.LoadScene("Demo");
    }

    public void LeaderBoard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings-Menu");
    }
}
