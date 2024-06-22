using System;
public class SinglyLinkedList<T>
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
        public Node(T value)
        {
            Value = value;
            Next = null;
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
    private Node GetPreviousNodeAtPosition(int position)
    {
        Node previous = head;
        int iterator = 0;
        while (iterator < position - 1)
        {
            previous = previous.Next;
            iterator = iterator + 1;
        }
        return previous;
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
    private Node GetNodeBeforeLastNode()
    {
        Node previousOfLastNode = head;
        while (previousOfLastNode.Next.Next != null)
        {
            previousOfLastNode = previousOfLastNode.Next;
        }
        return previousOfLastNode;
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
            count = count + 1;
        }
    }
    public void InsertNodeAtPosition(T value, int position)
    {
        if (position == 0)
        {
            InsertNodeAtStart(value);
        }
        else if (position == Count - 1)
        {
            InsertNodeAtEnd(value);
        }
        else if (position >= Count)
        {
            throw new Exception("The introduced value goes beyond the length of the list");
        }
        else
        {
            Node previous = GetPreviousNodeAtPosition(position);
            Node next = previous.Next;
            Node newNode = new Node(value);
            previous.Next = newNode;
            newNode.Next = next;
            count = count + 1;
        }
    }
    public void ModifyAtStart(T value)
    {
        if (head == null)
        {
            throw new Exception("There is no node.");
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
    public void ModifyNodeAtPosition(T value, int position)
    {
        if (position == 0)
        {
            ModifyAtStart(value);
        }
        else if (position == Count - 1)
        {
            ModifyAtEnd(value);
        }
        else if (position >= Count)
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
        else if (position == Count - 1)
        {
            return GetNodeFromEnd();
        }
        else if (position >= Count)
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
            Node previousOfLastNode = GetNodeBeforeLastNode();
            Node lastNode = previousOfLastNode.Next;
            lastNode = null;
            previousOfLastNode.Next = null;
            count = count - 1;
        }
    }
    public void DeleteNodeByPosition(int position)
    {
        if (position == 0)
        {
            DeleteAtStart();
        }
        else if (position == Count - 1)
        {
            DeleteAtEnd();
        }
        else if (position >= Count)
        {
            throw new Exception("The introduced value goes beyond the length of the list");
        }
        else
        {
            Node previous = GetPreviousNodeAtPosition(position);
            Node next = previous.Next.Next;
            Node nodePosition = previous.Next;
            nodePosition.Next = null;
            nodePosition = null;
            previous.Next = null;
            previous.Next = next;
            count = count - 1;
        }
    }
    public T[] ListToArray()
    {
        T[] array = new T[Count];
        Node current = head;
        int index = 0;
        while (current != null)
        {
            array[index] = current.Value;
            current = current.Next;
            index = index + 1;
        }
        return array;
    }
}
