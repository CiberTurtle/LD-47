﻿#pragma warning disable 649
using UnityEngine;
using Ciber_Turtle.Input;
using Ciber_Turtle.Audio;

[AddComponentMenu("Game/Player/Player Movement")]
public class PlayerMovement : MonoBehaviour
{
	public float fMoveSpeed;
	public float fMaxMoveSpeed;
	[SerializeField, Range(0, 1)] float fGroundDeceleration;
	[SerializeField, Range(0, 1)] float fAirDeceleration;
	[SerializeField, Range(0, 1)] float fGroundKBDeceleration;
	[SerializeField, Range(0, 1)] float fAirKBDeceleration;
	[Space]
	public float fJumpHeight;
	public float fMaxFallVel;
	public float fFallGravity;
	public float fFallSpeed;
	[Space]
	[SerializeField] float fJumpPressRem;
	[SerializeField, Range(0, 1)] float fJumpCut;
	[SerializeField] float fGroundedPressRem;
	[Space]
	[SerializeField] Collider2D c2dGroundedCollider;
	[SerializeField] Collider2D c2AirCollider;
	[SerializeField] LayerMask lmGround;
	[SerializeField] Transform tGroundCheck;
	public UnityEngine.Events.UnityEvent onLand = new UnityEngine.Events.UnityEvent();
	[Space]
	[SerializeField] float fTimeBetweenPuffs;
	[SerializeField] Transform tPuffPos;
	[SerializeField] GameObject pfWalkPuff;
	[SerializeField] GameObject pfJumpPuff;
	[SerializeField] GameObject pfLandPuff;
	[SerializeField] float fTimeBtPuffs;

	[HideInInspector] public bool bEnableMovement = true;
	[HideInInspector] public bool bEnableFlip = true;
	[HideInInspector] public bool bGrounded;
	[HideInInspector] public bool bWasGrounded;
	bool bIsJumpDown;
	bool bIsJumpUp;
	float fMoveInput;
	float fJumpPressMem;
	float fGroundedPressMem;
	float fHVel;
	Vector2 v2ExtraVel;
	float lastPuffSpawn;

	float fTimeScinceLastPuff;
	Animator anim;

	Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		onLand.AddListener(() => { if (fTimeScinceLastPuff > fTimeBetweenPuffs) { Util.TryInstantiate(pfLandPuff, tPuffPos.position, Quaternion.identity); fTimeScinceLastPuff = 0; } });
		onLand.AddListener(() => SFX.current.PlaySound("Land"));
	}

	private void Update()
	{
		fTimeScinceLastPuff += Time.deltaTime;

		fMoveInput = BasicInput.GetAxisRaw("X");
		if (BasicInput.GetButtonDown("Jump"))
		{
			bIsJumpDown = true;
			bIsJumpUp = false;
		}
		if (BasicInput.GetButtonUp("Jump"))
		{
			bIsJumpDown = false;
			bIsJumpUp = true;
		}

		//  Sprite Flipping
		if (bEnableFlip && Mathf.Abs(fMoveInput) > 0)
		{
			if (fMoveInput > 0)
				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			else
				transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
	}

	private void FixedUpdate()
	{
		bGrounded = Physics2D.OverlapBox(tGroundCheck.position, tGroundCheck.localScale, tGroundCheck.eulerAngles.z, lmGround);
		if (bWasGrounded != bGrounded)
			onLand.Invoke();

		if (bEnableMovement)
		{
			fGroundedPressMem -= Time.fixedDeltaTime;
			if (bGrounded)
			{
				fGroundedPressMem = fGroundedPressRem;
				c2dGroundedCollider.enabled = true;
				c2AirCollider.enabled = false;
			}
			else
			{
				c2dGroundedCollider.enabled = false;
				c2AirCollider.enabled = true;
			}

			fJumpPressMem -= Time.fixedDeltaTime;
			if (bIsJumpDown)
			{
				fJumpPressMem = fJumpPressRem;
			}
			bIsJumpDown = false;

			if (fJumpPressMem > 0 && fGroundedPressMem > 0)
			{
				fGroundedPressMem = 0;
				fJumpPressMem = 0;
				rb.velocity = new Vector2(rb.velocity.x, fJumpHeight);
				Util.TryInstantiate(pfJumpPuff, tPuffPos.position, Quaternion.identity);
				SFX.current.PlaySound("Jump");
			}

			if (bIsJumpUp && !bGrounded && rb.velocity.y > 0)
			{
				bIsJumpUp = false;
				rb.velocity *= fJumpCut;
			}

			if (rb.velocity.y > 0)
			{
				rb.gravityScale = 1f;
			}
			else
			{
				rb.gravityScale = fFallGravity;
				rb.velocity += new Vector2(0, fFallSpeed) * Time.fixedDeltaTime;
			}

			if (Mathf.Abs(fMoveInput) > 0.1f)
			{
				fHVel = Mathf.Clamp(fHVel + fMoveInput * fMoveSpeed * Time.fixedDeltaTime * 10, -fMaxMoveSpeed, fMaxMoveSpeed);
			}
			else
			{
				if (bGrounded)
					fHVel *= fGroundDeceleration * Time.fixedDeltaTime * 50;
				else
					fHVel *= fAirDeceleration * Time.fixedDeltaTime * 50;
			}

			if (Mathf.Abs(rb.velocity.x) > 0.1f && bGrounded)
			{
				fTimeScinceLastPuff += Time.fixedDeltaTime;
				if (fTimeScinceLastPuff > fTimeBetweenPuffs)
				{
					Util.TryInstantiate(pfWalkPuff, tPuffPos.position, Quaternion.identity).transform.localScale = transform.localScale;
					fTimeScinceLastPuff = 0;
				}
			}
			else
			{
				fTimeScinceLastPuff = 0;
			}

			// bool bGrabWallSide = Physics2D.OverlapBox(tWallGrabCheck.position, tWallGrabCheck.localScale, tWallGrabCheck.eulerAngles.z, lmGround);
			// bool bGrabWallTop = Physics2D.OverlapBox(tWallGrabTopCheck.position, tWallGrabTopCheck.localScale, tWallGrabTopCheck.eulerAngles.z, lmGround);
			// bool bCanGrabWall = bGrabWallSide && !bGrabWallTop && rb.velocity.y < 0;

			// if (bCanGrabWall) Debug.Log("Grabbing Wall");
		}
		else
		{
			c2dGroundedCollider.enabled = false;
			c2AirCollider.enabled = true;

			if (bGrounded)
				fHVel *= fGroundKBDeceleration * Time.fixedDeltaTime * 50;
			else
				fHVel *= fAirKBDeceleration * Time.fixedDeltaTime * 50;
		}

		rb.velocity = new Vector2(fHVel, Mathf.Clamp(rb.velocity.y, fMaxFallVel, float.MaxValue)) + v2ExtraVel;

		anim.SetFloat("X", Mathf.Round(rb.velocity.x));
		anim.SetFloat("Y", Mathf.Round(rb.velocity.y));

		bWasGrounded = bGrounded;
	}

	public void SetVelocity(Vector2 v2Velocity)
	{
		rb.velocity = v2Velocity;
	}

	private float CalculateJumpSpeed(float jumpHeight, float gravity)
	{
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.25f);
		if (tGroundCheck) Gizmos.DrawWireCube(tGroundCheck.position, tGroundCheck.localScale);
		Gizmos.color = new Color(1, 0, 1, 0.25f);
		if (rb) Gizmos.DrawLine(transform.position, (Vector2)transform.position + rb.velocity * 0.25f);
	}
}