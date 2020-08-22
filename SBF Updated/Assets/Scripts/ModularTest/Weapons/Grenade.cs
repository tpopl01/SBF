using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Utils;

public class Grenade : MonoBehaviour
{
    Timer t = new Timer(2f);
    [SerializeField]float aOE = 5;
    [SerializeField]
    float maxDamage = 130;
    [SerializeField]
    float minDamage = 30;
    [SerializeField]
    GameObject explosionEffect = null;
    [SerializeField] AudioSource aS = null;
    [SerializeField] AudioProfileGeneral grenadeExplosion = null;

    private void OnEnable()
    {
        t.StartTimer();
    }

    private void Update()
    {
        if(t.GetComplete())
        {
            Explode();
        }
    }

    void Explode()
    {
        Collider[] itemsHit = UnityEngine.Physics.OverlapSphere(transform.position, aOE / 10);
        if (explosionEffect)
        {
            Instantiate(explosionEffect, transform.position + Vector3.up * 0.4f, Quaternion.identity);
            grenadeExplosion.PlaySound(aS);
        }

        foreach (var item in itemsHit)
        {
            StaticMaths.AddExplosiveForce(item.transform, minDamage, maxDamage, transform.position, aOE);
        }
        Destroy(this.gameObject);
    }
}
