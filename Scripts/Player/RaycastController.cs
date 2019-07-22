using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
	public LayerMask collisionMask;

	public const float skinWidth = 0.015f;
	const float distanceBetweenRays = .25f;

	[HideInInspector] public int horizontalRayCount;
	[HideInInspector] public int verticalRayCount;

	[HideInInspector] public float horizontalRaySpacing;
	[HideInInspector] public float verticalRaySpacing;

	[HideInInspector] public BoxCollider2D collider;
	[HideInInspector] public CircleCollider2D circleCollider;
	public RaycastOrigins raycastOrigins;
	
	public virtual void Start()
	{
		collider = GetComponent<BoxCollider2D>();
		if (!collider)
		{
			circleCollider = GetComponent<CircleCollider2D>();
		}
		CalculateRaySpacing();
	}

	public void UpdateRaycastOrigins()
	{
		Bounds bounds = new Bounds();
		if (collider)
		{
			bounds = collider.bounds;
		}
		else
		{
			bounds = circleCollider.bounds;
		}
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	public void CalculateRaySpacing()
	{
		Bounds bounds = new Bounds();
		if (collider)
		{
			bounds = collider.bounds;
		}
		else
		{
			bounds = circleCollider.bounds;
		}
		bounds.Expand(skinWidth * -2);

		float boundsWidth = bounds.size.x; 
		float boundsHeight = bounds.size.y; 

		horizontalRayCount = Mathf.RoundToInt(boundsHeight / distanceBetweenRays);
		verticalRayCount = Mathf.RoundToInt(boundsWidth / distanceBetweenRays);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

}
