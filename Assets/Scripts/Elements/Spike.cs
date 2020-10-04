#pragma warning disable 649
using UnityEngine;

public class Spike : MonoBehaviour, IToggleable
{
	[SerializeField] Sprite spUp;
	[SerializeField] Sprite spDown;
	[SerializeField] LayerMask lmPlayer;

	[SerializeField] Vector2 fSize;
	[SerializeField] float fPos;

	SpriteRenderer rend;

	void Awake()
	{
		rend = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		// if (rend.sprite == spUp) if (Physics2D.OverlapBox(transform.position + new Vector3(0, fPos, 0), new Vector2(rend.size.x + fSize.x, fSize.y), transform.eulerAngles.z, lmPlayer)) FindObjectOfType<Player>().Die();
	}

	public void SetActive(bool state)
	{
		if (state)
		{
			rend.sprite = spUp;
		}
		else
		{
			rend.sprite = spDown;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && rend.sprite == spUp)
		{
			other.GetComponent<Player>().Die();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.25f);

		rend = GetComponent<SpriteRenderer>();
		Gizmos.DrawWireCube(transform.position + new Vector3(0, fPos, 0), new Vector2(rend.size.x + fSize.x, fSize.y));
	}
}