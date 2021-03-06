﻿#pragma warning disable 649
using UnityEngine;
using Ciber_Turtle.Audio;

public class Button : MonoBehaviour
{
	[SerializeField] bool bInvert;
	[SerializeField] Transform tToggles;
	[Space]
	[SerializeField] Sprite spUp;
	[SerializeField] Sprite spDown;
	[SerializeField] Transform tActivationArea;
	[SerializeField] LayerMask lmPlayer;
	[SerializeField] LineRenderer lrLine;
	[SerializeField] Color cOffColor;
	[SerializeField] Color cOnColor;

	[SerializeField] bool wasActivated;

	SpriteRenderer sr;
	IToggleable toggle;

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();

		toggle = tToggles.GetComponent<IToggleable>();

		lrLine.SetPosition(0, transform.position);
		lrLine.SetPosition(1, tToggles.position);
	}

	void Update()
	{
		bool isPressed = Physics2D.OverlapBox(tActivationArea.position, tActivationArea.localScale, 0, lmPlayer);
		if (isPressed != wasActivated) SFX.current.transform.GetChild(0).GetComponent<AudioSource>().Play();

		lrLine.startColor = isPressed == true ? cOnColor : cOffColor;
		lrLine.endColor = isPressed == true ? cOnColor : cOffColor;

		if (bInvert)
		{
			toggle.SetActive(!isPressed);
			sr.sprite = isPressed ? spDown : spUp;
		}
		else
		{
			toggle.SetActive(Physics2D.OverlapBox(tActivationArea.position, tActivationArea.localScale, 0, lmPlayer));
			sr.sprite = isPressed ? spDown : spUp;
		}

		wasActivated = isPressed;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 1, 0, 0.25f);

		if (tActivationArea) Gizmos.DrawWireCube(tActivationArea.position, tActivationArea.localScale);

		if (tToggles) Gizmos.DrawLine(transform.position, tToggles.position);

		if (lrLine & tToggles)
		{
			lrLine.SetPosition(0, transform.position);
			lrLine.SetPosition(1, tToggles.position);
		}
	}
}