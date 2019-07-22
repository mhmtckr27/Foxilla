using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
	private static MusicPlayer musicPlayer;
	[SerializeField] private AudioClip[] soundTracks;
	[SerializeField] private bool[] isPlayed;
	public int playedTotal = 0;
	public bool isAppStartingFinished = false;

	[SerializeField] private AudioSource mainSource;
	[SerializeField] private AudioSource creditsSource;
	public bool isMusicOn = true;

	public static MusicPlayer getInstance()
	{
		return musicPlayer;
	}
	private void Awake()
	{
		DontDestroyOnLoad(this);

		if (musicPlayer == null) 
		{
			musicPlayer = this;
		} 
		else 
		{
			Destroy(gameObject);
		}
	}
	private AudioClip GetRandomTrack()
	{
		int index;
		if(playedTotal == soundTracks.Length)
		{
			for (int i = 0; i < isPlayed.Length; i++)
			{
				isPlayed[i] = false;
			}
			playedTotal = 0;
		}
		do
		{
			index = Random.Range(0, soundTracks.Length);
		} while (isPlayed[index]);
		isPlayed[index] = true;
		++playedTotal;
		return soundTracks[index];
	}

	private bool playEndGameClip = false;

	private void Update()
	{
		if (isAppStartingFinished && !mainSource.isPlaying && !creditsSource.isPlaying)
		{
			if (!playEndGameClip)
			{
				mainSource.clip = GetRandomTrack();
				mainSource.Play();
			}
			else
			{
				Invoke("CallUnPause", 1f);
			}
		}
		if(!playEndGameClip && SceneManager.GetActiveScene().buildIndex == 6)
		{
			playEndGameClip = true;
			mainSource.Stop();
			//GetComponent<AudioSource>().clip = soundTracks[7];
			creditsSource.Play();
		}
	}
	
	private void CallUnPause()
	{
		mainSource.Play();
		playEndGameClip = false;	
	}

	public AudioSource GetAudioSource()
	{
		return creditsSource;
	}
}
