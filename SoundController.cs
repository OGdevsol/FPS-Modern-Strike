using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;


public class SoundController : MonoBehaviour
{
	
	private float sfxVolume;
	private float musicVolume;

	public AudioSource audioMusic;
	public AudioSource[] gameAudioSources;
	
	public AudioClip[] musicSounds;

	public AudioPool[] pool;
	
	private Dictionary<AudioType, AudioPool> audioList;
	private static SoundController _instance;
	public AudioClip[] environmentVoices;
	public Slider MainMenuSFXSlider;
	public Slider MainMenuMUSICSlider;
	
	
	public static SoundController instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<SoundController>();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		
		GameObject gameObject = new GameObject();
		gameObject.name = "AudioPool";
		audioList = new Dictionary<AudioType, AudioPool>();
		foreach (AudioPool audioPool in pool)
		{
			audioPool.length = audioPool.clips.Length;
			audioPool.sources = new AudioSource[audioPool.length];
			for (int j = 0; j < audioPool.length; j++)
			{
				AudioClip audioClip = audioPool.clips[j];
				GameObject gameObject2 = new GameObject();
				gameObject2.name = audioClip.name;
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.AddComponent<AudioSource>();
				gameObject2.GetComponent<AudioSource>().clip = audioClip;
				audioPool.sources[j] = gameObject2.GetComponent<AudioSource>();
			}
			audioList[audioPool.type] = audioPool;
		}

		if (!PlayerPrefs.HasKey("MenuSfx"))
		{
			if (MainMenuSFXSlider!=null)
			{
				MainMenuSFXSlider.value = 1;
			}
			
		}
		else
		{
			if (MainMenuSFXSlider!=null)
			{
				MainMenuSFXSlider.value = PlayerPrefs.GetFloat("MenuSfx");
			}
			
		}if (!PlayerPrefs.HasKey("MenuMusic"))
		{
			if (MainMenuMUSICSlider!=null)
			{
				MainMenuMUSICSlider.value = 1;
			}
			
		}
		else
		{
			if (MainMenuMUSICSlider!=null)
			{
				MainMenuMUSICSlider.value = PlayerPrefs.GetFloat("MenuMusic");
			}
			
		}

		
		
		

		
		
	}

	public void playFromPool(AudioType audioType)
	{
		audioList[audioType].play();
	}

	public void SFXVolumeChangedMenu(float newVolume)
	{
		
		newVolume = MainMenuSFXSlider.value;
		sfxVolume = newVolume;
		foreach (KeyValuePair<AudioType, AudioPool> keyValuePair in audioList)
		{
			keyValuePair.Value.setVolume(newVolume);
		}

		foreach (AudioSource item in gameAudioSources)
		{
			item.volume = newVolume;
		}

		MainMenuSFXSlider.value = newVolume;
		PlayerPrefs.SetFloat("MenuSfx",MainMenuSFXSlider.value);

		
		
	}public void MusicVolumeChangedMenu(float newVolume)
	{
		
		newVolume = MainMenuMUSICSlider.value;
		audioMusic.volume = newVolume;
		/*foreach (KeyValuePair<AudioType, AudioPool> keyValuePair in audioList)
		{
			keyValuePair.Value.setVolume(newVolume);
		}

		foreach (AudioSource item in gameAudioSources)
		{
			item.volume = newVolume;
		}*/

		MainMenuMUSICSlider.value = newVolume;
		PlayerPrefs.SetFloat("MenuMusic",MainMenuMUSICSlider.value);

		
		
	}

	public void musicVolumeChanged(int newVolume)
	{
		musicVolume = newVolume;
		audioMusic.volume = newVolume;
		
	}

	public void startMusic()
	{
		
			StartCoroutine(EnvironmentVoices());
		
		
	}

	private IEnumerator EnvironmentVoices()
	{
		yield return new WaitForSeconds(4f);
		audioMusic.PlayOneShot(environmentVoices[Random.Range(0, environmentVoices.Length)]);
		audioMusic.Play();
		StartCoroutine(EnvironmentVoices());
		
		
	}

	

}
