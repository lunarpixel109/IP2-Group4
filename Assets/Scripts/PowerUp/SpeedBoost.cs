using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    public float duration = 2f;
    public float max_speed_boost = 50f;
    public float boost_accel = 20f;

    public bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var car = collision.GetComponentInParent<CarController>();
        car.ApplyBoost(duration,max_speed_boost,boost_accel);

        AudioManager.PlaySound(Sound_types.BOOST, 0);

        //if (collision.TryGetComponent<CarController>(out var car))
        //{
        //    print("dsnmdbmnsdabnmsdbnmdsbsdabnmasdbndssnbmbnm,bnm,bn");
        //    car.ApplyBoost(duration, max_speed_boost, boost_accel);
        //    //StartCoroutine(HandleBoost(car));
        //}
    }

    private IEnumerator HandleBoost(CarController car)
    {
        isActive = true;

        //car.ApplyBoost(speedIncrease, duration);


        yield return new WaitForSeconds(duration);

        isActive = false;
    }
}
