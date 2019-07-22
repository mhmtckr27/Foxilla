using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
	[SerializeField] public int roomType;

	public void DestructRoom()
	{
		Destroy(gameObject);
	}

}
