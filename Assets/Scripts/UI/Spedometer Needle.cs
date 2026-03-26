using System;
using System.IO;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class SpedometerNeedle : MonoBehaviour
{
	public GameObject player;
	float player_speed_forward;
	Rigidbody2D rb;
	CarController PlayerStats;

	public GameObject needle;
	public float Needle_min;
	public float Needle_max;
	float max_speed;
	RectTransform rt;

	Vector3 needlePos;
	Quaternion needleRot;

	public float _needleRotZ;
	public float desired_angle;
	public float current_angle;

	public float needle_max_weird;
	public float needle_min_weird;

	void Start()
	{
		PlayerStats = player.GetComponent<CarController>();
		max_speed = PlayerStats.boost_max_speed * 10;

		rb = player.GetComponent<Rigidbody2D>();
		rt = needle.GetComponent<RectTransform>();
	}

	void FixedUpdate()
	{
		player_speed_forward = rb.GetVector(rb.linearVelocity)[1] * 10;

		rt.GetLocalPositionAndRotation(out needlePos,out needleRot);

		if (player_speed_forward > max_speed ) // too fast
		{
			_needleRotZ = Needle_max;
		}
		else if (player_speed_forward >= 0) // forward
		{
			_needleRotZ = Mathf.Lerp(Needle_min,Needle_max, Mathf.InverseLerp(0,max_speed, math.abs(player_speed_forward)));
		}

		desired_angle = _needleRotZ;
		current_angle = rt.rotation.z;

		var delta_angle = desired_angle - current_angle;
		var delta_angle_weird = DegreesToWeird(delta_angle);


		rt.transform.rotation = quaternion.Euler(new float3(0, 0, (float)delta_angle_weird));
	}

	float DegreesToWeird(float degrees)
	{
		var weird = 0f;

		weird = Mathf.Lerp(needle_min_weird, needle_max_weird, Mathf.InverseLerp(Needle_min, Needle_max, degrees));

		return weird;
	}
}
