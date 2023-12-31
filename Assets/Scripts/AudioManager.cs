using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance {
		get;
		set;
	}
   
	public Sound[] musicSounds, sfxSounds;
	public AudioSource musicSource, sfxSource;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			DontDestroyOnLoad(gameObject);
		}else
		{
			Destroy(gameObject);
		}
	}

	public void PlayMusic(string name)
   {
	   Sound s = Array.Find(musicSounds, x => x.name == name);
		
	   if (s == null)
	   {
		   Debug.Log("Sound Not Found");
	   }
	   else
	   {
		   musicSource.Stop();
		   musicSource.clip = s.clip;
		   musicSource.Play();
	   }
   }
   
}
