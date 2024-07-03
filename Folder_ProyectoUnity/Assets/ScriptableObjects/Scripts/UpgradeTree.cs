using UnityEngine;
using System;
[CreateAssetMenu(fileName = "UpgradeTree", menuName = "ScriptableObjects/Game/UpgradeTree", order = 1)]
public class UpgradeTree : ScriptableObject
{
    [Serializable]
    public class UpgradeNode
    {
        [SerializeField] private int value;
        public int Value
        {
            get
            {
                return value;
            }
        }
        [SerializeField] private int parentValue;
        public int ParentValue
        {
            get
            {
                return parentValue;
            }
        }
        [SerializeField] private int cost;
        public int Cost
        {
            get
            {
                return cost;
            }
        }
    }
    [SerializeField] private UpgradeNode[] nodes;
    public BinaryTree<int> GetBinaryTree()
    {
        BinaryTree<int> tree = new BinaryTree<int>();
        if (nodes.Length > 0)
        {
            tree.InsertNewNode(nodes[0].Value);
            for (int i = 1; i < nodes.Length; ++i)
            {
                tree.InsertNewNode(nodes[i].Value, nodes[i].ParentValue);
            }
        }
        return tree;
    }

    public int GetUpgradeCost(int upgradeValue)
    {
        for (int i = 0; i < nodes.Length; ++i)
        {
            if (nodes[i].Value == upgradeValue)
            {
                return nodes[i].Cost;
            }
        }
        return 0; 
    }
}
