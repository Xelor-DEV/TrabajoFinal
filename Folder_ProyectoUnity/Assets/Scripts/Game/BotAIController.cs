using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Text;

public class BotAIController : MonoBehaviour
{
    private enum BotState
    {
        Offensive,
        Defensive,
        Balanced
    }

    [System.Serializable]
    public class ProductionItem
    {
        public RobotCard robotCard;
        public float remainingTime;
        public int targetColumn = -1; // Columna específica (-1 = aleatoria)
        public int targetRow = -1;    // Fila específica (-1 = aleatoria)
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
    [SerializeField] private float probabilityCooldown;
    [SerializeField] private int maxMoneyProducers;
    [SerializeField] private float robotPlacementProbability;
    [SerializeField] private int currentMoneyProducers;


    [Header("Debug")]
    [SerializeField] private bool debugEnabled = true;
    [SerializeField] private TMP_Text debugText;
    public BaseController PlayerBase
    {
        get
        {
            return playerBase;
        }
    }

    public List<ProductionItem> productionQueue = new List<ProductionItem>();
    public Dictionary<int, bool> protectedRows = new Dictionary<int, bool>(); // Cambiado a filas protegidas
    public List<SolarisSentinelController> activeSolaris = new List<SolarisSentinelController>();
    private bool isReactingToThreat = false;

    private void Start()
    {
        currentState = BotState.Offensive;
        currentMoneyProducers = 0;
        StartCoroutine(AIUpdateRoutine());

        // Inicializar columnas protegidas
        for (int i = 0; i < botGrid.Length; i++) // Usar Length (filas)
        {
            protectedRows[i] = false;
        }


        if (debugEnabled && debugText != null)
            StartCoroutine(DebugRoutine());
    }

    private IEnumerator DebugRoutine()
    {
        while (true)
        {
            UpdateDebugInfo();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void UpdateDebugInfo()
    {
        if (!debugEnabled || debugText == null) return;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Estado: {currentState}");
        sb.AppendLine($"Dinero: {money}");
        sb.AppendLine($"Productores: {currentMoneyProducers}/{maxMoneyProducers}");
        sb.AppendLine("Cola de producción:");

        foreach (var item in productionQueue)
        {
            string target = item.targetColumn >= 0 ? $"Col: {item.targetColumn}" : "Aleatorio";
            sb.AppendLine($"- {item.robotCard.RobotName} ({item.remainingTime:F1}s) {target}");
        }

        sb.AppendLine("Filas protegidas:"); // Cambiado a filas
        foreach (var row in protectedRows)
        {
            sb.AppendLine($"- Fila {row.Key}: {(row.Value ? "PROTEGIDA" : "vulnerable")}");
        }

        debugText.text = sb.ToString();
    }

    private IEnumerator AIUpdateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(actionCooldown);

            if (!isReactingToThreat)
            {
                UpdateBotState();
                CheckForThreats(); // Llamada CORRECTA a CheckForThreats
                PerformBotAction();
            }
        }
    }

    private void Update()
    {
        UpdateProductionQueue();
    }

    private void UpdateProductionQueue()
    {
        for (int i = productionQueue.Count - 1; i >= 0; i--)
        {
            productionQueue[i].remainingTime -= Time.deltaTime;
            if (productionQueue[i].remainingTime <= 0)
            {
                PlaceRobotFromQueue(productionQueue[i]);
                productionQueue.RemoveAt(i);
            }
        }
    }



    private void UpdateBotState()
    {
        float lifePercentage = (float)botBase.Life / botBase.MaxLife * 100;
        currentState = lifePercentage > 75 ? BotState.Offensive :
                       lifePercentage > 50 ? BotState.Balanced :
                       BotState.Defensive;
    }

    private void PerformBotAction()
    {
        // Limitar la cola de producción a 5 robots
        if (productionQueue.Count >= 5) return;

        if (currentMoneyProducers < maxMoneyProducers)
        {
            ScheduleProduction(moneyProducer);
        }
        else
        {
            RobotCard robotToPlace = GetStrategicRobot();
            if (robotToPlace != null)
            {
                ScheduleProduction(robotToPlace);
            }
        }
    }
    private void CheckForThreats()
    {
        foreach (var solaris in activeSolaris.ToArray())
        {
            if (solaris == null || solaris.IsDead)
            {
                activeSolaris.Remove(solaris);
                continue;
            }

            // Si la vida es menor al 70% considerar bajo ataque
            if ((float)solaris.Life / solaris.MaxLife < 0.7f)
            {
                int row = GetRowForSolaris(solaris); // Obtener fila, no columna
                if (row != -1 && !protectedRows[row]) // Verificar fila
                {
                    ReactToThreat(row); // Pasar fila amenazada
                }
            }
        }
    }

    private void ReactToThreat(int threatenedRow)
    {
        isReactingToThreat = true;

        // Cancelar producción actual y reembolsar
        foreach (var item in productionQueue)
        {
            money += item.robotCard.Cost;
        }
        productionQueue.Clear();

        // Marcar fila como protegida
        protectedRows[threatenedRow] = true;

        // 1. Colocar Goliath en frente (Y=0) de la fila amenazada
        if (money >= defensiveRobots[1].Cost)
        {
            ScheduleProduction(defensiveRobots[1], threatenedRow, 0);
        }

        // 2. Colocar Beetle Blaster atrás (Y=1) de la fila amenazada
        if (money >= offensiveRobots[0].Cost)
        {
            ScheduleProduction(offensiveRobots[0], threatenedRow, 1);
        }

        // 3. Reponer Solaris en la misma fila si fue destruido
        if (currentMoneyProducers < maxMoneyProducers)
        {
            ScheduleProduction(moneyProducer, threatenedRow, botGrid.Width - 1);
        }

        isReactingToThreat = false;
    }

    private RobotCard GetAffordableAttacker()
    {
        List<RobotCard> affordableAttackers = new List<RobotCard>();

        // Beetle Blaster (ofensiveRobots[0])
        if (money >= offensiveRobots[0].Cost)
        {
            affordableAttackers.Add(offensiveRobots[0]);
        }

        // Rhino Rampart (ofensiveRobots[1])
        if (money >= offensiveRobots[1].Cost)
        {
            affordableAttackers.Add(offensiveRobots[1]);
        }

        // Bombardier Bastion (defensiveRobots[0])
        if (money >= defensiveRobots[0].Cost)
        {
            affordableAttackers.Add(defensiveRobots[0]);
        }

        return affordableAttackers.Count > 0 ?
               affordableAttackers[Random.Range(0, affordableAttackers.Count)] :
               null;
    }

    private int GetColumnForSolaris(SolarisSentinelController solaris)
    {
        for (int x = 0; x < botGrid.Length; x++)
        {
            for (int y = 0; y < botGrid.Width; y++)
            {
                if (botGrid.Robots[x, y] == solaris)
                {
                    return y; // Y es la columna
                }
            }
        }
        return -1;
    }

    private int GetRowForSolarisInColumn(int column)
    {
        for (int row = 0; row < botGrid.Length; row++)
        {
            // Verificar si el robot existe y es Solaris
            if (botGrid.Robots[row, column] != null &&
                botGrid.Robots[row, column] is SolarisSentinelController)
            {
                return row;
            }
        }
        return -1;
    }

    private bool IsPositionEmpty(int row, int column)
    {
        if (row < 0 || row >= botGrid.Length || column < 0 || column >= botGrid.Width)
            return false;

        return botGrid.Robots[row, column] == null;
    }

    private bool CheckForAttackerInRow(int row)
    {
        // Verificar si hay Beetle Blasters o Rhinos en la fila
        for (int col = 1; col < botGrid.Width - 1; col++) // Excluir primera y última columna
        {
            if (botGrid.Robots[row, col] is BeetleBlasterController ||
                botGrid.Robots[row, col] is RhinoRampartController)
            {
                return true;
            }
        }
        return false;
    }

    private void HandleComplexThreat(int threatenedRow)
    {
        // 1. Intentar colocar Goliath en fila adyacente (Y=0)
        int[] adjacentRows = {
        threatenedRow,
        (threatenedRow + 1) % botGrid.Length,
        (threatenedRow - 1 + botGrid.Length) % botGrid.Length
    };

        foreach (int row in adjacentRows)
        {
            if (IsPositionEmpty(row, 0) && money >= defensiveRobots[1].Cost)
            {
                ScheduleProduction(defensiveRobots[1], row, 0);
                return;
            }
        }

        // 2. Intentar colocar Beetle en fila amenazada (Y=1-4)
        if (money >= offensiveRobots[0].Cost)
        {
            for (int col = 1; col < botGrid.Width - 1; col++)
            {
                if (IsPositionEmpty(threatenedRow, col))
                {
                    ScheduleProduction(offensiveRobots[0], threatenedRow, col);
                    return;
                }
            }
        }
    }

    private void ScheduleProduction(RobotCard robot, int targetRow = -1, int targetColumn = -1)
    {
        if (money < robot.Cost) return;

        // Reasignar tiempo si hay ítems en cola
        float timeBonus = 0;
        if (productionQueue.Count > 0)
        {
            timeBonus = productionQueue[0].remainingTime;
            productionQueue.RemoveAt(0);
        }

        ProductionItem newItem = new ProductionItem()
        {
            robotCard = robot,
            remainingTime = robot.GenerationTime - timeBonus,
            targetColumn = targetColumn,
            targetRow = targetRow
        };

        money -= robot.Cost;
        productionQueue.Add(newItem);
    }

    private int GetRowForSolaris(SolarisSentinelController solaris)
    {
        for (int x = 0; x < botGrid.Length; x++)
        {
            for (int y = 0; y < botGrid.Width; y++)
            {
                if (botGrid.Robots[x, y] == solaris)
                {
                    return x; // Devolver fila (X)
                }
            }
        }
        return -1;
    }

    private RobotCard GetStrategicRobot()
    {
        RobotCard[] targetRobots = currentState switch
        {
            BotState.Offensive => offensiveRobots,
            BotState.Defensive => defensiveRobots,
            _ => balancedRobots
        };

        // Filtrar robots que podemos pagar
        List<RobotCard> affordableRobots = new List<RobotCard>();
        foreach (var robot in targetRobots)
        {
            if (money >= robot.Cost)
            {
                affordableRobots.Add(robot);
            }
        }

        if (affordableRobots.Count == 0) return null;
        return affordableRobots[Random.Range(0, affordableRobots.Count)];
    }

    private void PlaceRobotFromQueue(ProductionItem item)
    {
        GameObject slab = null;
        int maxAttempts = 10;
        int attempts = 0;

        while (slab == null && attempts < maxAttempts)
        {
            // 1. Intentar posición específica primero
            if (item.targetRow >= 0 && item.targetColumn >= 0)
            {
                slab = GetSlab(item.targetRow, item.targetColumn);
                if (slab != null && botGrid.Robots[item.targetRow, item.targetColumn] == null)
                {
                    // Posición está vacía, usarla
                    break;
                }
                else
                {
                    slab = null; // Posición ocupada, intentar otra vez
                }
            }

            // 2. Solaris: Última COLUMNA (Y = Width-1)
            else if (item.robotCard == moneyProducer)
            {
                // Priorizar fila específica si está definida
                if (item.targetRow >= 0)
                {
                    slab = botGrid.GetRandomEmptyPositionInColumn(botGrid.Width - 1, item.targetRow);
                }

                if (slab == null)
                {
                    slab = GetBestPositionForSolaris();
                }
            }
            // 3. Defensores: COLUMNA frontal (Y=0)
            else if (item.robotCard == defensiveRobots[1]) // Goliath Guardian
            {
                // Priorizar fila específica si está definida
                if (item.targetRow >= 0)
                {
                    slab = GetSlab(item.targetRow, 0);
                    if (slab != null && botGrid.Robots[item.targetRow, 0] == null)
                    {
                        break;
                    }
                }

                // Buscar en fila adyacente si no se encontró
                if (slab == null)
                {
                    slab = GetBestFrontlinePosition();
                }
            }
            // 4. Atacantes: COLUMNAS medias (Y=1 a Y=4)
            else if (item.robotCard == offensiveRobots[0] ||
                     item.robotCard == offensiveRobots[1] ||
                     item.robotCard == defensiveRobots[0])
            {
                // Priorizar fila específica si está definida
                if (item.targetRow >= 0)
                {
                    // Buscar en columnas medias (1-4) de la fila específica
                    for (int col = 1; col < botGrid.Width - 1; col++)
                    {
                        slab = GetSlab(item.targetRow, col);
                        if (slab != null && botGrid.Robots[item.targetRow, col] == null)
                        {
                            break;
                        }
                        slab = null;
                    }
                }

                // Buscar cualquier posición si no se encontró
                if (slab == null)
                {
                    slab = GetBestMidPosition();
                }
            }
            // 5. Otros robots: Posición estratégica
            else
            {
                slab = botGrid.GetRandomEmptyPosition();
            }

            attempts++;
        }


        if (slab == null)
        {
            money += item.robotCard.Cost;
            Debug.LogWarning("No se encontró posición vacía. Reembolsando dinero.");
            return;
        }

        SlabController slabData = slab.GetComponent<SlabController>();
        Vector3 position = new Vector3(
            slab.transform.position.x,
            slab.transform.position.y,
            slab.transform.position.z
        );

        GameObject robotObj = Instantiate(
            item.robotCard.RobotPrefab,
            position,
            item.robotCard.RobotPrefab.transform.rotation
        );

        Robot newRobot = robotObj.GetComponent<Robot>();
        newRobot.Bot = this;
        newRobot.SetData(item.robotCard);
        newRobot.StartBehavior();

        // Registrar en el grid
        botGrid.Robots[slabData.XIndex, slabData.YIndex] = newRobot;

        // Registrar Solaris para monitoreo
        if (item.robotCard == moneyProducer)
        {
            SolarisSentinelController solaris = robotObj.GetComponent<SolarisSentinelController>();
            if (solaris != null)
            {
                activeSolaris.Add(solaris);
                currentMoneyProducers++;
            }
        }
    }

    private GameObject GetSlab(int x, int y)
    {
        if (x < 0 || x >= botGrid.Length || y < 0 || y >= botGrid.Width)
            return null;
        return botGrid.Slabs[x, y];
    }


    private GameObject GetSlabInFrontline(int row)
    {
        if (row < 0 || row >= botGrid.Length) return null;

        GameObject slab = GetSlab(row, 0);
        if (slab != null && botGrid.Robots[row, 0] == null)
        {
            return slab;
        }
        return null;
    }

    private GameObject GetAdjacentFrontlinePosition(int referenceRow)
    {
        // Buscar filas adyacentes en Y=0
        int[] searchOrder = {
        referenceRow,
        (referenceRow + 1) % botGrid.Length,
        (referenceRow - 1 + botGrid.Length) % botGrid.Length
    };

        foreach (int row in searchOrder)
        {
            GameObject slab = GetSlab(row, 0);
            if (slab != null && botGrid.Robots[row, 0] == null)
            {
                return slab;
            }
        }

        // Fallback: cualquier posición en la columna izquierda
        return botGrid.GetRandomEmptyPositionInColumn(0);
    }

    private GameObject GetAdjacentPosition(int referenceRow, int column)
    {
        // Buscar filas adyacentes en la columna especificada
        int[] searchOrder = {
        referenceRow,
        (referenceRow + 1) % botGrid.Length,
        (referenceRow - 1 + botGrid.Length) % botGrid.Length
    };

        foreach (int row in searchOrder)
        {
            GameObject slab = GetSlab(row, column);
            if (slab != null && botGrid.Robots[row, column] == null)
            {
                return slab;
            }
        }

        // Fallback: cualquier posición en la columna
        return botGrid.GetRandomEmptyPositionInColumn(column);
    }

    // Llamar cuando un robot es destruido
    public void OnRobotDestroyed(Robot robot)
    {
        // Buscar y liberar la posición en el grid
        for (int x = 0; x < botGrid.Length; x++)
        {
            for (int y = 0; y < botGrid.Width; y++)
            {
                if (botGrid.Robots[x, y] == robot)
                {
                    botGrid.Robots[x, y] = null;

                    // Si era Solaris, actualizar contador
                    if (robot is SolarisSentinelController)
                    {
                        currentMoneyProducers--;
                        activeSolaris.Remove(robot as SolarisSentinelController);
                    }
                    return;
                }
            }
        }
    }


    private GameObject GetBestPositionForSolaris()
    {
        // Priorizar filas sin protección primero
        for (int row = 0; row < botGrid.Length; row++)
        {
            if (!protectedRows[row])
            {
                var slab = botGrid.GetRandomEmptyPositionInColumn(botGrid.Width - 1, row);
                if (slab != null) return slab;
            }
        }

        // Si todas están protegidas, cualquier posición en la última columna
        return botGrid.GetRandomEmptyPositionInColumn(botGrid.Width - 1);
    }

    private GameObject GetBestDefensivePosition(int targetColumn)
    {
        // 1. Priorizar fila frontal (X=0) en la columna amenazada
        for (int row = 0; row < botGrid.Length; row++)
        {
            GameObject slab = GetSlab(row, targetColumn);
            if (slab != null && botGrid.Robots[row, targetColumn] == null)
            {
                return slab;
            }
        }

        // 2. Si está ocupada, buscar en columna adyacente
        int adjacentCol = GetNearestEmptyColumn(targetColumn);
        if (adjacentCol >= 0)
        {
            for (int row = 0; row < botGrid.Length; row++)
            {
                GameObject slab = GetSlab(row, adjacentCol);
                if (slab != null && botGrid.Robots[row, adjacentCol] == null)
                {
                    return slab;
                }
            }
        }

        // 3. Fallback a cualquier posición en columna frontal
        return botGrid.GetRandomEmptyPositionInColumn(0);
    }

    private int GetNearestEmptyColumn(int referenceColumn)
    {
        int[] searchOrder = { referenceColumn,
                             (referenceColumn + 1) % botGrid.Width,
                             (referenceColumn - 1 + botGrid.Width) % botGrid.Width };

        foreach (int col in searchOrder)
        {
            if (botGrid.IsColumnEmpty(col))
            {
                return col;
            }
        }
        return -1;
    }

    private GameObject GetBestFrontlinePosition()
    {
        List<GameObject> nonProtectedSlabs = new List<GameObject>();
        List<GameObject> protectedSlabs = new List<GameObject>();

        // Buscar en todas las filas (X) en la columna frontal (Y=0)
        for (int row = 0; row < botGrid.Length; row++)
        {
            GameObject slab = GetSlab(row, 0);
            if (slab != null && botGrid.Robots[row, 0] == null)
            {
                // Priorizar filas no protegidas
                if (!protectedRows.ContainsKey(row) || !protectedRows[row])
                {
                    nonProtectedSlabs.Add(slab);
                }
                else
                {
                    protectedSlabs.Add(slab);
                }
            }
        }

        // 1. Devolver posición en fila no protegida si existe
        if (nonProtectedSlabs.Count > 0)
        {
            return nonProtectedSlabs[Random.Range(0, nonProtectedSlabs.Count)];
        }

        // 2. Si no hay filas no protegidas, usar fila protegida
        if (protectedSlabs.Count > 0)
        {
            return protectedSlabs[Random.Range(0, protectedSlabs.Count)];
        }

        // 3. Fallback a cualquier posición en la columna frontal (Y=0)
        return botGrid.GetRandomEmptyPositionInColumn(0);
    }

    private GameObject GetBestMidPosition()
    {
        // Columnas medias (1 a Width-2)
        for (int col = 1; col < botGrid.Width - 1; col++)
        {
            var slab = botGrid.GetRandomEmptyPositionInColumn(col);
            if (slab != null) return slab;
        }

        // Fallback a cualquier posición
        return botGrid.GetRandomEmptyPosition();
    }

    public void AddMoney(int amount) => money += amount;
    public void MoneyProducerEliminated() => currentMoneyProducers--;

}