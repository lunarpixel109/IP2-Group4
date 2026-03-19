using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static CarController;

public class trail_script : MonoBehaviour
{

    InputAction brake;
    private TrailRenderer trail;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brake = InputSystem.actions.FindAction("Brake");
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (brake.IsPressed())
        {
            trail.emitting = true;
        } else
        {
            trail.emitting = false;
        }
    }

    void Braking()
    {

    }
}
