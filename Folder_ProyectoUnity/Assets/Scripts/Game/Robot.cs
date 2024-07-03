using UnityEngine;
using System.Collections;
using System;
public class Robot : MonoBehaviour
{
    [SerializeField] protected int life;
    [SerializeField] protected int maxLife;
    [SerializeField] protected int damage;
    public Action onDestroy;
    [SerializeField] protected PlayerController player;
    [SerializeField] protected Animator animator;
    [SerializeField] protected float deathDuration;
    protected bool isDead = false;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public IEnumerator PlayAnimation(string name, float duration)
    {
        animator.SetBool(name,true);
        yield return new WaitForSeconds(duration);
        animator.SetBool(name,false);
    }
    protected void PlayDeathAnimation()
    {
        StartCoroutine(DeathAnimation());
    }
    protected IEnumerator DeathAnimation()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore");
        animator.SetTrigger("isDeath");
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }
    public int Life
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
                onDestroy?.Invoke();
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

