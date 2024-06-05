using UnityEngine;
using UnityEngine.UI;
public class SlabController : MonoBehaviour
{
    [SerializeField] private int xIndex;
    [SerializeField] private int yIndex;
    public int XIndex
    {
        get
        {
            return xIndex;
        }
        set
        {
            xIndex = value;
        }
    }
    public int YIndex
    {
        get
        {
            return yIndex;
        }
        set
        {
            yIndex = value;
        }
    }
}
