using UnityEngine;
using System.Collections;
using TMPro;

public class RaceStartCountdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public CarController player;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        player.canMove = false;

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(true);
        

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "GO!";
        player.canMove = true;
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);

       


    }

    
}
