using UnityEngine;
using TMPro;
using System;

public class PlayerInventory : MonoBehaviour
{
    private DoublyLinkedList<RobotCard> robotList = new DoublyLinkedList<RobotCard>();
    private RobotCard[] displayedRobots;
    [SerializeField] private int currentPage;
    [SerializeField] private int robotsPerPage;
    [SerializeField] private int totalPages;
    [SerializeField] private TMP_Text pageText;
    public Action<int> OnInventoryUpdated;

    public RobotCard[] DisplayedRobots => displayedRobots;

    void Start()
    {
        displayedRobots = new RobotCard[robotsPerPage];
        UpdatePageText();
    }

    public void AddRobot(RobotCard newRobot)
    {
        robotList.InsertNodeAtEnd(newRobot);
        if (robotList.Count > robotsPerPage * totalPages)
        {
            totalPages++;
        }
        UpdateDisplayedRobots();
    }

    public void RemoveRobot(int index)
    {
        if (index < 0 || index >= robotList.Count)
        {
            throw new IndexOutOfRangeException("Índice fuera de rango");
        }

        robotList.DeleteNodeByPosition(index);
        if (robotList.Count <= robotsPerPage * (totalPages - 1) && totalPages > 1)
        {
            totalPages--;
        }
        if (currentPage > totalPages - 1 && currentPage != 0)
        {
            currentPage = totalPages - 1;
        }
        UpdateDisplayedRobots();
    }

    public void NextPage()
    {
        if (totalPages == 0) return;

        if (currentPage == totalPages - 1)
        {
            currentPage = 0;
        }
        else
        {
            currentPage++;
        }
        UpdateDisplayedRobots();
    }

    public void PreviousPage()
    {
        if (totalPages == 0) return;

        if (currentPage == 0)
        {
            currentPage = totalPages - 1;
        }
        else
        {
            currentPage--;
        }
        UpdateDisplayedRobots();
    }

    public void UpdateDisplayedRobots()
    {
        UpdatePageText();

        if (robotList.Count == 0)
        {
            displayedRobots = new RobotCard[0];
            OnInventoryUpdated?.Invoke(0);
            return;
        }

        int start = currentPage * robotsPerPage;
        int end = Mathf.Min(start + robotsPerPage, robotList.Count) - 1;
        if (start > end)
        {
            start = end;
        }
        displayedRobots = robotList.GetNodesInRange(start, end);
        OnInventoryUpdated?.Invoke(start);
    }

    private void UpdatePageText()
    {
        if (pageText == null) return;

        if (robotList.Count == 0)
        {
            pageText.text = "0/0";
            return;
        }

        pageText.text = $"{currentPage + 1}/{totalPages}";
    }
}