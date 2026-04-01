using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Spedometer_Text : MonoBehaviour
{
	public GameObject player;
	float player_speed_forward;
	Rigidbody2D rb;
	public TextMeshProUGUI text;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rb = player.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		player_speed_forward = math.abs(rb.GetVector(rb.linearVelocity)[1]);

		text.text = math.round(player_speed_forward*10) + " k/hr";
	}
}
