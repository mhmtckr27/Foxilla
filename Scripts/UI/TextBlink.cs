using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextBlink : MonoBehaviour
{
	private Color blinkColor;
	private Color defaultColor;
	private Text blinkText;

	[SerializeField] private float waitDurationBetweenBlinks = .5f;

    void Start()
    {
		blinkText = GetComponent<Text>();
		StartBlinking();
		blinkColor = new Color(0,0,0,0);
		defaultColor = blinkText.color;
    }

	IEnumerator BlinkTheText()
	{
		while (true)
		{
			switch (blinkText.color.a.ToString())
			{
				case "0":
					blinkText.color = new Color(blinkText.color.r, blinkText.color.g, blinkText.color.b, 1);
					yield return new WaitForSeconds(waitDurationBetweenBlinks);
					break;
				case "1":
					blinkText.color = new Color(blinkText.color.r, blinkText.color.g, blinkText.color.b, 0);
					yield return new WaitForSeconds(waitDurationBetweenBlinks);
					break;
			}
		}
	}

	void StartBlinking()
	{
		StopCoroutine(BlinkTheText());
		StartCoroutine(BlinkTheText());
	}
}
