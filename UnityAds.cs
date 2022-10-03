using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class UnityAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
	public WeaponsStatsScriptables currentWeapon;
	public static UnityAds instance;
	private string gID;
	[SerializeField] private string androidGameID;
	[SerializeField] private string iosGameid;
	[SerializeField] private bool testMode = true;
	public bool doNotShowAds;

	public void InitializeAds()
	{
		gID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameid : androidGameID;
		Advertisement.Initialize(gID, testMode, this);
	}

	private void Awake()
	{
		instance = this;
		//	DontDestroyOnLoad(this);
		if (Advertisement.isInitialized)
		{
			LoadNextLevelInterstitialAd();
			LoadAndroidInterstitialAd();
			LoadBannerAd();
			LoadRewardedAdForDoNotShowAds();
			LoadRewardedAdForBuyingGuns();
		}
		else
		{
			InitializeAds();
		}
		//	Advertisement.Banner.Show("Banner_Android");
	}

	private void OnDisable()
	{
		Advertisement.Banner.Hide(true);
	}

	public void OnInitializationComplete()
	{
		Debug.Log("Initialization Complete");
	}


	#region InterstitialAds / VideoComplete Functionality

	public void OnInitializationFailed(UnityAdsInitializationError error, string message)
	{
		Debug.Log("Initialization Failed");
	}

	public void LoadNextLevelInterstitialAd()
	{
		//Advertisement.Load("Interstitial_Android", this);
		Advertisement.Load("NewLevel", this);
		Debug.Log("NewLvlInt");
	}

	public void LoadAndroidInterstitialAd()
	{
		//Advertisement.Load("Interstitial_Android", this);
		Advertisement.Load("Interstitial_Android", this);
		Debug.Log("AndroidInt");
	}


	public void OnUnityAdsAdLoaded(string placementId)
	{
		Advertisement.Show(placementId, this);
		Debug.Log("OnUnityAdLoaded");
	}

	public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
	{
		Debug.Log("Loading Failed");
	}

	public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
	{
		Debug.Log("OnUnityAdShowFailure");
	}

	public void OnUnityAdsShowStart(string placementId)
	{
		Advertisement.Banner.Hide(true);
		Debug.Log("OnUnityAdShowStart" + placementId);
		Time.timeScale = 0;
	}

	public void OnUnityAdsShowClick(string placementId)
	{
		Debug.Log("OnUnityAdShowClick");
	}

	private int TotalAdsWatched;

	public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
	{
		// Advertisement.Banner.Show("Banner_Android");
		Debug.Log("OnUnityAdShowComplete" + showCompletionState);
		Time.timeScale = 1;
		switch (placementId)
		{
			case "DoNotShowAds" when UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState):
				Advertisement.Banner.Hide(true);
				TotalAdsWatched++;
				Debug.Log("TotalAdsWatchedValue" + TotalAdsWatched);
				PlayerPrefs.SetInt("DoNotShowAdsAdCount", TotalAdsWatched);
				MainMenuManager.instance.adsWatchedText.text = TotalAdsWatched + "/5";
				if (TotalAdsWatched == 5)
				{
					PlayerPrefs.SetInt("DoNotShowAds", 1); ///// Only for a single placement id, i.e do not show ads
					PlayerPrefs.SetInt("Max", 9);
					CustomWeaponSelector.instance.UnlockAllWeapons();
					CustomWeaponSelector.instance.ReplaceAdIndicatorWithUnlocked();
					MainMenuManager.instance.removeAllAdsButton.interactable = false;
					MainMenuManager.instance.removeAllAdsButton.transform.gameObject.SetActive(false);
					MainMenuManager.instance.EnableReference(0);
					Debug.Log("DoNotShowAdsBought");
				}

				break;


			case "Rewarded_Android" when UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState):
				Advertisement.Banner.Hide(true);
				currentWeapon = CustomWeaponSelector.instance.currentWeapon;
				currentWeapon.adCount--;
				if (currentWeapon.adCount == 0)
				{
					currentWeapon.isLocked = false;
					PlayerPrefs.SetInt("WeaponUnlocked" + currentWeapon.weaponIndex, 0);
				}

				CustomWeaponSelector.instance.CheckIfWeaponIsLocked();

				break;
		}
	}

	#endregion

	#region BannerAds

	public void LoadBannerAd()
	{
		Advertisement.Banner.Load("Banner_Android",
			new BannerLoadOptions
			{
				loadCallback = OnAndroidBannerLoaded,
				errorCallback = OnBannerError
			}
		);
		Advertisement.Banner.Load("MainMenuBanner",
			new BannerLoadOptions
			{
				loadCallback = OnMainMenuBannerLoaded,
				errorCallback = OnBannerError
			}
		);
		Advertisement.Banner.Load("GameplayBanner",
			new BannerLoadOptions
			{
				loadCallback = OnGameplayBannerLoaded,
				errorCallback = OnBannerError
			}
		);
	}

	void OnAndroidBannerLoaded()
	{
		Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
	}

	void OnMainMenuBannerLoaded()
	{
		Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
	}

	void OnGameplayBannerLoaded()
	{
		Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
	}

	void OnBannerError(string message)
	{
	}

	#endregion

	#region RewardedVideo

	public void LoadRewardedAdForDoNotShowAds()
	{
		Advertisement.Load("DoNotShowAds", this);
		Debug.Log("DoNotShowAdsRewarded");
	}

	public void LoadRewardedAdForBuyingGuns()
	{
		Advertisement.Load("Rewarded_Android", this);
		Debug.Log("WeaponsRewarded");
	}

	#endregion
}