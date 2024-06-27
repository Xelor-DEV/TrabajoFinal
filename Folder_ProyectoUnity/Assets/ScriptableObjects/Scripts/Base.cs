using UnityEngine;
using System;
[CreateAssetMenu(fileName = "Base", menuName = "ScriptableObjects/Game/Base", order = 1)]
public class Base : ScriptableObject
{
    [SerializeField] private int maxLife;
    public int MaxLife
    {
        get
        {
            return maxLife;
        }
    }
    [SerializeField] private int life;
    public Action<int> onBaseAttacked;
    public Action onBaseDestroyed;
    public void ReceiveDamage(int damage)
    {
        life = life - damage;
        life = Mathf.Clamp(life, 0, maxLife);
        if (life <= 0)
        {
            onBaseDestroyed?.Invoke();
        }
        else
        {
            onBaseAttacked?.Invoke(life);
        }
    }
}
