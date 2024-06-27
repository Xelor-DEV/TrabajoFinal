using UnityEngine;
[CreateAssetMenu(fileName = "GameStats", menuName = "ScriptableObjects/Data/GameStats", order = 1)]
public class GameStats : ScriptableObject
{
    [Header("Properties")]
    [SerializeField] private bool playerWin;
    [SerializeField] private float gameDuration;
    [SerializeField] private int playerEliminations;
    [SerializeField] private int botEliminations;
    [SerializeField] private int deployedByPlayer;
    [SerializeField] private int deployedByBot;
    public bool PlayerWin
    {
        get 
        { 
            return playerWin; 
        }
        set 
        { 
            playerWin = value; 
        }
    }
    public float GameDuration
    {
        get 
        { 
            return gameDuration; 
        }
        set 
        { 
            gameDuration = value; 
        }
    }
    public int PlayerEliminations
    {
        get 
        { 
            return playerEliminations; 
        }
        set 
        { 
            playerEliminations = value; 
        }
    }
    public int BotEliminations
    {
        get 
        { 
            return botEliminations; 
        }
        set 
        { 
            botEliminations = value; 
        }
    }
    public int DeployedByPlayer
    {
        get 
        { 
            return deployedByPlayer; 
        }
        set 
        { 
            deployedByPlayer = value; 
        }
    }
    public int DeployedByBot
    {
        get 
        { 
            return deployedByBot; 
        }
        set 
        { 
            deployedByBot = value; 
        }
    }
}
