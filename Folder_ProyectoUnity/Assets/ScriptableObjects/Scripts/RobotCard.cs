using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "RobotIcon", menuName = "ScriptableObjects/UI/RobotIcon", order = 1)]
public class RobotCard : ScriptableObject
{
    [SerializeField] private Image robotIcon;
    public Image RobotIcon
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
}
