using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlatformParent : MonoBehaviour
{
	[SerializeField] private GameObject child;
	private Vector3 childPosition;
	private float animDuration = 0.5f;

	void Start()
	{
		childPosition = child.transform.position;
	}
    void Update()
    {
		if (child == null)
		{
			GetComponent<BoxCollider2D>().size = new Vector2(0,0);
			//transform.position=childPosition;
			GetComponent<Animator>().SetTrigger("Break");
			Destroy(gameObject,animDuration);
		}
    }
}
