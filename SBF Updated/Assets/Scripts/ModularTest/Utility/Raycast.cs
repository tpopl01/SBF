using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu
  (
      fileName = "Raycast",
      menuName = "Modular/Util/Raycast"
  )]
public class Raycast : ScriptableObject
{
    [SerializeField]LayerMask mask = ~(0);

    public bool RaycastTarget(Vector3 origin, Vector3 target, float dist)
    {
        Vector3 dir = target - origin;
        if (Physics.Raycast(origin, dir, dist, mask))
        {
            return true;
        }
        return false;
    }

    public bool RaycastTarget(Vector3 origin, Vector3 target, out RaycastHit hit, float dist, int targetLayer)
    {
        Vector3 dir = target - origin;
        if(Physics.Raycast(origin, dir, out hit, dist, mask))
        {
            if(hit.transform.gameObject.layer == targetLayer)
                return true;
        }
        return false;
    }
}
