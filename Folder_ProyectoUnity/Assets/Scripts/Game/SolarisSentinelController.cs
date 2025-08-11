using UnityEngine;
using System;
using System.Collections;
public class SolarisSentinelController : Robot
{
    [SerializeField] private float generateMoneyInterval;
    [SerializeField] private int moneyPerInterval;
    public event Action<int> onMoneyGenerated;

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
                    bot.MoneyProducerEliminated();
                    PlayDeathAnimation();
                }
            }
        }
    }

    public override void StartBehavior()
    {
        StartCoroutine(GenerateMoneyRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            TakeDamage(bullet.Damage);
        }
    }

    private IEnumerator GenerateMoneyRoutine()
    {
        if(isDead == false)
        {
            yield return new WaitForSeconds(generateMoneyInterval);
            onMoneyGenerated?.Invoke(moneyPerInterval);
            if (Player != null)
            {
                player.AddMoney(moneyPerInterval);
            }
            else if (Bot != null)
            {
                bot.AddMoney(moneyPerInterval);
            }

            PlayAnimation("isMoneyGenerated");
            StartCoroutine(GenerateMoneyRoutine());
        }
    }
}
