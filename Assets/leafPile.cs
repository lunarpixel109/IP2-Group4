using UnityEngine;
using System.Collections;

public class leafPile : MonoBehaviour
{
    public ParticleSystem leafBurstEffect;
    public float trailDuration = 1f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ParticleSystem burst = Instantiate(leafBurstEffect, transform.position, Quaternion.identity);
            burst.Play();
            

            var trail = collision.GetComponentInParent<LeafTrailController>();
            if (trail != null)
            {
                trail.PlayTrail(trailDuration);
            }

            gameObject.SetActive(false);
        }
    }
}
