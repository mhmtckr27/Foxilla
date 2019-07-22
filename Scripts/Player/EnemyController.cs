using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CircleCollider2D))]
public class EnemyController : RaycastController
{
	[SerializeField] private Vector3[] localWaypoints;
	Vector3[] globalWaypoints;
	
	[SerializeField] private float speed;
	[SerializeField] private float difficulty = 1;
	[SerializeField] private bool cyclic;
	[SerializeField] private float waitTime;
	[Range(0,2)] [SerializeField] private float easeAmount;

	[SerializeField] private bool isSaw;

	int fromWaypointIndex;
	float percentBetweenWaypoints;
	float nextMoveTime;
	bool facingRight;

    public override void Start()
    {
        base.Start();

		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < localWaypoints.Length; i++)
		{
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}

		difficulty = GameController.difficulty;
    }


    void Update()
    {
		UpdateRaycastOrigins();
		 Vector3 velocity = new Vector3();
		if(globalWaypoints.Length != 0)
		{
			velocity = CalculatePlatformMovement();
			transform.Translate(velocity);
		}
		if(!facingRight && velocity.x > 0)
		{
			facingRight = true;
			GetComponentInChildren<SpriteRenderer>().flipX = true;
		}
		else if(facingRight && velocity.x < 0)
		{
			facingRight = false;
			GetComponentInChildren<SpriteRenderer>().flipX = false;
		}
    }

	float Ease(float x)
	{
		float a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
	}

	
	Vector3 CalculatePlatformMovement()
	{
		if(Time.time < nextMoveTime)
		{
			return Vector3.zero;
		}
		fromWaypointIndex %= globalWaypoints.Length;
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * difficulty * speed / distanceBetweenWaypoints;
		percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
		float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

		Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

		if(percentBetweenWaypoints >= 1)
		{
			percentBetweenWaypoints = 0;
			++fromWaypointIndex;

			if(!cyclic)
			{
				if(fromWaypointIndex >= globalWaypoints.Length - 1)
				{
					fromWaypointIndex = 0;
					System.Array.Reverse(globalWaypoints);
				}
			}
			nextMoveTime = Time.time + waitTime;

		}

		return newPos - transform.position;
	}
	
	void OnDrawGizmos()
	{
		if(localWaypoints != null)
		{
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i = 0; i < localWaypoints.Length; i++)
			{
				Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}
}
