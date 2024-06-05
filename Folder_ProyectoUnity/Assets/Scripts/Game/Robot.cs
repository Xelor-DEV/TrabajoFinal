using UnityEngine;
public class Robot : MonoBehaviour
{
    private int life;
    private int maxLife;
    private int damage;
    public void SetData(RobotCard card)
    {
        life = card.Life;
        maxLife = card.Life;
        damage = card.Damage;
    }
}

