using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static CarController;

public class trail_script : MonoBehaviour
{
    public bool boosting = false;

    [SerializeField] private TrailRenderer[] brake_trails;
    [SerializeField] private TrailRenderer[] Boost_trails;

    private CarController car_ref;

    InputAction brake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        car_ref = GetComponentInParent<CarController>(); 
        brake = InputSystem.actions.FindAction("Brake");
        foreach (var trail in brake_trails)
        {
            trail.emitting = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(car_ref.boost);
        if (brake.IsPressed())
        {
            foreach (var trail in brake_trails)
            {
                trail.emitting = true;
            }
        } else
        {
            foreach (var trail in brake_trails)
            {
                trail.emitting = false;
            }
        }

        if (car_ref.boost > 0f && !car_ref.is_drifting)
        {
            foreach (var trail in Boost_trails)
            {
                trail.emitting = true;
            }
        } else
        {
            foreach (var trail in Boost_trails)
            {
                trail.emitting = false;
            }
        }

    }

    void Braking()
    {

    }
}
