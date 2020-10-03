#pragma warning disable 649
using UnityEngine;

public class Player : MonoBehaviour
{
	public Past past = new Past();

	Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		past.v2History.Add(rb.position);
	}

	public void Die()
	{
		past.bManualDeath = false;
		FindObjectOfType<Game>().Redo();
	}
}