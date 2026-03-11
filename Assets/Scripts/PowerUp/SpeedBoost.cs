using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float SpeedIncrease = 1f;
    public float duration = 2f;
    public bool OnSpeedPad = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Find the Car
        CarController car = collision.GetComponent<CarController>();
        if (!car) return;

        Boosted(car);

        OnSpeedPad = true;

    }
    public void Boosted(CarController car)
    {
        //car.ApplySpeedMultiplier(SpeedIncrease, duration);

    }

    void Update()
    {
        
    }
}
