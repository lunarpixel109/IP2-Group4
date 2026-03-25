using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class PlayerIconSwitcher : MonoBehaviour
{
    public Sprite[] PlayerIcon;
    public GameObject CurrentPlayerIcon;
    public GameObject BoostPad;

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

            case "OilSpill":
                Debug.Log("Oil");
                ShowIcon(3);
                break;

            case "Booster":
                Debug.Log("Boosted");
                ShowIcon(0); 
                break;

            default:
            
                Debug.Log("Hit something who knows what");
                break;
        }
    }
    private void ShowIcon(int spriteIndex)
    {
        CurrentPlayerIcon.GetComponent<SpriteRenderer>().sprite = PlayerIcon[spriteIndex];

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
