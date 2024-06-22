using System;
using UnityEngine;
public class BinaryTree<T>
{
    private Node root;
    private SinglyLinkedList<Node> allNodes = new SinglyLinkedList<Node>();
    class Node
    {
        public T Value { get; set; }
        public Node LeftChild { get; set; }
        public Node RightChild { get; set; }
        public Node(T value)
        {
            Value = value;
            LeftChild = null;
            RightChild = null;
        }
        public void AddChild(Node newChild)
        {
            if (LeftChild == null)
            {
                LeftChild = newChild;
            }
            else if(RightChild == null)
            {
                RightChild = newChild;
            }
            else
            {
                throw new Exception("El arbol ya tiene asignado un hijo izquierdo y uno derecho.");
            }
        }
    }
    public void InsertNewNode(T value)
    {
        if (root == null)
        {
            Node newNode = new Node(value);
            root = newNode;
            allNodes.InsertNodeAtEnd(root);
        }
    }
    public void InsertNewNode(T value, T fatherValue)
    {
        if (root == null)
        {
            InsertNewNode(value);
        }
        else
        {
            Node newNode = new Node(value);
            Node fatherNode = SearchFather(fatherValue);
            fatherNode.AddChild(newNode);
            allNodes.InsertNodeAtEnd(newNode);
        }
    }
    private Node SearchFather(T value)
    {
        dynamic fatherValue = value;
        Node fatherNode = null;
        Node[] allNodesTmp = allNodes.ListToArray();
        for(int i = 0; i < allNodesTmp.Length; ++i)
        {
            if(fatherValue == allNodesTmp[i].Value)
            {
                fatherNode = allNodes.GetNodeByPosition(i);
                break;
            }
        }
        return fatherNode;
    }
    public void InDepthSearch()
    {
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(root);
        Node currentNode = null;
        while(queue.Count > 0)
        {
            currentNode = queue.DequeueAndGet();
            Debug.Log(currentNode.Value);
            if(currentNode.LeftChild != null)
            {
                queue.Enqueue(currentNode.LeftChild);
            }
            if (currentNode.RightChild != null)
            {
                queue.Enqueue(currentNode.RightChild); 
            }
        }
    }
}
