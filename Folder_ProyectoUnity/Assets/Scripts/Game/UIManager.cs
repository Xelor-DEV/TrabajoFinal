using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GridLayoutGroup CardHolder;
    [SerializeField] private GameObject robotCardPrefab;
    private void OnEnable()
    {
        playerInventory.OnInventoryUpdated += UpdateDisplayedRobots;
    }
    private void OnDisable()
    {
        playerInventory.OnInventoryUpdated -= UpdateDisplayedRobots;
    }
    private void UpdateDisplayedRobots(int start)
    {
        for (int i = 0; i < CardHolder.transform.childCount; ++i)
        {
            GameObject child = CardHolder.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        for (int i = 0; i < playerInventory.DisplayedRobots.Length; ++i)
        {
            RobotCard robot = playerInventory.DisplayedRobots[i];
            if (robot != null)
            {
                GameObject robotCard = Instantiate(robotCardPrefab, CardHolder.transform);
                RobotCardController robotCardController = robotCard.GetComponent<RobotCardController>();
                robotCardController.SetData(robot, start + i);
            }
        }
    }
}