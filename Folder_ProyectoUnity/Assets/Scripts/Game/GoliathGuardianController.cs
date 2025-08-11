using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoliathGuardianController : Robot
{
    [SerializeField] private float parryProbability;
    public override int Life
    {
        get
        {
            return life;
        }
        set
        {
            life = value;
            if (life <= 0 && isDead == false)
            {
                isDead = true;
                if (Player != null)
                {
                    PlayDeathAnimation();
                }
                else if (Bot != null)
                {
                    bot.OnRobotDestroyed(this);
                    PlayDeathAnimation();
                }
            }
        }
    }

    public void ReflectBullet(GameObject bullet)
    {
        if (Random.value < parryProbability)
        {
            PlayAnimation("isParry");
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
            PlayAnimation("isHit");
            Bullet proyectile = bullet.gameObject.GetComponent<Bullet>();
            TakeDamage(proyectile.Damage);      
            Destroy(bullet);
        }
    }
}
