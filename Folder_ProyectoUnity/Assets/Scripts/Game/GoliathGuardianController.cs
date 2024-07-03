using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoliathGuardianController : Robot
{
    [SerializeField] private float parryProbability;
    public void ReflectBullet(GameObject bullet)
    {
        if (Random.value < parryProbability)
        {
            Bullet proyectile = bullet.gameObject.GetComponent<Bullet>();
            MRU mru = bullet.gameObject.GetComponent<MRU>();
            mru.Velocity = -mru.Velocity;
            int currentLayer = bullet.gameObject.layer;
            if (currentLayer == LayerMask.NameToLayer("Player"))
            {
                bullet.gameObject.layer = LayerMask.NameToLayer("Bot");
            }
            else if (currentLayer == LayerMask.NameToLayer("Bot"))
            {
                bullet.gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
        else
        {
            Bullet proyectile = bullet.gameObject.GetComponent<Bullet>();
            TakeDamage(proyectile.Damage);
            StartCoroutine(PlayAnimation("isHit", 0.5f));
            Destroy(bullet);
        }
    }
}
