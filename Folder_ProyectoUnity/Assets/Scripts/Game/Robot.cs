using UnityEngine;
using System;
public class Robot : MonoBehaviour
{
    [SerializeField] protected int life;
    [SerializeField] protected int maxLife;
    [SerializeField] protected int damage;
    public Action onDestroy;
    [SerializeField] protected PlayerController player;
    public int Life
    {
        get
        {
            return life;
        }
        set
        {
            life = value;
            if (life <= 0)
            {
                onDestroy?.Invoke();
                Destroy(gameObject);
            }
        }
    }
    public PlayerController Player
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
        }
    }
    [SerializeField] protected BotAIController bot;
    public BotAIController Bot
    {
        get
        {
            return bot;
        }
        set
        {
            bot = value;
        }
    }
    public void SetData(RobotCard card)
    {
        life = card.Life;
        maxLife = card.Life;
        damage = card.Damage;
    }
    public virtual void TakeDamage(int damage)
    {
        Life = Life - damage;
    }
    public virtual void StartBehavior(bool isPlace)
    {

    }
}

