using UnityEngine;

public class BrokenKnockBack : MonoBehaviour
{
    public float bumpForce = 20f;
    public float forwardLoss = 0.6f;
    public float lifeTime = 1.5f;


    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("hit knock back");

        CarController car = collision.GetComponent<CarController>();
        if (!car) return;

        OnHit(car);
    }

    public void OnHit(CarController car)
    {
        Vector2 dir = (car.transform.position - transform.position).normalized;
        car.ApplyKnockBack(dir * bumpForce, forwardLoss);
    }

   
}
