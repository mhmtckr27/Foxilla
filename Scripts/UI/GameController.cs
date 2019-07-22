using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.LWRP;


public class GameController : MonoBehaviour
{
	[Header ("AppStartup")] [Space]
	[SerializeField] private Light2D pointLight2D;
	[SerializeField] private Light2D globalLight2D;
	[SerializeField] private GameObject PSTPText;
	[SerializeField] private GameObject settingsButton;
	[SerializeField] private GameObject closeButton;
	[SerializeField] private GameObject creditsButton;
	[SerializeField] private Button startButton;
	[SerializeField] private Image background;
	[SerializeField] private Sprite[] backgroundImages;
	//[SerializeField] private GameObject socialsButton;

 
	[Header ("Socials")] [Space]
	[SerializeField] private GameObject socials;
	[SerializeField] private GameObject socialsButton;
	[SerializeField] private float socialsClickTimeLimiter = 1f;
	private float timePassedSinceLastClickSocials;
	private bool isShowingSocials = false;
	private AiryUICustomAnimationElement[] socialButtons; 


	[Header ("Settings")] [Space]
	[SerializeField] private GameObject settings;
	[SerializeField] private float settingsClickTimeLimiter =0.3f;
	private float timePassedSinceLastClickSettings;
	private bool isShowingSettings = false;
	private AiryUICustomAnimationElement[] settingButtons; 
	[SerializeField] private Text joystickInfo;
	[SerializeField] private Text difficultyInfo;
	[SerializeField] private Text graphicsQualityInfo;
	[SerializeField] private GameObject musicOnOffButton;
	[SerializeField] private Image difficultyImage;
	[SerializeField] private Image graphicsQualityImage;
	[SerializeField] private Sprite[] difficultySprites;
	public static float difficulty = 1f;

	[Header ("Music")] [Space]
	[SerializeField] private AudioMixer musicMixer;
	[SerializeField] private Sprite musicOn;
	[SerializeField] private Sprite musicOff;


	[Header ("Language")] [Space]
	[SerializeField] private GameObject languageSelectButton;
	[SerializeField] private Sprite[] languages;
	private Image languageImage;

	public static bool appStartingForFirstTime = true;
	public static bool isControllerTypeJoystick = true;

	private void Awake()
	{
		if(appStartingForFirstTime && SceneManager.GetActiveScene().buildIndex == 0)
		{
			StartCoroutine(appStart());
		}
		if(socials != null)
		{
			socialButtons = socials.GetComponentsInChildren<AiryUICustomAnimationElement>();
		}
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			SelectDefaultLanguage();
		}
		if (settings)
		{
			settingButtons = settings.GetComponentsInChildren<AiryUICustomAnimationElement>();
			if (joystickInfo)
			{
				joystickInfo.text = isControllerTypeJoystick ? " Joystick" : " Buttons";
			}
		}
		
		timePassedSinceLastClickSocials = socialsClickTimeLimiter;
		timePassedSinceLastClickSettings = settingsClickTimeLimiter;
	}
	private void Update()
	{
		timePassedSinceLastClickSocials += Time.deltaTime;
		timePassedSinceLastClickSettings += Time.deltaTime;
	}

	IEnumerator appStart()
	{
		difficulty = PlayerPrefs.GetFloat("Difficulty", 1f);
		difficultyImage.sprite = difficultySprites[difficulty == 1.25f ? 3 : (difficulty == 1f ? 2 : (difficulty == .75f ? 1 : (difficulty == .5f ? 0 : 2)))];
		QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GraphicsQuality", 5));
		graphicsQualityImage.sprite = difficultySprites[QualitySettings.GetQualityLevel()];
		isControllerTypeJoystick = PlayerPrefs.GetInt("ControllerPref", 0) == 0 ? true : false;
		joystickInfo.text = PlayerPrefs.GetString("ControllerPrefText", " Joystick");


		background.sprite = backgroundImages[0];
		startButton.interactable = false;
		settingsButton.GetComponent<Button>().interactable = false;
		socialsButton.GetComponent<Button>().interactable = false;
		closeButton.GetComponent<Button>().interactable = false;
		creditsButton.GetComponent<Button>().interactable = false;


		PSTPText.GetComponent<TextBlink>().enabled = false;
		PSTPText.SetActive(false);

		globalLight2D.enabled = false;
		pointLight2D.enabled = false;

		settingsButton.SetActive(false);
		closeButton.SetActive(false);
		creditsButton.SetActive(false);
		socialsButton.SetActive(false);

		PSTPText.GetComponent<AutoType>().enabled = true;
		yield return new WaitForSeconds(2f);
		//globalLight2D.enabled = true;
		background.sprite = backgroundImages[1];
		GetComponent<AudioSource>().Play();
		settingsButton.SetActive(true);
		closeButton.SetActive(true);
		creditsButton.SetActive(true);
		socialsButton.SetActive(true);
		yield return new WaitForSeconds(1f);
		//pointLight2D.enabled = true;
		background.sprite = backgroundImages[2];
		GetComponent<AudioSource>().Play();
		appStartingForFirstTime=false;
		yield return new WaitForSeconds(.5f);
		PSTPText.SetActive(true);
		yield return new WaitForSeconds(2.75f);
		MusicPlayer.getInstance().isAppStartingFinished = true;
		PSTPText.GetComponent<TextBlink>().enabled = true;

		startButton.interactable = true;
		settingsButton.GetComponent<Button>().interactable = true;
		socialsButton.GetComponent<Button>().interactable = true;
		closeButton.GetComponent<Button>().interactable = true;
		creditsButton.GetComponent<Button>().interactable = true;
	}


	public void ShowSocials()
	{
		if(timePassedSinceLastClickSocials > socialsClickTimeLimiter)
		{
			if(!isShowingSocials)
			{
				socialsButton.GetComponent<AiryUICustomAnimationElement>().ShowElement();
				foreach(AiryUICustomAnimationElement anim in socialButtons)
				{
					anim.ShowElement();
				}
				isShowingSocials = true;
			}
			else
			{
				socialsButton.GetComponent<AiryUICustomAnimationElement>().HideElement();
				foreach(AiryUICustomAnimationElement anim in socialButtons)
				{
					anim.HideElement();
				}
				isShowingSocials = false;
			}
			timePassedSinceLastClickSocials = 0f;
		}
	}
	public void ShowSettings()
	{
		if (!MusicPlayer.getInstance().isMusicOn)
		{
			musicOnOffButton.GetComponent<Image>().sprite = musicOff;
		}
		if(SceneManager.GetActiveScene().buildIndex == 0)
		{
			SelectDefaultLanguage();
			graphicsQualityImage.sprite = difficultySprites[QualitySettings.GetQualityLevel()];
			difficultyImage.sprite = difficultySprites[(difficulty == 1.25f ? 3 : (difficulty == 1f ? 2 : (difficulty == .75f ? 1 : 0)))];
		}

		if(timePassedSinceLastClickSettings > settingsClickTimeLimiter)
		{
			if(!isShowingSettings)
			{
				if (settingsButton)
				{
					settingsButton.GetComponent<AiryUICustomAnimationElement>().ShowElement();
				}
				foreach(AiryUICustomAnimationElement anim in settingButtons)
				{
					anim.ShowElement();
				}
				if (joystickInfo)
				{
					joystickInfo.enabled = true;
					difficultyInfo.enabled = true;
					graphicsQualityInfo.enabled = true;
				}
				isShowingSettings = true;
			}
			else
			{
				if (settingsButton)
				{
					settingsButton.GetComponent<AiryUICustomAnimationElement>().HideElement();
				}
				foreach(AiryUICustomAnimationElement anim in settingButtons)
				{
					anim.HideElement();
				}
				if (joystickInfo)
				{
					joystickInfo.enabled = false;
					difficultyInfo.enabled = false;
					graphicsQualityInfo.enabled = false;
				}

				isShowingSettings = false;
			}
			timePassedSinceLastClickSettings = 0f;
		}
	}

	public void MusicOnOff(GameObject musicOnOffButton)
	{
		if(MusicPlayer.getInstance().isMusicOn)
		{
			musicMixer.SetFloat("musicVolume", -80);
			musicOnOffButton.GetComponent<Image>().sprite = musicOff;
			MusicPlayer.getInstance().isMusicOn = false;
		}
		else
		{
			musicMixer.SetFloat("musicVolume", 0);
			musicOnOffButton.GetComponent<Image>().sprite = musicOn;
			MusicPlayer.getInstance().isMusicOn = true;
		}
	}

	public void SelectControllerType()
	{
		if (isControllerTypeJoystick)
		{
			isControllerTypeJoystick = false;
			joystickInfo.text = " Buttons";
		}
		else
		{
			isControllerTypeJoystick = true;
			joystickInfo.text = " Joystick";
		}
	}

	public void SelectDifficulty()
	{
		switch (difficulty)
		{
			case 0.50f:
				difficulty = 0.75f;
				difficultyImage.sprite = difficultySprites[1];
				break;
			case 0.75f:
				difficulty = 1f;
				difficultyImage.sprite = difficultySprites[2];
				break;
			case 1f:
				difficulty = 1.25f;
				difficultyImage.sprite = difficultySprites[3];
				break;
			case 1.25f:
				difficulty = 0.50f;
				difficultyImage.sprite = difficultySprites[0];
				break;
		}
	}

	public void SelectGraphicsQuality()
	{
		Debug.Log("Before: " + QualitySettings.GetQualityLevel());
		switch (QualitySettings.GetQualityLevel())
		{
			case 0:
				QualitySettings.SetQualityLevel(1);
				graphicsQualityImage.sprite = difficultySprites[1];
				break;
			case 1:
				QualitySettings.SetQualityLevel(2);
				graphicsQualityImage.sprite = difficultySprites[2];
				break;
			case 2:
				QualitySettings.SetQualityLevel(3);
				graphicsQualityImage.sprite = difficultySprites[3];
				break;
			case 3:
				QualitySettings.SetQualityLevel(4);
				graphicsQualityImage.sprite = difficultySprites[4];
				break;
			case 4:
				QualitySettings.SetQualityLevel(5);
				graphicsQualityImage.sprite = difficultySprites[5];
				break;
			case 5:
				QualitySettings.SetQualityLevel(0);
				graphicsQualityImage.sprite = difficultySprites[0];
				break;
		}
		Debug.Log("After: " + QualitySettings.GetQualityLevel());
	}

	private void SelectDefaultLanguage()
	{
		languageImage = languageSelectButton.GetComponent<Image>();

		if (Lean.Localization.LeanLocalization.CurrentLanguage == "Turkish")
		{
			languageImage.sprite = languages[0];
		}
		else if (Lean.Localization.LeanLocalization.CurrentLanguage == "English")
		{
			languageImage.sprite = languages[1];
		}
		else if (Lean.Localization.LeanLocalization.CurrentLanguage == "French")
		{
			languageImage.sprite = languages[3];
		}
		else if (Lean.Localization.LeanLocalization.CurrentLanguage == "German")
		{
			languageImage.sprite = languages[4];
		}
		else
		{
			languageImage.sprite = languages[2];
		}
	}

	public void LanguageSelect()
	{
		int index = System.Array.IndexOf(languages, languageImage.sprite);
		switch (index)
		{
			case 0:
				languageImage.sprite = languages[1];
				Lean.Localization.LeanLocalization.CurrentLanguage = "English";
				break;
			case 1:
				languageImage.sprite = languages[2];
				Lean.Localization.LeanLocalization.CurrentLanguage = "English";
				break;
			case 2:
				languageImage.sprite = languages[3];
				Lean.Localization.LeanLocalization.CurrentLanguage = "French";
				break;
			case 3:
				languageImage.sprite = languages[4];
				Lean.Localization.LeanLocalization.CurrentLanguage = "German";
				break;
			case 4:
				languageImage.sprite = languages[0];
				Lean.Localization.LeanLocalization.CurrentLanguage = "Turkish";
				break;
		}
	}

	public void OpenSocialMediaAccounts(int id)
	{
		switch (id)
		{
			case 0:
				Application.OpenURL("https://www.instagram.com/hayricakirr/");
				break;
			case 1:
				Application.OpenURL("https://twitter.com/hayricakirr");
				break;
			case 2:
				Application.OpenURL("https://www.linkedin.com/in/hayri-çakır-714925127/");
				break;
			case 3:
				Application.OpenURL("https://www.hayricakir.com");
				break;
		}
	}

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(LoadLevelAsynchronously(sceneIndex));
	}

	public void CloseApplication()
	{
		Application.Quit();
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("GraphicsQuality", QualitySettings.GetQualityLevel());
		PlayerPrefs.SetFloat("Difficulty", difficulty);
		PlayerPrefs.SetInt("ControllerPref", isControllerTypeJoystick ? 0 : 1);
		PlayerPrefs.SetString("ControllerPrefText", isControllerTypeJoystick ? " Joystick" : " Buttons");
	}
	IEnumerator LoadLevelAsynchronously(int sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		while (!operation.isDone)
		{
			Debug.Log(operation.progress);
			yield return null;
		}
	}
}
