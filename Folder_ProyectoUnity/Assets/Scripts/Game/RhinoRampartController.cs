using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoRampartController : Robot
{
    [SerializeField] private float fireRate;
    [SerializeField] private Transform targetBase;
    [SerializeField] private GameObject launchPosition;
    [SerializeField] private GameObject bullet;
    private void OnEnable()
    {
        if (Player != null)
        {
            onDestroy += PlayDeathAnimation;
        }
        else if (Bot != null)
        {
            onDestroy += PlayDeathAnimation;
        }
    }

    private void OnDisable()
    {
        if (Player != null)
        {
            onDestroy -= PlayDeathAnimation;
        }
        else if (Bot != null)
        {
            onDestroy -= PlayDeathAnimation;
        }
    }
    private void Start()
    {
        if(player != null)
        {
            targetBase = player.BotBase.transform;
        }
        else if (bot != null)
        {
            targetBase = bot.PlayerBase.transform;
            
        }
        StartCoroutine(AttackLoop());
    }
    public  IEnumerator AttackLoop()
    {
        if(isDead == false)
        {
            StartCoroutine(PlayAnimation("Attack", 3));
            yield return new WaitForSeconds(0.5f);
            ParabolicMovement parabolicComponent = bullet.GetComponent<ParabolicMovement>();
            parabolicComponent.TargetObject = targetBase;
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.SetDamage(damage);
            GameObject projectile = Instantiate(bullet, launchPosition.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(fireRate);
            StartCoroutine(AttackLoop());
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            TakeDamage(bullet.Damage);
        }
    }
}

