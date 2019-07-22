using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameController : MonoBehaviour
{

	[SerializeField] public int cherriesCollected = 0;
	[SerializeField] private int totalCherries = 0;
	[SerializeField] private int starsCollected = 0;
	[SerializeField] public int deathCounter = 0;

	[Space] [Header("Level Finished Screen Stuff")] [Space]
	[SerializeField] private GameObject levelFinishedScreen;
	[SerializeField] private Sprite starSprite;
	[SerializeField] TextMeshProUGUI[] cherryAndDeathCountText;
	[SerializeField] private Image[] levelFinishedScreenStars;

	[SerializeField] private Animator transitionAnim;
	[SerializeField] private Text levelText;
	[SerializeField] private Button homeButton;
	[SerializeField] private Button nextLevelButton;
	AsyncOperation asyncOperation;

	public bool isLevelFinished;
	public TextMeshProUGUI deathText;

	private void Start()
	{
		deathText.text = "x" + PlayerPrefs.GetInt("deathCountTemp" +  (SceneManager.GetActiveScene().buildIndex - 1), 0);
	}
	public void LevelFinished()
	{
		isLevelFinished = true;
		++starsCollected;
		if(cherriesCollected == totalCherries)
		{
			starsCollected = 3;
		}
		else if(((double) cherriesCollected / totalCherries) > (0.49))
		{
			++starsCollected;
		}


		cherryAndDeathCountText[0].text = "x" + cherriesCollected;
		cherryAndDeathCountText[1].text = "x" + PlayerPrefs.GetInt("deathCountTemp" +  (SceneManager.GetActiveScene().buildIndex - 1), 0);
		for (int i = 0; i < starsCollected; i++)
		{
			levelFinishedScreenStars[i].sprite = starSprite;
		}

		levelFinishedScreen.SetActive(true);
		StartCoroutine(PauseGame());


		if(PlayerPrefs.GetInt("cherriesCollected" + (SceneManager.GetActiveScene().buildIndex - 1), 0) == cherriesCollected)
		{
			if(PlayerPrefs.GetInt("deathCount" + (SceneManager.GetActiveScene().buildIndex - 1), int.MaxValue) >= PlayerPrefs.GetInt("deathCountTemp" + (SceneManager.GetActiveScene().buildIndex - 1), 0))
			{
				PlayerPrefs.SetInt("deathCount" + (SceneManager.GetActiveScene().buildIndex - 1), PlayerPrefs.GetInt("deathCountTemp" + (SceneManager.GetActiveScene().buildIndex - 1), 0));
			}
		}
		else if(PlayerPrefs.GetInt("cherriesCollected" + (SceneManager.GetActiveScene().buildIndex - 1), 0) < cherriesCollected)
		{
			PlayerPrefs.SetInt("cherriesCollected" + (SceneManager.GetActiveScene().buildIndex - 1), cherriesCollected);
			PlayerPrefs.SetInt("deathCount" + (SceneManager.GetActiveScene().buildIndex - 1), PlayerPrefs.GetInt("deathCountTemp" + (SceneManager.GetActiveScene().buildIndex - 1), 0));
		}
		if(PlayerPrefs.GetInt("starsCollected" + (SceneManager.GetActiveScene().buildIndex - 1), 0) < starsCollected)
		{
			PlayerPrefs.SetInt("starsCollected" + (SceneManager.GetActiveScene().buildIndex - 1), starsCollected);
		}

		if(PlayerPrefs.GetInt("levelReached", 1) == (SceneManager.GetActiveScene().buildIndex - 1))
		{
			PlayerPrefs.SetInt("levelReached", PlayerPrefs.GetInt("levelReached", 1) + 1);
		}
		PlayerPrefs.DeleteKey("deathCountTemp" +  (SceneManager.GetActiveScene().buildIndex - 1));
	}


	//sceneIndexes
	// 0 = Load start scene (Home button)
	// 1 = Load same scene (Restart level)
	// 2 = Load next level.
	public void LoadScene(int sceneIndex)
	{
		switch (sceneIndex)
		{
			case 0:
				Time.timeScale = 1;
				StartCoroutine(LoadSceneParallel(0));
				break;
			case 1:
				Time.timeScale = 1;
				StartCoroutine(LoadSceneParallel(1));
				break;
			case 2:
				Time.timeScale = 1;
				StartCoroutine(LoadSceneParallel(2));
				break;
			default:
				break;
		}
	}

	IEnumerator LoadSceneParallel(int sceneIndex)
	{
		switch (sceneIndex)
		{
			case 0:
				homeButton.interactable = false;
				transitionAnim.SetTrigger("home");
				asyncOperation = SceneManager.LoadSceneAsync(1);
				asyncOperation.allowSceneActivation = false;
				yield return new WaitForSeconds(1f);
				asyncOperation.allowSceneActivation = true;
				break;
			case 1:
				SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
				break;
			case 2:
				nextLevelButton.interactable = false;
				transitionAnim.SetTrigger("end");
				asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
				asyncOperation.allowSceneActivation = false;
				yield return new WaitForSeconds(1f);
				if(SceneManager.GetActiveScene().buildIndex + 1 != 6)
				{
					//Credits ekranı yüklenmiyorsa level ekranı yükleniyordur.
					levelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex - 1);
				}
				levelText.gameObject.SetActive(true);
				yield return new WaitForSeconds(2f);
				asyncOperation.allowSceneActivation = true;
				break;
			default:
				break;
		}
	}

	IEnumerator PauseGame()
	{
		yield return new WaitForSeconds(.5f);
		Time.timeScale = 0;
	}
}
