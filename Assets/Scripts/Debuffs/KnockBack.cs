using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float bumpForce = 6f;
    public float forwardLoss = 0.6f;

    public GameObject brokenPrefab;
    public float breakForce = 5f;
    public float torqueForce = 5f;
    public float coneAngle = 40f;

    private bool destroyed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyed) return;

        Debug.Log("hit knock back");

        CarController car = collision.GetComponent<CarController>();
        if (!car) return;

        OnHit(car);
    }


    public void OnHit(CarController car)
    {
        destroyed = true;

        Vector2 dir = (car.transform.position - transform.position).normalized;
        car.ApplyKnockBack(dir * bumpForce, forwardLoss);

        BreakObject(car);
    }


    Vector2 GetConeDirection(Vector2 baseDir, float maxAngle)
    {
        float angle = Random.Range(-maxAngle, maxAngle);
        return Quaternion.Euler(0, 0, angle) * baseDir;
    }


    void BreakObject(CarController car)
    {
        Vector2 awayFromPlayer = (transform.position - car.transform.position).normalized;

        GameObject broken = Instantiate(brokenPrefab, transform.position, transform.rotation);

        foreach (Rigidbody2D rb in broken.GetComponentsInChildren<Rigidbody2D>())
        {
            Vector2 dir = GetConeDirection(awayFromPlayer, coneAngle);

            rb.AddForce(dir * breakForce, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-torqueForce, torqueForce), ForceMode2D.Impulse);
        }


        Destroy(gameObject, 0.025f);
    }

    
}
