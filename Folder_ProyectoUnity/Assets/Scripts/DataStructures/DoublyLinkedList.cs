using System;
public class DoublyLinkedList<T>
{
    private Node head;
    private int count = 0;
    public int Count
    {
        get
        {
            return count;
        }
    }
    class Node
    {
        public T Value { get; set; }
        public Node Next { get; set; }
        public Node Previous { get; set; }
        public Node(T value)
        {
            Value = value;
            Next = null;
            Previous = null;
        }
    }
    private Node GetLastNode()
    {
        Node lastNode = head;
        while (lastNode.Next != null)
        {
            lastNode = lastNode.Next;
        }
        return lastNode;
    }
    private Node GetNodeAtPosition(int position)
    {
        Node nodePosition = head;
        int iterator = 0;
        while (iterator < position)
        {
            nodePosition = nodePosition.Next;
            iterator = iterator + 1;
        }
        return nodePosition;
    }
    public void InsertNodeAtStart(T value)
    {
        if (head == null)
        {
            Node newNode = new Node(value);
            head = newNode;
            count = count + 1;
        }
        else
        {
            Node newNode = new Node(value);
            newNode.Next = head;
            head.Previous = newNode;
            head = newNode;
            count = count + 1;
        }
    }
    public void InsertNodeAtEnd(T value)
    {
        if (head == null)
        {
            InsertNodeAtStart(value);
        }
        else
        {
            Node lastNode = GetLastNode();
            Node newNode = new Node(value);
            lastNode.Next = newNode;
            newNode.Previous = lastNode;
            count = count + 1;
        }
    }
    public void InsertNodeByPosition(T value, int position)
    {
        if (position == 0)
        {
            InsertNodeAtStart(value);
        }
        else if (position == count - 1)
        {
            InsertNodeAtEnd(value);
        }
        else if (position >= count)
        {
            throw new Exception("The introduced value goes beyond the length of the list");
        }
        else
        {
            Node nodePosition = GetNodeAtPosition(position);
            Node newNode = new Node(value);
            Node previousNode = nodePosition.Previous;
            previousNode.Next = newNode;
            newNode.Previous = previousNode;
            newNode.Next = nodePosition;
            nodePosition.Previous = newNode;
            count = count + 1;
        }
    }
    public void ModifyAtStart(T value)
    {
        if (head == null)
        {
            throw new Exception("There are no nodes.");
        }
        else
        {
            head.Value = value;
        }
    }
    public void ModifyAtEnd(T value)
    {
        if (head == null)
        {
            ModifyAtStart(value);
        }
        else
        {
            Node lastNode = GetLastNode();
            lastNode.Value = value;
        }
    }
    public void ModifyNodeByPosition(T value, int position)
    {
        if (position == 0)
        {
            ModifyAtStart(value);
        }
        else if (position == count - 1)
        {
            ModifyAtEnd(value);
        }
        else if (position >= count)
        {
            throw new Exception("That position does not exist in the list");
        }
        else
        {
            Node nodePosition = GetNodeAtPosition(position);
            nodePosition.Value = value;
        }
    }
    public T GetNodeFromStart()
    {
        if (head == null)
        {
            throw new Exception("The list is empty");
        }
        else
        {
            return head.Value;
        }
    }
    public T GetNodeFromEnd()
    {
        if (head == null)
        {
            return GetNodeFromStart();
        }
        else
        {
            Node lastNode = GetLastNode();
            return lastNode.Value;
        }
    }
    public T GetNodeByPosition(int position)
    {
        if (position == 0)
        {
            return GetNodeFromStart();
        }
        else if (position == count - 1)
        {
            return GetNodeFromEnd();
        }
        else if (position >= count)
        {
            throw new Exception("The introduced value goes beyond the length of the list");
        }
        else
        {
            Node nodePosition = GetNodeAtPosition(position);
            return nodePosition.Value;
        }
    }
    public void DeleteAtStart()
    {
        if (head == null)
        {
            throw new Exception("There are no elements in the list");
        }
        else
        {
            Node newHead = head.Next;
            if (newHead != null)
            {
                newHead.Previous = null;
            }
            head.Next = null;
            head = newHead;
            count = count - 1;
        }
    }
    public void DeleteAtEnd()
    {
        if (head == null)
        {
            DeleteAtStart();
        }
        else
        {
            Node lastNode = GetLastNode();
            Node newLastNode = lastNode.Previous;
            lastNode.Previous = null;
            newLastNode.Next = null;
            lastNode = null;
            count = count - 1;
        }
    }
    public void DeleteNodeByPosition(int position)
    {
        if (position == 0)
        {
            DeleteAtStart();
        }
        else if (position == count - 1)
        {
            DeleteAtEnd();
        }
        else if (position >= count)
        {
            throw new Exception("The introduced value goes beyond the length of the list");
        }
        else
        {
            Node nodePosition = GetNodeAtPosition(position);
            Node previousNode = nodePosition.Previous;
            Node nextNode = nodePosition.Next;
            previousNode.Next = nextNode;
            nextNode.Previous = previousNode;
            nodePosition.Previous = null;
            nodePosition.Next = null;
            nodePosition = null;
            count = count - 1;
        }
    }
    public T[] GetNodesInRange(int start, int end)
    {
        if (start < 0 || end >= count)
        {
            throw new IndexOutOfRangeException("Positions must be within the range of the list.");
        }
        if (start > end)
        {
            throw new ArgumentException("The initial position must be less than or equal to the final position.");
        }
        T[] nodesInRange = new T[end - start + 1];
        Node current = head;
        int index = 0;
        while (index < start)
        {
            current = current.Next;
            index = index + 1;
        }
        for (int i = 0; i < nodesInRange.Length; ++i)
        {
            nodesInRange[i] = current.Value;
            current = current.Next;
        }
        return nodesInRange;
    }
}
