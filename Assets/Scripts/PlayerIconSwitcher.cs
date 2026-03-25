using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PlayerIconSwitcher : MonoBehaviour
{
    public Sprite[] PlayerIcon;
    public GameObject CurrentPlayerIcon;

    public bool isActive = false;
    public float duration = 1.0f;    

    public void Start()
    {
        CurrentPlayerIcon.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Wall":
                Debug.Log("Hit wall.");
                ShowIcon(2); 
                break;

            case "Obstacle":
                Debug.Log("Obstacle Hit");
                ShowIcon(3);
                break;

            case "BoosterPad":
                Debug.Log("Boosted");
                ShowIcon(0); 
                break;

            default:
            
                Debug.Log("Hit something??? who knows what");
                break;
        }
    }
    private void ShowIcon(int spriteIndex)
    {
        CurrentPlayerIcon.GetComponent<Image>().sprite = PlayerIcon[spriteIndex];

        // make sure there isnt overlap overlap
        StopAllCoroutines();
        StartCoroutine(IconDisplayTime());
    }

    public IEnumerator IconDisplayTime()
    {
        CurrentPlayerIcon.SetActive(true);

        yield return new WaitForSeconds(duration);

        CurrentPlayerIcon.SetActive(false);
    }

}
