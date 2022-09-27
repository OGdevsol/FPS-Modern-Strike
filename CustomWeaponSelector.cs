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
	public GameObject playButton;
	public GameObject buyButton;


	private void Awake()
	{
		instance = this;

		SelectWeapon(Weaponsreferences[PlayerPrefs.GetInt("SelectedWeapon")]);
		currentWeapon = gunsScriptables[PlayerPrefs.GetInt("SelectedWeapon")];
		CheckIfWeaponIsLocked();
		WeaponButtonSelectedState();
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
		}

		else
		{
			playButton.SetActive(false);
			buyButton.SetActive(true);
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
		weaponButtons[PlayerPrefs.GetInt("SelectedWeapon")].transform.GetChild(0).gameObject.SetActive(true);
	}

	public void UnlockAllWeapons()
	{
		for (int i = 0; i < gunsScriptables.Length; i++)
		{
			gunsScriptables[i].adCount = 0;
			gunsScriptables[i].isLocked = false;
			PlayerPrefs.SetInt("WeaponUnlocked" + gunsScriptables[i].weaponIndex , 0);
		}
	}
}