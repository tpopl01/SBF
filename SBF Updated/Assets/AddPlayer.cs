using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayer : MonoBehaviour, ISetup
{
    public void SetUp(Transform root)
    {
        GameManagerModular.instance.SetPlayerTeam(root.GetComponent<ModularController>().Team);
        Destroy(this);
    }
}
