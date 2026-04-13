using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCountdown : MonoBehaviour {
    
    [Header("Controllers")]
    public List<Animator> countdownAnim;
    
    [Header("References")]
    public CarController car;
    public LapTimerManager lapTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartRace()
    { 
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown() {
        car.canMove = false;
       // lapTimer.raceStarted = false;
        for (int i = 0; i < countdownAnim.Count; i++)
        {
            if (i == countdownAnim.Count - 1) {
                car.canMove = true;
                //lapTimer.raceStarted = true;
                AudioManager.PlaySound(Sound_types.COUNTDOWN, 1, 1);
            }
            else {
                AudioManager.PlaySound(Sound_types.COUNTDOWN, 0, 1);
            }

            countdownAnim[i].gameObject.SetActive(true);
            countdownAnim[i].enabled = true;
            yield return new WaitForSeconds(1f);
            countdownAnim[i].gameObject.SetActive(false);
        }
        car.canMove = true;
        //lapTimer.raceStarted = true;
        
    }
    
}
