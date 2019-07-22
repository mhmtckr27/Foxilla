using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private bool facingRight;
	[SerializeField] private bool facingUp;
    [SerializeField] private float moveOffset;
    [SerializeField] private bool isFrog;
    [SerializeField] private bool isOpossum;
	[SerializeField] private bool isSaw;
	[SerializeField] private bool isEagle;
    [SerializeField] private bool isPatrolVertical;
	[SerializeField] private float sawRotateMultiplier;
    private Vector3 initialPosition;
	public LayerMask playerLayer;
    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {

		if(isSaw)
		{
			transform.Rotate(0, 0, GetComponent<SpriteRenderer>().flipY ? -sawRotateMultiplier * Time.deltaTime : sawRotateMultiplier * Time.deltaTime, Space.Self);
		}
/*
        if (isFrog)
        {
            FrogPatrols();
        }
        else if(isOpossum)
        {
            OpossumPatrols();
        }
        else
        {
			SawPlatformEaglePatrols();
        }*/
    }

    void FrogPatrols()
    {
        transform.Translate(new Vector3(-1,1,0) * speed * Time.deltaTime);
        FrogOpossumCommonCode();
    }

    void OpossumPatrols()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        FrogOpossumCommonCode();
    }

    void SawPlatformEaglePatrols()
    {
        if (isPatrolVertical)
        {
			if (facingUp)
            {
				transform.Translate(Vector3.up * speed * Time.deltaTime,Space.World);
				if (isSaw)
				{
					transform.Rotate(0,0,-sawRotateMultiplier * Time.fixedDeltaTime,Space.Self);
				}
                if (transform.position.y - moveOffset > initialPosition.y)
				{
					facingUp = false;
				}
            }
			else
			{
				transform.Translate(Vector3.down * speed * Time.deltaTime,Space.World);
				if (isSaw)
				{
					transform.Rotate(0,0,sawRotateMultiplier * Time.fixedDeltaTime,Space.Self);
				}
				if (transform.position.y + moveOffset < initialPosition.y)
				{
					facingUp = true;
				}
			}
        }
        else
        {
            if (facingRight)
            {
				transform.Translate(Vector3.right * speed * Time.deltaTime,Space.World);
				if (isSaw)
				{
					transform.Rotate(0,0,-sawRotateMultiplier * Time.fixedDeltaTime,Space.Self);
				}
                if (transform.position.x - moveOffset > initialPosition.x)
				{
					facingRight = false;
					if (isEagle)
					{
						transform.eulerAngles = new Vector2(0, 0);
					}
				}
            }
			else
			{
				transform.Translate(Vector3.left * speed * Time.deltaTime,Space.World);
				if (isSaw)
				{
					transform.Rotate(0,0,sawRotateMultiplier * Time.fixedDeltaTime,Space.Self);
				}
				if (transform.position.x + moveOffset < initialPosition.x)
				{
					facingRight = true;
					if (isEagle)
					{
						transform.eulerAngles = new Vector2(0, 180);
					}
				}
			}
        }
    }

    void FrogOpossumCommonCode()
    {
        if (!facingRight)
        {
            if (transform.position.x + moveOffset < initialPosition.x)
            {
                transform.eulerAngles = new Vector2(0, 180);
                facingRight = true;
            }
        }
        else
        {
            if (transform.position.x - moveOffset > initialPosition.x)
            {
                transform.eulerAngles = new Vector2(0, 0);
                facingRight = false;
            }
        }
    }
}
