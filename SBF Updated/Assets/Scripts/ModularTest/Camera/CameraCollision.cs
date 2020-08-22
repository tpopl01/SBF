using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    Transform camMainTrans;
    Transform pivot;
    float z;

    private void Start()
    {
        pivot = transform.GetChild(0);
        camMainTrans = pivot.GetChild(0);
        z = camMainTrans.localPosition.z;
    }

    public void CollisionCamera(float z = -2.5f, float dist = 0.7f)
    {
        float step = Mathf.Abs(z);
        int stepCount = 4;
        float stepIncremental = step / stepCount;
        float actualZ = z;

        Vector3 origin = pivot.position;
        Vector3 direction = -pivot.forward;
        Debug.DrawRay(origin, direction * step, Color.blue);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, step))
        {
            float distance = Vector3.Distance(hit.point, origin);
            actualZ = -(distance / 2);
        }
        else
        {
            for (int s = 1; s < stepCount + 1; s++)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 dir = Vector3.zero;
                    Vector3 secondOrigin = origin + (direction * s) * stepIncremental;

                    switch (i)
                    {
                        case 0:
                            dir = camMainTrans.right;
                            break;
                        case 1:
                            dir = -camMainTrans.right;
                            break;
                        case 2:
                            dir = camMainTrans.up;
                            break;
                        case 3:
                            dir = -camMainTrans.up;
                            break;
                        default:
                            break;
                    }

                    Debug.DrawRay(secondOrigin, dir * 1, Color.red);
                    if (Physics.Raycast(secondOrigin, dir, dist, ~(1<<10)))
                    {
                        float distance = Vector3.Distance(secondOrigin, origin);
                        actualZ = -(distance / 2);
                        break;
                    }
                }
            }
        }
        Vector3 targetP = camMainTrans.localPosition;
        if (actualZ > -0.1f)
            actualZ = -0.1f;
        targetP.z = Mathf.Lerp(targetP.z, actualZ, Time.deltaTime * 5);
        camMainTrans.localPosition = targetP;
    }
}
