using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STurret : SSpawnable
{
    protected override void OnSpawn(Rigidbody rb, ModularController c)
    {
        base.OnSpawn(rb, c);
        StartCoroutine(Setup(rb, c));
    }

    IEnumerator Setup(Rigidbody rb, ModularController c)
    {
        yield return new WaitForSeconds(0.2f);
        ModularController m = rb.GetComponent<ModularController>();
        m.SetTeam(c.Team);
        GameManagerModular.instance.AddUnitToTeam(m);
    }
}
