using System;
using System.Collections;
using System.Collections.Generic;
using DG.DemiLib.Attributes;
using EnemyAI;
using TMPro;
using UnityEngine;

public class LevelsSpawner : MonoBehaviour
{
	public static LevelsSpawner instance;
	public GameObject[] enemiesVariantsPrefabs;
	public GameObject[] weapons;
	public Transform player;
	public Camera fpCamera;
	public Level[] level;
	private int x;
	public TMP_Text waveObjectiveGameObject;
	private Transform enemyTarget;
	private GameUIManager _gameUIManager;


	private int
		_currentWaveInLoop; // To only check and instantiate according to the total enemies kept in each wave while initializing enemy instantiation

	[HideInInspector] public int
		_currentWaveToKeepActiveIndex; // To activate a single wave at a given time. Next wave will be activated when all enemies of currently active wave are killed

	private void Awake()
	{
		Debug.LogError("SelectedLevel " + "==" + PlayerPrefs.GetInt("SelectedLevel"));
		instance = this;
		//	SetPlayerPosition();
	}

	private void Start()
	{
		AddWavesToList();

		InitializeEnemies();
		GameUIManager.instance.enemyDeathCount = level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel[0].enemy
			.Length;
		GameUIManager.instance.UpdateEnemiesKilledText();
		ActivateWeapon();
		_gameUIManager = GameUIManager.instance;
	}

	void ActivateWeapon()
	{
		weapons[PlayerPrefs.GetInt("SelectedWeapon")].SetActive(true);
	}

	private void
		AddWavesToList() // Method to initialize enemies in form of waves. When all enemies in the first wave are killed in a level, the second wave will be initiated. Level in completed when all waves are killed
		// Adding waves to a seperate list allows for a better control over wave properties at runtime without modifying original waves' properties
	{
		var wavesLengthInLevel = level[PlayerPrefs.GetInt("SelectedLevel")].waves.Length;
		int i;
		for (i = 0; i < wavesLengthInLevel; i++)
		{
			_currentWaveInLoop = i;
			level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel
				.Add(level[PlayerPrefs.GetInt("SelectedLevel")].waves[i]);
		}
	}

	public void SetPlayerPosition()
	{
		player.gameObject.transform.localPosition = level[PlayerPrefs.GetInt("SelectedLevel")].playerPosition.position;
		player.gameObject.transform.rotation = level[PlayerPrefs.GetInt("SelectedLevel")].playerPosition.rotation;
		Debug.Log("SETTING PLAYER POSITION");
	}

	private void InitializeEnemies()
	{
		//SetPlayerPosition();
		for (int j = 0;
			j < level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel[_currentWaveToKeepActiveIndex].enemy.Length;
			j++)
		{
			var E = Instantiate(enemiesVariantsPrefabs[CheckEnemiesType(j)],
				level[PlayerPrefs.GetInt("SelectedLevel")].waves[_currentWaveToKeepActiveIndex].enemy[j].enemyPosition
					.position,
				level[PlayerPrefs.GetInt("SelectedLevel")].waves[_currentWaveToKeepActiveIndex].enemy[j].enemyPosition
					.rotation);
			level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel[_currentWaveToKeepActiveIndex]
				.enemiesGameObjectInWave.Add(E.transform);
			for (int k = 0;
				k < level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel[_currentWaveToKeepActiveIndex].enemy[j]
					.patrollingWayPoints.Length;
				k++)
			{
				E.GetComponent<StateController>().patrolWayPoints.Add(level[PlayerPrefs.GetInt("SelectedLevel")]
					.wavesInLevel[_currentWaveToKeepActiveIndex].enemy[j].patrollingWayPoints[k]);
			}
		}
	}

	
	private static int waveToBeRemovedIndex = 0;

	public void
		CheckEnemiesInActiveWave() // When there are zero enemies left in currently active wave, next enemy wave will be initialized (Also being used in health script of the AI in AI Kit in KillAI method. NOTE: Don't forget to remove the enemy gameobject from respective wave when it is killed 
	{
		if (level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel[waveToBeRemovedIndex].enemiesGameObjectInWave
			.Count == 0)
		{
			level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel
				.RemoveAt(waveToBeRemovedIndex); // Wave with zero enemies will be removed/deactivated to initialize next wave and the next wave will take its place at waveToBeRemovedIndex. The cycle will repeat when 0 enemies are left in new wave.

			if (level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel.Count > 0)
			{
				InitializeEnemies();
			}
		}

		if (level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel.Count == 0)
		{
			Debug.Log("LEVEL COMPLETED");
			_gameUIManager.OnLevelComplete();

			_gameUIManager.StartCoroutine("LevelCompleteRoutine");
		}
	}


	private int
		CheckEnemiesType(int i) // Method to check which enemy type is selected in the editor in a level wave so THAT particular enemy can be instantiated in its respective position using InitializeEnemies() method
	{
		switch (level[PlayerPrefs.GetInt("SelectedLevel")].wavesInLevel[_currentWaveToKeepActiveIndex].enemy[i]
			.enemytype)
		{
			case Enemy.EnemyType.Soldier:
				x = 0;
				return x;
			case Enemy.EnemyType.EliteSoldier:
				x = 1;
				return x;

			case Enemy.EnemyType.Commander:
				x = 2;
				return x;
			case Enemy.EnemyType.Civillian:
				x = 3;
				return x;
			default:
				return x;
		}
	}
}

[Serializable]
public class Wave
{
	public string waveObjective;
	public Enemy[] enemy;

	public List<Transform>
		enemiesGameObjectInWave; // Each level's waves' enemies gameobjects will be placed in this list according to their waves placement. 
}

[Serializable]
public class Level
{
	public Transform playerPosition;
	public Wave[] waves; // Add Enemies Details in this array

	public List<Wave>
		wavesInLevel; //Total waves in each level will be added to this list to add and maintain functionality control over each wave's properties
}

[Serializable]
public class Enemy
{
//	public LevelsSpawner.EnemyType[] enemyType; // Add Enemies Type in this  array
	public enum EnemyType
	{
		Soldier,
		EliteSoldier,
		Commander,
		Civillian,
	}

	public EnemyType enemytype;
	public Transform enemyPosition;
	public Transform[] patrollingWayPoints;
}