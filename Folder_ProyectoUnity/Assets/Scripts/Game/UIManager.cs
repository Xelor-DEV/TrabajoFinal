using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private GameObject robotCardPrefab;

    private void OnEnable()
    {
        playerInventory.OnInventoryUpdated += UpdateDisplayedRobots;
    }

    private void OnDisable()
    {
        playerInventory.OnInventoryUpdated -= UpdateDisplayedRobots;
    }

    private void UpdateDisplayedRobots()
    {
        // Primero, destruimos las tarjetas de robots existentes
        for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
        {
            GameObject child = gridLayoutGroup.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        // Luego, creamos nuevas tarjetas de robots para cada robot en el inventario
        for (int i = 0; i < playerInventory.displayedRobots.Length; i++)
        {
            RobotCard robot = playerInventory.displayedRobots[i];
            if (robot != null)
            {
                GameObject robotCard = Instantiate(robotCardPrefab, gridLayoutGroup.transform);
                RobotCardController robotCardController = robotCard.GetComponent<RobotCardController>();
                robotCardController.SetData(robot);
            }
        }
    }
}