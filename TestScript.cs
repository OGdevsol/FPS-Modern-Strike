using System;
using System.Collections;
using System.Collections.Generic;
using EnemyAI;
using UnityEngine;
using UnityEngine.AI;

public class TestScript : MonoBehaviour
{
    private Rigidbody[] rb;
    private Animator am;
    private void Awake()
    {
        am = gameObject.GetComponent<Animator>();
        rb = GetComponentsInChildren<Rigidbody>();
        am.enabled = true;
        for (int i = 0; i < rb.Length; i++)
        {
            Debug.LogError(rb[i].gameObject.name);
            rb[i].isKinematic = true;
        }
        Debug.LogError(am.name);
    }

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < rb.Length; i++)
        {
            gameObject.GetComponent<TestScript>().rb[i].isKinematic = false;
        }
       gameObject.GetComponent<TestScript>().am.enabled = false;
//       gameObject.GetComponent<StateController>().enabled = false;
      //  gameObject.GetComponent<NavMeshAgent>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
