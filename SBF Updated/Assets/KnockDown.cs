using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDown : MonoBehaviour
{
    EnterExit enterExit;

    private void Start()
    {
        enterExit = GetComponentInParent<EnterExit>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (enterExit.CanEnter() == false)
        {
            if (other.gameObject.layer == 10)
            {
                ModularHealthOrganism r = other.GetComponent<ModularHealthOrganism>();
                if (r)
                {
                    r.RagdollChar();
                }
            }
        }
    }
}
