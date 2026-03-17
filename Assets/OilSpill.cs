using UnityEngine;

using System.Collections;

public class OilSpill : MonoBehaviour
{

    public float slowMultiplier = 0.5f;
    public float duration = 2.0f;
    public bool hitSpill = false;

    public float fadeTime = 0.8f;
    public float shrinkAmount = 0.9f;

    
    private SpriteRenderer sr;
    private Collider2D col;
    private Vector3 startScale;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        startScale = transform.localScale;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitSpill) return;

        Debug.Log("hit oil spill");
       

        CarController car = collision.GetComponent<CarController>();
        if (!car) return;

        OnHit(car);
        hitSpill = true;

        col.enabled = false;

        StartCoroutine(FadeAndDestroy());

    }

    public void OnHit(CarController car)
    {
        car.ApplyBoost();
        
    }

    IEnumerator FadeAndDestroy()
    {
        float t = 0f;
        Color startColor = sr.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float lerp = t / fadeTime;

            Color c = startColor;
            c.a = Mathf.Lerp(startColor.a, 0f, lerp);

            transform.localScale = Vector3.Lerp(startScale, startScale * shrinkAmount, lerp);

            yield return null;
        }

        Destroy(gameObject);
    }



}
