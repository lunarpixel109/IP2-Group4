using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PlayerIconSwitcher : MonoBehaviour
{
    public static PlayerIconSwitcher instance;

    public Sprite[] PlayerIcon;
    public GameObject CurrentPlayerIcon;

    public bool isActive = false;
    public float duration = 1.0f;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        CurrentPlayerIcon.GetComponent<Image>().sprite = PlayerIcon[4];

        TryGetComponent<LapTimerManager>(out var BestTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleIcon(collision.gameObject.tag);
    }
    private void OnTriggerEnter2D(Collider2D Other)
    {
        HandleIcon(Other.gameObject.tag);
    }
    private void HandleIcon(string tag)
    {
        Debug.Log("Hit " + tag);

        switch (tag)
        {
            case "Wall":
                ShowIcon(2);
                break;

            case "Obstacle":
                ShowIcon(2);
                break;

            case "BoosterPad":
                ShowIcon(0);
                break;

            default:

                Debug.Log("Hit something??? who knows what");
                break;
        }
    }

    public void ShowBestTimeIcon()
    {
        ShowIcon(1);
    }
    public void ShowSadIcon()
    {
        ShowIcon(3);
    }
    public void ShowDeterminedIcon()
    {
        ShowIcon(5);
    }

    private void ShowIcon(int spriteIndex)
    {
        CurrentPlayerIcon.GetComponent<Image>().sprite = PlayerIcon[spriteIndex];

        //make sure there isnt overlap overlap
        // Only in this script
       // StopAllCoroutines();

        StartCoroutine(IconDisplayTime());
    }

    public IEnumerator IconDisplayTime()
    {
        //CurrentPlayerIcon.SetActive(true);

        yield return new WaitForSeconds(duration);

        CurrentPlayerIcon.GetComponent<Image>().sprite = PlayerIcon[4];
    }

}
