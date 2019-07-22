using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
	DontDestroyOnLoad dontDestroyOnLoad;
    void Awake()
    {
    	DontDestroyOnLoad(this);

		if (dontDestroyOnLoad == null) 
		{
			dontDestroyOnLoad = this;
		} 
		else 
		{
			Destroy(gameObject);
		}
    }


}
