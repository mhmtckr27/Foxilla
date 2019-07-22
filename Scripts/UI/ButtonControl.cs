using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonControl : MonoBehaviour
{
	[SerializeField] private GameObject lockObject;
	[SerializeField] private GameObject cherryText;
	[SerializeField] private GameObject deathText;

	[SerializeField] private Sprite starImage;
	[SerializeField] private GameObject[] starFields;

	public void UnlockLevel()
	{
		Color tempColor;

		//Make level locked image transparent
		tempColor = lockObject.GetComponent<Image>().color;
		tempColor.a = 0;
		lockObject.GetComponent<Image>().color = tempColor;

		//Make play button visible
		gameObject.SetActive(true);

		//Make cherry collected text and death counter text visible and set text to appropriate value.
		cherryText.GetComponent<TextMeshProUGUI>().text = "x" + PlayerPrefs.GetInt("cherriesCollected" + this.tag, 0);
		deathText.GetComponent<TextMeshProUGUI>().text = "x" + PlayerPrefs.GetInt("deathCount" + this.tag, 0);

		for (int i = 0; i < PlayerPrefs.GetInt("starsCollected" + this.tag, 0); i++)
		{
			starFields[i].GetComponent<Image>().sprite = starImage;
		}
	}
}
