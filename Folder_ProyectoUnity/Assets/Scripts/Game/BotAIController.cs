using UnityEngine;
using System.Collections;
public class BotAIController : MonoBehaviour
{
    private enum BotState
    {
        Offensive,
        Defensive,
        Balanced
    }
    [SerializeField] private BotState currentState;
    [SerializeField] private int money;
    [SerializeField] private Base botBase;
    [SerializeField] private RobotCard[] offensiveRobots;
    [SerializeField] private RobotCard[] defensiveRobots;
    [SerializeField] private RobotCard[] balancedRobots;
    [SerializeField] private RobotCard[] moneyProducers;
    [SerializeField] private GameGrid botGrid;
    [SerializeField] private float actionCooldown = 2.0f;
    private float actionTimer;
    private int maxMoneyProducers = 4;
    [SerializeField] private int currentMoneyProducers;
    private void Start()
    {
        currentState = BotState.Balanced;
        actionTimer = actionCooldown;
        currentMoneyProducers = 0;
        StartCoroutine(AIUpdateRoutine());
    }

    private IEnumerator AIUpdateRoutine()
    {
        yield return new WaitForSeconds(actionCooldown);
        UpdateBotState();
        PerformBotAction();
        StartCoroutine(AIUpdateRoutine());
    }

    private void UpdateBotState()
    {
        if (botBase.Life > 75)
        {
            currentState = BotState.Offensive;
        }
        else if (botBase.Life > 50)
        {
            currentState = BotState.Balanced;
        }
        else
        {
            currentState = BotState.Defensive;
        }
    }
    private void PerformBotAction()
    {
        if (botGrid.IsFull() == false)
        {
            if (currentMoneyProducers < maxMoneyProducers)
            {
                PlaceMoneyProducer();
            }
            else
            {
                switch (currentState)
                {
                    case BotState.Offensive:
                        PlaceRobot(offensiveRobots);
                        break;
                    case BotState.Defensive:
                        PlaceRobot(defensiveRobots);
                        break;
                    case BotState.Balanced:
                        PlaceRobot(balancedRobots);
                        break;
                }
            }
        }
    }

    private void PlaceMoneyProducer()
    {
        if (money >= GetCost(moneyProducers[0]))
        {
            GameObject emptySlab = botGrid.GetRandomEmptyPosition();
            SlabController slab = emptySlab.GetComponent<SlabController>();
                if (moneyProducers[0].RobotPrefab.CompareTag("SolarisSentinel"))
                {
                    SolarisSentinelController sentinel = moneyProducers[0].RobotPrefab.GetComponent<SolarisSentinelController>();
                    sentinel.Bot = this.gameObject.GetComponent<BotAIController>();
                    sentinel.onMoneyGenerated += AddMoney;
                }
                GameObject producer = Instantiate(moneyProducers[0].RobotPrefab, emptySlab.transform.position, Quaternion.identity);
                botGrid.Robots[slab.XIndex, slab.YIndex] = producer.GetComponent<Robot>();
                money -= GetCost(moneyProducers[0]);
                currentMoneyProducers++;
        }
    }

    private void PlaceRobot(RobotCard[] robots)
    {
        int randomIndex = Random.Range(0, robots.Length);
        RobotCard robot = robots[randomIndex];

        if (money >= GetCost(robot))
        {
            GameObject emptySlab = botGrid.GetRandomEmptyPosition();
            SlabController slab = emptySlab.GetComponent<SlabController>();
            GameObject placedRobot = Instantiate(robot.RobotPrefab, emptySlab.transform.position, Quaternion.identity);
            botGrid.Robots[slab.XIndex, slab.YIndex] = placedRobot.GetComponent<Robot>();
            money -= GetCost(robot);
        }

    }
    private int GetCost(RobotCard robot)
    {
        return robot.Cost;
    }
    public void AddMoney(int amount)
    {
        Money = Money + amount;
    }
    public int Money
    {
        get 
        { 
            return money; 
        }
        set
        {
            money = value;
        }
    }
}
