using UnityEngine;
using System;
using System.Collections;
public class SolarisSentinelController : Robot
{
    [SerializeField] private float generateMoneyInterval;
    [SerializeField] private int moneyPerInterval;
    public event Action<int> onMoneyGenerated;
    private void OnEnable()
    {
        if (Player != null)
        {
            onMoneyGenerated += player.AddMoney;
            onDestroy += PlayDeathAnimation;
        }
        else if (Bot != null)
        {
            onMoneyGenerated += Bot.AddMoney;
            onDestroy += Bot.MoneyProducerEliminated;
            onDestroy += PlayDeathAnimation;
        }
    }

    private void OnDisable()
    {
        if (Player != null)
        {
            onMoneyGenerated -= player.AddMoney;
            onDestroy -= PlayDeathAnimation;
        }
        else if (Bot != null)
        {
            onMoneyGenerated -= bot.AddMoney;
            onDestroy -= bot.MoneyProducerEliminated;
            onDestroy -= PlayDeathAnimation;
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
    private void Start()
    {
        StartCoroutine(GenerateMoneyRoutine());
    }

    private IEnumerator GenerateMoneyRoutine()
    {
        if(isDead == false)
        {
            yield return new WaitForSeconds(generateMoneyInterval);
            onMoneyGenerated?.Invoke(moneyPerInterval);
            StartCoroutine(PlayAnimation("isMoneyGenerated", 1));
            StartCoroutine(GenerateMoneyRoutine());
        }
    }
}
