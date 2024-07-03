using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
public class RobotGenerator : MonoBehaviour
{
    [SerializeField] private RobotCard card;
    [SerializeField] private PlayerInventory inventory;
    BinaryTree<int> upgrades;
    [SerializeField] private UpgradeTree upgradeTree;
    private int currentUpgrade = 0;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text upgradeType;
    [SerializeField] private TMP_Text upgradeValue;
    [SerializeField] private TMP_Text leftOption;
    [SerializeField] private TMP_Text rightOption;
    [SerializeField] private TMP_Text generationText;
    private Timer timer;
    private bool isGenerating = false;
    private bool isFirstUpdate = true;
    private void Awake()
    {
        timer = new Timer(card.GenerationTime);
        upgrades = upgradeTree.GetBinaryTree();
        timer.OnTimerUpdate += UpdateProgressBar;
        timer.OnTimerComplete += GenerateRobot;
        generationText.text = "Generate " + card.RobotName + " \n Cost: " + card.Cost;
        FirstUpgradeOptions();
    }
    private void Update()
    {
        if (isGenerating == true)
        {
            timer.Update();
        }
    }
    public void BuyRobot()
    {
        if (PlayerController.Instance.Money >= card.Cost && isGenerating == false)
        {
            PlayerController.Instance.SubstractMoney(card.Cost);
            isGenerating = true;
            progressBar.gameObject.SetActive(true);
            timer.Start();
        }
    }
    private void GenerateRobot()
    {
        inventory.AddRobot(card);
        isGenerating = false;
        progressBar.gameObject.SetActive(false);
    }
    private void UpdateProgressBar(float progress)
    {
        progressBar.value = progress;
    }
    public void Upgrade(bool isLeft)
    {
        try
        {
            int nextUpgrade = upgrades.GetChildByFather(currentUpgrade, isLeft);
            int upgradeCost = upgradeTree.GetUpgradeCost(nextUpgrade);

            if (PlayerController.Instance.Money >= upgradeCost)
            {
                if (isLeft == true || (isLeft == false && isGenerating == false))
                {
                    PlayerController.Instance.SubstractMoney(upgradeCost);
                    currentUpgrade = nextUpgrade;

                    if (isLeft == true)
                    {
                        card.Life = currentUpgrade;
                        if (isFirstUpdate == true)
                        {
                            upgradeType.text = "Robot Life Upgrades";
                            isFirstUpdate = false;
                        }
                        upgradeValue.text = currentUpgrade.ToString();
                        UpdateLifeUpgradeOptions();
                    }
                    else
                    {
                        card.GenerationTime = currentUpgrade;
                        if (isFirstUpdate == true)
                        {
                            upgradeType.text = "Upgrades in the generation time of robot cards";
                            isFirstUpdate = false;
                        }
                        upgradeValue.text = currentUpgrade.ToString();
                        timer.SetDuration(card.GenerationTime);
                        UpdateTimeUpgradeOptions();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void FirstUpgradeOptions()
    {
        try
        {
            int leftUpgrade = upgrades.GetChildByFather(currentUpgrade, true);
            leftOption.text = "Life Upgrade: " + leftUpgrade + " Cost: " + upgradeTree.GetUpgradeCost(leftUpgrade);
        }
        catch
        {
            leftOption.text = "Not available";
        }

        try
        {
            int rightUpgrade = upgrades.GetChildByFather(currentUpgrade, false);
            rightOption.text = "Generation Time Upgrade: " + rightUpgrade + " Cost: " + upgradeTree.GetUpgradeCost(rightUpgrade);
        }
        catch
        {
            rightOption.text = "Not available";
        }
    }
    private void UpdateLifeUpgradeOptions()
    {
        try
        {
            int leftUpgrade = upgrades.GetChildByFather(currentUpgrade, true);
            leftOption.text = "Life Upgrade: " + leftUpgrade + " Costo: " + upgradeTree.GetUpgradeCost(leftUpgrade);
        }
        catch
        {
            leftOption.text = "No disponible";
        }

        try
        {
            int rightUpgrade = upgrades.GetChildByFather(currentUpgrade, false);
            rightOption.text = "Life Upgrade: " + rightUpgrade + " Costo: " + upgradeTree.GetUpgradeCost(rightUpgrade);
        }
        catch
        {
            rightOption.text = "Not available";
        }
    }

    private void UpdateTimeUpgradeOptions()
    {
        try
        {
            int leftUpgrade = upgrades.GetChildByFather(currentUpgrade, true);
            leftOption.text = "Generation Time Upgrade: " + leftUpgrade + " Cost: " + upgradeTree.GetUpgradeCost(leftUpgrade);
        }
        catch
        {
            leftOption.text = "Not available";
        }

        try
        {
            int rightUpgrade = upgrades.GetChildByFather(currentUpgrade, false);
            rightOption.text = "Generation Time Upgrade: " + rightUpgrade + " Cost: " + upgradeTree.GetUpgradeCost(rightUpgrade);
        }
        catch
        {
            rightOption.text = "Not available";
        }
    }

}
