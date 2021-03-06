﻿#pragma warning disable 649
using UnityEngine;

public class Wall : MonoBehaviour, IToggleable
{
	[SerializeField] SpriteRenderer srWall;
	[SerializeField] SpriteRenderer srOutline;
	[SerializeField] LayerMask lmPlayer;
	[SerializeField] float fGraceSize;

	bool bLastState = false;
	BoxCollider2D bc2dWall;

	void Awake()
	{
		bc2dWall = srWall.GetComponent<BoxCollider2D>();
	}

	public void SetActive(bool state)
	{
		if (state != bLastState && state == true)
		{
			if (Physics2D.OverlapBox(transform.position, srWall.size - Util.ToVector2(fGraceSize), 0, lmPlayer)) FindObjectOfType<Player>().Die();
		}

		srWall.enabled = state;
		bc2dWall.enabled = state;

		bLastState = state;
	}

	void OnDrawGizmosSelected()
	{
		if (srOutline && srWall)
		{
			srOutline.size = srWall.size;

			Gizmos.color = new Color(1, 0, 0, 0.1f);
			Gizmos.DrawWireCube(transform.position, srWall.size - Util.ToVector2(fGraceSize));
		}
	}
}