using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
	public float accel;
	public float braking;
	public float friction;

	public float max_speed;

	public float steering_speed;

	InputAction accelerate;
	InputAction brake;
	InputAction steering;
	InputAction drifting;

	public enum DrivingState
	{
		stationary,
		forward,
		barckward
	}

	Rigidbody2D rb;

	public Vector2 rb_speed_local;
	public float rb_speed_forward;
	public float rb_speed_right;

	float rb_direction;

	public DrivingState drivingState = DrivingState.stationary;
	bool is_drifting = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		accelerate = InputSystem.actions.FindAction("Accelerate");
		brake = InputSystem.actions.FindAction("Brake");
		steering = InputSystem.actions.FindAction("Steering");

		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		#region input
		rb_speed_local = rb.GetVector(rb.linearVelocity); // gets the speed of the car as a vector in global space and turns it into a vector in local space

		rb_speed_forward = rb_speed_local[1]; // splits local space vector into forward component and right component
		rb_speed_right = rb_speed_local[0];

		rb_direction = rb.rotation; // gets rotation of car via rigidbody2D

		//if (drifting.IsPressed() && rb_speed_forward >= 300) { is_drifting = true; }
		#endregion

		#region parallel movement

		// iffs decide which forces apply to car each frame
		if (drivingState == DrivingState.stationary) // stationary == no forces
		{
			if (accelerate.IsPressed()) { drivingState = DrivingState.forward; }
			else if (brake.IsPressed()) { drivingState = DrivingState.barckward; }
		}
		else if (drivingState == DrivingState.forward)
		{
			if (accelerate.IsPressed())
			{
				rb_speed_forward += (accel - (rb_speed_forward / 10)) * Time.fixedDeltaTime; // increments forward speed by appropriate value based on input and context
				if (rb_speed_forward > max_speed) { rb_speed_forward = max_speed; }
				print("accel " + accelerate.IsPressed());
			}
			else if (brake.IsPressed())
			{
				rb_speed_forward -= braking * Time.fixedDeltaTime;
				if (rb_speed_forward < 0) { Momentum_Change_Stationary(); }
				print("breaking " + brake.IsPressed());
			}
			else
			{
				rb_speed_forward -= friction * Time.fixedDeltaTime;
				if (rb_speed_forward < 0) { Momentum_Change_Stationary(); }
			}
		}
		else if (drivingState == DrivingState.barckward)
		{
			if (brake.IsPressed())
			{
				rb_speed_forward -= (accel / 2 + rb_speed_forward / 10) * Time.fixedDeltaTime;
				print("reversing " + brake.IsPressed());
			}
			else if (accelerate.IsPressed())
			{
				rb_speed_forward += braking * Time.fixedDeltaTime;
				if (rb_speed_forward > 0) { Momentum_Change_Stationary(); }
				print("R breaking " + accelerate.IsPressed());
			}
			else
			{
				rb_speed_forward += friction * Time.fixedDeltaTime;
				if (rb_speed_forward > 0) { Momentum_Change_Stationary(); }
			}
		}
		#endregion

		#region perpendicular movement
		rb_speed_right *= 0.5f; // always lowers right/left speed of car (will not effect drift dont worry)
		if (math.abs(rb_speed_right) < 0.05) rb_speed_right = 0;
		#endregion

		#region steering
		rb.angularVelocity = 0f;
		if (steering.IsPressed())
		{
			if (drivingState == DrivingState.forward)
			{
				rb_direction -= steering.ReadValue<float>() * Steering_Speed_Curve() * Time.fixedDeltaTime;
			}
			else if (drivingState == DrivingState.barckward)
			{
				rb_direction += steering.ReadValue<float>() * Steering_Speed_Curve() * Time.fixedDeltaTime;
			}

			print("steering" + steering.ReadValue<float>());
		}
		#endregion

		#region output

		rb_speed_local = new Vector2(rb_speed_right, rb_speed_forward); // recombines forward and right vectors
		rb.linearVelocity = rb.GetRelativeVector(rb_speed_local); // sets speed in global space based on speed in local space - NOT WORKING IDK WHY

		rb.rotation = rb_direction;
		#endregion
	}

	void Momentum_Change_Stationary()
	{
		rb_speed_forward = 0;
		drivingState = DrivingState.stationary;
	}

	float Steering_Speed_Curve()
	{
		return steering_speed;
	}
}
