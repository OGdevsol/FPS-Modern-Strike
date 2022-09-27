using System;
using System.Collections;
using System.Collections.Generic;
using EnemyAI;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBaseScript : MonoBehaviour
{
	
	

	[SerializeField] private Animator m_animator = null;
	private Rigidbody[] m_ragdollBodies;
	public bool m_isDead = false;
	private float enemyHealth=100;
	public Slider EnemyHealthSlider;

	void Awake()
	{
		m_ragdollBodies = gameObject.GetComponentsInChildren<Rigidbody>();
	
		gameObject.GetComponent<EnemyBaseScript>().RagdollBodiesIsKinematic(true);
	}

	private void OnEnable()
	{
		
	}

	private void OnDisable()
	{
		
	}

	

	private void Damage(float damage)
	{
		Debug.Log("RECIEVING DAMAGE");
		enemyHealth -= damage;
//		EnemyHealthSlider.value = enemyHealth / 100;
		if (enemyHealth<=0)
		{
			gameObject.GetComponent<EnemyBaseScript>().m_animator.enabled = false;
			gameObject.GetComponent<EnemyBaseScript>().m_isDead = true;
			gameObject.GetComponent<EnemyBaseScript>().RagdollBodiesIsKinematic(false);
			
			gameObject.GetComponent<EnemyAnimation>().enabled = false;
			gameObject.GetComponent<StateController>().enabled = false;
			gameObject.GetComponent<EnemyHealth>().enabled = false;
			gameObject.GetComponent<NavMeshAgent>().enabled = false;
			
		}
				
	
		
	}

	


	private void RagdollBodiesIsKinematic(bool activate)
	{
		for (int i = 0; i < m_ragdollBodies.Length; i++)
			m_ragdollBodies[i].isKinematic = activate;
	}

	
}