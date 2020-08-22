using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    [SerializeField] Color colour = Color.white;
    [SerializeField] Color endColour = Color.white;
   // [SerializeField] [Range(0.1f,10f)]float bulletLength = 0.1f;
    [SerializeField] GameObject hitPrefab = null;
    float maxLength = 20.0f;
    private LineRenderer line;
    private float length;
    private float anim;
    private float speed = 0.1f;
  //  HealthBase targetHealth;
    protected float damage;
    Stats stats;
    [SerializeField]AudioProfileGeneral impactAudio;
    AudioSource aS;
    Vector3 t;
    ModularController owner;
    ModularController target;

    public void SetStats(Stats stats)
    {
        this.stats = stats;
    }

    public bool CanAttack()
    {
        return line.enabled == false;
    }

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.startColor = colour;
        line.endColor = endColour;
        line.enabled = false;
        anim = 0;
        aS = GetComponentInChildren<AudioSource>();
        if(aS == null)
        {
            GameObject gO = new GameObject("AudioSource");
            gO.transform.parent = transform;
            gO.transform.localPosition = Vector3.zero;
            gO.transform.rotation = Quaternion.identity;
            aS = gO.AddComponent<AudioSource>();
            aS.spatialBlend = 1;
            aS.loop = false;
            aS.playOnAwake = false;
        }
    }

    void Fire(float damage, float maxLength)
    {
        //transform.position -= transform.forward * spawnOffset;
        this.damage = damage + this.damage;

        anim = 0;
        this.maxLength = maxLength;
       // speed /= maxLength * 0.1f;
        length = 0;
        line.enabled = true;
        
    }

    public void Fire(float damage, ModularController self, float maxLength, ModularController target)
    {
        this.target = target;
        this.owner = self;
        Fire(damage, maxLength);
    }

    public void Fire(float damage, float maxLength, Vector3 t, ModularController self, ModularController target)
    {
        this.target = target;
        this.owner = self;
        transform.LookAt(t);
        this.t = t;
     //   this.targetHealth = targetHealth;
        Fire(damage, maxLength);
    }

    void Update()
    {
        HandleBulletRender();
      //  BulletRaycast();
    }

    private void LateUpdate()
    {
        HandleBulletRender();
       // BulletRaycast();
    }
    int h = 0;
    void BulletRaycast()
    {
        transform.LookAt(t);
        float rayAmount = maxLength;//Mathf.Clamp(bulletLength, 0, maxLength);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayAmount))
        {
            h++;
            OnInpact(hit);
            maxLength = (hit.point - transform.position).magnitude;
        }

    }

    public virtual void OnInpact(RaycastHit hit)
    {
        aS.transform.position = hit.point;
        impactAudio.PlaySound(aS);
        //check tags to determine hit
        if (this.target != null && this.target.transform == hit.transform)
        {
            this.target.Health.DamageHealth(damage);
            this.target.Senses.ShotAtFrom = transform.position;
            if(this.target.Health.IsDead())
            {
                owner.Stats.OnKill();
            }
        }
        else
        {
           // ModularController h = hit.transform.GetComponentInParent<ModularController>();
            IHealth h = hit.transform.GetComponentInParent<IHealth>();
            if (h != null)
            {
                //  bool alive = !h.Health.IsDead();
                ModularController m = hit.transform.GetComponentInParent<ModularController>();
                if(m)
                    m.Senses.ShotAtFrom = transform.position;
                h.DamageHealth(damage);
                //if(h.Health.IsDead() && alive && stats != null)
                //{
                //    //ADD kill to stats
                //    stats.OnKill();
                //}
            }
        }
        line.enabled = false;
        GameObject p = Instantiate(hitPrefab, hit.point + hit.normal * 0.3f, transform.rotation);
        p.transform.SetParent(GameManagerModular.instance.weaponsFolder);
    //    Destroy(this.gameObject);
    }

    Vector3 GetPos1()
    {
        return transform.position;// GetPos2() - transform.forward * bulletLength;
    }

    Vector3 GetPos2()
    {
        return transform.position + (transform.forward * length);
    }

    void HandleBulletRender()
    {
        transform.LookAt(t);
        if (line.enabled)
        {
            anim += speed;

            if (anim > 1.0)
            {
                line.enabled = false;
                BulletRaycast();
               // anim = 0;
                //   Destroy(this.gameObject);
            }
            length = Mathf.Lerp(0, maxLength, anim);
            line.SetPosition(0, GetPos1());
            line.SetPosition(1, GetPos2());
        }
    }
}
