using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RobotCardController : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text robotName;
    [SerializeField] private TMP_Text damage;
    [SerializeField] private TMP_Text life;
    private void Awake()
    {
        icon = GetComponentInChildren<Image>();
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        robotName = texts[0];
        damage = texts[1];
        life = texts[2];
    }
    public void SetData(RobotCard data)
    {
        icon = data.RobotIcon;
        robotName.text = data.RobotName.ToString();
        damage.text = data.Damage.ToString();
        life.text = data.Life.ToString();
    }
}
