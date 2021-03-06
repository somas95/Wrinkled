﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class controller_2D : MonoBehaviour
{
	[SerializeField] private float JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform GroundCheck;								// A position marking where to check if the player is grounded.
	[SerializeField] private Transform CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Transform SpawnPoint;								// From where do we spawn the player
	[SerializeField] private Collider2D CrouchDisableCollider;					// A collider that will be disabled when crouching
	[SerializeField] private GameObject Corpse;									// Corpse

	private bool isInputEnabled = true;

	public Animator animator;

	const float groundedRadius = .01f; // Radius of the overlap circle to determine if grounded
	public bool isGrounded;            // Whether or not the player is grounded.
	const float ceilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D Rigidbody2D;
	private bool isFacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

	public AudioManager audioManager;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool wasCrouching = false;

	private void Awake()
	{
		Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		SpawnCharacter();
	}

	private void SpawnCharacter()
	{
		Rigidbody2D.transform.position = SpawnPoint.transform.position;
	}

	private void SpawnCorpse()
	{
		Instantiate(Corpse, Rigidbody2D.transform.position, Quaternion.identity);
	}

	private void FixedUpdate()
	{
		animator.SetBool("isGrounded", isGrounded);
		bool wasGrounded = isGrounded;
		isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, groundedRadius, WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
				if (!wasGrounded)
				{
					audioManager.Play("golpe");
					OnLandEvent.Invoke();
				}
			}
		}

		if (animator.GetBool("isJumpUp") && Rigidbody2D.velocity.y < 0)
		{
			animator.SetBool("isJumpUp", false);
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(CeilingCheck.position, ceilingRadius, WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if ((isGrounded || AirControl) && isInputEnabled)
		{

			// If crouching
			if (crouch)
			{
				if (!wasCrouching)
				{
					wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= CrouchSpeed;

				// Disable one of the colliders when crouching
				if (CrouchDisableCollider != null)
					CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (CrouchDisableCollider != null)
					CrouchDisableCollider.enabled = true;

				if (wasCrouching)
				{
					wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref velocity, MovementSmoothing);
			

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !isFacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && isFacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (isGrounded && jump && isInputEnabled)
		{
			// Add a vertical force to the player.
			isGrounded = false;
			animator.SetBool("isJumpUp", true);
			Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
			audioManager.Play("saltar");
		}

		//If the player is walking...
		if (isGrounded && move != 0)
		{
			audioManager.Play("pisada");
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		isFacingRight = !isFacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		// On death
		if (other.gameObject.tag == "Respawn")
		{
			Debug.Log("die");
			StartCoroutine(die());
			StopCoroutine(die());

		}
	}

	IEnumerator die() {
		audioManager.Play("muerte");
		isInputEnabled = false;
		animator.SetBool("isDeath", true);
		yield return new WaitForSeconds (1.75f);
		// first we spawn a corpse
		SpawnCorpse();
		animator.SetBool("isDeath", false);
		SpawnCharacter();
		isInputEnabled = true;

	}
}