using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class driving : MonoBehaviour
{
	float speed;
	float direction;
	public float accel;
	public float braking;
	public float friction;

	enum state { 
	
		stationary,
		forward,
		barckward
	}

	state drivingState;

	InputAction accelerate;
	InputAction brake;
	InputAction steering;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		accelerate = InputSystem.actions.FindAction("Accelerate");
		brake = InputSystem.actions.FindAction("Brake");
		steering = InputSystem.actions.FindAction("Steering");

		direction = transform.rotation.eulerAngles.z;
	
	}

	// Update is called once per frame
	void Update()
	{
		if (accelerate.IsPressed() == true)
		{
			speed += accel * Time.deltaTime;
			print("accel " + accelerate.IsPressed());
		}
		if (brake.IsPressed() == true)
		{
			speed -= braking * Time.deltaTime;
			print("breaking " + brake.IsPressed());
		}

        if (speed > 0)
        {
            drivingState = state.forward;
			speed -= friction;
			if (speed < 0) {speed = 0;}
        }
        else if (speed < 0)
        {
            drivingState = state.barckward;
			speed += friction;
			if (speed > 0) { speed = 0;}
        }
        else if (speed == 0)
        {
            drivingState = state.stationary;
        }

		

        if (math.abs(speed) < 0.0001) { speed = 0; }

		print(speed);
		//if (steering.IsPressed() == true)

		transform.position += new Vector3(speed * math.sin(direction)*Time.deltaTime, speed * math.cos(direction)*Time.deltaTime);
	}
}
