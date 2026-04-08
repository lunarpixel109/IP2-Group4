using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour
{

    public Button playButton;
    public Button leaderboardButton;
    public Button settingsButton;
    
    public Animator buttonAnimator;

    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        leaderboardButton.onClick.AddListener(LeaderboardButton);
        settingsButton.onClick.AddListener(SettingsButton);

        StartCoroutine(WaitForAnimFinish());
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void LeaderboardButton()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void SettingsButton()
    {
        ExitButtons();
        SceneManager.LoadScene("settings menu", LoadSceneMode.Additive);
    }

    IEnumerator WaitForAnimFinish()
    {
        yield return new WaitForSeconds(1f);
        playButton.Select();
    }

    public void EnterButtons()
    {
        buttonAnimator.Play("Enter");
    }

    public void ExitButtons()
    {
        buttonAnimator.Play("Exit");
    }

}
