using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanLocalizationDontDestroy : MonoBehaviour
{
	private static LeanLocalizationDontDestroy leanLocalizationDontDestroy;

    void Awake()
    {
		DontDestroyOnLoad(this);

		if (leanLocalizationDontDestroy == null) 
		{
			leanLocalizationDontDestroy = this;
		} 
		else 
		{
			Destroy(gameObject);
		}
    }
}
