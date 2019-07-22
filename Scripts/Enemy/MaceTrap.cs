using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceTrap : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private Transform checkDown;
    [SerializeField] private float gravityScale;
	public LayerMask playerLayer;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
		if (FindObjectOfType<InGameController>().isLevelFinished)
		{
			Destroy(gameObject);
		}
        RaycastHit2D ray = Physics2D.Raycast(checkDown.position, Vector2.down);
        if (ray && ray.collider.tag=="Player")
        {
            rb2d.bodyType=RigidbodyType2D.Dynamic;
            rb2d.gravityScale = gravityScale;
        }
    }
}
