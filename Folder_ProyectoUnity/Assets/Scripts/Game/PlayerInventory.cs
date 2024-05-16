using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    private DoubleLinkedCircularList<RobotCard> robotList = new DoubleLinkedCircularList<RobotCard>();
    public RobotCard[] displayedRobots;
    [SerializeField] private int currentPage;
    [SerializeField] private int robotsPerPage;
    [SerializeField] private int totalPages;
    public Action OnInventoryUpdated;

    void Start()
    {
        displayedRobots = new RobotCard[robotsPerPage];
        UpdateDisplayedRobots();
    }

    public void AddRobot(RobotCard newRobot)
    {
        robotList.InsertNodeAtEnd(newRobot);
        if (robotList.Count > robotsPerPage * totalPages)
        {
            totalPages = totalPages + 1;
        }
        UpdateDisplayedRobots();
    }

    public void RemoveRobot(RobotCard robot)
    {
        int position = FindRobotPosition(robot);
        if (position != -1)
        {
            robotList.DeleteNodeAtPosition(position);
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
    }

    public void NextPage()
    {
        if (currentPage == totalPages - 1)
        {
            currentPage = 0;
        }
        else
        {
            currentPage = currentPage + 1;
        }
        UpdateDisplayedRobots();
    }

    public void PreviousPage()
    {
        if (currentPage == 0)
        {
            currentPage = totalPages - 1;
        }
        else
        {
            currentPage = currentPage - 1;
        }
        UpdateDisplayedRobots();
    }

    private void UpdateDisplayedRobots()
    {
        int start = currentPage * robotsPerPage;
        for (int i = 0; i < robotsPerPage; ++i)
        {
            if (start + i < robotList.Count)
            {
                displayedRobots[i] = robotList.GetNodeAtPosition(start + i);
            }
            else
            {
                displayedRobots[i] = null;
            }
        }
        OnInventoryUpdated?.Invoke();
    }

    private int FindRobotPosition(RobotCard robot)
    {
        for (int i = 0; i < robotList.Count; i++)
        {
            if (robotList.GetNodeAtPosition(i) == robot)
            {
                return i;
            }
        }
        return -1;
    }
}