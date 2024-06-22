using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGenerator : MonoBehaviour
{
    GameObject robot;
    int generationTime;
    BinaryTree<float> upgrades;
    float currentUpgrade;
    private void Start()
    {
        upgrades = new BinaryTree<float>();
        upgrades.InsertNewNode(1);

    }
}
