using UnityEngine;

public class StartFinishLine : MonoBehaviour
{
    private bool canTrigger = true;             // prevents the trigger going off to many times

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if on cooldown ignore the collision
        if (!canTrigger)
            return;

        // only respond to the player crossing the line
        if (other.CompareTag("Player"))
        {
            LapTimerManager.instance.OnStartFinishCrossed();
            StartCoroutine(TriggerCooldown());
        }
    }

    // cooldown to stop multiple triggers
    private System.Collections.IEnumerator TriggerCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(1f); // prevent double counting
        canTrigger = true;
    }
}