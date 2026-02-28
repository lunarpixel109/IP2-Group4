using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public Animator TransitionAnim1;
    public Animator TransitionAnim2;

    public GameObject TrackMap;

    public string SceneName;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        TransitionAnim1.SetTrigger("End");
        TransitionAnim2.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneName);

    }

}

