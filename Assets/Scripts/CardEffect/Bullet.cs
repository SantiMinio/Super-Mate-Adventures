using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IAttacker
{
    float bulletDamage;
    [SerializeField] Rigidbody rb = null;
    Transform owner;
    [SerializeField] float skillShotSpeed = 15;
    float timer = 0;
    [SerializeField] float timeToDestroy = 5;
    [SerializeField] ParticleSystem deathpart;
    public float GetDamage()
    {
        return bulletDamage;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Shoot(Vector3 dir, float damage, Transform _owner)
    {
        transform.up = dir;
        bulletDamage = damage;
        owner = _owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        IHiteable hiteable = other.GetComponent<IHiteable>();
        if(hiteable != null && other.transform != owner)
        {
            hiteable.Hit(this);
            deathpart.Play();
            deathpart.transform.parent = null;
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        rb.velocity = transform.up * skillShotSpeed;

        timer += Time.deltaTime;
        if (timer >= timeToDestroy) Destroy(this.gameObject);
    }
}
