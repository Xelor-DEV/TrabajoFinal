using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BombardierBastionController : Robot
{
    [SerializeField] private float attackThreshold;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float attackInterval;
    [SerializeField] private int shotsBeforeDamageIncrease;
    [SerializeField] private float scaleDuration;
    [SerializeField] private Ease ease;
    [SerializeField] private int plusDamage;
    private bool isAttacking = false;
    private int shotsReceived = 0;
    
    private IEnumerator AttackCoroutine()
    {
        Bullet tmp = bullet.GetComponent<Bullet>();
        tmp.SetDamage(damage);
        Instantiate(bullet, transform.position, Quaternion.identity);
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
            if (isAttacking == false && (float)Life / (float)maxLife <= attackThreshold)
            {
                isAttacking = true;
                transform.DOScale(transform.localScale * 2, scaleDuration).SetEase(ease);
                StartCoroutine(AttackCoroutine());
            }
            else
            {
                shotsReceived = shotsReceived + 1;
            }
        }
    }
}
