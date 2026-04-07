using UnityEngine;
using UnityEngine.EventSystems;

public class MenuToggle : MonoBehaviour
{

    public GameObject pauseCanvas;
    public GameObject firstButton;
    public GameObject timerStuff;

    void Start()
    {
        pauseCanvas.SetActive(false);
        
    }




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.P))
        {
            if (pauseCanvas.activeSelf)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }

        }
    }



    public void pauseGame()
    {
        pauseCanvas.SetActive(true);
        timerStuff.SetActive(false);

        Time.timeScale = 0f;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void resumeGame()
    {
        pauseCanvas.SetActive(false);
        timerStuff.SetActive(true);

        Time.timeScale = 1f;
    }



   


}
