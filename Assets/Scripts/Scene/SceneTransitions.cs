using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    // when setting up references just drag the canvases of the track map, timer and player icon in 

    public Animator TransitionAnim1;
    public Animator TransitionAnim2;

    public GameObject TrackMap;
    public GameObject PlayerIcon;
    public GameObject Timer;

    public string SceneName;

    private void Start()
    {
        TrackMap.SetActive(false);
        PlayerIcon.SetActive(false);
        Timer.SetActive(false);

        StartCoroutine(HideUI());
    }
    public IEnumerator LoadScene()
    {
        TransitionAnim1.SetTrigger("End");
        TransitionAnim2.SetTrigger("End");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneName);
    }
    public IEnumerator HideUI()
    {
        yield return new WaitForSeconds(1f);
    
        TrackMap.SetActive(true);
        PlayerIcon.SetActive(true);
        Timer.SetActive(true);

    }

}

