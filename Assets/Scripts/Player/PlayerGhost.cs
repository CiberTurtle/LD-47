#pragma warning disable 649
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
	[SerializeField] Sprite spDead;
	[SerializeField] Sprite spLost;
	[SerializeField] SpriteRenderer srRend1;
	[SerializeField] SpriteRenderer srRend2;
	[SerializeField] SpriteRenderer srCosmetic;
	[SerializeField] LayerMask lmGround;

	[HideInInspector] public Past past;
	int index = 0;
	Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		srCosmetic.sprite = FindObjectOfType<Game>().spCosmetics[past.iCosmeticIndex];
	}

	public PlayerGhost SetPast(Past past)
	{
		this.past = past;
		return this;
	}

	void FixedUpdate()
	{
		if (index < (past.bManualDeath ? past.v2History.Count : past.v2History.Count - 1))
		{
			rb.position = past.v2History[index];

			if (index != 0 && past.v2History[index].x != past.v2History[index - 1].x) transform.localScale = new Vector3(past.v2History[index].x > past.v2History[index - 1].x ? 1 : -1, 1, 1);

			index++;
		}
		else
		{
			if (past.bManualDeath)
			{
				srRend1.sprite = spLost;
				srRend2.sprite = spLost;
			}
			else
			{
				srRend1.sprite = spDead;
				srRend2.sprite = spDead;
			}
		}
	}
}