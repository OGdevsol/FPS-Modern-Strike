using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
	public static MainMenuManager instance;
	public GameObject[] references;
	public Button[] levelButtons;
	public GameObject LoadingPanel;
	public Button removeAllAdsButton;
	public TMP_Text adsWatchedText;

	private void Awake()
	{
		instance = this;

		if (PlayerPrefs.GetInt("DoNotShowAds") != 1)
		{
			Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
			Advertisement.Banner.Show("MainMenuBanner");

			Debug.Log("ShowingMainMenuBanner");
		}

		if (PlayerPrefs.GetInt("DoNotShowAds") == 1)
		{
			removeAllAdsButton.transform.gameObject.SetActive(false);
			Advertisement.Banner.Hide(true);
			Debug.Log("HidingMainMenuBanner");
		}

		Debug.LogError(PlayerPrefs.GetInt("Unlockable"));
	}

	private void Start()
	{
	//	LevelsLock_Unlock();
	}

	

	private void OnDisable()
	{
		Advertisement.Banner.Hide(true);
	}

	public void DoNotShowAds()
	{
		UnityAds.instance.LoadRewardedAdForDoNotShowAds();
	}

	public void EnableReference(int index)
	{
		int i;
		int referencesLength = references.Length;
		for (i = 0; i < referencesLength; i++)
		{
			references[i].SetActive(false);
		}

		references[index].SetActive(true);
		if (index == 1)
		{
			LevelsLock_Unlock();
			Debug.Log("ON ENABLING NOW");
		}

		if (index==5)
		{
			CustomWeaponSelector.instance.CheckIfWeaponIsLocked();
		}

		SoundController.instance.playFromPool(AudioType.UIclick);
	}

	public void SelectLevel(int levelIndex)
	{
		PlayerPrefs.SetInt("SelectedLevel", levelIndex);
	}

	private Vector3 newScale;

	public void LevelSelectedState()
	{
		newScale = new Vector3(1.07f, 1.07f, 1.07f);
		int j;
		int buttonsLength = levelButtons.Length;
		for (j = 0; j < buttonsLength; j++)


		{
			levelButtons[j].transform.gameObject.transform.GetChild(0).gameObject.SetActive(false);

			levelButtons[j].transform.gameObject.transform.localScale = new Vector3(1, 1, 1);
		}


		levelButtons[PlayerPrefs.GetInt("SelectedLevel")].transform.gameObject.transform.GetChild(0).gameObject
			.SetActive(true);
		
		levelButtons[PlayerPrefs.GetInt("SelectedLevel")].GetComponent<Animation>().Play("ButtonScaleZoomInAnim");
	}

	public void PlayButtonClick()
	{
		StartCoroutine("Loading");
	}

	private IEnumerator Loading()
	{
		LoadingPanel.SetActive(true);
		yield return new WaitForSecondsRealtime(3f);
		Advertisement.Banner.Hide();
		SceneManager.LoadScene("Gameplay");
	}

	public void LevelsLock_Unlock()
	{
		Debug.LogError(PlayerPrefs.GetInt("Max"));
		for (var i = 0; i <= PlayerPrefs.GetInt("Max"); i++)
		{
			levelButtons[i].transform.GetChild(3).gameObject.SetActive(false);
			levelButtons[i].interactable = true;

			Debug.Log("UnlockingTill " + i);
		}
	}
}