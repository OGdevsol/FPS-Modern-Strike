using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(order = 1,menuName = "CustomWeaponStats", fileName = "CreateWeaponStats")]
public class WeaponsStatsScriptables : ScriptableObject
{
    public bool isLocked;
    public int adCount;
    public int weaponIndex;
    public GameObject buyWithAdButton;



}
