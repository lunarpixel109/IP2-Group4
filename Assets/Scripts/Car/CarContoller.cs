using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


public class CarController : MonoBehaviour
{
	public float accel;
	public float braking;
	public float friction;
	public float drift_speed_threshold;

	public float max_speed;

	public float steering_speed;
	public float drift_steering_speed_max;
	public float drift_steering_speeed_min;

	InputAction accelerate;
	InputAction brake;
	InputAction steering;
	InputAction drifting;

	enum DrivingState
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
	int drift_direction;


    DrivingState drivingState = DrivingState.stationary;

	public bool is_drifting = false;
	public float drifting_value = 0f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		accelerate = InputSystem.actions.FindAction("Accelerate");
		brake = InputSystem.actions.FindAction("Brake");
		steering = InputSystem.actions.FindAction("Steering");
		drifting = InputSystem.actions.FindAction("Drifting");

		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		#region input
		rb_speed_local = rb.GetVector(rb.linearVelocity); // gets the speed of the car as a vector in global space and turns it into a vector in local space

		rb_speed_forward = rb_speed_local[1]; // splits local space vector into forward component and right component
		rb_speed_right = rb_speed_local[0];

		if (rb_speed_forward > 0f) { drivingState = DrivingState.forward; }
		else if (rb_speed_forward < 0f) { drivingState = DrivingState.barckward; }
		else { drivingState = DrivingState.stationary; }

		rb_direction = rb.rotation; // gets rotation of car via rigidbody2D

		if (Drift_Check())
		{
			if (!is_drifting)
			{
				print("drift start");
				drift_direction = (int)math.sign(steering.ReadValue<float>());
            }
			drifting_value += 5f * Time.fixedDeltaTime;
			if (drifting_value > 1f)
			{
				drifting_value = 1f;
			}
		}
		else { drifting_value -= 5f * Time.fixedDeltaTime; if (drifting_value < 0f) { drifting_value = 0f; } }

		if (drifting_value > 0f) {is_drifting = true; print("drifting"); }
		else {is_drifting = false; }

		#endregion

		#region parallel movementel
		//normal
		if (!is_drifting)
		{
			// iffs decide which forces apply to car each frame
			if (drivingState == DrivingState.forward)
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
					//if (rb_speed_forward < 0) { Momentum_Change_Stationary(); }
					print("breaking " + brake.IsPressed());
				}
				else
				{
					rb_speed_forward -= friction * Time.fixedDeltaTime;
					//if (rb_speed_forward < 0) { Momentum_Change_Stationary(); }
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
					//if (rb_speed_forward > 0) { Momentum_Change_Stationary(); }
					print("R breaking " + accelerate.IsPressed());
				}
				else
				{
					rb_speed_forward += friction * Time.fixedDeltaTime;
					//if (rb_speed_forward > 0) { Momentum_Change_Stationary(); }
				}
			}
			else if (drivingState == DrivingState.stationary)
			{
				if (accelerate.IsPressed())
				{
					rb_speed_forward += (accel - (rb_speed_forward / 10)) * Time.fixedDeltaTime;
					print("reversing " + brake.IsPressed());
				}
				else if (brake.IsPressed())
				{
					rb_speed_forward -= (accel / 2 + rb_speed_forward / 10) * Time.fixedDeltaTime;
					//if (rb_speed_forward > 0) { Momentum_Change_Stationary(); }
					print("R breaking " + accelerate.IsPressed());
				}
			}
		}
		// drifting
		else
		{
            // rb_speed_forward starts constant
        }
        #endregion

        #region perpendicular movement
        rb_speed_right *= 0.5f; // always lowers right/left speed of car (will not effect drift dont worry)
		if (math.abs(rb_speed_right) < 0.05) rb_speed_right = 0;
		#endregion

		#region steering
		rb.angularVelocity = 0f;
		if (!is_drifting)
		{
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
		}
		else
		{
			rb_direction -= (Mathf.Lerp(drift_steering_speeed_min, drift_steering_speed_max, Mathf.InverseLerp(1 * drift_direction, -1 * drift_direction, steering.ReadValue<float>())) * drift_direction) * Time.fixedDeltaTime;
			print("drifting");
		}
		#endregion

		#region output

		rb_speed_local = new Vector2(rb_speed_right, rb_speed_forward); // recombines forward and right vectors
		rb.linearVelocity = Vector2.ClampMagnitude(rb.GetRelativeVector(rb_speed_local),max_speed); // sets speed in global space based on speed in local space - NOT WORKING IDK WHY

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

	bool Drift_Check()
	{
		if (
			drifting.IsPressed() && drivingState == DrivingState.forward
			&& !brake.IsPressed() && accelerate.IsPressed() &&
			((!is_drifting && steering.IsInProgress()) || is_drifting)
			&& rb_speed_forward >= drift_speed_threshold
			)
		{
			return true;
		}
		else { return false; }
	}
	
	public void ApplySpeedMultiplier(float multiplier, float duration)
	{
		StartCoroutine(SpeedMultiplierRoutine(multiplier, duration));
	}
    
	IEnumerator SpeedMultiplierRoutine(float multiplier, float duration)
	{
		float originalAccel = accel;
		float originalFriction = friction;
		float originalMaxSpeed = max_speed;

		accel *= multiplier;
		friction *= multiplier;
		max_speed *= multiplier;

		yield return new WaitForSeconds(duration);

		accel = originalAccel;
		friction = originalFriction;
		max_speed = originalMaxSpeed;
	}

	public void ApplyKnockBack(Vector2 worldForce, float forwardLoss)
	{
		rb.linearVelocity += worldForce;

		rb_speed_local = rb.GetVector(rb.linearVelocity);
		rb_speed_forward *= forwardLoss;
		rb_speed_right = rb_speed_local.x;
	}
}
