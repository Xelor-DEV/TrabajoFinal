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
        }
        else if (Bot != null)
        {
            onMoneyGenerated += Bot.AddMoney;
            onDestroy += Bot.MoneyProducerEliminated;
        }
    }

    private void OnDisable()
    {
        if (Player != null)
        {
            onMoneyGenerated -= player.AddMoney;
        }
        else if (Bot != null)
        {
            onMoneyGenerated -= bot.AddMoney;
            onDestroy -= bot.MoneyProducerEliminated;
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
        yield return new WaitForSeconds(generateMoneyInterval);
        onMoneyGenerated?.Invoke(moneyPerInterval);
        StartCoroutine(GenerateMoneyRoutine());
    }
}
