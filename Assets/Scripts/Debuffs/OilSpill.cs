using UnityEngine;

using System.Collections;
using UnityEngine.Serialization;


public class OilSpill : MonoBehaviour

{

    public float duration = 2.0f;
    public float slowMaxSpeed = 7f;
    public float minCrawlSpeed = 3f;

    [FormerlySerializedAs("trailRendererPrefab1")] public GameObject trailRenderer1;
    [FormerlySerializedAs("trailRendererPrefab2")] public GameObject trailRenderer2;
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
            car.ApplyBoost(duration, slowMaxSpeed, minCrawlSpeed);
            
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
        if (trailRenderer1 != null)
            SpawnSingleTrail(trailRenderer1);

        if (trailRenderer2 != null)
            SpawnSingleTrail(trailRenderer2);
    }

    void SpawnSingleTrail(GameObject trailRenderer)

    {
        //GameObject trailObj = Instantiate(prefab, anchor.position, anchor.rotation);

        //trailObj.transform.SetParent(anchor);

        TrailRenderer tr = trailRenderer.GetComponent<TrailRenderer>();

        if (tr != null)
        {
            tr.emitting = true;
            StartCoroutine(StopTrail(trailRenderer, tr));
        }
    }

    IEnumerator StopTrail(GameObject trailObj, TrailRenderer tr)

    {
        yield return new WaitForSeconds(trailDuration);

        if (trailObj != null && tr != null)
        {
            tr.emitting = false;
            //trailObj.transform.SetParent(null);

            //Destroy(trailObj, tr.time);
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