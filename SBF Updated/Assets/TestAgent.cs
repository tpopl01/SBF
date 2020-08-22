using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAgent : MonoBehaviour
{
    NavMeshAgent a;
    void Start()
    {
        a = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        a.SetDestination(new Vector3(10, 0, 10));
    }
}
