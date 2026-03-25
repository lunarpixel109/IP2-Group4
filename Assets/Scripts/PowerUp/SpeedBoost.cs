using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public class SpeedBoost : MonoBehaviour
{
     public float speedIncrease = 2f;
     public float duration = 2f;

    [Header("post processing effects")]
    public GameObject pp_GO; // used to get refrence to post processing game object in scene
    private Volume pp_volume; // used to get resfrence to the volume component of the post processing object
    [Range(0, 1)]
    public float max_chromatic_value;
    float elapsed_time = 0; // used to get the time since the effect started
    public ChromaticAberration CA_adjustment; // used for adjusting chromatic aberration settings

    public bool isActive = false;

    private void Start()
    {
        pp_GO = GameObject.FindWithTag("Post-processing");
        pp_volume = pp_GO.GetComponent<Volume>();
    }

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

        car.ApplyBoost(speedIncrease, duration);

        elapsed_time += Time.deltaTime; 
        float percentage_complete = elapsed_time / duration;
        pp_volume.profile.TryGet<ChromaticAberration>(out CA_adjustment); // gets the chromatic aberration settings of the profile

        CA_adjustment.intensity.value = Mathf.Lerp(max_chromatic_value, 0, percentage_complete);

        yield return new WaitForSeconds(duration);

        isActive = false;
    }
}
