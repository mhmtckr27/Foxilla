using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAdditionalRooms : MonoBehaviour
{
	[SerializeField] private LayerMask whatIsRoom;
	[SerializeField] private LevelGeneration levelGeneration;
	
    void Update()
    {
		if (Input.GetButtonDown("Jump"))
		{
			Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
			if(roomDetection == null && levelGeneration.isGenerationFinished)
			{
				int rand = Random.Range(0, levelGeneration.rooms.Length);
				Instantiate(levelGeneration.rooms[rand], transform.position, Quaternion.identity);
				Destroy(gameObject);
			}
		}

    }
}
