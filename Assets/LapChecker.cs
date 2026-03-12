using UnityEngine;

public class LapChecker : MonoBehaviour
{
    public static bool fullLap = false;
    private bool canTrigger = true;
    
  


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTrigger)
            return;


        if (collision.CompareTag("Player"))
        {
            fullLap = true;
        }


    }


    private System.Collections.IEnumerator TriggerCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(1f); // prevent double counting
        canTrigger = true;
    }

}
