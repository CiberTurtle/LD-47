#pragma warning disable 649
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
	public PastPlayer past;

	public int index = 0;

	Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public PlayerGhost SetPast(PastPlayer past)
	{
		this.past = past;
		return this;
	}

	void FixedUpdate()
	{
		if (index < past.v2History.Count)
		{
			rb.position = past.v2History[index];

			index++;
		}
	}
}