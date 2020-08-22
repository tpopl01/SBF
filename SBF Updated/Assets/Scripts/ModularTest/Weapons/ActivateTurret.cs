using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Utils;

public class ActivateTurret : MonoBehaviour
{
    Timer t = new Timer(3);
    bool setUp = false;
    bool movedToPos = false;
    Rigidbody rb;
    Vector3 startPos;

    private void Start()
    {
        t.StartTimer();
    }

    private void Update()
    {
        if (!setUp)
        {
            if (t.GetComplete())
            {
                if (rb == null)
                    rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
                startPos = transform.position;
                setUp = true;
            }
        }
        else if(!movedToPos)
        {
            transform.position = Vector3.Lerp(transform.position, startPos + Vector3.up*2, Time.deltaTime * 2);
            if(Vector3.Distance(transform.position, startPos+Vector3.up) < 0.3f)
            {
                //enable AI
                GetComponent<InputBrainBase>().brain = Resources.Load<BrainBase>("Modular/Brains/Laser Turret/Brain_Laser_Turret_AI");
                movedToPos = true;
            }
        }
    }

}
