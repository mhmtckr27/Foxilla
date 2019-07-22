using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; 

public class AutoType : MonoBehaviour {
 
	public float letterPause = 0f;
	public AudioSource sound;
 
	private TextMeshProUGUI text;
	private Text textAlt;
	string message;
	[SerializeField] private bool deleteTextAfterComplete;
 
	// Use this for initialization
	void Start () {
		text = GetComponent<TextMeshProUGUI>();
		if (text)
		{
			message = text.text;
			text.text = "";
			StartCoroutine(TypeTextMeshProUGUIText ());
		}
		else
		{
			textAlt = GetComponent<Text>();
			message = textAlt.text;
			textAlt.text = "";
			StartCoroutine(TypeText());
		}
	}
 
	IEnumerator TypeTextMeshProUGUIText () 
	{
		foreach (char letter in message.ToCharArray()) 
		{
			text.text += letter;
			if (sound)
			{
				sound.PlayOneShot(sound.clip);
				yield return 0;
			}
			yield return new WaitForSeconds (letterPause);
		}
		yield return new WaitForSeconds (1.5f);
		if (deleteTextAfterComplete)
		{
			text.gameObject.SetActive(false);
		}
	}

	IEnumerator TypeText ()
	{
		foreach (char letter in message.ToCharArray()) 
		{
			textAlt.text += letter;
			if (sound)
			{
				sound.PlayOneShot(sound.clip);
				yield return 0;
			}
			yield return new WaitForSeconds (letterPause);
		}
		yield return new WaitForSeconds (1.5f);
		if (deleteTextAfterComplete)
		{
			textAlt.gameObject.SetActive(false);
		}
	}
}