using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BombardierBastionController : Robot
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float attackThreshold;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float attackInterval;
    [SerializeField] private int shotsBeforeDamageIncrease;
    [SerializeField] private float scaleDuration;
    [SerializeField] private Ease ease;
    [SerializeField] private int plusDamage;
    private bool isAttacking = false;
    private int shotsReceived = 0;
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

    private IEnumerator AttackCoroutine()
    {
        PlayAnimation("isAttacking");
        Bullet tmp = bullet.GetComponent<Bullet>();
        tmp.SetDamage(damage);
        Instantiate(bullet, spawnPoint.position, Quaternion.identity);
        if (shotsReceived >= shotsBeforeDamageIncrease)
        {
            damage = damage + plusDamage;
            shotsReceived = 0;
        }
        yield return new WaitForSeconds(attackInterval);
        StartCoroutine(AttackCoroutine());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            TakeDamage(bullet.Damage);
            PlayAnimation("isHit");
            if (isAttacking == false && (float)Life / (float)maxLife <= attackThreshold)
            {
                PlayAnimation("isAttacking");
                isAttacking = true;
                transform.DOScale(transform.localScale * 1.5f, scaleDuration).SetEase(ease);
                StartCoroutine(AttackCoroutine());
            }
            else
            {
                shotsReceived = shotsReceived + 1;
            }
        }
    }
}
