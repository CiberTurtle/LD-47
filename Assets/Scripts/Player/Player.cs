#pragma warning disable 649
using UnityEngine;

public class Player : MonoBehaviour
{
	public Past past = new Past();
	[SerializeField] GameObject pfDeathEffect;
	[SerializeField] GameObject pfRedoEffect;
	[SerializeField] SpriteRenderer srCosmetic;

	Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		srCosmetic.sprite = FindObjectOfType<Game>().spCosmetics[past.iCosmeticIndex];
	}

	void FixedUpdate()
	{
		past.v2History.Add(rb.position);
	}

	public void Die()
	{
		Util.TryInstantiate(pfDeathEffect, transform.position, Quaternion.identity);
		past.bManualDeath = false;
		FindObjectOfType<Game>().Redo();
	}

	public void Redo()
	{
		Util.TryInstantiate(pfRedoEffect, transform.position, Quaternion.identity);
	}
}