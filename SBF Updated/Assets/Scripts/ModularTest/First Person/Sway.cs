using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    float smooth = 3;
    Vector3 def;

    //start
    void Start()
    {
        def = transform.localPosition;
    }

    public void WeaponSway(float amount = 0.055f, float maxAmount = 0.09f)
    {
        float factorX = -Input.GetAxis("Mouse X") * amount;
        float factorY = -Input.GetAxis("Mouse Y") * amount;

        if (factorX > maxAmount)
            factorX = maxAmount;
        if (factorX < -maxAmount)
            factorX = -maxAmount;
        if (factorY > maxAmount)
            factorY = maxAmount;
        if (factorY < -maxAmount)
            factorY = -maxAmount;

        Vector3 final = new Vector3(def.x + factorX, def.y + factorY, def.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, final, Time.deltaTime * smooth);
    }
}
