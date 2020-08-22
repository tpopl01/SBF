using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1Explosive : Bullet1
{
    [SerializeField][Range(1,10)] float aOE = 2;

    public override void OnInpact(RaycastHit hit)
    {
        base.OnInpact(hit);
        Explode(hit);
    }

    void Explode(RaycastHit hit)
    {
        Collider[] itemsHit = UnityEngine.Physics.OverlapSphere(hit.point, aOE / 10);
        foreach (var item in itemsHit)
        {
            StaticMaths.AddExplosiveForce(item.transform, damage, damage + 5, hit.point, aOE);
        }
    }
}
