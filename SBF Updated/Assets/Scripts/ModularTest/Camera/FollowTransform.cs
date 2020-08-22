using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField]float speed = 9;

    public void Follow(Transform targetTransform, Vector3 offset)
    {
        Vector3 target = targetTransform.position + offset;
        Follow(target, speed);
    }

    public void Follow(Transform target)
    {
        Follow(target.position, speed);
    }

    public void Follow(Vector3 target, float speed)
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
    }

}
