using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{

	[SerializeField] private Sprite star;
	[SerializeField] private Sprite restart;
	[SerializeField] private Button[] levelButtons;
	[SerializeField] private Button[] backgroundButtons;

	[SerializeField] private Animator transitionAnim;
	[SerializeField] private AutoType levelText;

	AsyncOperation asyncOperation;

	void Awake()
	{
		//PlayerPrefs.DeleteAll();
		int levelReached = PlayerPrefs.GetInt("levelReached", 1);

		for (int i = 0; i < levelReached; i++)
		{
			backgroundButtons[i].interactable = true;
			if(PlayerPrefs.GetInt("levelReached", 1) > (i + 1))
			{
				levelButtons[i].GetComponent<Image>().sprite = restart;
			}
			levelButtons[i].GetComponent<ButtonControl>().UnlockLevel();
		}
	}

	public void SelectLevel(int sceneIndex)
	{
		StartCoroutine(LoadSceneParallel(sceneIndex));
	}

	IEnumerator LoadSceneParallel(int sceneIndex)
	{
		foreach (Button levelButton in levelButtons)
		{
			levelButton.interactable = false;
		}
		transitionAnim.SetTrigger("fadeToLevel");
		asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;
		yield return new WaitForSeconds(1f);
		levelText.gameObject.GetComponent<Text>().text = " Level " + (sceneIndex - 2);
		levelText.gameObject.SetActive(true);
		yield return new WaitForSeconds(2f);
		asyncOperation.allowSceneActivation = true;
	}
}
