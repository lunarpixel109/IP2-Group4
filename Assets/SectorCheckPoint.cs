using UnityEngine;

public class SectorCheckPoint : MonoBehaviour
{
    public int sectorIndex;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LapTimerManager.instance.OnSectorCrossed(sectorIndex);
        }
    }
}
