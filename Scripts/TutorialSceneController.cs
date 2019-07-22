using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialSceneController : MonoBehaviour
{
	[SerializeField] private Animator transitionAnim;
	[SerializeField] private Text levelText;
	[SerializeField] private GameObject nextLevelButton;

	AsyncOperation asyncOperation;

	IEnumerator LoadNextLevel()
	{
		/*transitionAnim.SetTrigger("end");
		asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
		asyncOperation.allowSceneActivation = false;
		//Destroy(nextLevelButton);
		yield return new WaitForSeconds(1f);
		levelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex - 1);
		levelText.gameObject.SetActive(true);*/
		yield return new WaitForSeconds(.5f);
		nextLevelButton.SetActive(false);
		//asyncOperation.allowSceneActivation = true;

	}

	public void OnNextLevelButton()
	{
		StartCoroutine(LoadNextLevel());
	}
}
