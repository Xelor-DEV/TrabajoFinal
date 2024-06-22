using System;
public class Queue<T>
{
    private Node head;
    private Node tail;
    private int count = 0;
    public int Count
    {
        get
        {
            return count;
        }
    }
    private class Node
    {
        public T Value { get; set; }
        public Node Next { get; set; }
        public Node Previous { get; set; }
        public Node (T value)
        {
            Value = value;
            Next = null;
            Previous = null;
        }
    }
    public void Enqueue(T value)
    {
        Node newNode = new Node(value);
        if (head == null)
        {
            head = newNode;
            tail = newNode;
            count = count + 1;
        }
        else
        {
            tail.Next = newNode;
            newNode.Previous = tail;
            tail = newNode;
            count = count + 1;
        }
    }
    public T DequeueAndGet() 
    {
        if (head == null)
        {
            throw new NullReferenceException("No hay elementos en la cola");
        }
        else
        {
            Node oldHead = head;
            T value = oldHead.Value;
            head = head.Next;
            if (head != null)
            {
                head.Previous = null;
            }
            else
            {
                tail = null;
            }
            oldHead.Next = null;
            oldHead = null;
            count = count - 1;
            return value;
        }
    }
}
