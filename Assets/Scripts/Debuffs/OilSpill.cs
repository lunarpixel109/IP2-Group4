using UnityEngine;
using System.Collections;

public class OilSpill : MonoBehaviour
{
    public float duration = 2.0f;
    public float slowMaxSpeed = 7f;
    public float minCrawlSpeed = 3f;

    public GameObject trailRendererPrefab1;
    public GameObject trailRendererPrefab2;
    public float trailDuration = 2.5f; 

    public Transform leftTireAnchor;
    public Transform rightTireAnchor;

    private bool hitSpill = false; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitSpill) return;

        CarController car = collision.GetComponentInParent<CarController>();

        if (car != null)
        {
            hitSpill = true;

            car.ApplyBoost(duration, slowMaxSpeed, 15f);
            StartCoroutine(PreventCompleteStop(car));

            StartTireTrails();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CarController car = collision.GetComponentInParent<CarController>();
        if (car != null)
        {
            hitSpill = false;
        }
    }

    void StartTireTrails()
    {
        if (leftTireAnchor != null && trailRendererPrefab1 != null)
            SpawnSingleTrail(leftTireAnchor, trailRendererPrefab1);

        if (rightTireAnchor != null && trailRendererPrefab2 != null)
            SpawnSingleTrail(rightTireAnchor, trailRendererPrefab2);
    }

    void SpawnSingleTrail(Transform anchor, GameObject prefab)
    {
        GameObject trailObj = Instantiate(prefab, anchor.position, anchor.rotation);
        trailObj.transform.SetParent(anchor);

        TrailRenderer tr = trailObj.GetComponent<TrailRenderer>();
        if (tr != null)
        {
            tr.emitting = true;
            StartCoroutine(StopTrail(trailObj, tr));
        }
    }

    IEnumerator StopTrail(GameObject trailObj, TrailRenderer tr)
    {
        yield return new WaitForSeconds(trailDuration);

        if (trailObj != null && tr != null)
        {
            tr.emitting = false;
            trailObj.transform.SetParent(null);

            Destroy(trailObj, tr.time);
        }
    }

    IEnumerator PreventCompleteStop(CarController car)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            if (car.rb_speed_forward < minCrawlSpeed)
                car.rb_speed_forward = minCrawlSpeed;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}