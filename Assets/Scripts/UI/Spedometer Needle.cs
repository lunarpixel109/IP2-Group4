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

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		PlayerStats = player.GetComponent<CarController>();
		max_speed = PlayerStats.boost_max_speed * 10;


		rb = player.GetComponent<Rigidbody2D>();
		rt = needle.GetComponent<RectTransform>();
	}

	// Update is called once per frame
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
			_needleRotZ = Mathf.Lerp(Needle_min,Needle_max, Mathf.InverseLerp(0,max_speed, player_speed_forward));
		}
		else // backward
		{
            _needleRotZ = Mathf.Lerp(Needle_min, Needle_max, Mathf.InverseLerp(-max_speed, 0, player_speed_forward));
        }


		rt.SetLocalPositionAndRotation(needlePos, new Quaternion(needleRot.x, needleRot.y, _needleRotZ, needleRot.w));
	}
}
