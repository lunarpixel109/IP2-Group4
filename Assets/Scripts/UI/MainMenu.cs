using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour
{

    public Button playButton;
    public Button leaderboardButton;
    public Button settingsButton;
    public Button quitButton;
    [Space]
    public Animator buttonAnimator;
    public Animator quitAnimator;

    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        leaderboardButton.onClick.AddListener(LeaderboardButton);
        settingsButton.onClick.AddListener(SettingsButton);
        quitButton.onClick.AddListener(QuitButton);
        
        EnterButtons();
        StartCoroutine(WaitForAnimFinish());
    }


    public void PlayGame()
    {
        ExitButtons();
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void LeaderboardButton()
    {
        ExitButtons();
        SceneManager.LoadScene("LeaderboardScene", LoadSceneMode.Additive);
    }

    public void SettingsButton()
    {
        ExitButtons();
        SceneManager.LoadScene("settings menu", LoadSceneMode.Additive);
    }

    void QuitButton()
    {
        Application.Quit();
    }

    IEnumerator WaitForAnimFinish()
    {
        yield return new WaitForSeconds(1f);
        playButton.Select();
    }

    public void EnterButtons()
    {
        buttonAnimator.Play("Enter");
        quitAnimator.Play("QuitEnter");
        StartCoroutine(WaitForAnimFinish());
    }

    public void ExitButtons()
    {
        buttonAnimator.Play("Exit");
        quitAnimator.Play("QuitExit");
    }

}
