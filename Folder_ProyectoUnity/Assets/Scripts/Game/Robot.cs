using UnityEngine;
public class Robot : MonoBehaviour
{
    [SerializeField] protected int life;
    [SerializeField] protected int maxLife;
    [SerializeField] protected int damage;
    public void SetData(RobotCard card)
    {
        life = card.Life;
        maxLife = card.Life;
        damage = card.Damage;
    }
}

