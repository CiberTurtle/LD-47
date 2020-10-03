#pragma warning disable 649
using UnityEngine;

public class Player : MonoBehaviour
{
	public PastPlayer past = new PastPlayer();

	Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		past.v2History.Add(rb.position);
	}
}