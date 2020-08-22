using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipHangerEnter : MonoBehaviour
{
    SpaceshipHanger hanger;

    private void Start()
    {
        hanger = GetComponentInParent<SpaceshipHanger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Spaceship s = other.GetComponentInParent<Spaceship>();
        if (s)
        {
            hanger.Land(s);
        }
    }
}
