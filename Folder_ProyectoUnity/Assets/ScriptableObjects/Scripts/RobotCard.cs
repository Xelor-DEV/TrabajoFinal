using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "RobotCard", menuName = "ScriptableObjects/UI/RobotCard", order = 1)]
public class RobotCard : ScriptableObject
{
    [SerializeField] private Sprite robotIcon;
    public Sprite RobotIcon
    {
        get
        {
            return robotIcon;
        }
        set
        {
            robotIcon = value;
        }
    }
    [SerializeField] private string robotName;
    public string RobotName
    {
        get
        {
            return robotName;
        }
        set
        {
            robotName = value;
        }
    }
    [SerializeField] private int life;
    public int Life
    {
        get
        {
            return life;
        }
        set
        {
            life = value;
        }
    }
    [SerializeField] private int damage;
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }
    [SerializeField] private GameObject robotPrefab;
    public GameObject RobotPrefab
    {
        get
        {
            return robotPrefab;
        }
        set
        {
            robotPrefab = value;
        }
    }
    [SerializeField] private int cost;
    public int Cost
    {
        get
        {
            return cost;
        }
        set
        {
            cost = value;
        }
    }
}
