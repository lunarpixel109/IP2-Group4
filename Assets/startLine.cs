using UnityEngine;

public class StartFinishLine : MonoBehaviour
{
    private bool canTrigger = true;             // prevents the trigger going off to many times

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Start/Finish line trigger entered by: " + other.name);
        // if on cooldown ignore the collision
        if (!canTrigger)
            return;
        
            LapTimerManager.instance.OnStartFinishCrossed();
            StartCoroutine(TriggerCooldown());
    }

    // cooldown to stop multiple triggers
    private System.Collections.IEnumerator TriggerCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(1f); // prevent double counting
        canTrigger = true;
    }
}