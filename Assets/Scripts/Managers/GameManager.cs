using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    InputAction pause;
    private bool is_paused = false;
    private bool is_pausable = true;
    [SerializeField]private float timer_max;

    private void Awake()
    {
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pause = InputSystem.actions.FindAction("Pause");
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.IsPressed() && is_pausable)
        {
            is_pausable = false;
            StartCoroutine(Timer(timer_max));
            is_paused = !is_paused;
            PauseMenu();
        }
    } 

    public void PauseMenu()
    {
        if (is_paused)
        {
            Time.timeScale = 0f;
            SceneManager.LoadScene("settings menu", LoadSceneMode.Additive);
        }

        if (!is_paused)
        {
            Time.timeScale = 1f;
            try
            {
                SceneManager.UnloadScene("settings menu");
            }
            catch
            {

            }

        }
    }

    IEnumerator Timer(float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        is_pausable = true;
    }

}
