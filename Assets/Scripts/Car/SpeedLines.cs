using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem speedLines;
    private CarController carController;

    [Header("Settings")]
    public float thresholdSpeed = 10f;
    public float emissionMultiplier = 2f;

    private void Start()
    {
        carController = GetComponent<CarController>();

        if (speedLines != null)
        {
            var main = speedLines.main;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;

            main.startSpeed = 0f;
        }
    }

        public void Update()
        {
            if (carController == null || speedLines == null) return;

            float currentForwardSpeed = Mathf.Abs(carController.rb_speed_forward);

            var emission = speedLines.emission;

            if (currentForwardSpeed > thresholdSpeed)
            {
                emission.enabled = true;
                emission.rateOverTime = currentForwardSpeed * emissionMultiplier;
            }
            else
            {
                emission.enabled = false;
            }
        }
}
