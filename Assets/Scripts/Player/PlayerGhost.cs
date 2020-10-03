#pragma warning disable 649
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
	public Past past;

	public int index = 0;

	Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public PlayerGhost SetPast(Past past)
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