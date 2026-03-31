using UnityEngine;
using System.Collections;

public class LeafTrailController : MonoBehaviour
{
    public ParticleSystem leafTrail;

    public void PlayTrail(float duration)
    {
        StartCoroutine(TrailRoutine(duration));
    }


    IEnumerator TrailRoutine(float duration)
    {
        leafTrail.Play();

        yield return new WaitForSeconds(duration);

        leafTrail.Stop();
    }

}
