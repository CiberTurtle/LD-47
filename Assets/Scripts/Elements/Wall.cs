#pragma warning disable 649
using UnityEngine;

public class Wall : MonoBehaviour, IToggleable
{
	[SerializeField] SpriteRenderer srWall;
	[SerializeField] SpriteRenderer srOutline;

	BoxCollider2D bc2dWall;

	void Awake()
	{
		bc2dWall = srWall.GetComponent<BoxCollider2D>();
	}

	public void SetActive(bool state)
	{
		srWall.enabled = state;
		bc2dWall.enabled = state;
	}

	void OnDrawGizmos()
	{
		if (srOutline && srWall) srOutline.size = srWall.size;
	}
}