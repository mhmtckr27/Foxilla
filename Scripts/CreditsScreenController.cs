using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreenController : MonoBehaviour
{
	[SerializeField] private GameObject creditsText;


	void Update()
	{
		if (creditsText.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !creditsText.GetComponent<Animator>().IsInTransition(0))
		{
			StartCoroutine(wait());
		}
	}

	IEnumerator wait()
	{
		yield return new WaitForSeconds(3f);
		MusicPlayer.getInstance().GetAudioSource().Stop();
		SceneManager.LoadSceneAsync(0);
	}
}
