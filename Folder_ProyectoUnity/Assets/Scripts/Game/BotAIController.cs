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
    [SerializeField] private BaseController baseController;
    [SerializeField] private RobotCard[] offensiveRobots;
    [SerializeField] private RobotCard[] defensiveRobots;
    [SerializeField] private RobotCard[] balancedRobots;
    [SerializeField] private RobotCard moneyProducer;
    [SerializeField] private GameGrid botGrid;
    [SerializeField] private GameGrid playerGrid;
    [SerializeField] private BaseController playerBase;
    [SerializeField] private float actionCooldown;
    [SerializeField] private float analyzeCooldown;
    [SerializeField] private float probabilityCooldown;
    [SerializeField] private int maxMoneyProducers;
    [SerializeField] private float robotPlacementProbability;
    [SerializeField] private int currentMoneyProducers;
    public BaseController PlayerBase
    {
        get
        {
            return playerBase;
        }
    }
    private void Start()
    {
        currentState = BotState.Offensive;
        currentMoneyProducers = 0;
        StartCoroutine(AIUpdateRoutine());
    }
    private IEnumerator AIUpdateRoutine()
    {
        yield return new WaitForSeconds(actionCooldown);
        UpdateBotState();
        PerformBotAction();
        yield return new WaitForSeconds(analyzeCooldown);
        AnalyzePlayerGridAndPlaceRobots();
        yield return new WaitForSeconds(probabilityCooldown);
        PlaceRobotBasedOnProbability();
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
        if (money >= GetCost(moneyProducer))
        {
            GameObject emptySlab = botGrid.GetRandomEmptyPosition();
            if (emptySlab != null)
            {
                SlabController slab = emptySlab.GetComponent<SlabController>();
                if (moneyProducer.RobotPrefab.CompareTag("SolarisSentinel"))
                {
                    SolarisSentinelController sentinel = moneyProducer.RobotPrefab.GetComponent<SolarisSentinelController>();
                    sentinel.Bot = this.gameObject.GetComponent<BotAIController>();
                }
                GameObject producer = Instantiate(moneyProducer.RobotPrefab);
                producer.transform.position = emptySlab.transform.position;
                Robot tmp = producer.GetComponent<Robot>();
                botGrid.Robots[slab.XIndex, slab.YIndex] = tmp;
                tmp.SetData(moneyProducer);
                money -= GetCost(moneyProducer);
                currentMoneyProducers++;
            }
        }

    }
    private void PlaceRobotBasedOnProbability()
    {
        if (Random.value < robotPlacementProbability)
        {
            RobotCard randomOffensiveRobot = offensiveRobots[Random.Range(0, offensiveRobots.Length)];
            RobotCard randomDefensiveRobot = defensiveRobots[Random.Range(0, defensiveRobots.Length)];
            RobotCard randomBalancedRobot = balancedRobots[Random.Range(0, balancedRobots.Length)];
            RobotCard[] selectedRobots = { randomOffensiveRobot, randomDefensiveRobot, randomBalancedRobot };
            RobotCard chosenRobot = selectedRobots[Random.Range(0, selectedRobots.Length)];
            PlaceRobot(chosenRobot);
        }
    }
    private void PlaceRobot(RobotCard robot)
    {
        if (money >= GetCost(robot))
        {
            if (money - GetCost(robot) < GetCost(moneyProducer) && currentMoneyProducers < maxMoneyProducers)
            {
                PlaceMoneyProducer();
            }
            else
            {
                GameObject emptySlab = botGrid.GetRandomEmptyPosition();
                if (emptySlab != null)
                {
                    SlabController slab = emptySlab.GetComponent<SlabController>();
                    Robot tmp = robot.RobotPrefab.GetComponent<Robot>();
                    tmp.Bot = this.gameObject.GetComponent<BotAIController>();
                    GameObject placedRobot = Instantiate(robot.RobotPrefab);
                    placedRobot.transform.position = emptySlab.transform.position;
                    botGrid.Robots[slab.XIndex, slab.YIndex] = tmp;
                    tmp.SetData(robot);
                    money = money - GetCost(robot);
                    
                }
            }
        }
    }
    private void PlaceRobot(RobotCard[] robots)
    {
        int randomIndex = Random.Range(0, robots.Length);
        RobotCard robot = robots[randomIndex];

        if (money >= GetCost(robot))
        {
            if (money - GetCost(robot) < GetCost(moneyProducer) && currentMoneyProducers < maxMoneyProducers)
            {
                PlaceMoneyProducer();
            }
            else
            {
                GameObject emptySlab = botGrid.GetRandomEmptyPosition();
                if (emptySlab != null)
                {
                    SlabController slab = emptySlab.GetComponent<SlabController>();
                    Robot tmp = robot.RobotPrefab.GetComponent<Robot>();
                    tmp.Bot = this.gameObject.GetComponent<BotAIController>();
                    GameObject placedRobot = Instantiate(robot.RobotPrefab);
                    placedRobot.transform.position = emptySlab.transform.position;
                    botGrid.Robots[slab.XIndex, slab.YIndex] = tmp;
                    tmp.SetData(robot);
                    money -= GetCost(robot);
                }
            }
        }
    }
    private void AnalyzePlayerGridAndPlaceRobots()
    {
        for (int i = 0; i < playerGrid.Robots.GetLength(0); ++i)
        {
            int enemyCount = 0;
            int nonSolarisSentinelCount = 0;
            for (int j = 0; j < playerGrid.Robots.GetLength(1); ++j)
            {
                if (playerGrid.Robots[i, j] != null)
                {
                    enemyCount++;
                    if (playerGrid.Robots[i, j].CompareTag("SolarisSentinel") == false)
                    {
                        nonSolarisSentinelCount++;
                    }
                }
            }
            if (enemyCount > 3)
            {
                PlaceRobotInRow(defensiveRobots, i);
            }
            else if (nonSolarisSentinelCount < 2)
            {
                PlaceRobotInRow(offensiveRobots, i);
            }
            else
            {
                PlaceRobotInRow(balancedRobots, i);
            }
        }
    }
    private void PlaceRobotInRow(RobotCard[] robots, int rowIndex)
    {
        int randomIndex = Random.Range(0, robots.Length);
        RobotCard robot = robots[randomIndex];

        if (money >= GetCost(robot))
        {
            if (money - GetCost(robot) < GetCost(moneyProducer) && currentMoneyProducers < maxMoneyProducers)
            {
                PlaceMoneyProducer();
            }
            else
            {
                GameObject emptySlab = botGrid.GetRandomEmptyPositionInRow(rowIndex);
                if (emptySlab != null)
                {
                    SlabController slab = emptySlab.GetComponent<SlabController>();
                    Robot tmp = robot.RobotPrefab.GetComponent<Robot>();
                    tmp.Bot = this.gameObject.GetComponent<BotAIController>();
                    GameObject placedRobot = Instantiate(robot.RobotPrefab);
                    placedRobot.transform.position = emptySlab.transform.position;
                    botGrid.Robots[slab.XIndex, slab.YIndex] = tmp;
                    tmp.SetData(robot);
                    money = money - GetCost(robot);
                }
            }

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
    public void MoneyProducerEliminated()
    {
        currentMoneyProducers = currentMoneyProducers - 1;
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