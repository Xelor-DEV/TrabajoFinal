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
    private BotAIController bot;
    public BotAIController Bot
    {
        set
        {
            Bot = value;
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
        else
        {
            Debug.Log("f");
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
        Debug.Log("listo");
        StartCoroutine(GenerateMoneyRoutine());
    }
}
