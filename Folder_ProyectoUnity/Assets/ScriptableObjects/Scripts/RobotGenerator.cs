using UnityEngine;

public class RobotGenerator : MonoBehaviour
{
    [SerializeField] private RobotCard card;
    [SerializeField] private PlayerInventory inventory;
    GameObject robot;
    int generationTime;
    BinaryTree<float> upgrades;
    float currentUpgrade;
    public void BuyRobot()
    {
        if (PlayerController.Instance.Money >= card.Cost)
        {
            PlayerController.Instance.SubstractMoney(card.Cost);
            inventory.AddRobot(card);
        }
    }
}
