using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GameObject slabPrefab;
    [SerializeField] private GameObject[,] slabs;
    [SerializeField] private Robot[,] robots;
    [SerializeField] private int length;
    [SerializeField] private int width;
    [SerializeField] private Transform startPosition;
    [SerializeField] private float spacingX;
    [SerializeField] private float spacingY;

    public int Length
    {
        get
        {
            return length;
        }
    }

    public int Width
    {
        get
        {
            return width;
        }
    }

    public Robot[,] Robots
    {
        get
        {
            return robots;
        }
    }
    public GameObject[,] Slabs
    {
        get
        {
            return slabs;
        }
    }
    void Start()
    {
        InitializeGrid();
        GenerateSlabs();
    }
    void InitializeGrid()
    {
        slabs = new GameObject[length, width];
        robots = new Robot[length, width];
    }
    void GenerateSlabs()
    {
        for (int i = 0; i < length; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                Vector3 position = new Vector3(startPosition.position.x + (spacingX * j), startPosition.position.y, startPosition.position.z + (spacingY * i));
                GameObject slab = Instantiate(slabPrefab, position, slabPrefab.transform.rotation);
                SlabController slabComponent = slab.GetComponent<SlabController>();
                if (slabComponent != null)
                {
                    slabComponent.XIndex = i;
                    slabComponent.YIndex = j;
                }
                slabs[i, j] = slab;
            }
        }
    }
    public bool IsFull()
    {
        for (int i = 0; i < length; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (robots[i, j] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public GameObject GetRandomEmptyPositionInColumn(int columnIndex)
    {
        // Verificar que la columna exista
        if (columnIndex < 0 || columnIndex >= width) return null;

        List<GameObject> emptySlabs = new List<GameObject>();

        // Buscar en TODAS las filas de la columna
        for (int row = 0; row < length; row++)
        {
            if (robots[row, columnIndex] == null)
            {
                emptySlabs.Add(slabs[row, columnIndex]);
            }
        }

        return emptySlabs.Count > 0 ?
               emptySlabs[Random.Range(0, emptySlabs.Count)] :
               null;
    }


    public GameObject GetRandomEmptyPosition()
    {
        int emptyCount = 0;
        GameObject emptySlab = null;

        for (int i = 0; i < length; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (robots[i, j] == null)
                {
                    emptyCount = emptyCount + 1;
                    if (Random.Range(0, emptyCount) == 0)
                    {
                        emptySlab = slabs[i, j];
                    }
                }
            }
        }

        return emptySlab;
    }

    public bool IsColumnEmpty(int columnIndex)
    {
        // Verificar que la columna exista
        if (columnIndex < 0 || columnIndex >= width) return false;

        for (int row = 0; row < length; row++)
        {
            if (robots[row, columnIndex] != null)
            {
                return false;
            }
        }
        return true;
    }

    public GameObject GetRandomEmptyPositionInColumn(int columnIndex, int specificRow = -1)
    {
        if (columnIndex < 0 || columnIndex >= width) return null;

        List<GameObject> emptySlabs = new List<GameObject>();

        // Buscar en fila específica si está definida
        if (specificRow >= 0 && specificRow < length)
        {
            if (Robots[specificRow, columnIndex] == null)
            {
                return Slabs[specificRow, columnIndex];
            }
            return null;
        }

        // Buscar en todas las filas
        for (int row = 0; row < length; row++)
        {
            if (Robots[row, columnIndex] == null)
            {
                emptySlabs.Add(Slabs[row, columnIndex]);
            }
        }

        return emptySlabs.Count > 0 ?
               emptySlabs[Random.Range(0, emptySlabs.Count)] :
               null;
    }

    public GameObject GetRandomEmptyPositionInRow(int rowIndex)
    {
        // Verificar que el índice esté dentro de los límites
        if (rowIndex < 0 || rowIndex >= length)
        {
            Debug.LogError($"Índice de fila {rowIndex} fuera de rango. Rango válido: 0-{length - 1}");
            return null;
        }

        List<GameObject> emptySlabs = new List<GameObject>();

        for (int j = 0; j < width; j++)
        {
            // Verificar si la posición está vacía
            if (robots[rowIndex, j] == null)
            {
                emptySlabs.Add(slabs[rowIndex, j]);
            }
        }

        if (emptySlabs.Count > 0)
        {
            return emptySlabs[Random.Range(0, emptySlabs.Count)];
        }

        return null;
    }

    public GameObject GetSlab(int x, int y)
    {
        if (x < 0 || x >= length || y < 0 || y >= width)
            return null;
        return slabs[x, y];
    }
}
