using UnityEngine;

public class StartFinishLine : MonoBehaviour
{
    private bool canTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTrigger)
            return;

        if (other.CompareTag("Player"))
        {
            LapTimerManager.instance.OnStartFinishCrossed();
            StartCoroutine(TriggerCooldown());
        }
    }

    private System.Collections.IEnumerator TriggerCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(1f); // prevent double counting
        canTrigger = true;
    }
}