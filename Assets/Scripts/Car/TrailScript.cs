using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static CarController;

public class TrailScript : MonoBehaviour
{
    public bool boosting = false;

    [SerializeField] private TrailRenderer[] brake_trails;
    [SerializeField] private TrailRenderer[] Boost_trails;

    public bool isBoostEffect = false;

    InputAction brake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brake = InputSystem.actions.FindAction("Brake");

        foreach (var trail in brake_trails)
        {
            trail.emitting = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

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

        if (isBoostEffect)
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

    public void Boost(float duration)
    {
        StartCoroutine(BoostTimer(duration));
    }

    IEnumerator BoostTimer(float duration)
    {
        float timer = 0f;
        isBoostEffect = true;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        isBoostEffect = false;
    }    
}
