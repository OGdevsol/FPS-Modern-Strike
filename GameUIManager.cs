using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
	public static GameUIManager instance;

	public GameObject winPanel;
	public GameObject pausePanel;
	public GameObject losePanel;
	public Button nextLevelButton;
	public TMP_Text enemiesKilledTextCount;
	public Toggle AutoAim;
	public Toggle AutoShoot;
	public Image normalCrosshair;
	public Image aimOnEnemyCrosshair;
	public TMP_Text headShotText;

	[HideInInspector] public int enemyBaseCount = 0;
	[HideInInspector] public int enemyDeathCount;

	private void Awake()
	{
		instance = this;


		if (PlayerPrefs.GetInt("DoNotShowAds") != 1)
		{
			Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);

			Advertisement.Banner.Show("GameplayBanner");

			Debug.Log("ShowingGameplayBanner");
		}

		if (PlayerPrefs.GetInt("SelectedLevel") >= 9)
		{
			nextLevelButton.interactable = false;
		}
		CheckPlayerSettings();
		headShotText.text = "Headshots: " + PlayerPrefs.GetInt("Headshots");
	}

	private void CheckPlayerSettings()
	{
		AutoShoot.isOn = PlayerPrefs.GetInt("AutoShoot")==1;
		AutoAim.isOn = PlayerPrefs.GetInt("AutoAim")==1;
	}

	public void UpdateEnemiesKilledText()
	{
		enemiesKilledTextCount.text = enemyBaseCount + " - " + enemyDeathCount;
		enemiesKilledTextCount.GetComponent<Animation>().Play("KillAnim");
	}

	public IEnumerator LevelCompleteRoutine()
	{
		SoundController.instance.playFromPool(AudioType.GameWinPanelEffect);
		yield return new WaitForSecondsRealtime(3f);


		winPanel.SetActive(true);
		if (pausePanel.activeInHierarchy)
		{
			pausePanel.SetActive(false);
		}

		if (PlayerPrefs.GetInt("SelectedLevel") >= PlayerPrefs.GetInt("Max"))

		{
			if (PlayerPrefs.GetInt("Max") < 9)
			{
				PlayerPrefs.SetInt("Max", PlayerPrefs.GetInt("SelectedLevel") + 1);
				Debug.Log(PlayerPrefs.GetInt("Max"));
			}
		}

		Advertisement.Banner.Hide(true);
	}

	public IEnumerator LevelFailRoutine()
	{
		SoundController.instance.playFromPool(AudioType.Gamelose1);
		yield return new WaitForSecondsRealtime(1.5f);
		losePanel.SetActive(true);
	}

	public void NEXTButtonClick()
	{
		if (PlayerPrefs.GetInt("DoNotShowAds") != 1)
		{
			UnityAds.instance.LoadNextLevelInterstitialAd();
		}

		PlayerPrefs.SetInt("SelectedLevel", PlayerPrefs.GetInt("SelectedLevel") + 1);
		SceneManager.LoadScene("Gameplay");
		Debug.Log("HidingGameplayBanner");
	}

	private IEnumerator NextLevelRoutine()
	{
		yield return new WaitForSeconds(2f);
	}

	public void RESUMEButtonClick()
	{
		pausePanel.SetActive(false);
		Time.timeScale = 1;
	}

	public void PAUSEButtonClick()
	{
		pausePanel.SetActive(true);
		Time.timeScale = 0;
	}

	public void HOMEButtonClick()
	{
		if (PlayerPrefs.GetInt("DoNotShowAds") != 1)
		{
			UnityAds.instance.LoadAndroidInterstitialAd();
		}

		SceneManager.LoadScene("Menu");
		Time.timeScale = 1;
	}

	public void RESTARTButtonClick()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("Gameplay");
		Time.timeScale = 1;
	}

	public void AUTOSHOOT()
	{
		switch (AutoShoot.isOn)
		{
			case true:
				PlayerPrefs.SetInt("AutoShoot",1);
				break;
			case false:
				PlayerPrefs.SetInt("AutoShoot",0);
				break;
		}
		Debug.Log(AutoShoot.isOn);
		Debug.Log(PlayerPrefs.GetInt("AutoShoot"));
		
		//CheckPlayerSettings();
	}

	public void AUTOAIM()
	{
		switch (AutoAim.isOn)
		{
			case true:
				PlayerPrefs.SetInt("AutoAim",1);
				break;
			case false:
				PlayerPrefs.SetInt("AutoAim",0);
				break;
		}
		Debug.Log(AutoAim.isOn);
		Debug.Log(PlayerPrefs.GetInt("AutoAim"));
		//CheckPlayerSettings();
	}
}