using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    private CarController car;
    
    [Header("UI")] 
    public GameObject pausePanel;
    [Space]
    public Button resumeButton;
    public Button optionsButton;
    public Button quitButton;

    [Header("Other Settings")] 
    public Animator anim;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        car = FindObjectOfType<CarController>();
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        InputSystem.actions.FindAction("Pause").performed += _ => TogglePause();
        
        resumeButton.onClick.AddListener(ResumeButton);
        quitButton.onClick.AddListener(QuitButton);
        optionsButton.onClick.AddListener(OptionsButton);
    }

    void TogglePause()
    {
        if (isPaused)
            //StartCoroutine(ResumeButton());
            ResumeButton();
        else
            //StartCoroutine(PauseGame());
            PauseGame();
    }

    // Update is called once per frame
    void PauseGame()
    {
        anim.Play("PauseEnter");
        isPaused = true;
        resumeButton.Select();
        Time.timeScale = 0;
    }

    void ResumeButton()
    {
        Time.timeScale = 1;
        anim.Play("PauseExit");
       // yield return new WaitUntil(() => { return Mathf.Approximately(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1); });
        isPaused = false;
        
    }

    void OptionsButton()
    {
        SceneManager.LoadSceneAsync("Scenes/Menu/settings menu", LoadSceneMode.Additive);
    }

    void QuitButton()
    {
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }
    
}
