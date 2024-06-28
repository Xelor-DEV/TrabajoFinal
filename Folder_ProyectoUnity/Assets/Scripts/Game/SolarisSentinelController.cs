using UnityEngine;
using System;
using System.Collections;
public class SolarisSentinelController : Robot
{
    [SerializeField] private float generateMoneyInterval;
    [SerializeField] private int moneyPerInterval;
    public event Action<int> onMoneyGenerated;
    [SerializeField] private PlayerController player;
    public PlayerController Player
    {
        set
        {
            player = value;
        }
    }
    [SerializeField] private BotAIController bot;
    public BotAIController Bot
    {
        set
        {
            bot = value;
        }
    }
    private void OnEnable()
    {
        if (player != null)
        {
            onMoneyGenerated += player.AddMoney;
        }
        else if (bot != null)
        {
            onMoneyGenerated += bot.AddMoney;
        }
    }
    private void OnDisable()
    {
        if (player != null)
        {
            onMoneyGenerated -= player.AddMoney;
        }
        else if (bot != null)
        {
            onMoneyGenerated -= bot.AddMoney;
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
