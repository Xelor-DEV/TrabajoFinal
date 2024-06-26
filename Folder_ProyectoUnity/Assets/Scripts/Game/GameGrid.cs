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
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
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
}
