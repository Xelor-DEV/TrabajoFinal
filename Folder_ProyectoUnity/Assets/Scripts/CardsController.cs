using UnityEngine;

public class CardsController : MonoBehaviour
{
    [SerializeField] private RobotCard[] robotCards;

    private void Start()
    {
        for(int i = 0; i < robotCards.Length; ++i)
        {
            robotCards[i].ResetToDefault();
        }
    }
}