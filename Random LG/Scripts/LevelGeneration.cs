using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
	[SerializeField] private Transform[] startingPositions;

	[Tooltip ("0 = LR\n1 = LRB\n2 = LRT\n3 = LRBT")]
	[SerializeField] public GameObject[] rooms;
	[SerializeField] private float moveAmount;

	private int moveDirection;
	
	[SerializeField] private float startTimeBetweenRoom = .25f;
	private float timeBetweenRoom;

	[SerializeField] private int minX;
	[SerializeField] private int maxX;
	[SerializeField] private int minY;

	[SerializeField] private LayerMask roomLayerMask;
	public bool isGenerationFinished;

	private int downCounter;

	void Start()
	{
		int randStartingPos = Random.Range (0, startingPositions.Length);
		transform.position = startingPositions[randStartingPos].position;
		Instantiate(rooms[0], transform.position, Quaternion.identity);

		//we increase the chance that direction will be right or left than down.
		moveDirection = Random.Range(1, 6);
	}

	void Update()
	{
		if (timeBetweenRoom <= 0 && !isGenerationFinished)
		{
			Move();
			timeBetweenRoom = startTimeBetweenRoom;
		}
		else
		{
			timeBetweenRoom -= Time.deltaTime;
		}
	}

	void Move()
	{
		if (moveDirection == 1 || moveDirection == 2) //move to right
		{
			if(transform.position.x < maxX)
			{
				Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
				transform.position = newPos;

				int rand = Random.Range(0, rooms.Length);
				Instantiate(rooms[rand], transform.position, Quaternion.identity);

				moveDirection = Random.Range(1, 6);
				if(moveDirection == 3)
				{
					moveDirection = 2;
				}
				else if(moveDirection == 4)
				{
					moveDirection = 5;
				}
			}
			else
			{
				moveDirection = 5;
			}
		}
		else if (moveDirection == 3 || moveDirection == 4) //move to left
		{
			if(transform.position.x > minX)
			{
				downCounter = 0;

				Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
				transform.position = newPos;

				int rand = Random.Range(0, rooms.Length);
				Instantiate(rooms[rand], transform.position, Quaternion.identity);

				moveDirection = Random.Range(3, 6);
			}
			else
			{
				moveDirection = 5;
			}
		}
		else if(moveDirection == 5) //move down
		{
			++downCounter;

			if(transform.position.y > minY)
			{
				Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, roomLayerMask);
				if(roomDetection.GetComponent<RoomType>().roomType != 1 && roomDetection.GetComponent<RoomType>().roomType != 3)
				{

					if(downCounter >= 2)
					{
						roomDetection.GetComponent<RoomType>().DestructRoom();
						Instantiate(rooms[3], transform.position, Quaternion.identity);
					}
					else
					{
						roomDetection.GetComponent<RoomType>().DestructRoom();

						int randBottomRoom = Random.Range(1, 4);
						if(randBottomRoom == 2)
						{
							randBottomRoom = 1;
						}
						Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
					}

				}

				Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
				transform.position = newPos;

				int rand = Random.Range(2, 4);
				Instantiate(rooms[rand], transform.position, Quaternion.identity);

				moveDirection = Random.Range(1, 6);
			}
			else
			{
				// level generation ends here.
				isGenerationFinished = true;
			}
		}
	}
}
