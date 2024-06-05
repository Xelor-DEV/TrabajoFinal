using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RobotCardController : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text robotName;
    [SerializeField] private TMP_Text damage;
    [SerializeField] private TMP_Text life;
    [SerializeField] private RobotCard robotCard;
    [SerializeField] private int index;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();

        Image[] images = GetComponentsInChildren<Image>();
        icon = images[1];
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        robotName = texts[0];
        damage = texts[1];
        life = texts[2];
    }
    private void OnEnable()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnCardClick);
        }
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(OnCardClick);
    }
    public void SetData(RobotCard data, int index)
    {
        robotCard = data;
        icon.sprite = robotCard.RobotIcon;
        robotName.text = robotCard.RobotName.ToString();
        damage.text = robotCard.Damage.ToString();
        life.text = robotCard.Life.ToString();
        this.index = index;
    }
    public void OnCardClick()
    {
        if(PlayerController.Instance.SetCurrentRobot(robotCard) == true)
        {
            PlayerController.Instance.Inventory.RemoveRobot(index);
            PlayerController.Instance.Inventory.UpdateDisplayedRobots();
        }        
    }
}
