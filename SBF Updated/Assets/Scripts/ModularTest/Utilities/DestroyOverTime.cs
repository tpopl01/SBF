using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Utils
{
    public class DestroyOverTime : MonoBehaviour
    {
        [SerializeField] private float time = 10;
        float timer = 0;


        private void Update()
        {
            timer += Time.deltaTime;
            if(timer > time)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
