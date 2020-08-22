using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEffectFP : MonoBehaviour
{
    [SerializeField] Transform fallEffect = null;
    [SerializeField] Transform fallEffectWep = null;
    float t = 0;

    public void StartFall()
    {
        FallCamera(new Vector3(7, /*Random.Range(-1.0f, 1.0f)*/0, 0), new Vector3(3, /*Random.Range(-0.5f, 0.5f)*/0, 0), 20.15f);
    }

    void FallCamera(Vector3 d, Vector3 dw, float ta)
    {
        Quaternion s = fallEffect.localRotation;
        Quaternion sw = fallEffectWep.localRotation;
        Quaternion e = fallEffect.localRotation * Quaternion.Euler(d);
        // Quaternion ew = fallEffectWep.localRotation * Quaternion.Euler(dw);
        float r = 1.0f / ta;
        t += Time.deltaTime * r;
        fallEffect.localRotation = Quaternion.Slerp(s, e, t);
        fallEffectWep.localRotation = Quaternion.Slerp(sw, e, t);
        if(t > 1)
        {
            t = 0;
        }
    }
}
