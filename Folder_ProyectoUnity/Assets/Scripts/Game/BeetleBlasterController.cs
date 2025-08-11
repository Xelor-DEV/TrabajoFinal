using UnityEngine;
using System.Collections;
public class BeetleBlasterController: Robot
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletInterval;
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

    public override void StartBehavior()
    {
        StartCoroutine(GenerateBullets());
    }

    private IEnumerator GenerateBullets()
    {        
        PlayAnimation("isAttacking");
        yield return new WaitForSeconds(0.3f);
        Bullet tmp = bullet.GetComponent<Bullet>();
        tmp.SetDamage(damage);
        Instantiate(bullet, spawnPoint.position ,Quaternion.identity);
        yield return new WaitForSeconds(bulletInterval);
        StartCoroutine(GenerateBullets());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            TakeDamage(bullet.Damage);
            PlayAnimation("isHit");
        }
    }
}