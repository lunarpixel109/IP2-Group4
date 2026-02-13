using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class driving : MonoBehaviour
{
	bool DebugBool = false;
	public Vector2 debug_Vector = new Vector2(0,0);

	public float accel;
	public float braking;
	public float friction;
	float gear_change_cooldown;
	public float gear_change_cooldown_max;

    bool drifing = false;

	public float steering_speed_d;
    public float direction_d;


	Rigidbody2D rb;

	float posX;
	float posY;
	float prevX;
	float prevY;
    float speed;


	public float speed_display;
	public float speed_parallel;
	public float speed_perp;
	public float speed_direction;
	public float car_angle;
	public float speed_angle;

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
	
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		//speed_display = rb.linearVelocity.magnitude;
		SpeedStuff();

		direction_d = rb.rotation;
		//Debug.Break();

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

		if (steering.IsPressed() || DebugBool)
		{
            if ( drivingState == DrivingState.forward || DebugBool)
            {
                direction_d -= steering.ReadValue<float>() * steering_speed_d;
				//direction_d += 5f;
				//Debug.Break();
            }
            else if (drivingState == DrivingState.barckward)
            {
                direction_d += steering.ReadValue<float>() * steering_speed_d;
            }

            print(steering.ReadValue<float>());
			print("sadasd");
		}

        print(speed);

		//Debug.Break();
		rb.rotation = direction_d;
		//Debug.Break();
    
		debug_Vector = new Vector2(speed * math.cos(direction_d) * Time.fixedDeltaTime, speed * math.sin(direction_d) * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2 (speed * math.cos(direction_d) * Time.fixedDeltaTime, speed * math.sin(direction_d) * Time.fixedDeltaTime);
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

    void SpeedStuff()
	{
		var delta_angle = 0f;

		car_angle = rb.rotation * math.TODEGREES;
		speed_angle = math.atan(rb.linearVelocityY/rb.linearVelocityX);

		delta_angle = car_angle-speed_angle;

		speed_display = rb.linearVelocity.magnitude;
		speed_parallel = math.cos(delta_angle);
		speed_perp = math.sin(delta_angle);

	}
}
