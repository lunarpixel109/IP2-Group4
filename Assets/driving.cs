using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class driving : MonoBehaviour
{
	

	public float accel;
	public float braking;
	public float friction;
	float gear_change_cooldown;
	public float gear_change_cooldown_max;

    bool drifing = false;

	public float steering_speed_d;
	public float steering_speed_r;
    public float direction_d;
	public float direction_r;


	Rigidbody2D rb;

	float posX;
	float posY;
	float prevX;
	float prevY;
    float speed;

    enum Gear{
		forward,
		backward
	}

	enum DrivingState { 
	
		stationary,
		forward,
		barckward
	}

    DrivingState drivingState = DrivingState.stationary;


	InputAction accelerate;
	InputAction brake;
	InputAction steering;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		accelerate = InputSystem.actions.FindAction("Accelerate");
		brake = InputSystem.actions.FindAction("Brake");
		steering = InputSystem.actions.FindAction("Steering");

		steering_speed_r = steering_speed_d * math.PI / 180;
	
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		//speed = rb.linearVelocity.magnitude;
		direction_r = rb.rotation;
		direction_d = direction_r * math.TORADIANS;

        if (drivingState == DrivingState.stationary)
        {
			if (accelerate.IsPressed() == true) { drivingState = DrivingState.forward; }
			else if (brake.IsPressed() == true) { drivingState = DrivingState.barckward; }
        }
        else if (drivingState == DrivingState.forward)
		{
			if (accelerate.IsPressed() == true)
			{
				//rb.AddForce(transform.up * accel * Time.deltaTime);

				speed += (accel - (speed / 10)) * Time.fixedDeltaTime;
				print("accel " + accelerate.IsPressed());
			}
			else if (brake.IsPressed() == true)
			{
				//rb.AddForce(transform.forward * -braking * Time.deltaTime);

				speed -= braking * Time.fixedDeltaTime;
				if (speed < 0) { Momentum_Change_Stationary(); }
				print("breaking " + brake.IsPressed());
			}
			else
			{
				//rb.AddForce(transform.forward * -friction * Time.deltaTime);

				speed -= friction * Time.fixedDeltaTime;
				if (speed < 0) { Momentum_Change_Stationary(); }
			}
		}
		else if (drivingState == DrivingState.barckward)
		{
			if (brake.IsPressed() == true)
            {
				//rb.AddForce(transform.up * -accel * Time.deltaTime);

                speed -= (accel/2 + speed/10) * Time.fixedDeltaTime;
                print("reversing " + brake.IsPressed());
            }
            else if (accelerate.IsPressed() == true)
            {
				//rb.AddForce(transform.up * braking * Time.deltaTime);

                speed += braking * Time.fixedDeltaTime;
                if (speed > 0) { Momentum_Change_Stationary(); }
                print("R breaking " + accelerate.IsPressed());
            }
            else
            {
				//rb.AddForce(transform.up * friction * Time.deltaTime);

                speed += friction * Time.fixedDeltaTime;
                if (speed > 0) { Momentum_Change_Stationary(); }
            }
        }

		if (steering.IsPressed())
		{
            if ( drivingState == DrivingState.forward)
            {
                direction_r += steering.ReadValue<float>() * steering_speed_r;
                direction_d += steering.ReadValue<float>() * steering_speed_d;
            }
            else if (drivingState == DrivingState.barckward)
            {
                direction_r -= steering.ReadValue<float>() * steering_speed_r;
                direction_d -= steering.ReadValue<float>() * steering_speed_d;
            }

            print(steering.ReadValue<float>());
			print("sadasd");
		}

        print(speed);

		rb.rotation = direction_r;
        rb.linearVelocity = new Vector2 (speed * math.sin(direction_d) * Time.fixedDeltaTime, speed * math.cos(direction_d) * Time.fixedDeltaTime);
    }

    void Momentum_Change_Stationary()
	{
        speed = 0;
		drivingState = DrivingState.stationary;
    }

	float GetSpeed()
	{
		float velX = rb.linearVelocityX;
		float velY = rb.linearVelocityY;

        float vel = math.sqrt((velX * velX) + (velY * velY));

		float dir_deg = math.asin(velX / vel);
		float dir_r = math.TORADIANS * dir_deg;

		if (math.abs(transform.rotation.eulerAngles.z - dir_deg) > 180) { vel = -vel; }

		return vel;
	}
}
