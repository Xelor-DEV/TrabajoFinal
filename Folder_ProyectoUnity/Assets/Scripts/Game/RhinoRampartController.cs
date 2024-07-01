using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoRampartController : Robot
{
    [SerializeField] private float fireRate;
    [SerializeField] private Transform targetBase;
    [SerializeField] private GameObject launchPosition;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameGrid playerGrid;
    [SerializeField] private GameGrid botGrid;
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
    public GameGrid PlayerGrid
    {
        get 
        { 
            return playerGrid; 
        }
        set 
        { 
            playerGrid = value; 
        }
    }
    public GameGrid BotGrid
    {
        get 
        { 
            return botGrid; 
        }
        set 
        { 
            botGrid = value; 
        }
    }
    public  IEnumerator AttackLoop()
    {
        AttackBase();
        yield return new WaitForSeconds(fireRate);
        StartCoroutine(AttackLoop());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            TakeDamage(bullet.Damage);
        }
    }
    private void AttackBase()
    {
        ParabolicMovement parabolicComponent = bullet.GetComponent<ParabolicMovement>();
        parabolicComponent.TargetObject = targetBase;
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.SetDamage(damage);
        GameObject projectile = Instantiate(bullet, launchPosition.transform.position, Quaternion.identity);
    }

}

