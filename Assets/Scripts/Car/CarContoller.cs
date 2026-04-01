using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class CarController : MonoBehaviour
{
	[Header("Other Info")]

	Rigidbody2D rb;
	public GameObject Player;
	public GameObject carSprite;

	float drift_right_angle;
	float drift_left_angle;

	public GameObject drift_target_left;
	public GameObject drift_target_right;
	public GameObject drift_target_centre;

	InputAction accelerate;
	InputAction brake;
	InputAction steering;
	InputAction drifting;
	InputAction boosting;

	InputAction ActionKey1;
	InputAction ActionKey2;
	InputAction ActionKey3;
	InputAction ActionKey4;
	InputAction ActionKey5;
	InputAction ActionKey6;
	InputAction ActionKey7;
	InputAction ActionKey8;
	InputAction ActionKey9;


    public enum DrivingState
	{
		stationary,
		forward,
		barckward
	}

	public DrivingState drivingState = DrivingState.stationary;
	
	public bool canMove = false;

	[Header("Driving Setup")]

	public float accel;
	public float braking;
	public float friction;
	public float steering_speed;
	public float max_speed;
	public float max_speed_reverse;
	public float boost_accel;
	public float boost_max_speed;
	public float boost_slowdown;

	[Header("Drifting Setup")]

	public float drift_speed_threshold;
	public float drift_steering_speed_max;
	public float drift_steering_speeed_min;
	public float drift_steering_visual_max;
	public float drift_steering_visual_min;
	public float drift_transition_time_seconds;
	public float drift_slowdown;
	float drift_transition_time;

	public float drift_amount;

	public float drift_boost_gain;

	[Header("Live Info")]

	public UnityEngine.Vector2 rb_speed_local;
	public float rb_speed_forward;
	public float rb_speed_right;

	public float rb_direction;
	public float carSprite_direction;
	public int drift_direction;

	public float boost;
	public float _accel;
	public float _max_speed;

	public bool is_drifting = false;
	public float drifting_value = 0f;

	public float _steering;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		accelerate = InputSystem.actions.FindAction("Accelerate");
		brake = InputSystem.actions.FindAction("Brake");
		steering = InputSystem.actions.FindAction("Steering");
		drifting = InputSystem.actions.FindAction("Drifting");
		boosting = InputSystem.actions.FindAction("Boost");

		ActionKey1 = InputSystem.actions.FindAction("Action Key 1");
		ActionKey2 = InputSystem.actions.FindAction("Action Key 2");
		ActionKey3 = InputSystem.actions.FindAction("Action Key 3");
		ActionKey4 = InputSystem.actions.FindAction("Action Key 4");
		ActionKey5 = InputSystem.actions.FindAction("Action Key 5");
		ActionKey6 = InputSystem.actions.FindAction("Action Key 6");
		ActionKey7 = InputSystem.actions.FindAction("Action Key 7");
		ActionKey8 = InputSystem.actions.FindAction("Action Key 8");
		ActionKey9 = InputSystem.actions.FindAction("Action Key 9");

		rb = GetComponent<Rigidbody2D>();

		_accel = accel;
		_max_speed = max_speed;

		drift_transition_time = 1 / drift_transition_time_seconds;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (rb.rotation > 360) { rb.rotation -= 360; }
		if (rb.rotation < 0) { rb.rotation += 360; }

		#region input
		if (!canMove)
		{
			rb.linearVelocity = Vector2.zero;
			rb.angularVelocity = 0f;
			return;
		}
		
		rb_speed_local = rb.GetVector(rb.linearVelocity); // gets the speed of the car as a vector in global space and turns it into a vector in local space

		rb_speed_forward = rb_speed_local[1];// splits local space vector into forward component and right component
		rb_speed_right = rb_speed_local[0];

		if (rb_speed_forward > 0f) { drivingState = DrivingState.forward; }
		else if (rb_speed_forward < 0f) { drivingState = DrivingState.barckward; }
		else { drivingState = DrivingState.stationary; }

		rb_direction = rb.rotation; // gets rotation of car via rigidbody2D

		if (boosting.IsPressed()) { boost += 1.5f * Time.fixedDeltaTime; }
		
		
		if (!is_drifting)
		{
			boost -= 1f * Time.fixedDeltaTime;
			if (boost < 0f) { boost = 0f; Boost_Cancel(); }
		}

		//if (boost > 3f) { boost = 3f; }

		if (boost > 0f && !is_drifting)
		{
			_max_speed = boost_max_speed;
			_accel = boost_accel;
		}


		if (Drift_Check())
		{
			if (!is_drifting)
			{
				drift_direction = (int)math.sign(steering.ReadValue<float>());
				rb_speed_forward -= drift_slowdown;
			}
			drifting_value += drift_transition_time * Time.fixedDeltaTime;
			if (drifting_value > 1f)
			{
				drifting_value = 1f;
			}
		}
		else
		{
			drifting_value -= drift_transition_time*5 * Time.fixedDeltaTime;
			if (drifting_value < 0f) { drifting_value = 0f; }
		}

		if (drifting_value > 0f)
		{
			is_drifting = true; 
			boost += drift_boost_gain * Time.fixedDeltaTime;
		}
		else { is_drifting = false; }

		#endregion

		#region parallel (forward/brackward) movementel
		//normal
		if (!is_drifting)
		{
			// iffs decide which forces apply to car each frame
			if (drivingState == DrivingState.forward)
			{
				if (accelerate.IsPressed())
				{
					rb_speed_forward += (_accel - (rb_speed_forward / 10)) * Time.fixedDeltaTime; // increments forward speed by appropriate value based on input and context
					if (rb_speed_forward > _max_speed) { var time = Time.fixedDeltaTime; Max_speed_clamp(time); }
					//print("accel " + accelerate.IsPressed());
				}
				else if (brake.IsPressed())
				{
					rb_speed_forward -= braking * Time.fixedDeltaTime;
					if (rb_speed_forward < 0) { rb_speed_forward = 0; }
					Boost_Cancel();
					//print("breaking " + brake.IsPressed());
				}
				else
				{
					rb_speed_forward -= friction * Time.fixedDeltaTime;
					if (rb_speed_forward < 0) { rb_speed_forward = 0; }
					Boost_Cancel();
				}
			}
			else if (drivingState == DrivingState.barckward)
			{
				if (brake.IsPressed())
				{
					rb_speed_forward -= (_accel / 2 - rb_speed_forward / 10) * Time.fixedDeltaTime;
					if (rb_speed_forward < -max_speed_reverse) { rb_speed_forward = -max_speed_reverse; }
					//print("reversing " + brake.IsPressed());
				}
				else if (accelerate.IsPressed())
				{
					rb_speed_forward += braking * Time.fixedDeltaTime;
					if (rb_speed_forward > 0) { rb_speed_forward = 0; }
					//print("R breaking " + accelerate.IsPressed());
				}
				else
				{
					rb_speed_forward += friction * Time.fixedDeltaTime;
					if (rb_speed_forward > 0) { rb_speed_forward = 0; }
				}
			}
			else if (drivingState == DrivingState.stationary)
			{
				if (accelerate.IsPressed())
				{
					rb_speed_forward += (_accel - (rb_speed_forward / 10)) * Time.fixedDeltaTime;
					//print("accel " + accelerate.IsPressed());
				}
				else if (brake.IsPressed())
				{
					rb_speed_forward -= (_accel / 2 - rb_speed_forward / 10) * Time.fixedDeltaTime;
					//print("reversing " + brake.IsPressed());
				}
			}
		}
		// drifting
		else
		{
			if (rb_speed_forward > _max_speed) { rb_speed_forward = _max_speed; }
			// rb_speed_forward starts constant
		}
		#endregion

		#region perpendicular (sliding) movement
		rb_speed_right *= 0.5f; // always lowers right/left speed of car (will not effect drift dont worry)
		if (math.abs(rb_speed_right) < 0.05) rb_speed_right = 0;
		#endregion

		#region steering

		_steering = 0f;
		rb.angularVelocity = 0f;

		if (!is_drifting)
		{
			drift_amount = 0f;

			if (steering.IsPressed())
			{
                _steering = steering.ReadValue<float>();
                ActionKeys();

                if (drivingState == DrivingState.forward)
				{
					rb_direction -= _steering * Steering_Speed_Curve() * Time.fixedDeltaTime;
				}
				else if (drivingState == DrivingState.barckward)
				{
					rb_direction += _steering * Steering_Speed_Curve() * Time.fixedDeltaTime;
				}

			}

		}
		else
		{
			drift_amount = Mathf.Lerp(drift_steering_speeed_min, drift_steering_speed_max, Mathf.InverseLerp(1 * drift_direction, -1 * drift_direction, steering.ReadValue<float>())) * drift_direction;

			rb_direction -= drift_amount * Time.fixedDeltaTime;
        }

        var desired_angle = Mathf.Lerp(drift_steering_visual_min, drift_steering_visual_max, Mathf.InverseLerp(1 * drift_direction, -1 * drift_direction, steering.ReadValue<float>())) * drift_direction * drifting_value;
		var current_angle = carSprite.transform.localRotation.z;

		var delta_angle = current_angle - desired_angle;
		var delta_angle_rads = math.TORADIANS * delta_angle;
	

		carSprite.transform.localRotation = quaternion.Euler(new float3(0, 0, (float)delta_angle_rads));

		#endregion

		#region output

		rb_speed_local = new Vector2(rb_speed_right, rb_speed_forward); // recombines forward and right vectors
		rb.linearVelocity = rb.GetRelativeVector(rb_speed_local); // sets speed in global space based on speed in local space

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

	void Max_speed_clamp(float deltaTime)
	{
		if (_max_speed - rb_speed_forward < -0.5f)
		{
			rb_speed_forward -= boost_slowdown * deltaTime;
		}
		else
		{
			rb_speed_forward = _max_speed;
		}
	}

	void Boost_Cancel()
	{
		_max_speed = max_speed;
		_accel = accel;
	}

	public void ApplyBoost(float duration, float new_max_speed, float new_accel)
	{
		boost = duration;
		boost_max_speed = new_max_speed;
		boost_accel = new_accel;
		//StartCoroutine(SpeedMultiplierRoutine(multiplier, duration));
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

		print("knock");
	}

	void ActionKeys()
	{
		if (ActionKey1.IsPressed())
		{
			if (math.abs(_steering) > 0.1f) { _steering = 0.1f * math.sign(_steering); }
		}
        else if (ActionKey2.IsPressed())
        {
            if (math.abs(_steering) > 0.2f) { _steering = 0.2f * math.sign(_steering); }
        }
        else if (ActionKey3.IsPressed())
        {
            if (math.abs(_steering) > 0.3f) { _steering = 0.3f * math.sign(_steering); }
        }
        else if (ActionKey4.IsPressed())
        {
            if (math.abs(_steering) > 0.4f) { _steering = 0.4f * math.sign(_steering); }
        }
        else if (ActionKey5.IsPressed())
        {
            if (math.abs(_steering) > 0.5f) { _steering = 0.5f * math.sign(_steering); }
        }
        else if (ActionKey6.IsPressed())
        {
            if (math.abs(_steering) > 0.6f) { _steering = 0.6f * math.sign(_steering); }
        }
        else if (ActionKey7.IsPressed())
        {
            if (math.abs(_steering) > 0.7f) { _steering = 0.7f * math.sign(_steering); }
        }
        else if (ActionKey8.IsPressed())
        {
            if (math.abs(_steering) > 0.8f) { _steering = 0.8f * math.sign(_steering); }
        }
        else if (ActionKey9.IsPressed())
        {
            if (math.abs(_steering) > 0.9f) { _steering = 0.9f * math.sign(_steering); }
        }
    }
}
