using System.Collections;
using UnityEngine;

public class RhinoRampartController : Robot
{
    [SerializeField] private float fireRate;
    [SerializeField] private Transform targetBase;
    [SerializeField] private GameObject launchPosition;
    [SerializeField] private GameObject bullet;

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
    }

    public override void StartBehavior()
    {
        StartCoroutine(AttackLoop());
    }
    public  IEnumerator AttackLoop()
    {
        if(isDead == false)
        {
            PlayAnimation("isAttacking");
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

