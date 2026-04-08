using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class LogoFlicker : MonoBehaviour
{
    public Image logoImage; 
    public Sprite logoOff;
    public Sprite logoOn;

    public int flickerCount = 10;
    public float minDelay = 0.03f;
    public float maxDelay = 0.15f;

    void Start()
    {
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        logoImage.sprite = logoOff;

        for (int i = 0; i < flickerCount; i++)
        {
            // turn ON
            logoImage.sprite = logoOn;
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            // turn OFF
            logoImage.sprite = logoOff;
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }

        // final ON
        logoImage.sprite = logoOn;
    }
}