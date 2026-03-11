using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
     public float speedIncrease = 2f;
     public float duration = 2f;

    public bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive) return;

        if (collision.TryGetComponent<CarController>(out var car))
        {
            StartCoroutine(HandleBoost(car));
        }
    }

    private IEnumerator HandleBoost(CarController car)
    {
        isActive = true;

        car.ApplySpeedMultiplier(speedIncrease, duration);


        yield return new WaitForSeconds(duration);

        isActive = false;
    }
}
