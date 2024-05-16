using System;
public class DoubleLinkedCircularList<T>
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
        ~Node()
        {
            Next = null;
            Previous = null;
        }
    }
    public void InsertNodeAtStart(T value)
    {
        if (head == null)
        {
            Node newNode = new Node(value);
            head = newNode;
            head.Next = head;
            head.Previous = head;
            count = count + 1;
        }
        else
        {
            Node newNode = new Node(value);
            Node lastNode = head.Previous;
            newNode.Next = head;
            newNode.Previous = lastNode;
            lastNode.Next = newNode;
            head.Previous = newNode;
            head = newNode;
            count = count + 1;
        }
    }
    public void InsertNodeAtEnd(T value)
    {
        Node newNode = new Node(value);
        if (head == null)
        {
            head = newNode;
            head.Next = head;
            head.Previous = head;
        }
        else
        {
            if (head.Previous != null)
            {
                Node lastNode = head.Previous;
                newNode.Next = head;
                newNode.Previous = lastNode;
                lastNode.Next = newNode;
                head.Previous = newNode;
            }
            else
            {
                head.Next = newNode;
                head.Previous = newNode;
                newNode.Next = head;
                newNode.Previous = head;
            }
        }
        count = count + 1;
    }

    public void InsertNodeAtPosition(T value, int position)
    {
        if (position == 0)
        {
            InsertNodeAtStart(value);
        }
        else if (position == count - 1)
        {
            InsertNodeAtEnd(value);

        }
        else if (position >= count || position < 0)
        {
            throw new IndexOutOfRangeException("Es una posicion mas alla del tamaño de la lista o es un numero negativo");
        }
        else
        {
            Node currentNode = head;
            int iterator = 0;
            while (iterator < position)
            {
                currentNode = currentNode.Next;
                iterator = iterator + 1;
            }
            Node newNode = new Node(value);
            Node previousCurrentNode = currentNode.Previous;
            newNode.Previous = currentNode.Previous;
            newNode.Next = currentNode;
            previousCurrentNode.Next = newNode;
            currentNode.Previous = newNode;
            count = count + 1;
        }
    }
    public void ModifyAtStart(T value)
    {
        if (head == null)
        {
            throw new NullReferenceException();
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
            Node lastNode = head.Previous;
            lastNode.Value = value;
        }
    }
    public void ModifyAtPosition(T value, int position)
    {
        if (position == 0)
        {
            ModifyAtStart(value);
        }
        else if (position == count - 1)
        {
            ModifyAtEnd(value);
        }
        else if (position >= count || position < 0)
        {
            throw new IndexOutOfRangeException();
        }
        else
        {
            Node tmp = head;
            int iterator = 0;
            while (iterator < position)
            {
                tmp = tmp.Next;
                iterator = iterator + 1;
            }
            tmp.Value = value;
        }
    }
    public T GetNodeAtStart()
    {
        if (head == null)
        {
            throw new NullReferenceException();
        }
        else
        {
            return head.Value;
        }
    }
    public T GetNodeAtEnd()
    {
        if (head == null)
        {
            return GetNodeAtStart();
        }
        else
        {
            Node lastNode = head.Previous;
            return lastNode.Value;
        }
    }
    public T GetNodeAtPosition(int position)
    {
        if (position == 0)
        {
            return GetNodeAtStart();
        }
        else if (position == count - 1)
        {
            return GetNodeAtEnd();
        }
        else if (position >= count || position < 0)
        {
            throw new IndexOutOfRangeException();
        }
        else
        {
            Node tmp = head;
            int iterator = 0;
            while (iterator < position)
            {
                tmp = tmp.Next;
                iterator = iterator + 1;
            }
            return tmp.Value;
        }
    }
    public void DeleteNodeAtStart()
    {
        if (head == null)
        {
            throw new NullReferenceException();
        }
        else
        {
            Node lastNode = head.Previous;
            Node newHead = head.Next;
            Node oldHead = head;
            head = newHead;
            head.Previous = lastNode;
            lastNode.Next = head;
            oldHead.Next = null;
            oldHead.Previous = null;
            oldHead = null;
            count = count - 1;
        }
    }
    public void DeleteNodeAtEnd()
    {
        if (head == null)
        {
            DeleteNodeAtStart();
        }
        else
        {
            Node lastNode = head.Previous;
            Node newLastNode = lastNode.Previous;
            newLastNode.Next = head;
            head.Previous = newLastNode;
            lastNode.Next = null;
            lastNode.Previous = null;
            lastNode = null;
            count = count - 1;
        }
    }
    public void DeleteNodeAtPosition(int position)
    {
        if (position == 0)
        {
            DeleteNodeAtStart();
        }
        else if (position == count - 1)
        {
            DeleteNodeAtEnd();
        }
        else if (position >= count || position < 0)
        {
            throw new IndexOutOfRangeException("No existe esa posicion en la lista");
        }
        else
        {
            Node current = head;
            int iterator = 0;
            while (iterator < position)
            {
                current = current.Next;
                iterator = iterator + 1;
            }
            Node previousNode = current.Previous;
            Node nextNode = current.Next;
            previousNode.Next = nextNode;
            nextNode.Previous = previousNode;
            current.Next = null;
            current.Previous = null;
            current = null;
            count = count - 1;
        }
    }
}
