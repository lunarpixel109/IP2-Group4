using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PlayerIconSwitcher : MonoBehaviour
{
    public static PlayerIconSwitcher instance;

    public Sprite[] PlayerIcon;
    public Sprite[] ComicSprite;

    public GameObject CurrentPlayerIcon;
    public GameObject CurrentComicSprite;

    public float duration = 1.0f;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        CurrentPlayerIcon.GetComponent<Image>().sprite = PlayerIcon[4];

        TryGetComponent<LapTimerManager>(out var BestTime);

        CurrentComicSprite.SetActive(false);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleIcon(collision.gameObject.tag);
    }
    private void OnTriggerEnter2D(Collider2D Other)
    {
        HandleIcon(Other.gameObject.tag);
    }

    //Trigger collisions
    private void HandleIcon(string tag)
    {
        Debug.Log("Hit " + tag);

        switch (tag)
        {
            case "Wall":
                ShowPlayerIcon(2);

                int randomComic = Random.Range(0, 2);
                ShowComicIcon(randomComic);

                break;

            case "Oil":
                ShowPlayerIcon(4);
                ShowComicIcon(3);

                break;
            case "Log":
                ShowPlayerIcon(1);
                ShowComicIcon(2);

                break;

            case "BoosterPad":
                ShowPlayerIcon(0);
                ShowComicIcon(4);

                break;

            default:

                Debug.Log("Hit something??? who knows what");
                break;
        }
    }

    //Other collisions

    public void ShowBestTimeIcon()
        // when player gets a best lap ( and overall time - need to add )
    {
        ShowPlayerIcon(1);
    }
    public void ShowSadIcon()
    {
        // when player does a really slow lap in comparrison to their best lap

        ShowPlayerIcon(3);
    }
    public void ShowDeterminedIcon()
    {
        // when player doesnt quite beat best time

        ShowPlayerIcon(5);
        ShowComicIcon(4);
    }

    private void ShowPlayerIcon(int spriteIndex)
    {
        CurrentPlayerIcon.GetComponent<Image>().sprite = PlayerIcon[spriteIndex];

        //make sure there isnt overlap overlap
        // Only in this script
       // StopAllCoroutines();

        StartCoroutine(IconDisplayTime());
    }
    private void ShowComicIcon(int spriteIndex)
    {
        SpriteRenderer comicRend = CurrentComicSprite.GetComponent<SpriteRenderer>();

        if (comicRend != null)
        {
            comicRend.sprite = ComicSprite[spriteIndex];

            //CurrentComicSprite.transform.localScale = new Vector3(2f, 2f, 1f);
            //CurrentComicSprite.transform.localPosition = new Vector3(0f, 1.5f, 0f); 

            CurrentComicSprite.transform.localRotation = Quaternion.Euler(0, 0, 45f);
        }

        StartCoroutine(IconDisplayTime());
    }

    public IEnumerator IconDisplayTime()
    {
        CurrentComicSprite.SetActive(true);

        yield return new WaitForSeconds(duration);

        CurrentComicSprite.SetActive(false);

        CurrentPlayerIcon.GetComponent<Image>().sprite = PlayerIcon[4];
    }

}
