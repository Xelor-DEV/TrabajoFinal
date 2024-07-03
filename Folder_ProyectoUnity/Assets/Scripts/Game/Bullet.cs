using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage;
    public int Damage
    {
        get
        {
            return damage;
        }
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Robot>() != null)
        {
            if (other.GetComponent<GoliathGuardianController>() != null)
            {
                GoliathGuardianController GoliathGuardian = other.GetComponent<GoliathGuardianController>();
                GoliathGuardian.ReflectBullet(this.gameObject);
            }
            else
            {
                Robot robot = other.GetComponent<Robot>();
                if (robot.IsDead == false)
                {
                    StartCoroutine(robot.PlayAnimation("isHit", 0.5f));
                }
                Destroy(this.gameObject);
            }
        }
        else if (other.CompareTag("Base"))
        {
            Destroy(gameObject);
        }
    }
}
