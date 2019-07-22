using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMove : MonoBehaviour
{

    [SerializeField] private float speed;

    [SerializeField] private float moveOffset;

    private float t = 0f;
    

    private float minVal;
    private float maxVal;
    
    void Start()
    {
        minVal = -1 * moveOffset;
        maxVal = moveOffset;
    }

    void Update()
    {
        transform.Translate(new Vector3(0, Mathf.Lerp(minVal, maxVal, t)));

        t += speed * Time.fixedDeltaTime;
        if (t > 1f)
        {
            float temp = minVal;
            minVal = maxVal;
            maxVal = temp;
            t = 0f;
        }
    }
}