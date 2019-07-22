using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
{
	[SerializeField] float maxJumpHeight = 4;
	[SerializeField] float minJumpHeight = 1;
	[SerializeField] float timeToJumpApex = .4f;

	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = 0.0f;
	[SerializeField] float moveSpeed = 6;


	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .5f;
	float timeToWallUnStick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	public Vector3 velocity;
	float velocityXSmoothing;

	public Controller2D controller;

	Vector2 directionalInput;

	bool wallSliding;
	int wallDirectionX;


	void Start()
	{
		controller = GetComponent<Controller2D>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		print("Gravity: " + gravity + " Jump Velocity: " + maxJumpVelocity);
	}

	void Update()
	{
		CalculateVelocity();
		HandleWallSliding();

		controller.Move(velocity * Time.deltaTime, directionalInput);

		if(controller.collisions.above || controller.collisions.below)
		{
			accelerationTimeAirborne = 0f;
			velocity.y = 0;
		}
	}

	public void SetDirectionalInput(Vector2 input)
	{
		directionalInput = input;
	}

	public void OnJumpInputDown()
	{
		if (wallSliding)
		{
			accelerationTimeAirborne = .2f;
			if(wallDirectionX == directionalInput.x)
			{
				velocity.x = -wallDirectionX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if(directionalInput.x == 0)
			{
				//wall jump zor olduğu için walljumpoff iptal edilip wallleape dönüştürülecek.
				//velocity.x = -wallDirectionX * wallJumpOff.x;
				//velocity.y = wallJumpOff.y;
				velocity.x = -wallDirectionX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
			else
			{
				velocity.x = -wallDirectionX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (controller.collisions.below)
		{
			velocity.y = maxJumpVelocity;
		}
	}

	public void OnJumpInputUp()
	{
		if(velocity.y > minJumpVelocity)
		{
			velocity.y = minJumpVelocity;
		}
	}

	void HandleWallSliding()
	{
		wallDirectionX = (controller.collisions.left) ? -1 : 1;

		wallSliding = false;
		if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
		{
			wallSliding = true;

			if(velocity.y < -wallSlideSpeedMax)
			{
				velocity.y = -wallSlideSpeedMax;
			}

			if(timeToWallUnStick > 0)
			{
				velocityXSmoothing = 0;
				velocity.x = 0;

				if(directionalInput.x != wallDirectionX && directionalInput.x != 0)
				{
					timeToWallUnStick -= Time.deltaTime;
				}
				else
				{
					timeToWallUnStick = wallStickTime;
				}
			}
			else
			{
				timeToWallUnStick = wallStickTime;
			}
		}
	}

	void CalculateVelocity()
	{
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}
}
