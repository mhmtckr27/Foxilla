using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlatform : MonoBehaviour
{
	private float timeToDestroy = 1f;

	void OnCollisionEnter2D(Collision2D other)
	{
		Destroy(gameObject,timeToDestroy);
	}
}
