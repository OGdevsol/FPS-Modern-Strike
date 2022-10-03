using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class CustomWeaponSelector : MonoBehaviour
{
	public WeaponsStatsScriptables[] gunsScriptables;
	public static CustomWeaponSelector instance;
	public GameObject[] Weaponsreferences;
	public WeaponsStatsScriptables currentWeapon;
	[SerializeField] private GameObject[] weaponButtons;
	[SerializeField] private GameObject[] adIndicator;
	[SerializeField] private Sprite unlockedIndicator;
	public GameObject playButton;
	public GameObject buyButton;


	private void Awake()
	{
		instance = this;

		SelectWeapon(Weaponsreferences[PlayerPrefs.GetInt("SelectedWeapon")]);
		currentWeapon = gunsScriptables[PlayerPrefs.GetInt("SelectedWeapon")];
		CheckIfWeaponIsLocked();
		WeaponButtonSelectedState();
		ReplaceAdIndicatorWithUnlocked();
	}

	public void CheckIfWeaponIsLocked()
	{
		if (currentWeapon.isLocked == false)
		{
			PlayerPrefs.SetInt("WeaponUnlocked" + currentWeapon.weaponIndex, 0);
		}

		if (PlayerPrefs.HasKey("WeaponUnlocked" + currentWeapon.weaponIndex))
		{
			playButton.SetActive(true);
			buyButton.SetActive(false);
			adIndicator[PlayerPrefs.GetInt("SelectedWeapon")].gameObject.GetComponent<Image>().sprite =
				unlockedIndicator;
			//	weaponButtons[PlayerPrefs.GetInt("SelectedWeapon")].transform.GetChild(3).gameObject.GetComponent<Image>().sprite=unlockedIndicator;
		}

		else
		{
			playButton.SetActive(false);
			buyButton.SetActive(true);
		}
	}

	void TestFunc()
	{
		for (int i = 0; i < gunsScriptables.Length; i++)
		{
			if (PlayerPrefs.HasKey("WeaponUnlocked" + gunsScriptables[i].weaponIndex))
			{
				weaponButtons[PlayerPrefs.GetInt("SelectedWeapon")].transform.GetChild(3).gameObject
					.GetComponent<Image>().sprite = unlockedIndicator;
			}
		}
	}

	public void SelectWeapon(GameObject gunModel)
	{
		int i;
		int weaponsLength = Weaponsreferences.Length;
		for (i = 0; i < weaponsLength; i++)
		{
			/* Added on main button as an OnClickEvent to show the gun model on screen*/
			Weaponsreferences[i].SetActive(false);
		}

		gunModel.SetActive(true);
	}

	public void WeaponDetails(WeaponsStatsScriptables details)
	{
		currentWeapon = details;
		PlayerPrefs.SetInt("SelectedWeapon", details.weaponIndex);
		WeaponButtonSelectedState();
		CheckIfWeaponIsLocked();
		/* Added on main button beside SelectWeapon method to fetch details of the weapon for further functionality  */
		Debug.Log("SelectedWeaponIs " + details.weaponIndex);
	}

	public void ShowAdAndBuy()
	{
		UnityAds.instance.LoadRewardedAdForBuyingGuns();
		Debug.Log("SHOWING AD"); /* Added as an OnClick event on buy button to check if the gun has been bought or not
                                                                        , OnAdCompletion logic is written in UnityAdsController  */
	}

	public void WeaponButtonSelectedState()
	{
		int i;
		int weaponLength = weaponButtons.Length;
		for (i = 0; i < weaponLength; i++)
		{
			weaponButtons[i].transform.GetChild(0).gameObject.SetActive(false);
		}

		weaponButtons[PlayerPrefs.GetInt("SelectedWeapon")].transform.GetChild(0).gameObject.SetActive(true); //Activate a BG image to indicate player selection of available weapons
	}

	public void UnlockAllWeapons()
	{
		for (int i = 0; i < gunsScriptables.Length; i++)
		{
			gunsScriptables[i].adCount = 0;
			gunsScriptables[i].isLocked = false;
			PlayerPrefs.SetInt("WeaponUnlocked" + gunsScriptables[i].weaponIndex, 0); // Will unlock all weapons at once and set PlayerPrefs in accordance with their data in scriptables
		}
	}

	public void ReplaceAdIndicatorWithUnlocked()
	{
		if (PlayerPrefs.GetInt("DoNotShowAds") == 1 )
		{
			int i;
			int indicatorsLength = adIndicator.Length;
			for (i = 0; i < indicatorsLength; i++)
			{
				adIndicator[i].gameObject.GetComponent<Image>().sprite = unlockedIndicator; // Will check if the weapon has been bought, it's ad indicator image will be replaced with another image, indicating
                                                                                           // that it has been unlocked in the game
			}
		}

		/*if (PlayerPrefs.HasKey("WeaponUnlocked" + currentWeapon.weaponIndex))
		{
			adIndicator[PlayerPrefs.GetInt("SelectedWeapon")].gameObject.GetComponent<Image>().sprite =
				unlockedIndicator;
		}*/
		/*if (PlayerPrefs.HasKey("WeaponUnlocked" + currentWeapon.weaponIndex))
		{
			playButton.SetActive(true);
			buyButton.SetActive(false);
			//	weaponButtons[PlayerPrefs.GetInt("SelectedWeapon")].transform.GetChild(3).gameObject.GetComponent<Image>().sprite=unlockedIndicator;
		}

		else
		{
			playButton.SetActive(false);
			buyButton.SetActive(true);
		}*/
	}
}