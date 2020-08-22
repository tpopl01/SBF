using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        public abstract void Initialise();
        public abstract float HitChance(float distance);
        public abstract float GetDamageAtDistance(float distance);
        public abstract bool Attack(Vector3 pos, float distance, ModularController self, ModularController target);
        public abstract float GetActualRange();
        public abstract float GetRange(float dist);
    }
}