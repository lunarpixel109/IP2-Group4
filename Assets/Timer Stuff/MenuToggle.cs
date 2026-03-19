using UnityEngine;
using UnityEngine.EventSystems;

public class MenuToggle : MonoBehaviour
{
    public GameObject leaderBoardButton;
    public GameObject resumeButton;


    void Start()
    {
        leaderBoardButton.SetActive(false);
        resumeButton.SetActive(false);
    }




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.P))
        {
            if (leaderBoardButton.activeSelf)
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
        leaderBoardButton.SetActive(true);
        resumeButton.SetActive(true);

        Time.timeScale = 0f;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(leaderBoardButton);
    }

    public void resumeGame()
    {
        leaderBoardButton.SetActive(false);
        resumeButton.SetActive(false);

        Time.timeScale = 1f;
    }



   


}
