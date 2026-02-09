using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class driving : MonoBehaviour
{
	float speed;
	float direction;
	public float accel;
	public float braking;

	InputAction accelerate;
	InputAction brake;
	InputAction steering;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		accelerate = InputSystem.actions.FindAction("Accelerate");
		brake = InputSystem.actions.FindAction("Brake");
		steering = InputSystem.actions.FindAction("Steering");
	
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

		speed *= 0.95f;

		if (math.abs(speed) < 0.0001) { speed = 0; }

		print(speed);
		//if (steering.IsPressed() == true)

		transform.position.x += math.sin(speed)*Time.deltaTime;
	}
}
